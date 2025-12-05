using Microsoft.AspNetCore.Mvc;
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

        public GameController(IUserService userService, IItemService itemService)
        {
            _userService = userService;
            _itemService = itemService;
        }

        [HttpPost("login")]
        public ActionResult<Result<string>> Login([FromBody] AuthRequest authRequest)
        {
            try
            {
                if(authRequest.Password == "" || authRequest.UserId == "")
                {
                    throw new GameException(ErrorCode.PARAM_ILLEGAL);
                }
                string userId = _userService.Login(authRequest.UserId, authRequest.Password);
                return Ok(Result<string>.Ok(userId));

            }
            catch(GameException e)
            {
                return BadRequest(Result<string>.Fail(e.Code));
            }
        }

        [HttpPost("register")]
        public ActionResult<Result<string>> Register([FromBody] AuthRequest authRequest)
        {
            try
            {
                if(authRequest.Password == "" || authRequest.UserId == "")
                {
                    throw new GameException(ErrorCode.PARAM_ILLEGAL);
                }
                string userId = _userService.Register(authRequest.UserId, authRequest.Password);
                return Ok(Result<string>.Ok(userId));

            }
            catch(GameException e)
            {
                return BadRequest(Result<string>.Fail(e.Code));
            }
        }

        [HttpPost("recordScore/{userId}")]
        public async Task<ActionResult<Result<bool>>> RecordScore([FromBody] RecordScoreRequest request, string userId)
        {
            try
            {
                bool newHighScore = await _userService.RecordNewScore(userId, request.Score);
                string message = newHighScore ? "New high score achieved!" : "";
                return Ok(Result<bool>.Ok(newHighScore, message));

            }
            catch(GameException e)
            {
                return BadRequest(Result<bool>.Fail(e.Code));
            }
        }

        [HttpGet("items/{userId}")]
        public ActionResult<Result<List<Item>>> ShowItem(string userId)
        {
            try
            {
                List<Item> items = _itemService.GetItems(userId);
                return Ok(Result<List<Item>>.Ok(items));
            }
            catch(GameException e)
            {
                return BadRequest(Result<List<Item>>.Fail(e.Code));
            }
        }

        [HttpPost("addItem/{userId}")]
        public ActionResult<Result<bool>> AddItem([FromBody] AddItemRequest request, string userId)
        {
            try
            {
                if(request.ItemId == "")
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

        [HttpGet("randomItem")]
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
    }
}