using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingoMediaAssignment.Models
{
    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [DisplayName("Reminder DateTime")]
        [Required(ErrorMessage = "Reminder DateTime is required")]
        public DateTime ReminderDateTime { get; set; }
    }
}
