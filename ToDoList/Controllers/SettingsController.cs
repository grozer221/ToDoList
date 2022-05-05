using Microsoft.AspNetCore.Mvc;
using ToDoList.Enums;

namespace ToDoList.Controllers
{
    public class SettingsController : Controller
    {
        [HttpPost]
        public IActionResult ChangeDataProvider(DataProvider dataProvider)
        {
            Response.Cookies.Append("DataProvider", dataProvider.ToString());
            return RedirectToAction(nameof(ToDosController.Index), nameof(ToDosController).ReplaceInEnd("Controller", ""));
        }
    }
}
