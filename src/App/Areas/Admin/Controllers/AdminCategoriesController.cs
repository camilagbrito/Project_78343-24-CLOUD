using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("Admin")]
    [Route("admin/admin-categories")]
    public class AdminCategoriesController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public AdminCategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }

        [Route("categories-list")]
        public async Task<IActionResult> List()
        {
            return View(_mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAll()));
        }

        [Route("new-category")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("new-category")]
        public async Task<IActionResult> Create(CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            var category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.Add(category);

            return RedirectToAction(nameof(List));


        }

        [Route("edit-category/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var categoryViewModel = _mapper.Map<CategoryViewModel>(await _categoryRepository.GetbyId(id));

            if (categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit-category/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id, CategoryViewModel categoryViewModel)
        {
            if (id != categoryViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            var category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.Update(category);

            return RedirectToAction(nameof(List));
        }

        [Route("delete-category/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryViewModel = _mapper.Map<CategoryViewModel>(await _categoryRepository.GetbyId(id));

            if (categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete-category/{id:guid}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
           
            await _categoryRepository.Delete(id);

            return RedirectToAction(nameof(List));
        }
    }
}
