using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AlsetTest.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlsetTest.Controllers;

public class UserController: Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
    }
    
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result =
                await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.UserName == model.UserName);

                await _signInManager.SignInAsync(user, isPersistent: false);
                
                return RedirectToAction("Message", "Shared", new { message = "You have logged in succesfully" });
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }
    
    [HttpGet]
    public ActionResult Signin()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Signin(SigninViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email
            };

            var result =
                await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Message", "Shared", new { message = "You have signned in succesfully" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpGet]
    public ActionResult SearchResearcher()
    {
        if (User.Identity.IsAuthenticated)
        {
            return View();
        }
        
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public async Task<IActionResult> AllResearchers()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var users = await _userManager.Users.Where(u => u.Id != currentUser.Id).ToListAsync();
        return View(users);
    }
    
    [HttpPost]
    public async Task<ActionResult> SearchResearcher(string UserSearch)
    {
        if (string.IsNullOrWhiteSpace(UserSearch))
        {
            ViewBag.Message = "Please enter a search term.";
            return View();
        }

        var user = await _userManager.GetUserAsync(User);
        
        var users = await _userManager.Users
            .Where(u => u.UserName.Contains(UserSearch))
            .ToListAsync();

        users.Remove(user);

        return View("SearchResearcherResults", users);
    }

    [HttpGet]
    public ActionResult Subscribe()
    {
        return View();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Subscribe(string userId)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var userToSubscribe = await _userManager.FindByIdAsync(userId);
        
        var userIsSubscribed = await _context.UserSubscriber
            .AnyAsync(u => u.UserId == userToSubscribe.Id && u.SubscriberId == currentUser.Id);

        if (userIsSubscribed)
        {
            ModelState.AddModelError(string.Empty, "You are already subscribed to this researcher");            return View("SearchResearcher");
        }

        if (userToSubscribe == null || currentUser == null)
        {
            return NotFound();
        }

        var subscription = new UserSubscriber
        {
            UserId = userToSubscribe.Id,
            SubscriberId = currentUser.Id
        };

        _context.UserSubscriber.Add(subscription);
        await _context.SaveChangesAsync();

        return RedirectToAction("Message", "Shared", new { message = "You have subscribed other researcher succesfully" });
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Message", "Shared", new { message = "You have logged out in succesfully" });
    }
}