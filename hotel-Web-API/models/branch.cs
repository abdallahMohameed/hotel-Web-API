using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel_Web_API.models
{
    public class branch
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int branchId { get; set; }
        [Required]
        public string locationName { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public branch()
        {
            Rooms = new HashSet<Room>();
        }
    }
}
