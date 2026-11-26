// using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using aspnetWebApp.Models;
using aspnetWebApp.Data;
using System.Security.Claims;

namespace aspnetWebApp.Pages;

public class CreateResumeModel : PageModel
{
    private readonly ApplicationDbContext _context;
    public CreateResumeModel(ApplicationDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public required string Name { get; set; }
    [BindProperty]
    public required string Phone { get; set; }
    [BindProperty]
    public required string Email { get; set; }
    [BindProperty]
    public required string Speciality { get; set; }
    [BindProperty]
    public required string Course { get; set; }
    [BindProperty]
    public required string BirthDate { get; set; }
    [BindProperty]
    public string[] Technologies { get; set; } = Array.Empty<string>();
    public static List<Student> Students { get; set; } = new();
    public required string Message { get; set; }
    public void OnGet()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var student = new Student 
        {
            Id = Students.Any() ? Students.Max(x => x.Id) + 1 : 1,
            Name = Name,
            Phone = Phone,
            Email = Email,
            Speciality = Speciality,
            Course = Course,
            BirthDate = BirthDate,
            Technologies = Technologies,
            UserId = userId!
        };
        _context.Students.Add(student);
        await _context.SaveChangesAsync();
        return new JsonResult(student);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var students = await  _context.Students 
            .AsNoTracking()
            .ToListAsync();
        return new JsonResult(students);
    }

    public async Task<IActionResult> OnGetStudentsAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes,NameIdentifier);
        var student = await _context.Students 
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if(student == null)
        {
            return new JsonResult(new 
            {
                success = false,
                message = "Студент не найден или нет прав на удаление"
            });
        }
        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return new JsonResult(new
        {
            success = true
        });
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {

    }
    
}
    // public IActionResult OnPost() {

    //     string technologies = Technologies.Length > 0
    //         ? string.Join(", ", Technologies)
    //         : "Не выбраны";

    //     var student = new Student
    //     {
    //         Id = Students.Any() ? Students.Max(x => x.Id) + 1 : 1,
    //         Name = Name,
    //         Phone = Phone,
    //         Email = Email,
    //         Speciality = Speciality,
    //         Course = Course,
    //         BirthDate = BirthDate,
    //         Technologies = Technologies
    //     };

    //     Students.Add(student);

    //     return new JsonResult(student);
    // }

    // public IActionResult OnGetStudents() {
    //     return new JsonResult(Students);
    // }

    // public IActionResult OnPostDelete(int id) {
    //     var student = Students.FirstOrDefault(x => x.Id == id);
    //     if (student == null) {
    //         return new JsonResult(new{
    //             success = false,
    //             message = "Студент не найден"
    //         });
    //     }
    //     Students.Remove(student);
    //     return new JsonResult(new{
    //         success = true
    //     });
    // }


