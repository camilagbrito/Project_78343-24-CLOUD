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
            return View(_mapper.Map<IEnumerable<CouponViewModel>>(await _couponRepository.GetCouponsAndUsers()));
        }
    }
}
