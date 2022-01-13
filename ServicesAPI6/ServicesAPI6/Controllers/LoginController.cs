using DAL.Model;
using EFLibCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ServicesAPI6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<Token> Login([FromForm] UserLogin userLogin)
        {
            Login user = new Login { Password = "123", Email = "xx@gmail.com" };
            if (user != null)
            {
                //Token üretiliyor.
                TokenHandler tokenHandler = new TokenHandler();
                Token token = tokenHandler.CreateAccessToken(user);

                //Refresh token Users tablosuna iþleniyor.
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);

                return token;
            }
            return null;
        }

    }
}