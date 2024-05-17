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
        public async Task<IActionResult> VerifyAnswer(ChallengeViewModel challengeViewModel)
        {
            if (challengeViewModel.UserAnswer.ToUpper().Equals(challengeViewModel.RightAnswer.ToUpper()))
            {
                var user = await _userManager.GetUserAsync(User);
                var coupons = _mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsByUserId(user.Id));

                if (ValidateChallenge(challengeViewModel, user.Id, coupons) == true && user != null)
                {
                    var coupon = GenerateCoupon(user, challengeViewModel);
                    challengeViewModel.Coupons = new List<CouponViewModel>();
                    challengeViewModel.Coupons.Add(coupon.Result);
                }
            }
            else
            {
                TempData["WrongAnswer"] = "Parece que a resposta não está correta! Tente novamente! " +
                    "Verifique se escreveu corretamente ou procure um sinônimo para esta planta.";
            };

            return View("Index", challengeViewModel);
        }

        public async Task<CouponViewModel> GenerateCoupon(ApplicationUser user, ChallengeViewModel challenge)
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

           await _couponRepository.Add(_mapper.Map<Coupon>(couponViewModel));

            return couponViewModel;
        }

        public bool ValidateChallenge(ChallengeViewModel challenge, string userId, IEnumerable<CouponViewModel> coupons)
        {

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
                if (coupon.AssociatedOrderId.ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    coupon.Used = true;
                }
                else
                {
                    if (coupon.ExpirationDate.Date < DateTime.Now.Date)
                    {
                        coupon.Expired = true;
                    }
                }

                if (coupon.Expired == true || coupon.Used == true)
                {
                   await _couponRepository.Update(_mapper.Map<Coupon>(coupon));
                }
            }

            return coupons;
        }
    }
}
