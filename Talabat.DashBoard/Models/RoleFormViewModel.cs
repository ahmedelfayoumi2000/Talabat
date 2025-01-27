using System.ComponentModel.DataAnnotations;

namespace Talabat.DashBoard.Models
{
    public class RoleFormViewModel
    {
        [Required(ErrorMessage ="Nmae Is Required")]
        [StringLength(256)]
        public string Name { get; set; }
    }
}
