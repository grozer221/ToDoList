using Microsoft.AspNetCore.Mvc;
using ToDoList.ViewModels.Error;

namespace ToDoList.Controllers
{
    public class ErrorController : Controller
    {
        // GET: ErrorController
        public IActionResult Index(int statusCode)
        {
            string message;
            switch (statusCode)
            {
                case 500:
                    message = "Internal server error";
                    break;
                case 404:
                default:
                    message = "Page Not Found";
                    break;

            }
            var errorIndexViewModel = new ErrorIndexViewModel
            {
                StatusCode = statusCode,
                Message = message
            };
            return View(errorIndexViewModel);
        }
    }
}
