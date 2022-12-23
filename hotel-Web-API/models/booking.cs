using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_Web_API.models
{
    public class booking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int bookingId { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public int totalPrice { get; set; }
        public user user { get; set; }
    }
}
