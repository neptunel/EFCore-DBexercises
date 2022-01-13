using DAL.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ServicesAPI6.Controllers
{
    public class TokenHandler
    {
        public Token CreateAccessToken(Login user)
        {
            Token tokenInstance = new Token();

            //Security  Key'in simetriğini alıyoruz.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TEST"));

            //Şifrelenmiş kimliği oluşturuyoruz.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Oluşturulacak token ayarlarını veriyoruz.
            tokenInstance.Expiration = DateTime.Now.AddMinutes(5);
            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: "www.myapi.com", //Oluşturulacak token değerini kimin dağıttığını ifade edeceğimiz alandır
                audience: "www.cc.com", //Oluşturulacak token değerini kimlerin / hangi originlerin / sitelerin kullanacağını belirlediğimiz alandır
                expires: tokenInstance.Expiration,//Token süresini 5 dk
                notBefore: DateTime.Now,//Token üretildikten ne kadar süre sonra devreye girsin ayarlıyouz.
                signingCredentials: signingCredentials
                );

            //Token oluşturucu sınıfında bir örnek alıyoruz.
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Token üretiyoruz.
            tokenInstance.AccessToken = tokenHandler.WriteToken(securityToken);

            //Refresh Token üretiyoruz.
            tokenInstance.RefreshToken = CreateRefreshToken();
            return tokenInstance;
        }

        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}
