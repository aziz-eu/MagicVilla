using AutoMapper;
using MagicVilla.Data;
using MagicVilla.Logging;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;

namespace MagicVilla.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class VillaNumberAPIController : ControllerBase
    {
        //private readonly ApplicationDbContext _db;
        private readonly IVillaNumberRepository _villaNumberDb;
        private readonly IVillaRepository _villaDb;
        private readonly IMapper _mapper;
        protected readonly APIResponse _response;
     
        public VillaNumberAPIController(IVillaNumberRepository villaNumberDb, IMapper mapper, IVillaRepository villaDb)
        {
            _villaNumberDb = villaNumberDb;
            _villaDb = villaDb;
            _mapper = mapper;
            this._response = new();
          
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villas = await _villaNumberDb.GetAll();
                _response.Result = _mapper.Map<List<VillaNumberDto>>(villas);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString()};
            }
            return _response;
        }

        [HttpGet("id", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var villa = await _villaNumberDb.Get(u => u.VillaNo == id);

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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto createVillaNumberDto)
        {
            try
            {
                if (createVillaNumberDto == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if(await _villaDb.Get(u=> u.Id == createVillaNumberDto.VillaId) == null)
                {
                    ModelState.AddModelError("customeError", "Invalid VillaId");
                    return BadRequest(ModelState);
                }

                if(await _villaNumberDb.Get(u=> u.VillaNo == createVillaNumberDto.VillaNo) != null)
                {
                    ModelState.AddModelError("customeError", "Villa Already Exits!");
                    return BadRequest(ModelState);
                }

                if (await _villaNumberDb.Get(u => u.VillaNo == createVillaNumberDto.VillaNo) !=null){

                    ModelState.AddModelError("CustomError", "Villa Already Exits");
                    //_response.IsSuccess =false;
                    return BadRequest(ModelState);
                
                }

                VillaNumber villaNumber = _mapper.Map<VillaNumber>(createVillaNumberDto);

                await _villaNumberDb.Create(villaNumber);
                _response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);

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

                var villaNumber = await _villaNumberDb.Get(u => u.VillaNo == id);

                if (villaNumber == null)
                {
                    return NotFound();
                }
                await _villaNumberDb.Remove(villaNumber);
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
        public async Task <ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    return BadRequest();
                }
                VillaNumber villaNumber = _mapper.Map<VillaNumber>(updateDto);
                await _villaNumberDb.UpdateAsync(villaNumber);
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