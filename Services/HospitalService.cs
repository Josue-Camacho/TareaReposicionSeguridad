
using TareaReposicionSecure.Models;
using TareaReposicionSecure.Models.DTOS;
using TareaReposicionSecure.Repositories;
using TareaReposicionSecure.Services;

namespace TareaReposicionSecure.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _repo;
        public HospitalService(IHospitalRepository repo)
        {
            _repo = repo;
        }

        public async Task<Hospital> CreateHospital(CreateHospitalDto dto)
        {
            var hospital = new Hospital
            {
                Id = dto.Id,
                Name = dto.Name,
                Address = dto.Address,
                Type = dto.Type
            };
            await _repo.Add(hospital);
            return hospital;
        }

        public async Task<IEnumerable<Hospital>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Hospital> GetOne(Guid id)
        {
            var hospital = await _repo.GetOne(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital with ID {id} not found");
            return hospital;
        }

        public async Task<Hospital> UpdateHospitalAsync(Guid id, UpdateHospitalDto dto)
        {
            var hospital = await _repo.GetOne(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital with ID {id} not found");

            hospital.Name = dto.Name;
            hospital.Address = dto.Address;
            hospital.Type = dto.Type;

            await _repo.Update(hospital);
            return hospital;
        }

        public async Task DeleteHospitalAsync(Guid id)
        {
            var hospital = await _repo.GetOne(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital with ID {id} not found");

            await _repo.Delete(id);
        }

        public async Task<IEnumerable<Hospital>> GetHospitalsByTypesAsync(int[] types)
        {
            return await _repo.GetByTypesAsync(types);
        }
    }
}