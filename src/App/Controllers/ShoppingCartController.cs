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
        private ICollection<OrderItemViewModel> cartItems = new List<OrderItemViewModel>();
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
       

        public ShoppingCartController(UserManager<ApplicationUser> userManager, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _userManager = userManager;
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
                TotalPrice = cartItemsSession.Sum(item => item.Product.Price * item.Quantity)
            };

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

    }
}
