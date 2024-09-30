using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;

        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string userName)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName == userName);
            if( user ==  null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = await _db.LocalUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequest.UserName.ToLower() && u.Password == loginRequest.Password);

            if( user == null)
            {
                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescription); 
            LoginResponseDTO loginResponse = new LoginResponseDTO()
            {
                User = user,
                Token = tokenHandler.WriteToken(token)
            };

            return loginResponse;

        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequest)
        {
            LocalUser newUser = new()
            {
                Name = registrationRequest.Name,
                Password = registrationRequest.Password,
                UserName = registrationRequest.UserName,
                Role = registrationRequest.Role,
            };

            await _db.LocalUsers.AddAsync(newUser);
            await _db.SaveChangesAsync();

            newUser.Password = "";
            return newUser;
        }
    }
}
