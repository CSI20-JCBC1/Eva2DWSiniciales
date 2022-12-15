using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace Eva2DWSiniciales.Pages.Insertar
{
    public class InsertarModel : PageModel
    {
        public void OnGet()
        {
        }

        public string Mensaje { get; set; }


        [ActionName("MiInsercion")]
        public void OnPost(string cod_alumno, string nota_evaluacion,string cod_evaluacion)
        {

            ///Se recoge la información de la vista
            var connection = new NpgsqlConnection("Host=localhost;Port=5432;Pooling=true;Database=bd_evaluacion;UserId=postgres;Password=Juancarbc2001;");
            Console.WriteLine("Conexión base de datos abierta.");
            connection.Open();
            //Establecida la conexion con la base de datos

            DateTime dateTime = DateTime.Now;
            string fecha = dateTime.ToString("yyyy-MM-dd HH:mm:ss");
            //Damos formato a la fecha

            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var Charsarr = new char[8];
            var random = new Random();

            for (int i = 0; i < Charsarr.Length; i++)
            {
                Charsarr[i] = characters[random.Next(characters.Length)];
            }

            var mduuid = new String(Charsarr);
            Console.WriteLine(mduuid);
            //mduuid


                   
            

                if (cod_alumno==null||nota_evaluacion==null||cod_evaluacion==null)
                {
                    Console.WriteLine(" Ninguno de los campos a introducir puede ser null");
                    this.Mensaje = string.Format("Ninguno de los datos a introducir puede ser null.");
                    //Controlamos los errores al introducir valores nulos
                }
                else if (cod_evaluacion=="PR"|| cod_evaluacion == "SG"|| cod_evaluacion == "TC")
                {
                    //Controlado que solo se introduzcan las iniciales deseadas

                    NpgsqlCommand consulta = new NpgsqlCommand($"SELECT * FROM \"sc_evaluacion\".\"eva_tch_notas_evaluacion\" WHERE cod_evaluacion='{cod_evaluacion}'", connection);
                    NpgsqlDataReader resultadoConsulta = consulta.ExecuteReader();
                    //Consulta hecha a la base de datos



                    if (resultadoConsulta.HasRows)
                    {
                        //La consulta obtienne resultado
                        Console.WriteLine("Ya está esta evaluación");
                        this.Mensaje = string.Format("El alumno ya tiene una nota en esta evaluación.");

                    }
                    else
                    {
                        //No hay resultados en la base de datos que coincidan con los datos introducidos
                        //Insertamos los valores para el registro
                        connection.Close();

                        connection.Open();
                        Console.WriteLine("Insertando usuario en la base de datos");
                        consulta = new NpgsqlCommand($"INSERT INTO \"sc_evaluacion\".\"eva_tch_notas_evaluacion\" (md_uuid, md_fch, cod_alumno, nota_evaluacion, cod_evaluacion) VALUES ('{mduuid}', '{fecha}', '{cod_alumno}', '{nota_evaluacion}', '{cod_evaluacion}');", connection);
                        //Pondremos por defecto nivel de acceso 1 y se lo cambiaremos en la base de datos
                        consulta.ExecuteNonQuery();
                        Console.WriteLine("Insert realizado con éxito");
                        this.Mensaje = string.Format("Alumno agregado.");

                    }
                }
                else
                {


                    Console.WriteLine("No existe esta evaluación");
                    this.Mensaje = string.Format("La evaluación debe ser PR SG o TC");

                }





            
            



            Console.WriteLine("Cerrando conexion");
            connection.Close();
        }

    }
}

