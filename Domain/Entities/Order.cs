using System.ComponentModel.DataAnnotations;

namespace UserOrdersApiDemo.Domain
{
    public class Order : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        public int UserId { get; set;}
        public User User { get; set; }
    }
}