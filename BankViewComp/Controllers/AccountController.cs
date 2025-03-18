using BankViewComp.Models;
using BankViewComp.ViewComponents.Components;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace BankViewComp.Controllers
{
    public class AccountController : Controller
    {
        private readonly BankingContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AccountController(BankingContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Account.Include(c => c.Customers!).ThenInclude(p => p.Type).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["Models"] = new SelectList(_context.Type, "Tid", "AccType");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccountId,Branch,Customers")] Account category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Account
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Account.FindAsync(id);
            if (category != null)
            {
                _context.Account.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> PostForViewComponent(Account category, Customer product)
        {
            ViewData["ModelId"] = new SelectList(_context.Type, "Tid", "AccType", product.Tid);
            Types? model = await _context.Type.FindAsync(product.Tid);
            if (category is null)
            {
                return ViewComponent(typeof(AccountWithCustomers), new object[] { new Account(), new Customer() });
            }
            else
            {
                if (category.Customers is not null)
                {
                    foreach (Customer p in category.Customers!)
                    {
                        if (p.Tid > 0)
                        {
                            Types aModel = await _context.Type.SingleAsync(m => m.Tid == p.Tid);
                            p.Type = aModel;
                        }
                    }
                }
                if (product is null)
                {
                    return ViewComponent(typeof(AccountWithCustomers), new object[] { category, new Customer() });
                }
                else
                {
                    product.Type = model;
                    return ViewComponent(typeof(AccountWithCustomers), new { category, product });
                }
            }
        }

        [HttpPost]
        public JsonResult UploadImage(IFormFile file)
        {
            if (file != null)
            {
                string fileName = Path.Combine(_hostingEnvironment.WebRootPath, "images", file.FileName);
                file.CopyTo(new FileStream(fileName, FileMode.Create));
            }
            string url = HttpContext.Request.GetEncodedUrl();
            return Json("http://localhost:5181/images/" + file!.FileName);
        }
        private bool CategoryExists(int id)
        {
            return _context.Account.Any(e => e.AccountId == id);
        }
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Account
                .Include(c => c.Customers)
                    .ThenInclude(p => p.Type)
                .FirstOrDefaultAsync(m => m.AccountId == id);

            if (category == null)
            {
                return NotFound();
            }

            ViewData["Models"] = new SelectList(_context.Type, "Tid", "AccType");
            return View(category);
        }

        [HttpGet]
        public IActionResult GetProductForEdit(int productId)
        {
            var product = _context.Customer
                .Include(p => p.Type)
                .FirstOrDefault(p => p.CustomerId == productId);

            if (product == null)
            {
                return NotFound();
            }

            // Prepare models dropdown for the form
            ViewBag.Models = _context.Type.Select(m => new SelectListItem
            {
                Value = m.Tid.ToString(),
                Text = m.AccType
            }).ToList();

            return PartialView("_EditProduct", product);
        }
        [HttpPost]
        public IActionResult UpdateProduct(Customer product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = _context.Customer.Find(product.CustomerId);

                    if (existingProduct == null)
                    {
                        return NotFound();
                    }

                    // Update product properties
                    existingProduct.CustomerName = product.CustomerName;
                    existingProduct.Deposit = product.Deposit;
                    existingProduct.DateCreate = product.DateCreate;
                    existingProduct.IsAvailable = product.IsAvailable;
                    existingProduct.ImageUrl = product.ImageUrl;
                    existingProduct.Tid = product.Tid;

                    _context.Update(existingProduct);
                    _context.SaveChanges();

                    // Get updated category with products
                    var category = _context.Account
                        .Include(c => c.Customers)
                        .ThenInclude(p => p.Type)
                        .FirstOrDefault(c => c.AccountId == existingProduct.AccountId);

                    return ViewComponent("CategoryWithProducts", new { category = category, product = new Customer() });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to update product: " + ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}
