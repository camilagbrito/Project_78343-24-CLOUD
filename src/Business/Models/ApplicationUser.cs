using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace Business.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public IEnumerable<Address> Addresses { get; set; } = new List<Address>();
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}

