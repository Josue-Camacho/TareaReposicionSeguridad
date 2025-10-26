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

        [HttpGet]
        public async Task<IActionResult> GetAllHospitals()
        {
            IEnumerable<Hospital> items = await _service.GetAll();
            return Ok(items);
        }

        // NUEVO: Endpoint público para tipos 1 y 3
        [HttpGet("types/{types}")]
        public async Task<IActionResult> GetHospitalsByTypes(string types)
        {
            try
            {
                // Convertir string "1,3" a array [1, 3]
                var typeArray = types.Split(',')
                    .Select(t => int.Parse(t.Trim()))
                    .ToArray();

                var hospitals = await _service.GetHospitalsByTypesAsync(typeArray);
                return Ok(hospitals);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid types format. Use comma-separated numbers (e.g., '1,3')");
            }
        }

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

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateHospital([FromBody] CreateHospitalDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var hospital = await _service.CreateHospital(dto);
            return CreatedAtAction(nameof(GetOne), new { id = hospital.Id }, hospital);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateHospital(Guid id, [FromBody] UpdateHospitalDto dto)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

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