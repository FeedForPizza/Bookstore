using Bookstore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Bookstore.Web.Pages
{
    
    public class PaymentModel : PageModel
    {
        public string PublicKey { get; set; }
        private string SecretKey { get; set; }
        private readonly OrdersService _os;

        public PaymentModel(OrdersService os,IConfiguration configuration)
        {
            _os = os;
            PublicKey = configuration["APIS:StripeKeys:PublicKey"];
            SecretKey = configuration["APIS:StripeKeys:SecretKey"];
        }
        [BindProperty]
        public string StripeToken { get; set; }
        public async Task OnGetAsync()
        {
            var order = await _os.GetLatestOrderByUser(User.Identity.Name);
            if (order.IsPaid)
            {
                // �������� ��������� �� ������, �� ��������� ���� � ���� �������
                RedirectToPage("/");
            }
            // �������� ����� � appsettings.json, �� �� �� ���� ����� � ����
            // ����� ��� �� ���������� � �������� ���������� � Configuration
            StripeConfiguration.ApiKey = SecretKey;
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.OrderDetails.Sum(x => x.Quantity *
               x.UnitPrice)) * 100, // ��� � � ��������, ������ * 100
                Currency = "bgn",
                Description = "������� �� ����� #" + order.Id,
                AutomaticPaymentMethods = new
           PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };
            var service = new PaymentIntentService();
            var paymentIntent = service.Create(options);
            this.StripeToken = paymentIntent.ClientSecret;
            // ��� ���� �� � ����� ���� �� ������� ���� ������ � ���������, �� �� ���� ����� �� � ������� �� ����
        }
    }

}
