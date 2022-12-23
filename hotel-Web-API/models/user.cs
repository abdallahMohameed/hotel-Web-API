using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_Web_API.models
{
    public class user
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int? id { get; set; }
        public string? name { get; set; }
        [Required]

        public string? password { get; set; }

        public int? phoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        public int? userType { get; set; }

        public bool? isFirstBooking { get; set; }

        public user()
        {
            bookings=new HashSet<booking>();
        }

        public ICollection<booking>? bookings { get; set; }
    }
}
