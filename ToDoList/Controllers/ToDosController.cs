using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Business.Enums;
using ToDoList.ViewModels.ToDos;
using static ToDoList.ViewModels.ToDos.ToDosIndexViewModel;

namespace ToDoList.Controllers
{
    public class ToDosController : Controller
    {
        private readonly IToDoRepository toDoRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;

        public ToDosController(IEnumerable<IToDoRepository> toDoRepositories, IEnumerable<ICategoryRepository> categoryRepositories, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            string? dataProvider = httpContextAccessor.HttpContext.Request.Cookies["DataProvider"];
            toDoRepository = toDoRepositories.GetPropered(dataProvider);
            categoryRepository = categoryRepositories.GetPropered(dataProvider);
            this.mapper = mapper;
        }

        // GET: ToDosController
        public async Task<IActionResult> Index(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            var toDos = await toDoRepository.GetWithCategoryAsync(like, sortOrder, categoryId);
            var categories = await categoryRepository.GetAsync(string.Empty, CategoriesSortOrder.NameAsc);
            ToDosIndexViewModel toDosIndexViewModel = new ToDosIndexViewModel
            {
                ToDos = mapper.Map<List<ToDoIndexViewModel>>(toDos),
                Categories = categories
            };
            return View(toDosIndexViewModel);
        }

        // GET: ToDosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ToDoModel toDo = await toDoRepository.GetByIdOrDefaultAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            return View(mapper.Map<ToDosDetailsViewModel>(toDo));
        }

        // POST: ToDosController/Create
        [HttpPost]
        public async Task<IActionResult> Create(ToDosCreateViewModel toDosCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                var toDos = await toDoRepository.GetWithCategoryAsync(like: null, ToDosSortOrder.DeadlineAcs, categoryId: null);
                var categories = await categoryRepository.GetAsync(like: string.Empty, CategoriesSortOrder.NameAsc);
                ToDosIndexViewModel toDosIndexViewModel = new ToDosIndexViewModel
                {
                    CreateToDo = toDosCreateViewModel,
                    ToDos = mapper.Map<List<ToDoIndexViewModel>>(toDos),
                    Categories = categories
                };
                return View(nameof(Index), toDosIndexViewModel);
            }
            ToDoModel toDo = mapper.Map<ToDoModel>(toDosCreateViewModel);
            await toDoRepository.CreateAsync(toDo);
            return RedirectToAction(nameof(Index));
        }

        // GET: ToDosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ToDoModel toDo = await toDoRepository.GetByIdOrDefaultAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));
            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.ToDo = mapper.Map<ToDoEditViewModel>(toDo);
            toDosEditViewModel.Categories = await categoryRepository.GetAsync(string.Empty, CategoriesSortOrder.DateDesc);
            return View(toDosEditViewModel);
        }

        // POST: ToDosController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ToDoEditViewModel toDoEditViewModel)
        {
            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.ToDo = toDoEditViewModel;
            toDosEditViewModel.Categories = await categoryRepository.GetAsync(string.Empty, CategoriesSortOrder.DateDesc);

            if (!ModelState.IsValid)
                return View(toDosEditViewModel);

            try
            {
                ToDoModel toDo = mapper.Map<ToDoModel>(toDoEditViewModel);
                await toDoRepository.UpdateAsync(toDo);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(toDosEditViewModel);
            }
        }

        // GET: ToDosController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            ToDoModel toDo = await toDoRepository.GetByIdOrDefaultAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            return View(mapper.Map<ToDosDeleteViewModel>(toDo));
        }

        // POST: ToDosController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ToDoModel toDo = await toDoRepository.GetByIdOrDefaultAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            await toDoRepository.RemoveAsync(toDo.Id);
            return RedirectToAction(nameof(Index));
        }

        // POST: ToDosController/SwitchIsDone?id=5&isDone=true
        [HttpPost]
        public async Task<IActionResult> SwitchIsComplete(int id, bool isComplete)
        {
            ToDoModel toDo = await toDoRepository.GetByIdOrDefaultAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            toDo.IsComplete = isComplete;
            await toDoRepository.UpdateAsync(toDo);
            return Ok(toDo);
        }

        // GET: ToDosController/NotFound
        public IActionResult NotFound()
        {
            return View();
        }
    }
}