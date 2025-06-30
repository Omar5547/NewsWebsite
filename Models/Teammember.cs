using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCoreApp.Models
{
    public class Teammember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }

        // إزالة التحقق
        public string Image { get; set; }

        [NotMapped]
        public IFormFile imageFile { get; set; }
    }


}
