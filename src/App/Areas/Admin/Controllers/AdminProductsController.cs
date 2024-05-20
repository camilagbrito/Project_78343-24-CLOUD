using App.ViewModels;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-products")]
    public class AdminProductsController : Controller

    {
        private const string url = "https://project78343images.blob.core.windows.net/productsimages/";
        private const string ContainerName = "productsimages";
        private readonly string _blobConnectionString;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public AdminProductsController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _blobConnectionString = configuration.GetConnectionString("BlobConnectionString");
        }

        [Route("products-list")]
        public async Task<IActionResult> List()
        {
            return View(_mapper.Map<IEnumerable<ProductViewModel>>(await _productRepository.GetProductsandCategory()));
        }

        [Route("product-details/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var productViewModel = await GetProduct(id);

            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);
        }

        [Route("new-product")]
        public async Task<IActionResult> Create()
        {
            var productViewModel = await SeedCategories(new ProductViewModel());
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-product")]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            productViewModel = await SeedCategories(productViewModel);

            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            var imgPrefix = Guid.NewGuid() + "_";

            await UploadFileStorage(productViewModel.ImageUpload, imgPrefix);

            if (productViewModel.ImageUpload != null)
            {
                productViewModel.Image = url + imgPrefix + productViewModel.ImageUpload.FileName;
            }

            await _productRepository.Add(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(List));
        }

        [Route("edit-product/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var productViewModel = await GetProduct(id);
            productViewModel = await SeedCategories(productViewModel);

            if (productViewModel == null)
            {
                return NotFound();
            }

            return View(productViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-product/{id:guid}")]
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


                if (!await UploadFileStorage(productViewModel.ImageUpload, imgPrefix))
                {
                    return View(productViewModel);
                }

                if (productViewModel.Image != null)
                {
                    await DeleteFileStorage(productViewModel.Image);
                }

                productViewModel.Image = url + imgPrefix + productViewModel.ImageUpload.FileName;
            }

            await _productRepository.Update(_mapper.Map<Product>(productViewModel));

            return RedirectToAction(nameof(List));
        }

        [Route("delete-product/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await GetProduct(id);

            if (id == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete-product/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await GetProduct(id);

            if (id == null)
            {
                return NotFound();
            }

            await DeleteFileStorage(product.Image);

            await _productRepository.Delete(id);

            return RedirectToAction(nameof(List));
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

        private async Task<bool> UploadFileStorage(IFormFile file, string imgPrefix)
        {
            if (file == null) return false;

            var name = imgPrefix + file.FileName;

            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            await blobContainerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = blobContainerClient.GetBlobClient(name);

            BlobHttpHeaders headers = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            await blobClient.UploadAsync(file.OpenReadStream(), headers);

            return true;

        }
        private async Task<bool> DeleteFileStorage(string file)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_blobConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);

            if (file == null) return false;

            string[] path = file.Split("/");

            string blobfile = path[path.Length - 1];

            BlobClient image = blobContainerClient.GetBlobClient(blobfile);

            if (await image.ExistsAsync() && image.GetBlobLeaseClient() != null)
            {
                await image.DeleteAsync();
            }
            return true;
        }

    }


}

