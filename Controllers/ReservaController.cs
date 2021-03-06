using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Restaurante_sal_salsa.Models;

namespace Restaurante_sal_salsa.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _env;

        public ReservaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                       SELECT servicio.nombre as servicio_nombre, reserva.*
                        FROM reserva
                        LEFT JOIN servicio
                        ON reserva.servicio_id = servicio.id;
            ";

            DataTable table = new DataTable();
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
                        SELECT * FROM reserva                        
                        WHERE id=@ReservaId;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@ReservaId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Models.Reserva reservaData)
        {
            string query = @"
                        INSERT INTO reserva (cliente_id, servicio_id, estado, fecha, asunto,correo,celular,cantidad_personas, nombre_referencia)
                        VALUES (@Ecliente_id, @Eservicio_id, @Eestado, @Efecha, @Easunto, @Ecorreo,@Ecelular,@Ecantidad_personas,@Enombre_referencia);         
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@Ecliente_id", reservaData.cliente_id);
                    myCommand.Parameters.AddWithValue("@Enombre_referencia", reservaData.nombre_referencia);
                    myCommand.Parameters.AddWithValue("@Eservicio_id", reservaData.servicio_id);
                    myCommand.Parameters.AddWithValue("@Eestado", reservaData.estado);
                    myCommand.Parameters.AddWithValue("@Efecha", reservaData.fecha);
                    myCommand.Parameters.AddWithValue("@Easunto", reservaData.asunto);
                    myCommand.Parameters.AddWithValue("@Ecorreo", reservaData.correo);
                    myCommand.Parameters.AddWithValue("@Ecelular", reservaData.celular);
                    myCommand.Parameters.AddWithValue("@Ecantidad_personas", reservaData.cantidad_personas);

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
                        DELETE FROM reserva 
                        WHERE id=@ReservaId;
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@ReservaId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }

        [HttpPut]
        public JsonResult Put(Models.Reserva reservaData)
        {
            string query = @"
                        UPDATE reserva SET 
                        estado =@Eestado
                        WHERE id =@ReservaId;   
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("TestAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myCommand.Parameters.AddWithValue("@ReservaId", reservaData.id);
                    myCommand.Parameters.AddWithValue("@Eestado", reservaData.estado);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

    }
}
