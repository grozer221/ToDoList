using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;
using ToDoList.Repositories.Abstraction;
using ToDoList.Services;
using ToDoList.ViewModels.ToDos;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ToDosController(IToDoRepository toDoRepository, ICategoryRepository categoryRepository)
        {
            _toDoRepository = toDoRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: ToDosController
        public async Task<IActionResult> Index(string? like, ToDosSortOrder sortOrder, int? categoryId)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            ToDosIndexViewModel toDosIndexViewModel = new ToDosIndexViewModel
            {
                ToDos = await _toDoRepository.GetMyWithCategory(currentUserId, like, sortOrder, categoryId),
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
        public async Task<IActionResult> Create(ToDosCreateViewModel toDosCreateViewModel)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (!ModelState.IsValid)
            {
                toDosCreateViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
                toDosCreateViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));
                return View(toDosCreateViewModel);
            }
            ToDoModel toDo = new ToDoModel(toDosCreateViewModel);
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

            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel(toDo);
            toDosEditViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosEditViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));
            return View(toDosEditViewModel);
        }

        // POST: ToDosController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ToDosEditViewModel toDosEditViewModel)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            toDosEditViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosEditViewModel.Categories.AddRange(await _categoryRepository.GetMyAsync(currentUserId, string.Empty, CategoriesSortOrder.DateDesc));

            if (!ModelState.IsValid)
                return View(toDosEditViewModel);

            ToDoModel checkAccessTodo = await _toDoRepository.GetByIdAsync(toDosEditViewModel.Id);
            if (checkAccessTodo.UserId != currentUserId)
            {
                ModelState.AddModelError("", "You dont have access to edit it");
                return View(toDosEditViewModel);
            }

            try
            {
                ToDoModel toDo = new ToDoModel(toDosEditViewModel);
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

            return View(toDo);
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
