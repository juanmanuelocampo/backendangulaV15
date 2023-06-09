﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackV15II.Data;
using BackV15II.Models;
using Microsoft.Data.SqlClient;


namespace BackV15II
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly BackV15IIContext _context;

        public ClienteController(BackV15IIContext context)
        {
            _context = context;
        }
       
        // GET: api/Cliente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetCliente()
        {
            return await _context.Cliente.ToListAsync();
        }

        // GET: api/Cliente/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }

        // PUT: api/Cliente/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {            
            if (id != cliente.Id)
            {
                return BadRequest();
            }

            ValidacionesRequeridas();
            if (!ModelState.IsValid)            
            {
                return ValidationProblem("Validaciones requeridas", null, 400, null, null, ModelState);
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/ValidacionesCliente
        [HttpPost("validaciones")]        
        public async Task<ActionResult<IEnumerable<Validacion>>> ValidacionesCliente(Cliente cliente)
        {
            List<Validacion> validaciones = new List<Validacion>();            
            //validaciones.Add(new Validacion { Id = 1, Descripcion = "Validación Requerida1", Tipo = "Requerida" });
            //validaciones.Add(new Validacion { Id = 2, Descripcion = "Validación Requerida2", Tipo = "Requerida" });
            //validaciones.Add(new Validacion { Id = 3, Descripcion = "Validación Opcional1", Tipo = "Opcional" });
            //validaciones.Add(new Validacion { Id = 4, Descripcion = "Validación Opcional2", Tipo = "Opcional" });
            return validaciones;
        }

        // POST: api/Cliente              
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {            
            try {   
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }

                ValidacionesRequeridas();
                if (!ModelState.IsValid)
                {
                    return ValidationProblem("Validaciones requeridas", null, 400, null, null, ModelState);
                }

                ValidacionesOpcionales(cliente);
                if (!ModelState.IsValid)
                {
                    return ValidationProblem("Validaciones opcionales", null, 400, null, null, ModelState);
                }


                _context.Cliente.Add(cliente);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCliente", new { id = cliente.Id }, cliente);
            }
            catch (Exception e)
            {
                //return BadRequest("No se puede completar el registro debido a datos incorrectos o incompletos." + e.Message, StatusCode: 500);
                return ValidationProblem(e.Message +" - " + e.InnerException);
            }            
        }

        // DELETE: api/Cliente/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cliente>> DeleteCliente(int id)
        {
            var cliente = await _context.Cliente.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();

            return cliente;
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.Id == id);
        }

        private void ValidacionesRequeridas()
        {
            //if (1 != 1)
            //{
            //    ModelState.AddModelError("VRC1", "Descripción de la validación requerida 1 de cliente");                
            //}
            //if (2 != 2)
            //{
            //    ModelState.AddModelError("VRC2", "Descripción de la validación 2 requerida de cliente");
            //}            
        }

        private void ValidacionesOpcionales(Cliente cliente)
        {
            if (cliente.Id >= 0)
            {
                ModelState.AddModelError("VOC1", "Existe otro cliente con el mismo nombre.");                
            }
            if (cliente.Id >= 0)
            {
                ModelState.AddModelError("VOC1", "Existe otro cliente con el mismo CUIT.");
            }
            if (cliente.Id >= 0)
            {
                ModelState.AddModelError("VOC1", "Existe otro cliente con el mismo número de documento.");
            }
        }


    }
}
