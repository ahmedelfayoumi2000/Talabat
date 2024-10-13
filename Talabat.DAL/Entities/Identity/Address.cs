    using System.ComponentModel.DataAnnotations;
    using System.Reflection.PortableExecutable;

    namespace Talabat.DAL.Entities.Identity
    {
        public class Address
        {
            public int ID { get; set; }
            public string FristName { get; set; }
            public string LastName { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            [Required]
            public string AppUserId { get; set; } //Forienkey
            public AppUser User { get; set; }

        }
    }