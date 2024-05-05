using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Models.Enum;
using Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Azure;
using System.Security.Policy;

namespace App.Areas.Admin.Controllers
{

    //parei aqui
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-orders")]
    public class AdminOrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AdminOrdersController(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IAddressRepository addressRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _addressRepository = addressRepository; 
            _mapper = mapper;
        }

        [Route("orders-list")]
        public async Task<IActionResult> List()
        {
            var ordersViewModel = new List<OrderViewModel>();

            var orders = await _orderRepository.GetOrdersAndUsers();
            
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

        [Route("order-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
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

        [Route("edit-order/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var orderViewModel = await GetOrder(id);
            var status = new List<OrderStatus> {
                OrderStatus.PROCESSING,
                OrderStatus.PAID,
                OrderStatus.SHIPPED,
                OrderStatus.DELIVERED,
                OrderStatus.CANCELED
            };
            orderViewModel.ListStatus = status;

            if (orderViewModel == null)
            {
                return NotFound();
            }

            return View(orderViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-order/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, OrderViewModel orderViewModel)
        {
    
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            var orderUpdate = await GetOrder(id);
            orderUpdate.Status = orderViewModel.Status;

            if (!ModelState.IsValid)
            {
                return View(orderViewModel);
            }

            await _orderRepository.Update(_mapper.Map<Order>(orderUpdate));

            return RedirectToAction(nameof(List));
        }

    private async Task<OrderViewModel> GetOrder(Guid id)
        {
            var order = _mapper.Map<OrderViewModel>(await _orderRepository.GetOrderandItems(id)); 
            order.Items = _mapper.Map<IEnumerable<OrderItemViewModel>>(await _orderItemRepository.GetOrderItemsByOrderId(id));
            return order;
        }


    }
}
