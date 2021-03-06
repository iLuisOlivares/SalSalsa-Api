using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Restaurante_sal_salsa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactanosController : ControllerBase
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;

        public ContactanosController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                       SELECT * 
                        FROM contactanos
            ";

            DataTable table = new DataTable();
            // Bring the pool connetion from appsetting.json
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpGet("{id}")]
        public JsonResult GetOne(int id)
        {
            string query = @"
                        SELECT * FROM contactanos                        
                        WHERE id=@CId;
            ";

            DataTable table = new DataTable();
            // Bring the pool connetion from appsetting.json
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open(); // open connection
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@CId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Models.Contactanos contactanosData)
        {
            string query = @"
                        INSERT INTO contactanos (nombre, correo,asunto,descripcion)
                        VALUES (@Cnombre, @Ccorreo,@Casunto, @Cdescripcion) ;         
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Cnombre", contactanosData.nombre);
                    myCommand.Parameters.AddWithValue("@CCorreo", contactanosData.correo);
                    myCommand.Parameters.AddWithValue("@Casunto", contactanosData.asunto);
                    myCommand.Parameters.AddWithValue("@Cdescripcion", contactanosData.descripcion);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                        DELETE FROM contactanos
                        WHERE id=@id;
            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

        // No put comentario


    }
}
