using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackV15II.Data;
using BackV15II.Models;

namespace BackV15II
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalidadesController : ControllerBase
    {
        private readonly BackV15IIContext _context;

        public LocalidadesController(BackV15IIContext context)
        {
            _context = context;
        }

        // GET: api/Localidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Localidad>>> GetLocalidad()
        {
            return await _context.Localidad.ToListAsync();
        }

        // GET: api/Localidades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Localidad>> GetLocalidad(int id)
        {
            var localidad = await _context.Localidad.FindAsync(id);

            if (localidad == null)
            {
                return NotFound();
            }

            return localidad;
        }

        // PUT: api/Localidades/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalidad(int id, Localidad localidad)
        {
            if (id != localidad.Id)
            {
                return BadRequest();
            }

            _context.Entry(localidad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocalidadExists(id))
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

        // POST: api/Localidades
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Localidad>> PostLocalidad(Localidad localidad)
        {
            _context.Localidad.Add(localidad);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLocalidad", new { id = localidad.Id }, localidad);
        }

        // DELETE: api/Localidades/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Localidad>> DeleteLocalidad(int id)
        {
            var localidad = await _context.Localidad.FindAsync(id);
            if (localidad == null)
            {
                return NotFound();
            }

            _context.Localidad.Remove(localidad);
            await _context.SaveChangesAsync();

            return localidad;
        }

        private bool LocalidadExists(int id)
        {
            return _context.Localidad.Any(e => e.Id == id);
        }
    }
}
