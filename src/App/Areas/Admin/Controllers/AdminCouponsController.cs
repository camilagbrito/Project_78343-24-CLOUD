using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-coupons")]
    public class AdminCouponsController : Controller
    {

        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public AdminCouponsController(ICouponRepository couponRepository, IMapper mapper)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        [Route("all-coupons")]
        public async Task<IActionResult> List()
        {

            var couponsViewModel = new List<CouponViewModel>();

            var coupons = _mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsAndUsers());

            foreach (var coupon in coupons)
            {

                var couponViewModel = ValidateExpiration(coupon);

                if (couponViewModel.Expired == true)
                {
                    _couponRepository.Update(_mapper.Map<Coupon>(couponViewModel));
                }

                couponsViewModel.Add(couponViewModel);
            }


            return View(couponsViewModel);
        }

        private CouponViewModel ValidateExpiration(CouponViewModel coupon)
        {
            if (coupon.Used != true && coupon.ExpirationDate.Date < DateTime.Now.Date)
            {
                coupon.Expired = true;
            }

            return coupon;
        }
    }
}
