using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackV15II.Data;
using BackV15II.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static Common.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace BackV15II
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolicitudController : ControllerBase
    {
        private readonly BackV15IIContext _context;

        public SolicitudController(BackV15IIContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ObtenerSolicitudes()
        {
            try
            {
                SqlConnection cnnSQL = new SqlConnection("Data Source=localhost;Initial Catalog=clixerv15;User ID=sa;Password=MihuyaX");
                cnnSQL.Open();
                var solicitudes = cnnSQL.Query<object>("select * From solicitud left join planes on solicitud.planid = planes.id");
                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet("Obtenerprecarga")]
        public async Task<IActionResult> Obtenerprecarga(int id)
        {
            //int id = 1;
            try
            {
                var connection = new MySqlConnector.MySqlConnection("Server=167.250.5.42;Database=alcancep_test;Uid=alcancep_testus;Pwd=VWijLV+]ETeG;" + "; AllowLoadLocalInfile = True");
                await connection.OpenAsync();

                var precargas = connection.QueryFirst<object>($"SELECT * FROM test_precarga s LEFT JOIN test_planes p ON s.plan = p.id where s.id = {id}");
                return Ok(precargas);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Actualizar()
        {
            try
            {
                SqlConnection cnnSQL = new SqlConnection("Data Source=localhost;Initial Catalog=precargaalcance;User ID=sa;Password=MihuyaX");
                cnnSQL.Open();
                cnnSQL.Execute($@"Update solicitud Set prueba = Case When Prueba = 'Actualizado' Then 'Desactualizado' Else 'Actualizado' End");
                cnnSQL.Close();
                return Ok("ok");
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        // GET: api/Solicitud
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Solicitud>>> GetSolicitud()
        public ActionResult<IEnumerable<Solicitud>> GetSolicitud()
        //public ActionResult GetSolicitud()
        {
            //return await _context.Solicitud.ToListAsync();
            SqlConnection cnnSQL = new SqlConnection("Data Source=localhost;Initial Catalog=clixerv15;User ID=sa;Password=MihuyaX");
            cnnSQL.Open();
            var solicitudes = cnnSQL.Query<Solicitud>("Select solicitud.* From solicitud left join planes on solicitud.planid = planes.id");           
            return Ok(solicitudes);
        }

        // GET: api/Solicitud/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Solicitud>> GetSolicitud(int id)
        //{
        //    var solicitud = await _context.Solicitud.FindAsync(id);

        //    if (solicitud == null)
        //    {
        //        return NotFound();
        //    }

        //    return solicitud;
        //}

        // GET: api/Solicitud/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Solicitud>> GetPrecarga(int id)
        //{
        //    var solicitud = await _context.Solicitud.FindAsync(id);

        //    if (solicitud == null)
        //    {
        //        return NotFound();
        //    }

        //    return solicitud;
        //}

        // PUT: api/Solicitud/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitud(int id, Solicitud solicitud)
        {            
            if (id != solicitud.Id)
            {
                return BadRequest();
            }

            ValidacionesRequeridas(solicitud);
            if (!ModelState.IsValid)
            {
                return ValidationProblem("Validaciones requeridas", null, 400, null, null, ModelState);
            }           

            _context.Entry(solicitud).State = EntityState.Modified;
            try
            {
                //using (var cnnSQL = new SqlConnection("Data Source=localhost;Initial Catalog=precargaalcance;User ID=sa;Password=MihuyaX"))
                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=clixerv15;User ID=sa;Password=MihuyaX"))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;
                    transaction = connection.BeginTransaction();
                    // Must assign both transaction object and connection
                    // to Command object for a pending local transaction
                    command.Connection = connection;
                    command.Transaction = transaction;
                    try
                    {
                        command.CommandText = $@"Update solicitud Set 
                                nombre = {TxtSQL(solicitud.Nombre)},
                                planid = {NVal(solicitud.Planid)},
                                prueba = {TxtSQL(solicitud.Prueba)},
                                numerodecimal = {NVal(solicitud.Numerodecimal)},
                                fecha = {FecSQL(solicitud.Fecha)}
                                Where id = {solicitud.Id}";

                        command.ExecuteNonQuery();
                        
                        command.CommandText = "INSERT INTO titular (nombre) VALUES ('titular')";
                        command.ExecuteNonQuery();
                        
                        transaction.Commit();                        
                        //return CreatedAtAction("GetCliente", new { id = solicitud.Id }, solicitud);
                        return Ok(new { id = solicitud.Id, solicitud });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                        Console.WriteLine("  Message: {0}", ex.Message);
                        // Attempt to roll back the transaction.
                        try
                        {
                            transaction.Rollback();
                            return BadRequest(ex.Message);
                        }
                        catch (Exception ex2)
                        {
                            // This catch block will handle any errors that may have occurred
                            // on the server that would cause the rollback to fail, such as
                            // a closed connection.
                            Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                            Console.WriteLine("  Message: {0}", ex2.Message);
                            return BadRequest();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

            return NoContent();
        }


        // POST: api/ValidacionesSolicitud
        [HttpPost("validaciones")]        
        //public async Task<ActionResult<IEnumerable<Validacion>>> ValidacionesSolicitud(Solicitud solicitud)
        public async Task<ActionResult<Solicitud>> ValidacionesSolicitud(Solicitud solicitud)
        {
            List<Validacion> validaciones = new List<Validacion>();

            ValidacionesRequeridas(solicitud);
            //if (!ModelState.IsValid)
            //{
            //    return ValidationProblem("Validaciones requeridas", null, 400, null, null, ModelState);
            //}

            if (ModelState.IsValid) ValidacionesOpcionales(solicitud);

            //if (!ModelState.IsValid)
            //{
            //    return ValidationProblem("Validaciones opcionales", null, 400, null, null, ModelState);
            //}
            //validaciones.Add(new Validacion { Id = 1, Descripcion = "Validación Requerida1", Tipo = "Requerida" });
            //validaciones.Add(new Validacion { Id = 2, Descripcion = "Validación Requerida2", Tipo = "Requerida" });
            //validaciones.Add(new Validacion { Id = 3, Descripcion = "Validación Opcional1", Tipo = "Opcional" });
            //validaciones.Add(new Validacion { Id = 4, Descripcion = "Validación Opcional2", Tipo = "Opcional" });
            //return validaciones;
            if (!ModelState.IsValid)
            {
                //return ValidationProblem("Validaciones", null, 400, null, null, ModelState);
                var errors2 = ModelState.Values;
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    var errorMessage = error.ErrorMessage;                    
                    var errorKey = ModelState.Keys.FirstOrDefault(k => ModelState[k].Errors.Contains(error));
                    validaciones.Add(new Validacion { Id = errorKey, Descripcion = errorMessage, Tipo = errorKey.Contains("VO")? "Opcional":"Requerida" });
                }

                return Ok(validaciones);
            }
            else
            {
                return Ok();
            }
                
        }

        // POST: api/Solicitud              
        [HttpPost]
        public async Task<ActionResult<Solicitud>> PostSolicitud(Solicitud solicitud)
        {            
            try {   
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                ValidacionesRequeridas(solicitud);
                if (!ModelState.IsValid)
                {
                    return ValidationProblem("Validaciones requeridas", null, 400, null, null, ModelState);
                }

                ValidacionesOpcionales(solicitud);
                if (!ModelState.IsValid)
                {
                    return ValidationProblem("Validaciones opcionales", null, 400, null, null, ModelState);
                }


                _context.Solicitud.Add(solicitud);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetSolicitud", new { id = solicitud.Id }, solicitud);
            }
            catch (Exception e)
            {
                //return BadRequest("No se puede completar el registro debido a datos incorrectos o incompletos." + e.Message, StatusCode: 500);
                return ValidationProblem(e.Message +" - " + e.InnerException);
            }            
        }
       
        private void ValidacionesRequeridas(Solicitud solicitud)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("VRC1", "Campos requeridos."); ;
            }            
            if (solicitud.Nombre.ToUpper() == "REQUERIDA")
            {
                ModelState.AddModelError("VRC2", "validación requerida 1 de solicitud.");
            }
        }

        private void ValidacionesOpcionales(Solicitud solicitud)
        {
            if (solicitud.Nombre.ToUpper() == "OPCIONAL")
            {
                ModelState.AddModelError("VOC1", "Validación opcional 1.");                
            }
        }
    }    
}
