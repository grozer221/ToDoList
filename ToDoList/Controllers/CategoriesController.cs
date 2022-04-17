using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;
using ToDoList.Services;

namespace ToDoList.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoriesController(CategoryRepository categoryRepository)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                category.UserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
                await _categoryRepository.CreateAsync(category);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
                var checkAccessCategory = await _categoryRepository.GetByIdAsync(category.Id);
                if (checkAccessCategory.UserId != currentUserId)
                {
                    ModelState.AddModelError("", "You dont have access to edit it");
                }
                else
                {
                    try
                    {
                        await _categoryRepository.UpdateAsync(category);
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            return View(category);
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
        [ValidateAntiForgeryToken]
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
