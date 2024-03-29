﻿using MySql.Data.MySqlClient;
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
    public partial class FrmCargo : Form
    {

        // variavel de conexao para utilizar nos botoes
        Conexao conect = new Conexao();
        string sql;
        MySqlCommand cmd;
        string id;
        public FrmCargo()
        {
            InitializeComponent();
        }

        //Formata o Data Grid para uma listagem mais limpa
        private void FormatarDG()
        {
            grid.Columns[0].HeaderText = "ID";
            grid.Columns[1].HeaderText = "Cargo";

            //ocultando a coluna desnecessaria
            grid.Columns[0].Visible = false;

            //tamanho da coluna
            grid.Columns[1].Width = 200;
        }

        //Lista as informações do banco de dados
        private void Listar()
        {
           
            conect.abrirConexao();
            sql = "SELECT * FROM cargo order by cargo asc";
            cmd = new MySqlCommand(sql, conect.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conect.fecharConexao();
            FormatarDG();
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            txtNome.Enabled = true;
            btnSalvar.Enabled = true;
            btnNovo.Enabled = false;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            txtNome.Focus();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o Cargo", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }

            //programação do botao salvar
            conect.abrirConexao();
            sql = "INSERT INTO cargo (cargo) VALUES (@cargo)";
            cmd = new MySqlCommand(sql, conect.con);
            cmd.Parameters.AddWithValue("@cargo", txtNome.Text);
            cmd.ExecuteNonQuery();
            conect.fecharConexao();

            MessageBox.Show("Registro Salvo com Sucesso", "Dados Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Text = "";
            txtNome.Enabled = false;
            Listar();
        }

        private void FrmCargo_Load(object sender, EventArgs e)
        {
            Listar();
        }


        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o Cargo", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }


            //CÓDIGO DO BOTÃO PARA EDITAR
            conect.abrirConexao();
            sql = "UPDATE cargo SET cargo =@cargo where id =@id";
            cmd = new MySqlCommand(sql, conect.con);
            cmd.Parameters.AddWithValue("@cargo", txtNome.Text);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            conect.fecharConexao();


            MessageBox.Show("Registro Editado com Sucesso!", "Dados Editados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            txtNome.Text = "";
            txtNome.Enabled = false;
            Listar();
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Deseja Realmente Excluir o Registro?", "Excluir Registro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                //CÓDIGO DO BOTÃO PARA EXCLUIR
                conect.abrirConexao();
                sql = "DELETE FROM cargo where id =@id";
                cmd = new MySqlCommand(sql, conect.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conect.fecharConexao();

                MessageBox.Show("Registro Excluido com Sucesso!", "Registro Excluido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNovo.Enabled = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                txtNome.Text = "";
                txtNome.Enabled = false;
                Listar();
            }
        }

        private void TxtNome_TextChanged(object sender, EventArgs e)
        {

        }


        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Enabled = true;

            //recuperando como texto o indice da linha que clicar 
            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
        }

        private void Grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
