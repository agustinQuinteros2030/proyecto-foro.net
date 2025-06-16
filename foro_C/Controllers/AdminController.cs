using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult PanelAdmin()
    {
        return View();
    }
}