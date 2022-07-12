using CoreApi1.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreApi1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto logindto)
        {
            //1.验证用户账号密码是否正确，暂时忽略，因为我们是模拟登录

            //2.生成JWT
            //Header,选择签名算法
            var signingAlogorithm = SecurityAlgorithms.HmacSha256;
            //Payload,存放用户信息，下面我们放了一个用户id
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"user_id")
            };
            //Signature
            //取出私钥并以utf8编码字节输出
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            //使用非对称算法对私钥进行加密
            var signingKey = new SymmetricSecurityKey(secretByte);
            //使用HmacSha256来验证加密后的私钥生成数字签名
            var signingCredentials = new SigningCredentials(signingKey, signingAlogorithm);
            //生成Token
            var Token = new JwtSecurityToken(
                    issuer: _configuration["Authentication:Issuer"],        //发布者
                    audience: _configuration["Authentication:Audience"],    //接收者
                    claims: claims,                                         //存放的用户信息
                    notBefore: DateTime.UtcNow,                             //发布时间
                    expires: DateTime.UtcNow.AddDays(1),                      //有效期设置为1天
                    signingCredentials                                      //数字签名
                );
            //生成字符串token
            var TokenStr = new JwtSecurityTokenHandler().WriteToken(Token);
            return Ok(TokenStr);
        }
        [HttpGet("test")]
        [Authorize]
        public IActionResult test()
        {
            return Ok("test");
        }

    }
}
