using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotel_Web_API.Data;
using hotel_Web_API.models;
using hotel_Web_API.DTOs;
using System.Collections;
using Microsoft.AspNetCore.Authorization;

namespace hotel_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class bookingsController : ControllerBase
    {
        private readonly hotel_DB_Context _context;

        public bookingsController(hotel_DB_Context context)
        {
            _context = context;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<booking>>> Getbookings()
        {
            return await _context.bookings.ToListAsync();
        }

        // GET: api/bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<booking>> Getbooking(int id)
        {
            var booking = await _context.bookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putbooking(int id, booking booking)
        {
            if (id != booking.bookingId)
            {
                return BadRequest();
            }

            _context.Entry(booking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bookingExists(id))
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

        // POST: api/bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<booking>> Postbooking(booking booking)
        {
            _context.bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getbooking", new { id = booking.bookingId }, booking);
        }

        // DELETE: api/bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletebooking(int id)
        {
            var booking = await _context.bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool bookingExists(int id)
        {
            return _context.bookings.Any(e => e.bookingId == id);
        }


        [HttpPost("addNewBooking")]
        public async Task<ActionResult<int>> PostNewBooking(bookingModel[] bookingModel)
        {
                
                var totalPrice = getPrices(bookingModel).Result.Value.priceAfterDiscount;
                 booking book = new booking();


            if (bookingModel.Length != 0)
            {

                //get total price
        
                //add booking
                book = new booking() { userId = bookingModel[0].userId ,totalPrice= (int)totalPrice };
                _context.bookings.Add(book);
                await _context.SaveChangesAsync();

                //change user first booking to flase
                var user = _context.users.FirstOrDefault(u => u.id == bookingModel[0].userId);
                user.isFirstBooking = false;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                //add room booking for each room
                for (int i = 0; i < bookingModel.Length; i++)
                {
                    roomBooking roomBooking = new roomBooking()
                    { 
                        bookingId = book.bookingId,
                        roomId = bookingModel[i].roomId,
                        bookingFrom = bookingModel[i].bookingFrom,
                        bookingTo= bookingModel[i].bookingTo.AddDays(1)
                    };
                    var oldRoomBooking = _context.roomBookings.FirstOrDefault(u => u.roomId == roomBooking.roomId && (u.bookingFrom == roomBooking.bookingFrom ||u.bookingTo==roomBooking.bookingTo));
                    if (oldRoomBooking != null)
                    {
                        return BadRequest($"This room with id = {oldRoomBooking.roomId} is already book in this date");
                    }

                    _context.roomBookings.Add(roomBooking);
                    await _context.SaveChangesAsync();
                }
            }
            return CreatedAtAction("Getbooking", new { id = book.bookingId }, book);
        }


        [HttpGet("GetroomBookedDays/{roomId}")]
        public async Task<ActionResult<List<DateTime>>> GetroomBookedDays(int roomId)
        {
            var bookings =  _context.roomBookings.Where(a=>a.roomId==roomId).ToList();

                var dates = new List<DateTime>();
            for (int i = 0; i < bookings.Count; i++)
            {

                for (var dt = bookings[i].bookingFrom; dt <= bookings[i].bookingTo; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                }
            }
            return dates;
        }



        [HttpPost("getPrices")]
        public  async Task<ActionResult<getFinalPriceModel>> getPrices(bookingModel[] bookingModel)
        {
            var bookingTotal=0;
            var user = await _context.users.FirstOrDefaultAsync(u => u.id == bookingModel[0].userId);
            if (user == null)
            {
                return NotFound("user not found");
            }
            else
            {
            for (int i = 0; i < bookingModel.Length; i++)
            {
                    var room = await _context.rooms.FirstOrDefaultAsync(u => u.roomId == bookingModel[i].roomId);
                    var numOfDays = (bookingModel[i].bookingTo - bookingModel[i].bookingFrom).Days + 1;
                    int roomTotal = numOfDays * room.PricePerDay;
                    bookingTotal += roomTotal;
            }

            }
            double priceAfterDiscount = bookingTotal;
            double discountValue=0;

            if (user.isFirstBooking == false)
            {
                discountValue = bookingTotal * 0.95 ;
                priceAfterDiscount = bookingTotal - discountValue;

            }
            var result = new getFinalPriceModel()
            {
                hasDiscount = !user.isFirstBooking,
                priceBeforeDiscount = bookingTotal,
                priceAfterDiscount = priceAfterDiscount,
                discountValue = discountValue
            };

            return result;
        }

    }


}
