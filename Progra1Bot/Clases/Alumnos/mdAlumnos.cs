using System;
using System.Collections.Generic;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class mdAlumnos
    {


        public String apellido { get; set; }
        public String nombre { get; set; }
        public String correo { get; set; }
        public String carnet { get; set; }
        public String seccion { get; set; }
        public List<mdAlumnos> Alumnos { get; set; }


        public mdAlumnos()
        {
            Alumnos = new List<mdAlumnos>();
        }


        public List<mdAlumnos> cargaDatos(String archivo, String Seccion="T")
        {
            return new ClsManejoArchivos().LeerArchivoCargaDatos(archivo, Seccion);
        }


    }// fin de la clase
}
