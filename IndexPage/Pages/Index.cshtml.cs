using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using IndexPage.Models;
using System.Collections.Generic;
using System.Linq;
using IndexPage.Helpers;

namespace IndexPage.Pages
{
    public class IndexModel : PageModel
    {
        public static List<ClassInformationModel> _classes = new List<ClassInformationModel>();

        [BindProperty]
        public ClassInformationModel? NewClass { get; set; }

        public List<ClassInformationTable> FilteredPagedClasses { get; set; } = new();
        public string? ClassNameFilter { get; set; }
        public int? StudentCountFilter { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; } = 10;

        private void EnsureDataIsLoaded()
        {
            if (!_classes.Any())
            {
                for (int i = 1; i <= 100; i++)
                {
                    _classes.Add(new ClassInformationModel
                    {
                        Id = i,
                        ClassName = $"Class {i}",
                        StudentCount = 10 + (i % 50),
                        Description = $"Description for Class {i}"
                    });
                }
            }
        }

        public IActionResult OnGet(string classNameFilter, int? studentCountFilter, int pageNumber = 1)
        {
            EnsureDataIsLoaded();

            ClassNameFilter = classNameFilter;
            StudentCountFilter = studentCountFilter;

            var pageData = _classes
                .Skip((pageNumber - 1) * PageSize)
                .Take(PageSize)
                .AsQueryable();

            if (!string.IsNullOrEmpty(ClassNameFilter))
            {
                pageData = pageData.Where(c => c.ClassName.Contains(ClassNameFilter));
            }

            if (StudentCountFilter.HasValue)
            {
                pageData = pageData.Where(c => c.StudentCount == StudentCountFilter.Value);
            }

            FilteredPagedClasses = pageData
                .Select(c => new ClassInformationTable
                {
                    Id = c.Id,
                    ClassName = c.ClassName,
                    StudentCount = c.StudentCount,
                    Description = c.Description
                })
                .ToList();

            TotalPages = (int)Math.Ceiling((double)_classes.Count / PageSize);
            CurrentPage = pageNumber;

            return Page();
        }

        public IActionResult OnPostExport([FromBody] ExportRequest request)
        {
            if (request == null) return BadRequest();

            List<ClassInformationModel> data;

            if (request.SelectedIds?.Any() == true)
            {
                var ids = request.SelectedIds.Select(int.Parse).ToList();
                data = _classes.Where(c => ids.Contains(c.Id)).ToList();
            }
            else
            {
                var pageData = _classes
                    .Skip((request.CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.ClassNameFilter))
                {
                    pageData = pageData.Where(c => c.ClassName.Contains(request.ClassNameFilter));
                }

                if (request.StudentCountFilter.HasValue)
                {
                    pageData = pageData.Where(c => c.StudentCount == request.StudentCountFilter.Value);
                }

                data = pageData.ToList();
            }

            var jsonBytes = Util.ExportToJson(data);
            return File(jsonBytes, "application/json", Util.GenerateExportFilename());
        }

        public IActionResult OnPostAdd()
        {
            if (NewClass == null || string.IsNullOrWhiteSpace(NewClass.ClassName) || NewClass.StudentCount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid class information.");
                return Page();
            }

            var existingClass = _classes.FirstOrDefault(c => c.Id == NewClass.Id);
            if (existingClass != null)
            {
                _classes.Remove(existingClass);
            }
            else
            {
                NewClass.Id = _classes.Count > 0 ? _classes.Max(c => c.Id) + 1 : 1;
            }

            _classes.Add(NewClass);
            _classes = _classes.OrderBy(c => c.Id).ToList();

            NewClass = new ClassInformationModel();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var classToDelete = _classes.FirstOrDefault(c => c.Id == id);
            if (classToDelete != null)
            {
                _classes.Remove(classToDelete);
                for (int i = 0; i < _classes.Count; i++)
                {
                    _classes[i].Id = i + 1;
                }
            }
            return RedirectToPage();
        }

        public IActionResult OnPostEdit(int id)
        {
            var classToEdit = _classes.FirstOrDefault(c => c.Id == id);
            if (classToEdit != null)
            {
                NewClass = new ClassInformationModel
                {
                    Id = classToEdit.Id,
                    ClassName = classToEdit.ClassName,
                    StudentCount = classToEdit.StudentCount,
                    Description = classToEdit.Description
                };
            }
            return Page();
        }
    }

    public class ExportRequest
    {
        public int CurrentPage { get; set; }
        public string? ClassNameFilter { get; set; }
        public int? StudentCountFilter { get; set; }
        public List<string>? SelectedIds { get; set; }
    }
}
