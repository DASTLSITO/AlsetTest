using AlsetTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlsetTest.Controllers;

public class JournalController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _context;

    public JournalController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
        IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _env = env;
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }

    [HttpGet]
    public ActionResult CreateJournal()
    {
        if (User.Identity.IsAuthenticated)
        {
            return View();
        }
        
        return RedirectToAction("Message", "Shared", new { message = "You're not authenticated" });
    }

    [HttpPost]
    public async Task<ActionResult> CreateJournal(FileUploadViewModel model)
    {
        if (model.PdfFile == null || model.PdfFile.Length <= 0)
        {
            ViewBag.Message = "No file selected.";
            return View();
        }
        
        if (Path.GetExtension(model.PdfFile.FileName).ToLower() != ".pdf")
        {
            ViewBag.Message = "Please upload a PDF file.";
            return View();
        }
        
        var contentURL = await SaveFile(model.PdfFile);

        var user = await _userManager.GetUserAsync(User);
        
        var journal = new Journal
        {
            Title = model.Title,
            UserId = user.Id,
            ContentURL = contentURL
        };
        
        _context.Journals.Add(journal);
        await _context.SaveChangesAsync();
        ViewBag.Message = "File uploaded successfully!";
        return RedirectToAction("Message", "Shared", new { message = "You have created a journal succesfully" });
        
    }

    public async Task<string> SaveFile(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        string folder = _env.WebRootPath;

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        string ruta = Path.Combine(folder, fileName);
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            var content = ms.ToArray();
            await System.IO.File.WriteAllBytesAsync(ruta, content);
        }
        
        var url = $"{_httpContextAccessor.HttpContext!.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        var urlArchivo = Path.Combine(url, fileName).Replace("\\", "/");

        return urlArchivo;
    }
    
    [HttpGet]
    public async Task<IActionResult> SeeMyJournals()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            var files = await _context.Journals
                .Where(j => j.UserId == user.Id).ToListAsync();

            return View(files);
        }
        
        return RedirectToAction("Message", "Shared", new { message = "You're not authenticated" });
    }
    
    [HttpGet]
    public async Task<IActionResult> SubscribedJournals()
    {
        if(User.Identity.IsAuthenticated)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var subscribedUsers = await _context.UserSubscriber
                .Where(us => us.SubscriberId == currentUser.Id)
                .Select(us => us.UserId)
                .ToListAsync();

            var journals = _context.Journals
                .Where(j => subscribedUsers.Contains(j.UserId))
                .Include(j => j.User)
                .ToList();

            return View(journals);
        }

        return RedirectToAction("Message", "Shared", new { message = "You're not authenticated" });
    }
}