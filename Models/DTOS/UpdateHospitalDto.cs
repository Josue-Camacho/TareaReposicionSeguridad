using System.ComponentModel.DataAnnotations;

namespace TareaReposicionSecure.Models.DTOS
{
    public class UpdateHospitalDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Range(1, 3)]
        public int Type { get; set; }
    }
}
