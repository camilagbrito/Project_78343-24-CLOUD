using App.Config;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Controllers
{
    public class ShoppingCartController : Controller
    {

        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly ICouponRepository _couponRepository;
        private ICollection<OrderItemViewModel> cartItems = new List<OrderItemViewModel>();
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
       
       

        public ShoppingCartController(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, ICouponRepository couponRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _couponRepository = couponRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> AddToCart(Guid id)

        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetbyId(id));

            var cartItemsSession = HttpContext.Session.Get<List<OrderItemViewModel>>("Cart") ?? new List<OrderItemViewModel>();

            var existCarItem = cartItemsSession.FirstOrDefault(item => item.Product.Id == id);

            if (existCarItem != null)
            {
                if(existCarItem.Quantity < 40)
                {
                    existCarItem.Quantity++;
                  
                }
                else
                {
                    TempData["MaxItems"] = "Máximo de 40 items por produto! Para compras maiores, contactar por telefone.";
                    return RedirectToAction(nameof(ViewCart));
                }
            }
            else
            {
                cartItemsSession.Add(new OrderItemViewModel
                {
                    Product = product,
                    Quantity = 1
                });
            }

            HttpContext.Session.Set("Cart", cartItemsSession);
            return RedirectToAction(nameof(ViewCart));
        }

        public IActionResult ViewCart()
        {
           
            var cartItemsSession = HttpContext.Session.Get<List<OrderItemViewModel>>("Cart") ?? new List<OrderItemViewModel>();
            var cartViewModel = new ShoppingCartViewModel
            {
                Items = cartItemsSession,
                TotalPrice = cartItemsSession.Sum(item => (item.Product.Price * item.Quantity))
            };

            cartViewModel = AddCoupon(cartViewModel).Result;
            cartViewModel.TotalPrice = cartViewModel.TotalPrice - (cartViewModel.TotalPrice * (cartViewModel.DiscountPercent / 100));

            return View(cartViewModel);
        }

        public IActionResult DecreaseFromCart(Guid id)
        {

            var cartItemsSession = HttpContext.Session.Get<List<OrderItemViewModel>>("Cart") ?? new List<OrderItemViewModel>();
            var existCarItem = cartItemsSession.FirstOrDefault(item => item.Product.Id == id);

            if (existCarItem != null)
            {
                if (existCarItem.Quantity > 1)
                {
                    existCarItem.Quantity--;
                }
                else
                {
                    cartItemsSession.Remove(existCarItem);
                }
            }

            HttpContext.Session.Set("Cart", cartItemsSession);

            return RedirectToAction(nameof(ViewCart));
        }

        public IActionResult RemoveFromCart(Guid id)
        {

            var cartItemsSession = HttpContext.Session.Get<List<OrderItemViewModel>>("Cart") ?? new List<OrderItemViewModel>();
            var existCarItem = cartItemsSession.FirstOrDefault(item => item.Product.Id == id);

            if (existCarItem != null)
            {

                cartItemsSession.Remove(existCarItem);

            }

            HttpContext.Session.Set("Cart", cartItemsSession);

            return RedirectToAction(nameof(ViewCart));
        }

        [Authorize]
        public async Task<ShoppingCartViewModel> AddCoupon(ShoppingCartViewModel shoppingCartViewModel)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var coupons = _mapper.Map<IEnumerable<CouponViewModel>>( await _couponRepository.GetCouponsByUserId(userId));
            var activeCoupons = new List<CouponViewModel>();

            foreach (var c in coupons)
            {
                if(!c.Expired == true && !c.Used == true)
                {
                    activeCoupons.Add(c);
                }
            }

            var coupon = activeCoupons.LastOrDefault();

            if(coupon != null)
            {
                shoppingCartViewModel.Coupon = coupon;
                shoppingCartViewModel.DiscountPercent = coupon.DiscountPercent;
            }
           
            return shoppingCartViewModel;
        }

    }
}
