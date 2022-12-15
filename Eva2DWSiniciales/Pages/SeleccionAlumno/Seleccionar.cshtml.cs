using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Npgsql;

namespace Eva2DWSiniciales.Pages.SeleccionAlumno
{
    public class SeleccionarModel : PageModel
    {
        public void OnGet()
        {
        }

        public string Mensaje { get; set; }
        public string Mensaje2 { get; set; }
        [ActionName("MiSelect")]
        public void OnPost(string cod_alumno, string cod_evaluacion)
        {
            
            //Se recoge la información de la vista
            var connection = new NpgsqlConnection("Host=localhost;Port=5432;Pooling=true;Database=bd_evaluacion;UserId=postgres;Password=Juancarbc2001;");
            Console.WriteLine("Conexión base de datos abierta.");
            connection.Open();
            //Establecida la conexion con la base de datos


            NpgsqlCommand consulta = new NpgsqlCommand($"SELECT cod_alumno, nota_evaluacion FROM \"sc_evaluacion\".\"eva_tch_notas_evaluacion\" WHERE cod_alumno='{cod_alumno}';", connection);
            NpgsqlDataReader resultadoConsulta = consulta.ExecuteReader();
            //Ejecutamos la query dentro de la base de datos

            List<object> nombre = new List<object>();
            List<object> nota = new List<object>();



            if (resultadoConsulta.HasRows)
            {
                while (resultadoConsulta.Read())
                {
                    nombre.Add(resultadoConsulta[0]);
                    nota.Add(resultadoConsulta[1]);
                }

                this.Mensaje = string.Format("El usuario {0} tiene la nota {1} ", nombre[0], nota[0]);

            }
            else
            {
                Console.WriteLine("Error al consultar usuario.");
                //La base de datos no tene el alumno que hemos introducido
                this.Mensaje = string.Format("No existe este usuario");
            }
            connection.Close();
            connection.Open();
            NpgsqlCommand consulta2 = new NpgsqlCommand($"SELECT desc_evaluacion FROM \"sc_evaluacion\".\"eva_cat_evaluacion\" WHERE cod_evaluacion='{cod_evaluacion}';", connection);
            NpgsqlDataReader resultadoConsulta2 = consulta.ExecuteReader();

            List<object> evaluacion = new List<object>();
            if (resultadoConsulta2.HasRows)
            {
                while (resultadoConsulta2.Read())
                {
                    evaluacion.Add(resultadoConsulta2[0].ToString());
                    
                }

                this.Mensaje = string.Format("en la evaluacion {0}", evaluacion[0]);

            }

            Console.WriteLine("Cerrando conexion");
            //Conexión cerrada
            connection.Close();

        }

    }
}
