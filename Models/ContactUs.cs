using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FirstCoreApp.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [StringLength(15)]
        [DisplayName("your Name")]

        public string Name { get; set; }    
        public string Message { get; set; }   
        public string Email { get; set; }
        public string Subject { get; set; }
    }
}
