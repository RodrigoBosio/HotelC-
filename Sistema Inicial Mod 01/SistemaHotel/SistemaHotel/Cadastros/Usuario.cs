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
        string id, usuarioAntigo;
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

            //Verificar se o nome do usuario ja existe no Banco de Dados
            MySqlCommand cmdVerificar;
            cmdVerificar = new MySqlCommand("SELECT * FROM usuario where usuario =@usuario", conect.con);
            cmdVerificar.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerificar;
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Usuario ja registrado", "Erro ao Registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUsuario.Text = "";
                txtUsuario.Focus();
                return;
            }
           

            cmd.ExecuteNonQuery();
            conect.fecharConexao();

            MessageBox.Show("Registro Salvo com Sucesso!", "Dados Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }

        private void Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEditar.Enabled = true;
            btnExcluir.Enabled = true;
            btnSalvar.Enabled = false;
            txtNome.Enabled = true;
            habilitarCampos();

            //recuperando como texto o indice da linha que clicar 
            id = grid.CurrentRow.Cells[0].Value.ToString();
            txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
            cbCargo.Text = grid.CurrentRow.Cells[2].Value.ToString();
            txtUsuario.Text = grid.CurrentRow.Cells[3].Value.ToString();
            txtSenha.Text = grid.CurrentRow.Cells[4].Value.ToString();
           
            usuarioAntigo = grid.CurrentRow.Cells[3].Value.ToString();
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("Deseja Realmente Excluir o Usuário?", "Excluir Usuário", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resultado == DialogResult.Yes)
            {
                //CÓDIGO DO BOTÃO PARA EXCLUIR
                conect.abrirConexao();
                sql = "DELETE FROM usuario where id =@id";
                cmd = new MySqlCommand(sql, conect.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                conect.fecharConexao();

                MessageBox.Show("Usuário Excluido com Sucesso!", "Usuário Excluido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnNovo.Enabled = true;
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                txtNome.Text = "";
                txtNome.Enabled = false;
                txtUsuario.Text = "";
                txtUsuario.Enabled = false;
                txtSenha.Text = "";
                txtSenha.Enabled = false;
                cbCargo.Enabled = false;


                Listar();
            }
        }

        private void TxtBuscarNome_TextChanged(object sender, EventArgs e)
        {
            BuscarNome();
        }

        private void BtnEditar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                txtNome.Text = "";
                MessageBox.Show("Preencha o Nome", "Campo Vazio", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNome.Focus();
                return;
            }

            //CÓDIGO DO BOTÃO PARA EDITAR
            conect.abrirConexao();
            sql = "UPDATE usuario SET nome =@nome, cargo =@cargo, usuario =@usuario, senha =@senha where id =@id";
            cmd = new MySqlCommand(sql, conect.con);
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@cargo", cbCargo.Text);
            cmd.Parameters.AddWithValue("@usuario", txtUsuario.Text);
            cmd.Parameters.AddWithValue("@senha", txtSenha.Text);
            cmd.Parameters.AddWithValue("@id", id);

            //Verificação USUARIO existente e teste para poder modificar o cadastro do usuario
            if (txtUsuario.Text != usuarioAntigo)
            {
                MySqlCommand cmdVerificar;
                cmdVerificar = new MySqlCommand("SELECT * FROM usuario where usuario =@usuario", conect.con);
                cmdVerificar.Parameters.AddWithValue("@usuario", txtUsuario.Text);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("USUARIO ja registrado", "Erro ao Registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtUsuario.Text = "";
                    txtUsuario.Focus();
                    return;
                }
            }

            cmd.ExecuteNonQuery();
            conect.fecharConexao();

            MessageBox.Show("Registro Editado com Sucesso!", "Dados Editados", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnNovo.Enabled = true;
            btnEditar.Enabled = false;
            btnExcluir.Enabled = false;
            limparCampos();
            desabilitarCampos();
            Listar();
        }
    }
}
