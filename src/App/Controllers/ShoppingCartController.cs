using App.Config;
using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace App.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private ICollection<OrderItemViewModel> cartItems = new List<OrderItemViewModel>();
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(IOrderItemRepository orderItemRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderItemRepository = orderItemRepository;
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

            return RedirectToAction("ViewCart");
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

            return RedirectToAction("ViewCart");
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

            return RedirectToAction("ViewCart");
        }
    }
}
