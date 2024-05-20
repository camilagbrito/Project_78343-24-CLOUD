using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace App.Controllers
{

    public class ProductsController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> List(Guid? categoryId, string ProductName)
        {
            var categories = await _categoryRepository.GetAll();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            if (categoryId.HasValue)
            {
                var products = _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsByCategory(categoryId.Value));
                return View(products);
            }
            else if (!string.IsNullOrEmpty(ProductName))
            {
                var products = _mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsByName(ProductName));
                return View(products);
            }
            else
            {
                return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsandCategory()));
            }


            //return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsandCategory()));

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
