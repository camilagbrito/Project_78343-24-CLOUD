using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.IdentityModel.Tokens;

namespace App.Controllers
{

    public class ProductsController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        
        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<IActionResult> List()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsandCategory()));
        }
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = _mapper.Map<ProductViewModel>(await _productRepository.GetProductandCategoryById(id));

            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

    }
}
