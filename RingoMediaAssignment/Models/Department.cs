using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RingoMediaAssignment.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL for the Logo")]
        public string Logo { get; set; }

        [DisplayName("Parent Department")]
        public int? ParentDepartmentId { get; set; }
        [DisplayName("Parent Department")]
        public Department? ParentDepartment { get; set; }

        public ICollection<Department> SubDepartments { get; set; } = new List<Department>();

    }
}
