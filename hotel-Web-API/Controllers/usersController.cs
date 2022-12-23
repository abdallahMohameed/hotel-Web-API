using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotel_Web_API.Data;
using hotel_Web_API.models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NuGet.Protocol.Plugins;
using hotel_Web_API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace hotel_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly hotel_DB_Context _context;
        private readonly IConfiguration _configuration;

        public usersController(IConfiguration configuration, hotel_DB_Context context)
        {
            _configuration = configuration;
            _context = context;
        }

        // GET: api/users


    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<user>>> Getusers()
        {
            return await _context.users.ToListAsync();
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<user>> Getuser(int id)
        {
            var user = await _context.users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putuser(int id, user user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!userExists(id))
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

        // POST: api/users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<user>> Postuser(user newUser)
        {
            var user = _context.users.FirstOrDefault(u => u.Email == newUser.Email);
            if(user != null)
            {
                return BadRequest("This email is already exist");
            }

            var tok = "";
            if (ModelState.IsValid)
            {
                string cryptPass = encryptPass(newUser.password);
                newUser.password = cryptPass;
                newUser.isFirstBooking = true;
                _context.users.Add(newUser);
                await _context.SaveChangesAsync();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("secretKey")));
                var data = new List<Claim>();
                data.Add(new Claim("id", newUser.id.ToString()));

                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(

                claims: data,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

                tok = new JwtSecurityTokenHandler().WriteToken(token);
            }


            return CreatedAtAction("Getuser", new { id = newUser.id }, new
            {
                userName = newUser.name,
                userId = newUser.id.ToString(),
                isFirstBooking = newUser.isFirstBooking,
                theToken = tok 
            });
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deleteuser(int id)
        {
            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool userExists(int id)
        {
            return _context.users.Any(e => e.id == id);
        }

        public static string encryptPass(string pass)
        {
            string secrateKey = "PassW@EdhAshIng12354SeCretKet@&258a";
            byte[] toArr = new byte[pass.Length + secrateKey.Length];
            toArr = Encoding.UTF8.GetBytes(pass + secrateKey);
            string cryptPass = Convert.ToBase64String(toArr);
            return cryptPass;
        }

        [HttpPost("login")]
        public ActionResult login(login userdata)
        {
            if (ModelState.IsValid)
            {
                user? user;
                string cryptPass = encryptPass(userdata.password);
                user = _context.users.FirstOrDefault( u => u.Email == userdata.Email && u.password == cryptPass);
                if (user == null)
                {
                    return Unauthorized("email or Password is incorrect");
                }
                else
                {
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("secretKey")));
                    var data = new List<Claim>();
                    data.Add(new Claim("id", user.id.ToString()));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(

                    claims: data,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials);

                    var tok = new JwtSecurityTokenHandler().WriteToken(token);
                    var json = new
                    {
                        theToken = tok,
                        userName = user.name,
                        userId = user.id.ToString(),
                        isFirstBooking = user.isFirstBooking
                    };
                    return Ok(json);
                }
            }
            else
            {
                return BadRequest("worng inputs, please check");
            }
        }
    }
}
