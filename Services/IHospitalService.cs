using TareaReposicionSecure.Models;
using TareaReposicionSecure.Models.DTOS;

namespace TareaReposicionSecure.Services
{
    public interface IHospitalService
    {
        Task<IEnumerable<Hospital>> GetAll();
        Task<Hospital> GetOne(Guid id);
        Task<Hospital> CreateHospital(CreateHospitalDto dto);
        Task<Hospital> UpdateHospitalAsync(Guid id, UpdateHospitalDto dto);
        Task DeleteHospitalAsync(Guid id);
        Task<IEnumerable<Hospital>> GetHospitalsByTypesAsync(int[] types);
    }
}
