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

namespace SistemaHotel.Cadastros
{
    public partial class FrmUsuario : Form
    {
        // variavel de conexao para utilizar nos botoes
        Conexao conect = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;
        public FrmUsuario()
        {
            InitializeComponent();
        }

        private void CarregarComboBox()
        {
            conect.abrirConexao();
            sql = "SELECT * FROM cargo order by cargo asc";
            cmd = new MySqlCommand(sql, conect.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbCargo.DataSource = dt;
            cbCargo.DisplayMember = "cargo";

            conect.fecharConexao();
        }
        //Formata o Data Grid para uma listagem mais limpa
        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[5].HeaderText = "Cargo";
            grid.Columns[3].HeaderText = "Usuário";
            grid.Columns[4].HeaderText = "Senha";
            grid.Columns[5].HeaderText = "Data";

            //ocultando a coluna desnecessaria
            grid.Columns[0].Visible = false;

        }
        //Lista as informações do banco de dados
        private void Listar()
        {

            conect.abrirConexao();
            sql = "SELECT * FROM usuario order by nome asc";
            cmd = new MySqlCommand(sql, conect.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conect.fecharConexao();
            FormatarDG();
        }

        private void BuscarNome()
        {
            conect.abrirConexao();
            // sql linha de codigo onde busca o nome letra por letra utilizando o comando LIKE
            sql = "SELECT * FROM usuario where nome LIKE @nome order by nome asc";
            cmd = new MySqlCommand(sql, conect.con);
            cmd.Parameters.AddWithValue("@nome", txtBuscarNome.Text + "%");
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conect.fecharConexao();
            FormatarDG();
        }

        private void habilitarCampos()
        {
            txtNome.Enabled = true;
            txtUsuario.Enabled = true;
            txtSenha.Enabled = true;
            cbCargo.Enabled = true;
            txtNome.Focus();

        }


        private void desabilitarCampos()
        {
            txtNome.Enabled = false;
            txtUsuario.Enabled = false;
            txtSenha.Enabled = false;
            cbCargo.Enabled = false;
        }


        private void limparCampos()
        {
            txtNome.Text = "";
            txtUsuario.Text = "";
            txtSenha.Text = "";
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {
            Listar();
            CarregarComboBox();
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            if (cbCargo.Text == "")
            {
                MessageBox.Show("Cadastre antes um cargo!");
                Close();
            }

            habilitarCampos();
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o Nome", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }

            //CÓDIGO DO BOTÃO PARA SALVAR
            conect.abrirConexao();
            sql = "INSERT INTO usuario (nome, cargo, usuario, senha, data) VALUES (@nome, @cargo, @usuario, @senha, curDate())";
            cmd = new MySqlCommand(sql, conect.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@cargo", cbCargo.Text);
            cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            cmd.Parameters.AddWithValue("@senha", txtSenha.Text);

            cmd.ExecuteNonQuery();
            conect.fecharConexao();

            MessageBox.Show("Registro Salvo com Sucesso!", "Dados Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }
    }
}
