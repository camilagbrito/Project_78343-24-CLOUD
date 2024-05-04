using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{

    //parei aqui
    public class AdminOrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AdminOrdersController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> List()
        {
            return View(_mapper.Map<IEnumerable<OrderViewModel>>(await _orderRepository.GetOrdersOrderItemsUser()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var OrderViewModel = _mapper.Map<OrderViewModel>(await _orderRepository.GetOrderandItems(id));

            if(OrderViewModel == null)
            {
                return NotFound();
            }
            return View(OrderViewModel);
        }
    }
}
