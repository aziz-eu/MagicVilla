using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class VillaAPIController : ControllerBase
    {
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _villaDb;
        private readonly IMapper _mapper;
        protected readonly APIResponse _response;
     
        public VillaAPIController(IVillaRepository villaRepository, IMapper mapper)
        {
            _villaDb = villaRepository;
            _mapper = mapper;
            this._response = new();
          
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villas = await _villaDb.GetAll();
                _response.Result = _mapper.Map<List<VillaDto>>(villas);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString()};
            }
            return _response;
        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await _villaDb.Get(u => u.Id == id);

                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = villa;
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex) {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
           

            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (createDto == null)
                {

                    return BadRequest();
                }

                Villa villa = _mapper.Map<Villa>(createDto);

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

                await _villaDb.Create(villa);
                _response.Result = _mapper.Map<VillaDto>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);

            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
           
        }

        [HttpDelete("id")]

        public async Task <ActionResult<APIResponse>> Delete(int id)
        {

            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }

                var villa = await _villaDb.Get(u => u.Id == id);

                if (villa == null)
                {
                    return NotFound();
                }
                await _villaDb.Remove(villa);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("id")]
        public async Task <ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    return BadRequest();
                }
                Villa model = _mapper.Map<Villa>(updateDto);
                await _villaDb.Update(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
              

                    return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
    }

}