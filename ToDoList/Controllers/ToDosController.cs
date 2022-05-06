using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Business.Enums;
using ToDoList.Enums;
using ToDoList.ViewModels.ToDos;
using static ToDoList.ViewModels.ToDos.ToDosIndexViewModel;

namespace ToDoList.Controllers
{
    public class ToDosController : Controller
    {
        private readonly IToDoRepository ToDoRepository;
        private readonly ICategoryRepository CategoryRepository;
        private readonly IMapper Mapper;

        public ToDosController(IEnumerable<IToDoRepository> toDoRepositories, IEnumerable<ICategoryRepository> categoryRepositories, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            DataProvider dataProvider;
            Enum.TryParse(httpContextAccessor.HttpContext.Request.Cookies["DataProvider"], out dataProvider);
            ToDoRepository = toDoRepositories.GetPropered(dataProvider);
            CategoryRepository = categoryRepositories.GetPropered(dataProvider);
            Mapper = mapper;
        }

        // GET: ToDosController
        public async Task<IActionResult> Index(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            var toDos = await ToDoRepository.GetWithCategoryAsync(like, sortOrder, categoryId);
            var categories = await CategoryRepository.GetAsync(string.Empty, CategoriesSortOrder.NameAsc);
            ToDosIndexViewModel toDosIndexViewModel = new ToDosIndexViewModel
            {
                ToDos = Mapper.Map<List<ToDoIndexViewModel>>(toDos),
                Categories = categories
            };
            return View(toDosIndexViewModel);
        }

        // GET: ToDosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ToDoModel toDo = await ToDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            return View(Mapper.Map<ToDosDetailsViewModel>(toDo));
        }

        // POST: ToDosController/Create
        [HttpPost]
        public async Task<IActionResult> Create(ToDosCreateViewModel toDosCreateViewModel)
        {
            if (!ModelState.IsValid)
            {
                var toDos = await ToDoRepository.GetWithCategoryAsync(like: null, ToDosSortOrder.DeadlineAcs, categoryId: null);
                var categories = await CategoryRepository.GetAsync(like: string.Empty, CategoriesSortOrder.NameAsc);
                ToDosIndexViewModel toDosIndexViewModel = new ToDosIndexViewModel
                {
                    CreateToDo = toDosCreateViewModel,
                    ToDos = Mapper.Map<List<ToDoIndexViewModel>>(toDos),
                    Categories = categories
                };
                return View(nameof(Index), toDosIndexViewModel);
            }
            ToDoModel toDo = Mapper.Map<ToDoModel>(toDosCreateViewModel);
            await ToDoRepository.CreateAsync(toDo);
            return RedirectToAction(nameof(Index));
        }

        // GET: ToDosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ToDoModel toDo = await ToDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));
            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.ToDo = Mapper.Map<ToDoEditViewModel>(toDo);
            toDosEditViewModel.Categories = await CategoryRepository.GetAsync(string.Empty, CategoriesSortOrder.DateDesc);
            return View(toDosEditViewModel);
        }

        // POST: ToDosController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ToDoEditViewModel toDoEditViewModel)
        {
            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.ToDo = toDoEditViewModel;
            toDosEditViewModel.Categories = await CategoryRepository.GetAsync(string.Empty, CategoriesSortOrder.DateDesc);

            if (!ModelState.IsValid)
                return View(toDosEditViewModel);

            try
            {
                ToDoModel toDo = Mapper.Map<ToDoModel>(toDoEditViewModel);
                await ToDoRepository.UpdateAsync(toDo);
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
            ToDoModel toDo = await ToDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            return View(Mapper.Map<ToDosDeleteViewModel>(toDo));
        }

        // POST: ToDosController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ToDoModel toDo = await ToDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            await ToDoRepository.RemoveAsync(toDo.Id);
            return RedirectToAction(nameof(Index));
        }

        // POST: ToDosController/SwitchIsDone?id=5&isDone=true
        [HttpPost]
        public async Task<IActionResult> SwitchIsComplete(int id, bool isComplete)
        {
            ToDoModel toDo = await ToDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return View(nameof(NotFound));

            toDo.IsComplete = isComplete;
            await ToDoRepository.UpdateAsync(toDo);
            return Ok(toDo);
        }

        // GET: ToDosController/NotFound
        public IActionResult NotFound()
        {
            return View();
        }
    }
}