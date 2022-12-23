using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_Web_API.models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]

        public int roomId { get; set; }
        [Required]

        public string roomType { get; set; }
        [Required]

        public int PricePerDay { get; set; }

        public int branchId { get; set; }

        public branch? branch { get; set; }
    }
}
