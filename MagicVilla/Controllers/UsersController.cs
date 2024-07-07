using AutoMapper;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userDb;
        private readonly IMapper _mapper;
        protected readonly APIResponse _response;

        public UsersController(IUserRepository userDb, IMapper mapper)
        {
            _userDb = userDb;
            _mapper = mapper;
            this._response = new();

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var loginRespone = await _userDb.Login(loginRequestDTO);
                if (loginRespone.LocalUser == null ||
                    string.IsNullOrEmpty(loginRespone.Token))
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessages.Add("Invalid User Name Or Password");
                    return BadRequest(_response);
                }

                _response.StatusCode = System.Net.HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = loginRespone;
                return Ok(_response);
            }
            catch (Exception ex) {

                throw;
            }

        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration ([FromBody] RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                bool isUserNameUnique = _userDb.IsUniqueUser(registrationRequestDTO.UserName);

                if (!isUserNameUnique)
                {
                    _response.IsSuccess = false;    
                    return BadRequest(_response);
                }

                var user = _userDb.Register(registrationRequestDTO);

                if (user == null) {
                    _response.IsSuccess = false;
                    return BadRequest(_response);
                }
                _response.IsSuccess = true;
                _response.StatusCode =System.Net.HttpStatusCode.OK;

                return Ok(_response);


            }
            catch (Exception ex) {

                throw;
            }

        }
    }
}
