using Microsoft.AspNetCore.Mvc;

namespace AlsetTest.Controllers;

public class SharedController: Controller
{
    public SharedController()
    {
        
    }
    
    [HttpGet]
    public ActionResult Message(string message)
    {
        ViewBag.Message = message;
        return View();
    }
}