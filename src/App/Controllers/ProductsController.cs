using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsandCategory()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);

            return View();
        }

        public async Task <IActionResult> Create()
        {
            var productViewModel = await SeedCategories(new ProductViewModel());
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await SeedCategories(productViewModel);

            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            await _productRepository.Add(_mapper.Map<Product>(productViewModel));

            return View(productViewModel);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            productViewModel = await SeedCategories(productViewModel);

            if(productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
        {
           
            if (id != productViewModel.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }
            await _productRepository.Update(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProduct(id);

            if(product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, ProductViewModel productViewModel)
        {
            var product = await GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<ProductViewModel> GetProduct(Guid id)
        {
            var product = _mapper.Map<ProductViewModel>(await _productRepository.GetProductandCategoryById(id));
            product.Categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAll());
            return product;
        }

        private async Task<ProductViewModel> SeedCategories(ProductViewModel product)
        {
            product.Categories = _mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAll());
            return product;
        }
        //return list of categories

    }
}
