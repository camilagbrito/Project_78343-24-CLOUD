using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace App.Controllers
{
    [Route("challenge")]
    public class ChallengesController : Controller
    {

        private readonly IChallengeRepository _challengeRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChallengesController(IChallengeRepository challengeRepository, ICouponRepository couponRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _challengeRepository = challengeRepository;
            _couponRepository = couponRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var challengeViewModel = _mapper.Map<ChallengeViewModel>(await _challengeRepository.GetByDate());
            var user = await _userManager.GetUserAsync(User);
            var coupons = _mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsByUserId(user.Id));

            if (coupons.Any(x => x.Expired != false && x.Used == false))
            {
                TempData["ActivedCoupon"] = "Você já possui um cupão ativo. Os cupões não são cumulativos. " +
                    "Poderá participar novamente após usar o cupão ou este expirar!";
                return View();
            }

            if(coupons.Any(x => x.ChallengeId == challengeViewModel.Id))
            {
                TempData["AlreadyParticipated"] = "Você já recebeu um cupão por este desafio.";
                return View();
            }

            if (challengeViewModel == null)
            {
                TempData["Message"] = "Ainda não temos desafio para hoje.";
                return View();
            }


            return View(challengeViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> VerifyAnswer(ChallengeViewModel challengeViewModel)
        {
            if (challengeViewModel.UserAnswer.ToUpper().Equals(challengeViewModel.RightAnswer.ToUpper()))
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var coupon = GenerateCoupon(user, challengeViewModel);
                    challengeViewModel.Coupons = new List<CouponViewModel>();
                    challengeViewModel.Coupons.Add(coupon);     
                }

              
            };
            return View("Index", challengeViewModel);
        }

        private CouponViewModel GenerateCoupon(ApplicationUser user, ChallengeViewModel challenge)
        {

            var couponViewModel = new CouponViewModel
            {
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1),
                Discount = 10,
                UserId = user.Id,
                ChallengeId = challenge.Id
            };

            _couponRepository.Add(_mapper.Map<Coupon>(couponViewModel));

            return couponViewModel; 
        }
    }
}
