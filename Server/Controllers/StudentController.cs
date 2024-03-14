using Microsoft.AspNetCore.Mvc;
using HPCTech2024SpringProjectBoilerPlate.Shared;
using Microsoft.AspNetCore.Authorization;

namespace HPCTech2024SpringProjectBoilerPlate.Server.Controllers;

public class StudentController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    [Route("api/students")]
    public IEnumerable<Student> Get()
    {
        return new List<Student>
        {
            new Student { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.net"},
            new Student { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.net"},
            new Student { Id = 3, FirstName = "Max", LastName = "Mahlke", Email = "max@example.net"},
            new Student { Id = 4, FirstName = "Ben", LastName = "Weidner", Email = "ben@example.net"},
            new Student { Id = 5, FirstName = "Kendall", LastName = "Wilcox", Email = "kendall@example.net"}
        };
    }
}
