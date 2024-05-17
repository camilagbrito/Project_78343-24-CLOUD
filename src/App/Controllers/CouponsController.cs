using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace App.Controllers
{
    [Authorize]
    [Route("coupon/")]
    public class CouponsController : Controller
    {

        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public CouponsController(ICouponRepository couponRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Route("my-coupons")]
        public async Task<IActionResult> UserCoupons()
        {
            if (User.Identity.IsAuthenticated)
            {
                var couponsViewModel = new List<CouponViewModel>();

                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var coupons = await _couponRepository.GetCouponsByUserId(userId);

                foreach (var coupon in coupons)
                {
                    var couponViewModel = new CouponViewModel
                    {
                        Id = coupon.Id,
                        User = _mapper.Map<ApplicationUserViewModel>(coupon.User),
                        UserId = userId,
                        Discount = coupon.Discount,
                        CreatedDate = coupon.CreatedDate,
                        ExpirationDate = coupon.ExpirationDate,
                        Expired = coupon.Expired,
                        Used = coupon.Used,
                        ChallengeId = coupon.ChallengeId,
                        AssociatedOrderId = coupon.AssociatedOrderId

                    };

                    couponViewModel = ValidateUse(couponViewModel);

                    if(coupon.Expired == true || coupon.Used == true)
                    {
                        await _couponRepository.Update(_mapper.Map<Coupon>(couponViewModel));
                    }
                    
                    couponsViewModel.Add(couponViewModel);
                }
                return View(couponsViewModel);
            }
            else
            {
                return RedirectToAction("Home", "Index");
            }

        }

        private CouponViewModel ValidateUse(CouponViewModel coupon)
        {
            if (coupon.AssociatedOrderId.ToString() != "00000000-0000-0000-0000-000000000000") 
            { 
                coupon.Used = true; 
            }
            else
            {
                ValidateExpiration(coupon);  
            }
            return coupon;
        }

        private CouponViewModel ValidateExpiration(CouponViewModel coupon)
        {
           if(coupon.ExpirationDate.Date < DateTime.Now.Date)
            {
                coupon.Expired = true;
            }

            return coupon;
        }
    }
}

