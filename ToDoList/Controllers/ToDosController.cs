using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;
using ToDoList.Services;
using ToDoList.ViewModels.ToDos;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ToDosController : Controller
    {
        private readonly ToDoRepository _toDoRepository;
        private readonly CategoryRepository _categoryRepository;

        public ToDosController(ToDoRepository toDoRepository, CategoryRepository categoryRepository)
        {
            _toDoRepository = toDoRepository;
            _categoryRepository = categoryRepository;
        }

        // GET: ToDosController
        public async Task<IActionResult> Index(string? like, ToDosSortOrder sortOrder)
        {
            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            return View(await _toDoRepository.GetMyWithCategory(currentUserId, like, sortOrder));
        }

        // GET: ToDosController/Create
        public async Task<IActionResult> Create()
        {
            ToDosCreateViewModel toDosCreateViewModel = new ToDosCreateViewModel();
            toDosCreateViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosCreateViewModel.Categories.AddRange(await _categoryRepository.GetAsync());
            return View(toDosCreateViewModel);
        }

        // POST: ToDosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Deadline,CategoryId")] ToDoModel toDo)
        {
            if (ModelState.IsValid)
            {
                toDo.UserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
                await _toDoRepository.CreateAsync(toDo);
                return RedirectToAction(nameof(Index));
            }
            return View(toDo);
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
            toDosEditViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosEditViewModel.Categories.AddRange(await _categoryRepository.GetAsync());
            toDosEditViewModel.ToDo = toDo;
            return View(toDosEditViewModel);
        }

        // POST: ToDosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,IsDone,Deadline,CategoryId")] ToDoModel toDo)
        {
            ToDosEditViewModel toDosEditViewModel = new ToDosEditViewModel();
            toDosEditViewModel.Categories = new List<CategoryModel> { new CategoryModel { Name = "--Select category--" } };
            toDosEditViewModel.Categories.AddRange(await _categoryRepository.GetAsync());
            toDosEditViewModel.ToDo = toDo;

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            var checkAccessTodo = await _toDoRepository.GetByIdAsync(toDo.Id);
            if(checkAccessTodo.UserId != currentUserId)
            {
                ModelState.AddModelError("", "You dont have access to edit it");
                return View(toDosEditViewModel);
            }
            toDosEditViewModel.ToDo.UserId = currentUserId;

            if (ModelState.IsValid)
            {
                try
                {
                    await _toDoRepository.UpdateAsync(toDo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(toDosEditViewModel);
                }
            }
            return View(toDosEditViewModel);
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
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> SwitchIsDone(int id, bool isDone)
        {
            ToDoModel toDo = await _toDoRepository.GetByIdAsync(id);
            if (toDo == null)
                return NotFound();

            int currentUserId = int.Parse(HttpContext.User.Claims.First(c => c.Type == AccountService.DefaultIdClaimType).Value);
            if (toDo.UserId != currentUserId)
            {
                return Forbid();
            }

            toDo.IsDone = isDone;
            await _toDoRepository.UpdateAsync(toDo);
            return Ok();
        }
    }
}
