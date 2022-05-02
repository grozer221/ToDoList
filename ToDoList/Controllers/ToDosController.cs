using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Business.Enums;
using ToDoList.Services;
using ToDoList.ViewModels.ToDos;
using static ToDoList.ViewModels.ToDos.ToDosCreateViewModel;
using static ToDoList.ViewModels.ToDos.ToDosIndexViewModel;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ToDosController(IToDoRepository toDoRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _toDoRepository = toDoRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET: ToDosController
        public async Task<IActionResult> Index(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            var toDos = await _toDoRepository.GetMyWithCategory(currentUserId, like, sortOrder, categoryId);
            ToDosIndexViewModel toDosIndexViewModel = new ToDosIndexViewModel
            {
                ToDos = _mapper.Map<List<ToDoIndexViewModel>>(toDos),
                Categories = new List<CategoryModel> { new CategoryModel { Name = "---Select category---"} }
            };
            toDosIndexViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.NameAsc));
            return View(toDosIndexViewModel);
        }

        // GET: ToDosController/Create
        public async Task<IActionResult> Create()
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            ToDosCreateViewModel toDosCreateViewModel = new ToDosCreateViewModel();
            toDosCreateViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosCreateViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));
            return View(toDosCreateViewModel);
        }

        // POST: ToDosController/Create
        [HttpPost]
        public async Task<IActionResult> Create(ToDoCreateViewModel toDoCreateViewModel)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (!ModelState.IsValid)
            {
                ToDosCreateViewModel toDosCreateViewModel = new ToDosCreateViewModel();
                toDosCreateViewModel.ToDo = toDoCreateViewModel;
                toDosCreateViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
                toDosCreateViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));
                return View(toDosCreateViewModel);
            }
            ToDoModel toDo = _mapper.Map<ToDoModel>(toDoCreateViewModel);
            toDo.UserId = currentUserId;
            await _toDoRepository.CreateAsync(toDo);
            return RedirectToAction(nameof(Index));
        }

        // GET: ToDosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            ToDoModel toDo = await _toDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (toDo.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to edit it.";
                return RedirectToAction(nameof(Index));
            }

            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.ToDo = _mapper.Map<ToDoEditViewModel>(toDo);
            toDosEditViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosEditViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));
            return View(toDosEditViewModel);
        }

        // POST: ToDosController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ToDoEditViewModel toDoEditViewModel)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.ToDo = toDoEditViewModel;
            toDosEditViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosEditViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));

            if (!ModelState.IsValid)
                return View(toDosEditViewModel);

            ToDoModel checkAccessTodo = await _toDoRepository.GetByIdAsync(toDoEditViewModel.Id);
            if (checkAccessTodo.UserId != currentUserId)
            {
                ModelState.AddModelError("", "You dont have access to edit it");
                return View(toDosEditViewModel);
            }

            try
            {
                ToDoModel toDo = _mapper.Map<ToDoModel>(toDoEditViewModel);
                toDo.UserId = currentUserId;
                await _toDoRepository.UpdateAsync(toDo);
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
            ToDoModel toDo = await _toDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (toDo.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to delete it.";
                return RedirectToAction(nameof(Index));
            }

            return View(_mapper.Map<ToDosDeleteViewModel>(toDo));
        }

        // POST: ToDosController/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ToDoModel toDo = await _toDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if(toDo.UserId != currentUserId)
            {
                TempData["Error"] = "You dont have access to delete it.";
                return View(toDo);
            }

            await _toDoRepository.RemoveAsync(toDo.Id);
            return RedirectToAction(nameof(Index));
        }

        // POST: ToDosController/SwitchIsDone?id=5&isDone=true
        [HttpPost]
        public async Task<IActionResult> SwitchIsComplete(int id, bool isComplete)
        {
            ToDoModel toDo = await _toDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (toDo.UserId != currentUserId)
            {
                return Forbid();
            }

            toDo.IsComplete = isComplete;
            await _toDoRepository.UpdateAsync(toDo);
            return Ok(toDo);
        }
    }
}