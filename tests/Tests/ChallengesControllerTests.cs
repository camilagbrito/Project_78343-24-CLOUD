using System.Security.Claims;
using App.Controllers;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace App.Tests
{
    public class ChallengesControllerTests
    {
        private readonly Mock<IChallengeRepository> _mockChallengeRepository;
        private readonly Mock<ICouponRepository> _mockCouponRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly ChallengesController _controller;

        public ChallengesControllerTests()
        {
            _mockChallengeRepository = new Mock<IChallengeRepository>();
            _mockCouponRepository = new Mock<ICouponRepository>();
            _mockMapper = new Mock<IMapper>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new ChallengesController(
                _mockChallengeRepository.Object,
                _mockCouponRepository.Object,
                _mockMapper.Object,
                _mockUserManager.Object
            );

            // Configurar utilizador falso para testes
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithValidChallenge()
        {
            // Arrange
            var challenge = new Challenge { Id = Guid.NewGuid(), DiscountPercent = 10 };
            var challengeViewModel = new ChallengeViewModel { Id = challenge.Id, DiscountPercent = challenge.DiscountPercent };
            var user = new ApplicationUser { Id = "test-user-id" };
            var coupons = new List<Coupon>();

            _mockChallengeRepository.Setup(repo => repo.GetByDate()).ReturnsAsync(challenge);
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockCouponRepository.Setup(repo => repo.GetCouponsByUserId(user.Id)).ReturnsAsync(coupons);
            _mockMapper.Setup(m => m.Map<ChallengeViewModel>(challenge)).Returns(challengeViewModel);
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponViewModel>>(coupons)).Returns(new List<CouponViewModel>());

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ChallengeViewModel>(viewResult.ViewData.Model);
            Assert.Equal(challenge.Id, model.Id);
        }

        [Fact]
        public async Task GenerateCoupon_ReturnsViewResult_WithValidCoupon()
        {
            // Arrange
            var challengeViewModel = new ChallengeViewModel
            {
                Id = Guid.NewGuid(),
                UserAnswer = "Answer",
                RightAnswer = "Answer",
                DiscountPercent = 10
            };
            var user = new ApplicationUser { Id = "test-user-id" };
            var coupons = new List<Coupon>();

            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _mockCouponRepository.Setup(repo => repo.GetCouponsByUserId(user.Id)).ReturnsAsync(coupons);
            _mockMapper.Setup(m => m.Map<IEnumerable<CouponViewModel>>(coupons)).Returns(new List<CouponViewModel>());

            // Act
            var result = await _controller.GenerateCoupon(challengeViewModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ChallengeViewModel>(viewResult.ViewData.Model);
            Assert.Equal(challengeViewModel.Id, model.Id);
            Assert.Single(model.Coupons);
        }

        [Fact]
        public void ValidateChallenge_ReturnsFalse_IfChallengeIsNull()
        {
            // Act
            var result = _controller.ValidateChallenge(null, "test-user-id", new List<CouponViewModel>());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateChallenge_ReturnsTrue_IfValid()
        {
            // Arrange
            var challengeViewModel = new ChallengeViewModel { Id = Guid.NewGuid() };
            var coupons = new List<CouponViewModel>();

            // Act
            var result = _controller.ValidateChallenge(challengeViewModel, "test-user-id", coupons);

            // Assert
            Assert.True(result);
        }
    }
}
