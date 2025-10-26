using System.ComponentModel.DataAnnotations;

namespace TareaReposicionSecure.Models.DTOS
{
    public record CreateHospitalDto
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public int Type { get; set; }
    }
}
