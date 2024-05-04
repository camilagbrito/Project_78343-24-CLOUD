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

                existCarItem.Quantity++;
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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var cartItemsSession = HttpContext.Session.Get<List<OrderItemViewModel>>("Cart") ?? new List<OrderItemViewModel>();

            //var order = new Order();
            var orderViewModel = new OrderViewModel(); 

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            //order.Date = DateTime.Now;
            //order.UserId = user.Id;
            //order.Total = cartItemsSession.Sum(item => item.Product.Price * item.Quantity);

            orderViewModel.Date = DateTime.Now;
            orderViewModel.UserId = user.Id;
            orderViewModel.Total = cartItemsSession.Sum(item => item.Product.Price * item.Quantity);

            await _orderRepository.Add(_mapper.Map<Order>(orderViewModel));
           
            //se utilizador não estiver logado redirecionar para login

            foreach (var item in cartItemsSession)
            {
                var orderItemViewModel = new OrderItemViewModel
                {
                    ProductId = item.Product.Id,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    OrderId = orderViewModel.Id
                };
                await _orderItemRepository.Add(_mapper.Map<OrderItem>(orderItemViewModel));
            }

            HttpContext.Session.Set("Cart", new List<ShoppingCartViewModel>());

            return RedirectToAction(nameof(ViewCart));

        }

    }
}
