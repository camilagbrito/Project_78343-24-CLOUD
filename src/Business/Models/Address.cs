
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Models
{
    public class Address:Entity
    {
 
        public string Street{ get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [ForeignKey(nameof(Address.User))]
        public string ApplicationId { get; set; }
        public ApplicationUser User { get; set; }  

    }
}
