using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Progra1Bot.Clases
{
    class clsConexion
    {
        private string servidor = "localhost"; //Nombre o ip del servidor de MySQL
        private string bd = "versusbot"; //Nombre de la base de datos
        private string usuario = "root"; //Usuario de acceso a MySQL
        private string password = ""; //Contraseña de usuario de acceso a MySQL
        private string datos = null; //Variable para almacenar el resultado
        private bool iniciado = false;
        public long ChatId { get; private set; }
        MySqlConnection conexionBD;
        MySqlDataReader reader;
        public clsConexion(long ChatId)
        {
            this.ChatId = ChatId;
            this.iniciar();
        }
        private void iniciar()
        {
            if (!iniciado)
            {
                //Crearemos la cadena de conexión concatenando las variables
                string cadenaConexion = "Database=" + bd + "; Data Source=" + servidor + "; User Id=" + usuario + "; Password=" + password + "";
                //Instancia para conexión a MySQL, recibe la cadena de conexión
                conexionBD = new MySqlConnection(cadenaConexion);
                this.iniciado = true;
            }
        }
        private DataTable ConsultaSimple(string consulta)
        {

            try
            {
                MySqlCommand comando = new MySqlCommand(consulta); //Declaración SQL para ejecutar contra una base de datos MySQL
                comando.Connection = conexionBD; //Establece la MySqlConnection utilizada por esta instancia de MySqlCommand
                conexionBD.Open(); //Abre la conexión

                DataTable dt = new DataTable();
                dt.Load(comando.ExecuteReader());
                return dt;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                DataTable dt = new DataTable();
                return dt;
            }
            finally
            {
                conexionBD.Close(); //Cierra la conexión a MySQL
            }
        }

       
        public DataTable Posicion()
        {
            string consulta = $"select * from posiciones where idchat={this.ChatId} limit 1";
            Console.WriteLine(consulta);
            return this.ConsultaSimple(consulta);
        }
        public DataTable MostrarPosiciones()
        {
            string consulta = $"SELECT * FROM posiciones ORDER BY punteo desc";
            Console.WriteLine(consulta);
            return this.ConsultaSimple(consulta);
        }
        public DataTable Registrar(long idchat, string usuario)
        {
            string consulta = $"insert into posiciones (idposicion, idchat, usuario,punteo,pregunta_actual) VALUES ('0','{idchat}', '{usuario}', '0','1');";
            this.ConsultaSimple(consulta);
            return this.Posicion();
        }
        
        public DataTable ObtenerPregunta(int idpregunta)
        {
            string actualizar = $"update posiciones set pregunta_actual={idpregunta}, resuelto=0 where idchat={this.ChatId};";
            this.ConsultaSimple(actualizar);
            string consulta = $"select * from preguntas where idpregunta={idpregunta} limit 1;";
            //this.ConsultaSimple(consulta);
            return this.ConsultaSimple(consulta);
        }
        public DataTable Responder(bool correcto = false)
        {
            string consulta;
            if (correcto)
            {
                consulta = $"Update posiciones set resuelto=1, punteo=punteo+1, intentos=intentos+1 where idchat={this.ChatId} ";
            }
            else
            {
                consulta = $"Update posiciones set resuelto=1, intento=intento+1 where  idchat={this.ChatId} ";
            }

            this.ConsultaSimple(consulta);
            return this.Posicion();
        }
        public DataTable RestablecerPuntaje()
        {
            string consulta = $"Update posiciones set resuelto=0, punteo=0, intentos=0,pregunta_actual=1 where idchat={this.ChatId} ";
            this.ConsultaSimple(consulta);
            return this.Posicion();
        }
    }
}
