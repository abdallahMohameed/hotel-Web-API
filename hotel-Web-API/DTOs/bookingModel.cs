
using hotel_Web_API.models;

namespace hotel_Web_API.DTOs
{
    public class bookingModel
    {
        public int userId { get; set; }
        public int roomId { get; set; }
        public DateTime bookingFrom { get; set; }
        public DateTime bookingTo { get; set; }
        public int? DifferenceInDays { get; set; }

        public Room? room { get; set; }

    }
}
