using Bookstore.Services;
using Bookstore.Services.DTO.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Web.Pages
{
    
    public class CartModel : PageModel
    {
        private readonly OrdersService _os;
        public CartModel(OrdersService os)
        {
            _os = os;
        }
        public OrderDto Order { get; set; }
        public async void OnGet()
        {
            this.Order = await _os.GetLatestOrderByUser(User.Identity.Name);
        }
    }

}
