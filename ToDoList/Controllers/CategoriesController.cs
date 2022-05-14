using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Business.Enums;
using ToDoList.ViewModels.Categories;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(IEnumerable<ICategoryRepository> categoryRepositories, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            categoryRepository = categoryRepositories.GetPropered(dataProvider);
            this.mapper = mapper;
        }

        // GET: CategoriesController
        public async Task<IActionResult> Index(string? like, CategoriesSortOrder sortOrder)
        {
            var categories = await categoryRepository.GetAsync(like, sortOrder);
            return View(mapper.Map<IEnumerable<CategoriesIndexViewModel>>(categories));
        }

        // GET: CategoriesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CategoryModel category = await categoryRepository.GetByIdWithTodosOrDefaultAsync(id);
            if(category == null)
                return View(nameof(NotFound));

            return View(mapper.Map<CategoriesDetailsViewModel>(category));
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

            CategoryModel category = mapper.Map<CategoryModel>(categoriesCreateViewModel);
            await categoryRepository.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            CategoryModel category = await categoryRepository.GetByIdOrDefaultAsync(id);
            if (category == null)
                return View(nameof(NotFound));

            return View(mapper.Map<CategoriesEditViewModel>(category));
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(CategoriesEditViewModel categoriesEditViewModel)
        {
            if (!ModelState.IsValid)
                return View(categoriesEditViewModel);

            try
            {
                CategoryModel category = mapper.Map<CategoryModel>(categoriesEditViewModel);
                await categoryRepository.UpdateAsync(category);
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
            CategoryModel category = await categoryRepository.GetByIdOrDefaultAsync(id);
            if (category == null)
                return View(nameof(NotFound));

            return View(mapper.Map<CategoriesDeleteViewModel>(category));
        }

        // POST: CategoriesController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CategoryModel category = await categoryRepository.GetByIdOrDefaultAsync(id);
            if (category == null)
                return View(nameof(NotFound));

            await categoryRepository.RemoveAsync(category.Id);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriesController/NotFound
        public IActionResult NotFound()
        {
            return View();
        }
    }
}