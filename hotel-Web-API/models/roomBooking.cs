using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_Web_API.models
{
    public class roomBooking
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int RoombookingId { get; set; }

        public int bookingId { get; set; }
        public int roomId { get; set; }
        public DateTime bookingFrom { get; set; }
        public DateTime bookingTo { get; set; }
        [ForeignKey("bookingId")]

        public booking booking { get; set; }
        [ForeignKey("roomId")]
        public Room Room { get; set; }

    }
}
