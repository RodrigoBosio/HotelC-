using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaHotel
{
    public partial class FrmLogin : Form
    {
        Conexao conect = new Conexao();
        public FrmLogin()
        {
            
            InitializeComponent();
            pnlLogin.Visible = false;
            
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
           
            pnlLogin.Location = new Point(this.Width / 2 - 166, this.Height / 2 - 170);
            pnlLogin.Visible = true;
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(21, 114, 160);
            btnLogin.FlatAppearance.MouseDownBackColor = Color.FromArgb(8, 72, 103);

        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            ChamarLogin();
        }

        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ChamarLogin();
            }
        }


        private void ChamarLogin()
        {
            if (txtUsuario.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Preencha o Usuário", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsuario.Text = "";
                txtUsuario.Focus();
                return;
            }

            if (txtSenha.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Preencha a Senha", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSenha.Text = "";
                txtSenha.Focus();
                return;
            }

            //Código de Verificação do usuario no banco de dados para fazer login no programa
            MySqlCommand cmdVerificar;
            MySqlDataReader reader;

            conect.abrirConexao();
            cmdVerificar = new MySqlCommand("SELECT * FROM usuario where usuario =@usuario and senha =@senha", conect.con);
            cmdVerificar.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            cmdVerificar.Parameters.AddWithValue("@senha", txtSenha.Text);
            reader = cmdVerificar.ExecuteReader();
            if (reader.HasRows)
            {
                //pegando as informações da consulta de login
                while(reader.Read())
                {
                    Program.nomeUsuario = Convert.ToString(reader["nome"]);
                    Program.cargoUsuario = Convert.ToString(reader["cargo"]);
                }

                MessageBox.Show("Bem Vindo " + Program.nomeUsuario, "Login Efetuado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FrmMenu form = new FrmMenu();
                //this.Hide();
                Limpar();
                form.Show();
            }
            else
            {
                MessageBox.Show("Dados estão incorretos!", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Text = "";
                txtUsuario.Focus();
                txtSenha.Text = "";
            }
            conect.fecharConexao();
           

           
        }


        private void Limpar()
        {
            txtUsuario.Text = "";
            txtSenha.Text = "";
            txtUsuario.Focus();
        }

        private void FrmLogin_Resize(object sender, EventArgs e)
        {
            pnlLogin.Location = new Point(this.Width / 2 - 166, this.Height / 2 - 170);
        }
    }
}
