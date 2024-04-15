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

            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
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

            var imgPrefix = Guid.NewGuid() + "_";

            if(productViewModel.ImageUpload != null)
            {
                productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;
            }

            await _productRepository.Add(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(Index));
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

            var productUpdate = await GetProduct(id);

            productViewModel.Image = productUpdate.Image;

            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            if (productViewModel.ImageUpload != null)
            {
                var imgPrefix = Guid.NewGuid() + "_";


                if (!await UploadFile(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }

                if (productViewModel.Image != null)
                {
                    DeleteFile(productViewModel.Image);
                }

                productViewModel.Image = imgPrefix + productViewModel.ImageUpload.FileName;
            }

            await _productRepository.Update(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProduct(id);

            if(id == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProduct(id);

            if (id == null)
            {
                return NotFound();
            }

            DeleteFile(product.Image);

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

        private async Task<bool> UploadFile(IFormFile file, string imgPrefix)
        {
            if (file.Length <= 0 || file == null) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imgPrefix + file.FileName);

            if(System.IO.File.Exists(path))
            {
                ModelState.AddModelError(String.Empty, "Já existe um ficheiro com este nome");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return true;

        }

        private static void DeleteFile(string file)
        {
         
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", file);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

    }
}
