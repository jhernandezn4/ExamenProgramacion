using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Progra1Bot.Clases.Alumnos
{
    class ClsManejoArchivos
    {

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivoApertura"></param>
        /// <param name="seccion">sin parametro trae todas</param>
        /// <returns></returns>
        public List<mdAlumnos> LeerArchivoCargaDatos(String archivoApertura, String seccion="T")
        {
            mdAlumnos alumno = new mdAlumnos();
            List<mdAlumnos> Alumnos = new List<mdAlumnos>();
            String contenido = String.Empty;
            String linea;

            System.IO.StreamReader archivo = new System.IO.StreamReader(archivoApertura);
            while ((linea = archivo.ReadLine()) != null)
            {
                var datos = linea.Split(';');
                if (seccion.Equals("T")) // todas las secciones
                {
                    // el codigo se repite, no es una buena practica repetir el codigo
                    // sin embargo, decidi repetirlo para el entendimiento del estudiante
                    alumno.apellido = datos[0];
                    alumno.nombre = datos[1];
                    alumno.correo = datos[2];
                    alumno.carnet = datos[3];
                    alumno.seccion = datos[4];
                    Alumnos.Add(alumno);
                    alumno = new mdAlumnos();
                } else  if (datos[4].Equals(seccion)) // filtra seccion
                {
                    alumno.apellido = datos[0];
                    alumno.nombre = datos[1];
                    alumno.correo = datos[2];
                    alumno.carnet = datos[3];
                    alumno.seccion = datos[4];
                    Alumnos.Add(alumno);
                    alumno = new mdAlumnos();
                }
            }
            return Alumnos;
        }

        public  void CambioLinea(string nuevoTexto, string NombreArchivo, int LineaParaEditar)
        {
            string[] arrLine = File.ReadAllLines(NombreArchivo);
            arrLine[LineaParaEditar - 1] = nuevoTexto;
            File.WriteAllLines(NombreArchivo, arrLine);
        }

        public string ListaArchivos (string Directorio)
        {
            String resultado = "";
            string[] todos = Directory.GetFiles(Directorio);
            foreach (string nombres in todos)
            {
                resultado += nombres + "\n";
            }
            return resultado;
        }

    }// fin de la clase
}
