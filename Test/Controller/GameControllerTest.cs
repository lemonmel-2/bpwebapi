using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Moq;
using webapi.Controller;
using webapi.Controller.DTO.Request;
using webapi.Controller.DTO.Result;
using webapi.Enum;
using webapi.Exception;
using webapi.Model;
using webapi.service;
using Xunit;
using Xunit.Sdk;

namespace webapi.Test.Controller
{
    public class GameControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IItemService> _itemServiceMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly GameController _controller;

        public GameControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _itemServiceMock = new Mock<IItemService>();
            _configMock = new Mock<IConfiguration>();
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("your-app");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("your-client");
            _configMock.Setup(c => c["Jwt:Key"]).Returns("your-very-secret-key-32-bytes-min");

            _controller = new GameController(_userServiceMock.Object, _itemServiceMock.Object, _configMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public void Login_Normal()
        {
            var request = new AuthRequest { UserId = "testuser", Password = "password" };
            _userServiceMock.Setup(s => s.Login("testuser", "password")).Returns("testuser");

            var result = _controller.Login(request);

            VerifySuccessResult<Token>(result);
        }

        [Fact]
        public void Login_InvalidInput()
        {
            var request = new AuthRequest { UserId = "", Password = "" };

            var result = _controller.Login(request);

            var tokenResult = VerifyBadResult<Token>(result);
            Assert.Equal(ErrorCode.PARAM_ILLEGAL, tokenResult.Code);
        }

        [Fact]
        public void Login_IncorrectPassword()
        {
            var request = new AuthRequest { UserId = "asdas", Password = "asdsad" };

            _userServiceMock.Setup(s => s.Login("asdas", "asdsad")).Throws(new GameException(ErrorCode.INCORRECT_PASSWORD));

            var result = _controller.Login(request);

            var tokenResult = VerifyBadResult<Token>(result);
            Assert.Equal(ErrorCode.INCORRECT_PASSWORD, tokenResult.Code);
        
        }

        [Fact]
        public void Register_Normal()
        {
            var request = new AuthRequest { UserId = "newuser", Password = "password" };
            _userServiceMock.Setup(s => s.Register("newuser", "password")).Returns("newuser");

            var result = _controller.Register(request);

            VerifySuccessResult<Token>(result);
        }

        [Fact]
        public void Register_InvalidInput()
        {
            var request = new AuthRequest { UserId = "", Password = ""};
            
            var result = _controller.Register(request);

            var tokenResult = VerifyBadResult<Token>(result);
            Assert.Equal(ErrorCode.PARAM_ILLEGAL, tokenResult.Code);
        }
        
        [Fact]
        public void Register_UserExist()
        {
            var request = new AuthRequest { UserId = "newuser", Password = "password" };

            _userServiceMock.Setup(s => s.Register(It.IsAny<string>(), It.IsAny<string>())).Throws(new GameException(ErrorCode.USER_ID_EXIST));

            var result = _controller.Register(request);

            var tokenResult = VerifyBadResult<Token>(result);
            Assert.Equal(ErrorCode.USER_ID_EXIST, tokenResult.Code);
        }

        [Fact]
        public void GetUser_Normal()
        {
            User user = new User("testuser");

            _userServiceMock.Setup(s => s.GetUser(It.IsAny<string>())).Returns(user);
            var result = _controller.GetUser();

            VerifySuccessResult<User>(result);

            _userServiceMock.Verify(s => s.GetUser("testuser"), Times.Once);
        }

        [Fact]
        public void GetUser_UserNotExist()
        {
            _userServiceMock.Setup(s => s.GetUser(It.IsAny<string>())).Throws(new GameException(ErrorCode.USER_NOT_EXIST));
            var result = _controller.GetUser();

            var userResult = VerifyBadResult<User>(result);
            Assert.Equal(ErrorCode.USER_NOT_EXIST, userResult.Code);
        }

        [Fact]
        public async Task RecordScore()
        {
            var request = new RecordScoreRequest { Score = 100 };
            _userServiceMock.Setup(s => s.RecordNewScore("testuser", 100)).ReturnsAsync(true);

            var result = await _controller.RecordScore(request);

            var scoreResult = VerifySuccessResult<bool>(result);
            
            Assert.True(scoreResult.Data);
        }

        [Fact]
        public void ShowItem()
        {
            var items = new List<Item> { new Item("items", "name") };
            _itemServiceMock.Setup(s => s.GetItems("testuser")).Returns(items);

            var result = _controller.ShowItem();

            var itemResult = VerifySuccessResult<List<Item>>(result);
            Assert.Single(itemResult.Data);
        }

        [Fact]
        public async Task AddItem_Normal()
        {
            _itemServiceMock.Setup(s => s.GenerateItem()).Returns(new Item("item1", "name"));
            var result = await _controller.AddItem();

            VerifySuccessResult<Item>(result);
            _itemServiceMock.Verify(s => s.AddItem("testuser", "item1"), Times.Once);

        }

        [Fact]
        public async Task AddItem_InsufficientPoints()
        {
            _itemServiceMock.Setup(s => s.GenerateItem()).Returns(new Item("item1", "name"));
            _userServiceMock.Setup(s => s.UpdateUserPoints(It.IsAny<string>(), It.IsAny<int>())).Throws(new GameException(ErrorCode.INSUFFICIENT_POINTS));

            var result = await _controller.AddItem();

            var badResult = VerifyBadResult<Item>(result);
            Assert.Equal(ErrorCode.INSUFFICIENT_POINTS, badResult.Code);

        }

        [Fact]
        public void ShowLeaderboard_ReturnsLeaderboard()
        {
            var users = new List<User> { new User("tet1") };
            _userServiceMock.Setup(s => s.GetLeaderboard()).Returns(users);

            var result = _controller.ShowLeaderboard();

            var leaderboardResult = VerifySuccessResult<List<User>>(result);
            Assert.Single(leaderboardResult.Data);
        }

        [Fact]
        public void GetUserId_SessionInvalid()
        {
            // Arrange
            var controller = new GameController(_userServiceMock.Object, _itemServiceMock.Object, _configMock.Object);
            var httpContext = new DefaultHttpContext();

            // Make the user unauthenticated and with no claims
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity()); // IsAuthenticated == false

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = controller.GetUser();

            var userResult = VerifyBadResult<User>(result);
            Assert.Equal(ErrorCode.SESSION_INVALID, userResult.Code);
        }

        private Result<T> VerifySuccessResult<T>(ActionResult<Result<T>> result)
        {
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var resultBody = Assert.IsType<Result<T>>(okResult.Value);
            Assert.True(resultBody.Success);
            Assert.NotNull(resultBody.Data);
            return resultBody;
        }

        private Result<T> VerifyBadResult<T>(ActionResult<Result<T>> result)
        {
            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var resultBody = Assert.IsType<Result<T>>(badResult.Value);
            Assert.False(resultBody.Success);
            return resultBody;
        }
    }
}
