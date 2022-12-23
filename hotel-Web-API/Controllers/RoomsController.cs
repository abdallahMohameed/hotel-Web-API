using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotel_Web_API.Data;
using hotel_Web_API.models;
using Microsoft.AspNetCore.Authorization;

namespace hotel_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RoomsController : ControllerBase
    {
        private readonly hotel_DB_Context _context;

        public RoomsController(hotel_DB_Context context)
        {
            _context = context;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> Getrooms()
        {
            return await _context.rooms.ToListAsync();
        }

        // GET: api/Rooms/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.rooms.Include(a=>a.branch).FirstOrDefaultAsync(room=>room.roomId==id);

            if (room == null)
            {
                return NotFound();
            }

            return room;
        }

        [HttpGet("getRooms/{branshId}")]
        public async Task<ActionResult<List<Room>>> getRoomsByBranchId(int branshId)
        {
            var rooms = await _context.rooms.Where(a => a.branchId == branshId).ToListAsync();

            if (rooms == null || rooms.Count == 0)
            {
                return NotFound("No Rooms in This Branch");
            }

            return rooms;
        }

        // POST: api/Rooms
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(Room room)
        {
            _context.rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoom", new { id = room.roomId }, room);
        }

        // DELETE: api/Rooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _context.rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RoomExists(int id)
        {
            return _context.rooms.Any(e => e.roomId == id);
        }


    }
}

