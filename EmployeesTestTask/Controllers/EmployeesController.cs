using EmployeesTestTask.Data;
using EmployeesTestTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeesTestTask.Controllers;

public class EmployeesController: Controller
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(
            string searchName, 
            bool? isActive, 
            string employmentStatus,
            int page = 1,
            string sortBy = "LastName",
            bool sortAscending = true)
        {
            const int pageSize = 20;

            var query = _context.Employees
                .Include(e => e.EmploymentPeriods)
                .AsQueryable();

            // Фильтрация по ФИО
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(e => 
                    e.FirstName.Contains(searchName) ||
                    e.LastName.Contains(searchName) ||
                    (e.MiddleName != null && e.MiddleName.Contains(searchName)));
            }

            // Фильтрация по активности
            if (isActive.HasValue)
            {
                query = query.Where(e => e.IsActive == isActive.Value);
            }

            // Фильтрация по статусу трудоустройства
            if (!string.IsNullOrEmpty(employmentStatus) && employmentStatus != "All")
            {
                var today = DateTime.Today;
                if (employmentStatus == "Employed")
                {
                    query = query.Where(e => e.EmploymentPeriods.Any(ep => 
                        ep.StartDate <= today && (ep.EndDate == null || ep.EndDate > today)));
                }
                else if (employmentStatus == "Dismissed")
                {
                    query = query.Where(e => e.EmploymentPeriods.All(ep => 
                        ep.EndDate != null && ep.EndDate <= today));
                }
            }

            // Сортировка
            query = sortBy?.ToLower() switch
            {
                "id" => sortAscending ? query.OrderBy(e => e.Id) : query.OrderByDescending(e => e.Id),
                "firstname" => sortAscending ? query.OrderBy(e => e.FirstName) : query.OrderByDescending(e => e.FirstName),
                "dateofbirth" => sortAscending ? query.OrderBy(e => e.DateOfBirth) : query.OrderByDescending(e => e.DateOfBirth),
                "createddate" => sortAscending ? query.OrderBy(e => e.CreatedDate) : query.OrderByDescending(e => e.CreatedDate),
                _ => sortAscending ? query.OrderBy(e => e.LastName) : query.OrderByDescending(e => e.LastName)
            };

            // Получаем общее количество для пагинации
            var totalCount = await query.CountAsync();

            // Пагинация
            var employees = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Передаем параметры фильтрации в ViewBag для сохранения в форме
            ViewBag.SearchName = searchName;
            ViewBag.IsActive = isActive;
            ViewBag.EmploymentStatus = employmentStatus;
            ViewBag.Page = page;
            ViewBag.SortBy = sortBy;
            ViewBag.SortAscending = sortAscending;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedDate = DateTime.Now;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.EmploymentPeriods)
                .FirstOrDefaultAsync(e => e.Id == id);
                
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // POST: Employees/ToggleStatus/5
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = !employee.IsActive;
                await _context.SaveChangesAsync();
            }

            // Возвращаем к фильтру, который был применен
            return RedirectToAction(nameof(Index), new {
                searchName = ViewBag.SearchName,
                isActive = ViewBag.IsActive,
                employmentStatus = ViewBag.EmploymentStatus,
                page = ViewBag.Page
            });
        }

        // POST: Employees/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new {
                searchName = ViewBag.SearchName,
                isActive = ViewBag.IsActive,
                employmentStatus = ViewBag.EmploymentStatus,
                page = ViewBag.Page
            });
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }