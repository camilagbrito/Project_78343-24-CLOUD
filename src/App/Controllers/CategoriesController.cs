using App.ViewModels;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }
    

        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAll()));
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var categoryViewModel = _mapper.Map<CategoryViewModel>(await _categoryRepository.GetbyId(id));
            
            if(categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create (CategoryViewModel categoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(categoryViewModel);
            }

            var category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.Add(category);

            return RedirectToAction(nameof(Index));


        }

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
        public async Task<IActionResult> Edit(Guid id, CategoryViewModel categoryViewModel)
        {
            if(id != categoryViewModel.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
              return View(categoryViewModel);
            }

            var category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.Update(category);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryViewModel = _mapper.Map<CategoryViewModel>(await _categoryRepository.GetbyId(id));
            
            if(categoryViewModel == null)
            {
                return NotFound();
            }

            return View(categoryViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var categoryViewModel = _mapper.Map<CategoryViewModel>(await _categoryRepository.GetbyId(id));

            if(categoryViewModel == null)
            {
                return NotFound();
            }

            await _categoryRepository.Delete(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
