using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaHotel
{
    class Conexao
    {
        //informaçõe necessarias para a conexao com o banco de dados(atualmente conectado localmente)
        string conect = "SERVER=localhost; DATABASE=hotel; UID=root, PWD=; PORT=;";
        public MySqlConnection con = null;

        //metodo para abrir a conexao, ele necessita de um metodo para fecha-la
        public void abrirConexao()
        {
            //tratamento de erros na conexao
            try
            {
                con = new MySqlConnection(conect);
                con.Open();
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        //metodo para fechar a conexao
        public void fecharConexao()
        {
            try
            {
                con = new MySqlConnection(conect);
                con.Close();
            }
            catch (Exception e)
            {

                throw e;
            }

        }


    }
}
