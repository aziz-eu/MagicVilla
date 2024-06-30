using AutoMapper;
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
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
     
        public VillaAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
          
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            IEnumerable<Villa> villas = _db.Villas.ToList();

            
            return Ok(_mapper.Map<List<VillaDto>>(villas));
        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
           

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

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
        public ActionResult<VillaDto> CreateVilla([FromBody] VillaCreateDto createDto)
        {

            

            if (createDto == null)
            {

                return BadRequest();
            }

            Villa model = _mapper.Map<Villa>(createDto);

            //Villa model = new()
            //{
            //    Amenity = villaDto.Amenity,
            //    Details = villaDto.Details,
            //    Rate = villaDto.Rate,
            //    Name = villaDto.Name,
            //    Sqft = villaDto.Sqft,
            //    Occupancy = villaDto.Occupancy,
            //    ImageUrl = villaDto.ImageUrl

            //};

            _db.Villas.Add(model);
            _db.SaveChanges();
         
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("id")]

        public IActionResult Delete(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            _db.Villas.Remove(villa);
            return NoContent();
        }

        [HttpPut("id")]
        public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }
            Villa model = _mapper.Map<Villa>(updateDto);

            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }
    }

}