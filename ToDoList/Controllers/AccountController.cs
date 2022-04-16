using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Services;
using ToDoList.ViewModels.Accounts;

namespace ToDoList.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly AccountService _accountService;
        private readonly EmailService _emailService;
        public AccountController(UserRepository userRepository, AccountService accountService, EmailService emailService)
        {
            _userRepository = userRepository;
            _accountService = accountService;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        
        [HttpGet("{confirmatioEmailToken}")]
        public async Task<IActionResult> ConfirmationEmail(string confirmatioEmailToken)
        {
            try
            {
                int userId = _accountService.ValidateConfirmationEmailToken(confirmatioEmailToken);
                UserModel user = await _userRepository.GetByIdAsync(userId);
                if (user.IsEmailConfirmed)
                {
                    TempData["Error"] = "You already confirmed your email.";
                }
                else
                {
                    user.IsEmailConfirmed = true;
                    await _userRepository.UpdateAsync(user);
                    TempData["Success"] = "You successfully confirmed your email.";
                }
            }
            catch(Exception e)
            {
                TempData["Error"] = e.Message;
            }
            return RedirectToAction(string.Empty, string.Empty);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountsLoginViewModel accountsLoginViewModel)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userRepository.GetByEmailAsync(accountsLoginViewModel.Email);
                if (user != null)
                {
                    if(user.Password != accountsLoginViewModel.Password)
                    {
                        ModelState.AddModelError("", "Bad credentials");
                        return View(accountsLoginViewModel);
                    }

                    if (!user.IsEmailConfirmed)
                    {
                        string protocol = Request.IsHttps ? "https" : "http";
                        string host = $"{protocol}://{Request.Host}";
                        await _accountService.SendConfirmationEmail(host, user.Id, user.Email);
                        ModelState.AddModelError("", "Your email is not confirmed. Go to your email and confirm!");
                        return View(accountsLoginViewModel);
                    }

                    await _accountService.Authenticate(HttpContext, user.Id, user.Email);
                    return RedirectToAction(string.Empty, string.Empty);
                }
                ModelState.AddModelError("", "Bad credentials");
            }
            return View(accountsLoginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountsRegisterViewModel accountsRegisterViewModel)
        {
            if (ModelState.IsValid)
            {
                UserModel user = await _userRepository.GetByEmailAsync(accountsRegisterViewModel.Email);
                if (user == null)
                {
                    user = await _userRepository.CreateAsync(new UserModel { Email = accountsRegisterViewModel.Email, Password = accountsRegisterViewModel.Password });
                    string protocol = Request.IsHttps ? "https" : "http";
                    string host = $"{protocol}://{Request.Host}";
                    await _accountService.SendConfirmationEmail(host, user.Id, user.Email);
                    TempData["Success"] = "You successfully registered. Go to your email and confirm!";
                    return RedirectToAction(string.Empty, string.Empty);
                }
                else
                    ModelState.AddModelError("", "User with wrote email already exists");
            }
            return View(accountsRegisterViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(AccountController.Login), nameof(AccountController).ReplaceInEnd("Controller", string.Empty));
        }
    }
}
