using AspNetCoreHero.ToastNotification.Abstractions;
using hw6.Data;
using hw6.Data.Entities;
using hw6.Interfaces;
using hw6.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace hw6.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Expense> _expenseRepository;
        private readonly INotyfService _toastNotification;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IRepository<Category> categoryRepository, IRepository<Expense> expenseRepository, INotyfService toastNotification)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _expenseRepository = expenseRepository;
            _toastNotification = toastNotification;

        }
        
        public async Task<IActionResult> Category(int page = 1, int pageSize = 5)
        {
            List<CategoryViewModel> viewModels = new List<CategoryViewModel>();
            List<Category> categories = await _categoryRepository.GetJoinEntities("Expenses").ToListAsync();
            foreach (Category category in categories)
            {
                decimal totalMoney = 0;
                var exp = category.Expenses;
                foreach (Expense expense in category.Expenses)
                {
                    totalMoney += expense.Cost;
                }
                viewModels.Add(new CategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                    TotalMoney = totalMoney
                });
            }

            var data = PageData.GetPage(viewModels, await _categoryRepository.TotalCountOfEntitiesAsync(), page, pageSize);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(string name)
        {
            if (await _categoryRepository.AddAsync(new Category() { Name = name }))
            {
                await _categoryRepository.SaveChangesAsync();
                _toastNotification.Success($"{name} category created");
                return RedirectToAction("Category");
            }
            _toastNotification.Error("Category with same name exists");
            return RedirectToAction("Category");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int id, string name)
        {
            if (_categoryRepository.Update(new Category() {Id = id, Name = name }))
            {
                await _categoryRepository.SaveChangesAsync();
                _toastNotification.Success($"Category updated");
                return RedirectToAction("Category");
            }
            _toastNotification.Error("Error occurred");
            return RedirectToAction("Category");
        }

        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (await _categoryRepository.DeleteAsync(int.Parse(id)))
            {
                await _categoryRepository.SaveChangesAsync();
                _toastNotification.Success($"Category {id} deleted");
                return RedirectToAction("Category");
            }
            _toastNotification.Error("Error occurred");
            return RedirectToAction("Category");
        }

        public async Task<IActionResult> Expense(int page = 1, int pageSize = 5)
        {
            List<Expense> expenses = await _expenseRepository.GetEntititesByPage(page, pageSize).ToListAsync();
            List<ExpenseViewModel> viewModels = new List<ExpenseViewModel>();
            foreach (Expense expense in expenses)
            {
                viewModels.Add(new ExpenseViewModel()
                {
                    Id = expense.Id,
                    Cost = expense.Cost,
                    Comment = expense.Comment,
                    Date = expense.Date,
                    CategoryName = (await _categoryRepository.FindByIdAsync(expense.CategoryId)).Name
                });
            }
            var data = PageData.GetPage(viewModels, await _expenseRepository.TotalCountOfEntitiesAsync(), page, pageSize);
            return View(data);
        }

        public async Task<IActionResult> CreateExpense()
        {
            List<Category> categories = await _categoryRepository.GetAllEntities().ToListAsync();
            NewExpenseViewModel viewModel = new NewExpenseViewModel()
            {
                AvailableCategories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense(string cost, int categoryId, string comment)
        {
            if(await _expenseRepository.AddAsync(new Expense() { Cost = Convert.ToDecimal(cost), CategoryId = categoryId, Comment = comment}))
            {
                await _expenseRepository.SaveChangesAsync();
                _toastNotification.Success($"Expense created");
                return RedirectToAction("Expense");
            }
            _toastNotification.Error("Error occurred");
            return RedirectToAction("Expense");
        }

        public async Task<IActionResult> UpdateExpense(int id)
        {
            List<Category> categories = await _categoryRepository.GetAllEntities().ToListAsync();
            Expense expense = await _expenseRepository.FindByIdAsync(id);
            NewExpenseViewModel viewModel = new NewExpenseViewModel() 
            {
                Id = expense.Id,
                Cost = Convert.ToDecimal(expense.Cost),
                CategoryId = expense.CategoryId,
                Comment = expense.Comment,
                AvailableCategories = categories
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateExpense(int cur_id, string cost, int categoryId, string comment)
        {
            if (_expenseRepository.Update(new Expense() { Id = cur_id, Cost = Convert.ToDecimal(cost), CategoryId = categoryId, Comment = comment }))
            {
                await _expenseRepository.SaveChangesAsync();
                _toastNotification.Success($"Expense updated");
                return RedirectToAction("Expense");
            }
            _toastNotification.Error("Error occurred");
            return RedirectToAction("Expense");
        }

        public async Task<IActionResult> DeleteExpense(string id)
        {
            if (await _expenseRepository.DeleteAsync(int.Parse(id)))
            {
                await _expenseRepository.SaveChangesAsync();
                _toastNotification.Success($"Expense {id} deleted");
                return RedirectToAction("Expense");
            }
            _toastNotification.Error("Error occurred");
            return RedirectToAction("Expense");
        }

        public async Task<IActionResult> MonthlyExpense()
        {
            List<Expense> allExpenses = await _expenseRepository.GetAllEntities().ToListAsync();
            List<ExpenseViewModel> monthlyExpenseViewModels = new List<ExpenseViewModel>();
            foreach(Expense expense in allExpenses)
            {
                if (expense.Date >= DateTime.Now.AddMonths(-1))
                {
                    monthlyExpenseViewModels.Add(new ExpenseViewModel()
                    {
                        Id = expense.Id,
                        Cost = expense.Cost,
                        Comment = expense.Comment,
                        Date = expense.Date,
                        CategoryName = (await _categoryRepository.FindByIdAsync(expense.CategoryId)).Name
                    });
                }
            }
            return View(monthlyExpenseViewModels);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}