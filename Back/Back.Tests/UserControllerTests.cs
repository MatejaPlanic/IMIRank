using Back.Controllers;
using Back.DTO.Profile;
using Back.Models.User;
using Back.Models.Review;
using Back.Models.Enums;
using Back.Repositories.Review;
using Back.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Back.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IReviewRepository> _reviewRepoMock;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _reviewRepoMock = new Mock<IReviewRepository>();
            _controller = new UserController(_userRepoMock.Object, _reviewRepoMock.Object);
        }

        [Fact]
        public async Task Search_ReturnsOkResult_WithUsers()
        {
            var users = new List<User>
            {
                new User { Id = "1", UserName = "user1", Role = UserRole.RegularUser, ProfilePicturePath = "path1" },
                new User { Id = "2", UserName = "user2", Role = UserRole.Admin, ProfilePicturePath = null }
            };
            var reviews1 = new List<Review> { new Review() };
            var reviews2 = new List<Review>();

            _userRepoMock.Setup(repo => repo.SearchByUserNameAsync("test", 1, 20)).ReturnsAsync(users);
            _userRepoMock.Setup(repo => repo.CountByUserNameAsync("test")).ReturnsAsync(2);
            _reviewRepoMock.Setup(repo => repo.GetByUserIdAsync("1")).Returns(Task.FromResult(reviews1));
            _reviewRepoMock.Setup(repo => repo.GetByUserIdAsync("2")).Returns(Task.FromResult(reviews2));

            var result = await _controller.Search("test", 1, 20);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenUserNotFound()
        {
            _userRepoMock.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync((User)null);

            var result = await _controller.Get("1");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsOkResult_WithUser()
        {
            var user = new User { Id = "1", UserName = "user1", Role = UserRole.RegularUser };
            var reviews = new List<Review> { new Review() };

            _userRepoMock.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync(user);
            _reviewRepoMock.Setup(repo => repo.GetByUserIdAsync("1")).Returns(Task.FromResult(reviews));

            var result = await _controller.Get("1");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }
    }
}