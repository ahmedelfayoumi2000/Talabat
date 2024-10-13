using System.ComponentModel.DataAnnotations;

namespace Talabat.API.Dtos
{
    public class AddressDto
    {
        [Required]
        public string FristName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Street { get; set; }
    }
}
