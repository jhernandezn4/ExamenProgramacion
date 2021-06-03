using System;
using System.Collections.Generic;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class clsCapaNegocio
    {

        public clsCapaNegocio()
        {
                
        }


        public mdAlumnos LocalizaAlumnoPorMail(String correo,String IDUsuarioTelegram)
        {
            mdAlumnos AlumnoEncontrado = new mdAlumnos();
            AlumnoEncontrado.nombre = "no encontrado";
            int linea = 1;
            int linea_encontrada = 0;

            var TodosLosAlunnos = new mdAlumnos().cargaDatos("c:\\tmp\\alumnos.csv","T");
            ClsManejoArchivos ClaseArchivos = new ClsManejoArchivos();
            foreach (mdAlumnos item in TodosLosAlunnos)
            {
                if (correo.ToLower().Equals(item.correo))
                {
                    linea_encontrada = linea;
                    AlumnoEncontrado = item;
                   // string nuevaLinea = item.apellido + ";" + item.nombre + ";" + item.correo + ";" + item.carnet + ";" + item.seccion + ";" + IDUsuarioTelegram;
                 //   ClaseArchivos.CambioLinea(nuevaLinea, "c:\\tmp\\alumnos.csv", linea_encontrada);


                }
                linea++;
            }
            return AlumnoEncontrado;
        }

    }
}
