using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using webapi.Controller.DTO.Request;
using webapi.Controller.DTO.Result;
using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.service;

namespace webapi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static IUserService _userService;
        private static IItemService _itemService;
        private static IConfiguration _config;
        private static JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

        public GameController(IUserService userService, IItemService itemService, IConfiguration config)
        {
            _userService = userService;
            _itemService = itemService;
            _config = config;
        }

        [HttpPost("login")]
        public ActionResult<Result<string>> Login([FromBody] AuthRequest authRequest)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(authRequest.Password) || string.IsNullOrWhiteSpace(authRequest.UserId))
                {
                    throw new GameException(ErrorCode.PARAM_ILLEGAL);
                }
                string userId = _userService.Login(authRequest.UserId, authRequest.Password);
                
                Token accessToken = RequestToken(authRequest.UserId);

                return Ok(Result<Token>.Ok(accessToken));

            }
            catch(GameException e)
            {
                return BadRequest(Result<Token>.Fail(e.Code));
            }
        }

        [HttpPost("register")]
        public ActionResult<Result<Token>> Register([FromBody] AuthRequest authRequest)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(authRequest.Password) || string.IsNullOrWhiteSpace(authRequest.UserId))
                {
                    throw new GameException(ErrorCode.PARAM_ILLEGAL);
                }
                string userId = _userService.Register(authRequest.UserId, authRequest.Password);
                
                Token accessToken = RequestToken(authRequest.UserId);

                return Ok(Result<Token>.Ok(accessToken));
            }
            catch(GameException e)
            {
                return BadRequest(Result<Token>.Fail(e.Code));
            }
        }

        [HttpPost("score")]
        public async Task<ActionResult<Result<bool>>> RecordScore([FromBody] RecordScoreRequest request)
        {
            try
            {
                var userId = GetUserId();
                bool newHighScore = await _userService.RecordNewScore(userId, request.Score);
                string message = newHighScore ? "New high score achieved!" : "";
                return Ok(Result<bool>.Ok(newHighScore, message));

            }
            catch(GameException e)
            {
                return BadRequest(Result<bool>.Fail(e.Code));
            }
        }

        [HttpGet("items")]
        public ActionResult<Result<List<Item>>> ShowItem()
        {
            try
            {
                var userId = GetUserId();
                List<Item> items = _itemService.GetItems(userId);
                return Ok(Result<List<Item>>.Ok(items));
            }
            catch(GameException e)
            {
                return BadRequest(Result<List<Item>>.Fail(e.Code));
            }
        }

        [HttpPost("item-add")]
        public ActionResult<Result<bool>> AddItem([FromBody] AddItemRequest request)
        {
            try
            {
                var userId = GetUserId();
                if(string.IsNullOrWhiteSpace(request.ItemId))
                {
                    throw new GameException(ErrorCode.PARAM_ILLEGAL);
                }
                _itemService.AddItem(userId, request.ItemId);
                return Ok(Result<bool>.Ok(true));
            }
            catch(GameException e)
            {
                return BadRequest(Result<List<Item>>.Fail(e.Code));
            }
        }

        [AllowAnonymous]
        [HttpGet("item-random")]
        public ActionResult<Result<Item>> GenerateItem()
        {
            try
            {
                Item item = _itemService.GenerateItem();
                return Ok(Result<Item>.Ok(item));
            }
            catch(GameException e)
            {
                return BadRequest(Result<Item>.Fail(e.Code));
            }
        }

        [AllowAnonymous]
        [HttpGet("leaderboard")]
        public ActionResult<Result<List<User>>> ShowLeaderboard()
        {
            try
            {
                List<User> result = _userService.GetLeaderboard();
                return Ok(Result<List<User>>.Ok(result));

            }
            catch(GameException e)
            {
                return BadRequest(Result<List<User>>.Fail(e.Code));
            }
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                throw new GameException(ErrorCode.SESSION_INVALID);
            }
            return userId;
        }

        private Token RequestToken(string userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId)
            };

            var issuer   = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key      = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds    = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var accessToken = jwtHandler.WriteToken(token);
            
            return new Token(accessToken, "bearer", 3600);

        }
    }
}