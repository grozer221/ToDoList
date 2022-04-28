using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;
using ToDoList.Repositories.Abstraction;
using ToDoList.Services;
using ToDoList.ViewModels.Categories;

namespace ToDoList.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: CategoriesController
        public async Task<IActionResult> Index(string? like, CategoriesSortOrder sortOrder)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            return View(await _categoryRepository.GetMyAsync(currentUserId, like, sortOrder));
        }

        // GET: CategoriesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CategoryModel category = await _categoryRepository.GetByIdWithTodosAsync(id);
            if(category == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (category.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to details it.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // GET: CategoriesController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CategoriesController/Create
        [HttpPost]
        public async Task<IActionResult> Create(CategoriesCreateViewModel categoriesCreateViewModel)
        {
            if (!ModelState.IsValid)
                return View(categoriesCreateViewModel);

            CategoryModel category = new CategoryModel(categoriesCreateViewModel);
            category.UserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            await _categoryRepository.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            CategoryModel category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (category.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to edit it.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(CategoriesEditViewModel categoriesEditViewModel)
        {
            if (!ModelState.IsValid)
                return View(categoriesEditViewModel);

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            CategoryModel checkAccessCategory = await _categoryRepository.GetByIdAsync(categoriesEditViewModel.Id);
            if (checkAccessCategory.UserId != currentUserId)
            {
                ModelState.AddModelError("", "You dont have access to edit it");
                return View(categoriesEditViewModel);
            }

            try
            {
                CategoryModel category = new CategoryModel(categoriesEditViewModel);
                await _categoryRepository.UpdateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(categoriesEditViewModel);
            }
        }

        // GET: CategoriesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            CategoryModel category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (category.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to delete it.";
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // POST: CategoriesController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CategoryModel category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (category.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to delete it.";
                return View(category);
            }

            await _categoryRepository.RemoveAsync(category.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
