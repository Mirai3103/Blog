using BlogApp.DAL;
using BlogApp.Dto;
using BlogApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Controllers
{

    public class UserController : Controller
    {

        private readonly BlogContext _context;
        private readonly UserService _userService;


        public UserController(BlogContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Users != null
                ? View(await _context.Users.ToListAsync())
                : Problem("Entity set 'BlogContext.Users'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Register
        public IActionResult Register()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                TempData["Info"] = "Bạn đã đăng nhập rồi";
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimsPrincipal = HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
            {
                TempData["Info"] = "Bạn đã đăng nhập rồi";
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([Bind("Email,Password,RememberMe")] LoginDto user)
        {

            if (ModelState.IsValid)
            {
                var loggedUser = _userService.Login(user);
                if (loggedUser != null)
                {
                    List<Claim> claims = new List<Claim>(){
                        new Claim(ClaimTypes.NameIdentifier, loggedUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, loggedUser.DisplayName),
                        new Claim(ClaimTypes.Email, loggedUser.Email),
                        new Claim(ClaimTypes.Role, loggedUser.UserRole.ToString() )
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = user.RememberMe
                    };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    TempData["Success"] = "Đăng nhập thành công";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Error"] = "Tài khoản hoặc mật khẩu không đúng";
                }
            }

            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("DisplayName,Email,Password")] RegisterDto user)
        {
            if (_userService.FindByEmail(user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
            }
            if (ModelState.IsValid)
            {
                _userService.Register(user);
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,DisplayName,Email,HashPassword,Avatar,Description")] BlogApp.Models.User editedUser)
        {
            if (id != editedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(editedUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(editedUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(editedUser);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'BlogContext.Users'  is null.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}