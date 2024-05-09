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
    [Authorize]
    [Route("orders")]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrdersController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IAddressRepository addressRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

  
        [Route("orders-list")]
        public async Task<IActionResult> List()
        {
            var ordersViewModel = new List<OrderViewModel>();

            if (User.Identity.IsAuthenticated)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;         
                var orders = await _orderRepository.GetOrdersByUserId(userId);

                foreach (var order in orders)
                {
                    var orderViewModel = new OrderViewModel
                    {
                        Id = order.Id,
                        Date = order.Date,
                        Total = order.Total,
                        Status = order.Status,
                        ApplicationUserViewModel = _mapper.Map<ApplicationUserViewModel>(order.User)

                    };
                    ordersViewModel.Add(orderViewModel);
                }
                return View(ordersViewModel);
            }
            else
            {
                return RedirectToAction("Home", "Index");
            }
        }

        [Route("order-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var orderViewModel = await GetOrder(id);
                var orderUser = await _orderRepository.GetOrderUser(id);
                orderViewModel.ApplicationUserViewModel = _mapper.Map<ApplicationUserViewModel>(orderUser.User);
                orderViewModel.ApplicationUserViewModel.Addresses = _mapper.Map<IEnumerable<AddressViewModel>>(await _addressRepository.GetAddressesByUserId(orderViewModel.UserId));

                if (orderViewModel == null)
                {
                    return NotFound();
                }

                return View(orderViewModel);
            }
            else
            {
                  return RedirectToAction("Home", "Index");
            }

        }
        private async Task<OrderViewModel> GetOrder(Guid id)
        {
            var order = _mapper.Map<OrderViewModel>(await _orderRepository.GetOrderandItems(id));
            order.Items = _mapper.Map<IEnumerable<OrderItemViewModel>>(await _orderItemRepository.GetOrderItemsByOrderId(id));
            return order;
        }
    }

}

