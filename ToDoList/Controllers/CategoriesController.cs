using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Business.Enums;
using ToDoList.Enums;
using ToDoList.ViewModels.Categories;

namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository CategoryRepository;
        private readonly IMapper Mapper;

        public CategoriesController(IEnumerable<ICategoryRepository> categoryRepositories, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            DataProvider dataProvider;
            Enum.TryParse(httpContextAccessor.HttpContext.Request.Cookies["DataProvider"], out dataProvider);
            CategoryRepository = categoryRepositories.GetPropered(dataProvider);
            Mapper = mapper;
        }

        // GET: CategoriesController
        public async Task<IActionResult> Index(string? like, CategoriesSortOrder sortOrder)
        {
            var categories = await CategoryRepository.GetAsync(like, sortOrder);
            return View(Mapper.Map<IEnumerable<CategoriesIndexViewModel>>(categories));
        }

        // GET: CategoriesController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            CategoryModel category = await CategoryRepository.GetByIdWithTodosAsync(id);
            if(category == null)
                return View(nameof(NotFound));

            return View(Mapper.Map<CategoriesDetailsViewModel>(category));
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

            CategoryModel category = Mapper.Map<CategoryModel>(categoriesCreateViewModel);
            await CategoryRepository.CreateAsync(category);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriesController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            CategoryModel category = await CategoryRepository.GetByIdAsync(id);
            if (category == null)
                return View(nameof(NotFound));

            return View(Mapper.Map<CategoriesEditViewModel>(category));
        }

        // POST: CategoriesController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(CategoriesEditViewModel categoriesEditViewModel)
        {
            if (!ModelState.IsValid)
                return View(categoriesEditViewModel);

            try
            {
                CategoryModel category = Mapper.Map<CategoryModel>(categoriesEditViewModel);
                await CategoryRepository.UpdateAsync(category);
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
            CategoryModel category = await CategoryRepository.GetByIdAsync(id);
            if (category == null)
                return View(nameof(NotFound));

            return View(Mapper.Map<CategoriesDeleteViewModel>(category));
        }

        // POST: CategoriesController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CategoryModel category = await CategoryRepository.GetByIdAsync(id);
            if (category == null)
                return View(nameof(NotFound));

            await CategoryRepository.RemoveAsync(category.Id);
            return RedirectToAction(nameof(Index));
        }

        // GET: CategoriesController/NotFound
        public IActionResult NotFound()
        {
            return View();
        }
    }
}