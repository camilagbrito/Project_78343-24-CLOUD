using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace App.Controllers
{
    [Authorize]
    [Route("coupon/")]
    public class CouponsController : Controller
    {

        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;


        public CouponsController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [Route("my-coupons")]
        public async Task<IActionResult> UserCoupons()
        {
            if (User.Identity.IsAuthenticated)
            {

                var couponsViewModel = new List<CouponViewModel>();

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var coupons = _mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsByUserId(userId));

                foreach (var coupon in coupons)
                {

                    var couponViewModel = ValidateExpiration(coupon);

                    if (couponViewModel.Expired == true)
                    {
                        _couponRepository.Update(_mapper.Map<Coupon>(couponViewModel));
                    }

                    couponsViewModel.Add(couponViewModel);
                }


                if (couponsViewModel.IsNullOrEmpty())
                {
                    TempData["Empty"] = "Ainda não possui cupões.";  
                }

                return View(couponsViewModel);
            }
            else
            {
                return RedirectToAction("Home", "Index");
            }

        }

        private CouponViewModel ValidateExpiration(CouponViewModel coupon)
        {
           if(coupon.Used != true && coupon.ExpirationDate.Date < DateTime.Now.Date)
            {
                coupon.Expired = true;
            }

            return coupon;
        }
    }
}

