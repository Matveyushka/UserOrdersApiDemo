using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserOrdersApiDemo.Domain
{
    public class User : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Range(0, 200)]
        public int Age { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
