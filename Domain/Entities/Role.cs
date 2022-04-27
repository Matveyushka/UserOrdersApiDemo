using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserOrdersApiDemo.Domain
{
    public class Role : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}