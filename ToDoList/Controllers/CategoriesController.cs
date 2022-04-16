using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            return View(await _categoryRepository.GetAsync());
        }

        // GET: CategoriesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CategoryModel category = await _categoryRepository.GetByIdWithTodosAsync(id);
            if(category == null)
                return NotFound();
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
            return View(category);
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name")] CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryRepository.UpdateAsync(category);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(category);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: CategoriesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            CategoryModel category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return NotFound();
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
            await _categoryRepository.RemoveAsync(category.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
