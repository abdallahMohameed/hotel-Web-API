using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotel_Web_API.Data;
using hotel_Web_API.models;

namespace hotel_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class branchesController : ControllerBase
    {
        private readonly hotel_DB_Context _context;

        public branchesController(hotel_DB_Context context)
        {
            _context = context;
        }

        // GET: api/branches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<branch>>> Getbranches()
        {
            return await _context.branches.Include(a=>a.Rooms).ToListAsync();
        }

        // GET: api/branches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<branch>> Getbranch(int id)
        {
            var branch = await _context.branches.FindAsync(id);

            if (branch == null)
            {
                return NotFound();
            }

            return branch;
        }

        // PUT: api/branches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putbranch(int id, branch branch)
        {
            if (id != branch.branchId)
            {
                return BadRequest();
            }

            _context.Entry(branch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!branchExists(id))
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

        // POST: api/branches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<branch>> Postbranch(branch branch)
        {
            _context.branches.Add(branch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getbranch", new { id = branch.branchId }, branch);
        }

        // DELETE: api/branches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletebranch(int id)
        {
            var branch = await _context.branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            _context.branches.Remove(branch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool branchExists(int id)
        {
            return _context.branches.Any(e => e.branchId == id);
        }
    }
}
