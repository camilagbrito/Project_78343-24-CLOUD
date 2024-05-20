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
        [Route("challenge")]
        public async Task<IActionResult> Index()
        {
            var challengeViewModel = _mapper.Map<ChallengeViewModel>(await _challengeRepository.GetByDate());
            var user = await _userManager.GetUserAsync(User);
            var coupons = _mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsByUserId(user.Id));

            if (ValidateChallenge(challengeViewModel, user.Id, coupons) == true && user != null)
            {
                return View(challengeViewModel);
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
        public async Task<IActionResult> GenerateCoupon(ChallengeViewModel challengeViewModel)
        {
            
            var user = await _userManager.GetUserAsync(User);
            var coupons = _mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsByUserId(user.Id));
            var validate = ValidateChallenge(challengeViewModel, user.Id, coupons);

            if (challengeViewModel.UserAnswer.ToUpper().Equals(challengeViewModel.RightAnswer.ToUpper()) &&
                validate == true && user != null)
            {
                var couponViewModel = new CouponViewModel
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    DiscountPercent = challengeViewModel.DiscountPercent,
                    UserId = user.Id,
                    ChallengeId = challengeViewModel.Id
                };

                await _couponRepository.Add(_mapper.Map<Coupon>(couponViewModel));
             
                challengeViewModel.Coupons = new List<CouponViewModel> { couponViewModel };

                return View("Index", challengeViewModel);
            }
            else
            {
                TempData["WrongAnswer"] = "Parece que a resposta não está correta! Tente novamente! " +
                    "Verifique se escreveu corretamente ou procure um sinônimo para esta planta.";
                return View("Index", challengeViewModel);
            }
        }

        public bool ValidateChallenge(ChallengeViewModel challenge, string userId, IEnumerable<CouponViewModel> coupons)
        {
            if (challenge == null)
            {
                return false;
            }

            coupons = ValidateUse(coupons).Result;

            if (coupons.Any(x => x.Expired == false && x.Used == false))
            {
                TempData["ActivedCoupon"] = "Você já possui um cupão ativo. Os cupões não são cumulativos. " +
                    "Poderá participar novamente após usar o cupão ou este expirar!";
                return false;
            }

            if (coupons.Any(x => x.ChallengeId == challenge.Id))
            {
                TempData["AlreadyParticipated"] = "Você já recebeu um cupão por este desafio.";
                return false;
            }

            return true;
        }

        private async Task<IEnumerable<CouponViewModel>> ValidateUse(IEnumerable<CouponViewModel> coupons)
        {
            foreach (var coupon in coupons)
            {

                if (coupon.ExpirationDate.Date < DateTime.Now.Date)
                {
                    coupon.Expired = true;
                }


                if (coupon.Expired == true)
                {
                    await _couponRepository.Update(_mapper.Map<Coupon>(coupon));
                }
            }

            return coupons;
        }
    }
}
