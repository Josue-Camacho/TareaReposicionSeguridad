using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TareaReposicionSecure.Models;
using TareaReposicionSecure.Models.DTOS;
using TareaReposicionSecure.Services;

namespace TareaReposicionSecure.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _service;

        public HospitalController(IHospitalService service)
        {
            _service = service;
        }

        // ============================
        // GET: /api/hospital
        // Público
        // ============================
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHospitals()
        {
            IEnumerable<Hospital> items = await _service.GetAll();
            return Ok(items);
        }

        // ============================
        // GET: /api/hospital/types/1,3
        // Público
        // ============================
        [HttpGet("types/{types}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHospitalsByTypes(string types)
        {
            var typeArray = types
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => int.TryParse(t.Trim(), out var v) ? v : (int?)null)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .Distinct()
                .ToArray();

            if (!typeArray.Any())
                return BadRequest("Invalid types format. Use comma-separated numbers (e.g., '1,3')");

            var hospitals = await _service.GetHospitalsByTypesAsync(typeArray);
            return Ok(hospitals);
        }

        // ============================
        // GET: /api/hospital/{id}
        // Protegido
        // ============================
        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOne(Guid id)
        {
            try
            {
                var hospital = await _service.GetOne(id);
                return Ok(hospital);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // ============================
        // POST: /api/hospital
        // Solo Admin
        // ============================
        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateHospital([FromBody] CreateHospitalDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var hospital = await _service.CreateHospital(dto);

            return CreatedAtAction(
                nameof(GetOne),
                new { id = hospital.Id },
                hospital
            );
        }

        // ============================
        // PUT: /api/hospital/{id}
        // Protegido
        // ============================
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateHospital(Guid id, [FromBody] UpdateHospitalDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var hospital = await _service.UpdateHospitalAsync(id, dto);
                return Ok(hospital);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // ============================
        // DELETE: /api/hospital/{id}
        // Solo Admin
        // ============================
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteHospital(Guid id)
        {
            try
            {
                await _service.DeleteHospitalAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
