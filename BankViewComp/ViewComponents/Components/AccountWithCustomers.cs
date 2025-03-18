using Microsoft.AspNetCore.Mvc;
using BankViewComp.Models;

namespace BankViewComp.ViewComponents.Components
{
    public class AccountWithCustomers : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Account? category, Customer? product)
        {
            if (category != null && product != null)
            {
                if (category.Customers is null)
                {
                    category.Customers = new List<Customer>() { product };
                }
                else
                {
                    category.Customers.Add(product);
                }
                return View(category);
            }
            else
            {
                return View(new Account());
            }
        }
    }
}
