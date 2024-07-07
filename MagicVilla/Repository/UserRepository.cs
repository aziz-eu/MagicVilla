using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("JWT:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(u=> u.UserName == username);
            if (user == null) { return true; }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() &&
            u.Password == loginRequestDTO.Password);

            if(user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    LocalUser = null,
                };
            }

            // If User Found Genetate JWT token

            var tokenHendler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, user.Id.ToString()),
                     new Claim(ClaimTypes.Role, user.Role)
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new( new SymmetricSecurityKey (key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHendler.CreateToken(tokenDescriptor);

            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHendler.WriteToken(token),
                LocalUser = user,
            };
            return loginResponseDTO;
        }
            
        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = registrationRequestDTO.UserName,
                Name = registrationRequestDTO.Name,
                Password = registrationRequestDTO.Password,
                Role = registrationRequestDTO.Role,
            };
           await _db.LocalUsers.AddAsync(user);  
           await _db.SaveChangesAsync();
            user.Password = "";
            return user;


        }
    }
}
