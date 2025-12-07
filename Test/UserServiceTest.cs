using Moq;
using Xunit;
using webapi.Repository;
using webapi.service.impl;
using webapi.Model;
using webapi.Enum;
using webapi.Exception;
using Microsoft.OpenApi.Any;

namespace webapi.Test
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepo> _userRepoMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _userRepoMock = new Mock<IUserRepo>();
            _userService = new UserService(_userRepoMock.Object);
        }

        [Fact]
        public async Task GetUser_UserExist()
        {
            // Arrange
            string userId = "testUser";
    
            SetupUser(userId);

            // Act
            User user = _userService.GetUser(userId);

            // Assert
            Assert.NotNull(user);
        }

        [Fact]
        public async Task GetUser_UserNotExist()
        {
            string userId = "testUser";
            SetupNotExistUser(userId);
            var ex = Assert.Throws<GameException>(() => _userService.GetUser(userId));
            Assert.Equal(ErrorCode.USER_NOT_EXIST, ex.Code);
        }

        [Fact]
        public async Task GetUserCredential_UserExist()
        {
            string userId = "testUser";
            SetupUserCredential(userId);
            UserCredential userCredential = _userService.GetUserCredential(userId);
            Assert.NotNull(userCredential);
        }

        [Fact]
        public async Task GetUserCredential_UserNotExist()
        {
            string userId = "testUser";
            _userRepoMock.Setup(repo => repo.GetUserCredential(userId)).Returns(() => null);
            var ex = Assert.Throws<GameException>(() => _userService.GetUserCredential(userId));
            Assert.Equal(ErrorCode.USER_NOT_EXIST, ex.Code);
        }

        [Fact]
        public async Task RecordNewScore_NewHighestScore()
        {
            string userId = "testUser";
            User user = new User(userId) { HighestScore = 50 };
            _userRepoMock.Setup(repo => repo.GetUser(userId)).Returns(user);

            bool isNewHighScore = await _userService.RecordNewScore(userId, 100);
            _userRepoMock.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.Once);
            Assert.True(isNewHighScore);
        }

        [Fact]
        public async Task RecordNewScore_LowScore()
        {
            string userId = "testUser";
            User user = new User(userId) { HighestScore = 50 };
            _userRepoMock.Setup(repo => repo.GetUser(userId)).Returns(user);

            bool isNewHighScore = await _userService.RecordNewScore(userId, 40);
            _userRepoMock.Verify(repo => repo.UpdateUser(It.IsAny<User>()), Times.Never);
            Assert.False(isNewHighScore);
        }

        [Fact]
        public async Task Login_Normal()
        {
            string userId = "user1";
            SetupUserCredential(userId);
            string loggedInUserId = _userService.Login(userId, "hello");
            Assert.Equal(userId, loggedInUserId);
        }

        [Fact]
        public async Task Login_IncorrectPassword()
        {
            string userId = "user1";
            SetupUserCredential(userId);
            var ex = Assert.Throws<GameException>(() => _userService.Login(userId, "hello123"));
            Assert.Equal(ErrorCode.INCORRECT_PASSWORD, ex.Code);
        }

        [Fact]
        public async Task Register_Normal()
        {
            string userId = "user1";
            SetupNotExistUser(userId);
            string registeredUserId = _userService.Register(userId, "hello");
            _userRepoMock.Verify(repo => repo.AddUser(It.IsAny<UserCredential>()), Times.Once);
        }

        [Fact]
        public async Task Register_UserExist()
        {
            string userId = "user1";
            SetupUser(userId);
            var ex = Assert.Throws<GameException>(() => _userService.Register(userId, "hello123"));
            Assert.Equal(ErrorCode.USER_ID_EXIST, ex.Code);
            _userRepoMock.Verify(repo => repo.AddUser(It.IsAny<UserCredential>()), Times.Never);
        }

        private void SetupUserCredential(string userId)
        {
            UserCredential user =  new UserCredential(userId, "k8ZgdzoSpfZ4SJGnD7G8AG4nPokHABjSCDt8D9i3IBE=","Za8aZYFdI1d2JvVDTWZIsw==");
            _userRepoMock.Setup(repo => repo.GetUserCredential(userId)).Returns(user);
        }

        private void SetupUser(string userId)
        {
            User user =  new User(userId);
            _userRepoMock.Setup(repo => repo.GetUser(userId)).Returns(user);
        }

        private void SetupNotExistUser(string userId)
        {
            _userRepoMock.Setup(repo => repo.GetUser(userId)).Returns(() => null);
        }
    }
}