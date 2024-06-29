using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ILogging _logger;
        public VillaAPIController(ILogging logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.Log("Getting All Villa", "");
            return Ok(VillaStore.villaDto);
        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            _logger.Log("Getting Villa of Id"+id, "error");

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaDto.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villa)
        {

            

            if (villa == null)
            {

                return BadRequest();
            }
            if (villa.Id < 0)
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            VillaStore.villaDto.Add(villa);
            return CreatedAtRoute("GetVilla", new { Id = villa.Id }, villa);
        }

        [HttpDelete("id")]

        public IActionResult Delete(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaDto.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            VillaStore.villaDto.Remove(villa);
            return NoContent();
        }

        [HttpPut("id")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaDto.FirstOrDefault(u=>u.Id == id);
            villa.Name = villaDto.Name;
            villa.Id = villaDto.Id;

            return NoContent();
        }
    }

}