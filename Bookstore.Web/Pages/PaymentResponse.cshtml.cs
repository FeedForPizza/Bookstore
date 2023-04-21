using Bookstore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;

namespace Bookstore.Web.Pages
{
    
    public class PaymentResponseModel : PageModel
    {
        private readonly OrdersService _os;
        public string PublicKey { get; set; }
        private string SecretKey { get; set; }
        public PaymentResponseModel(OrdersService os,IConfiguration configuration)
        {
            _os = os;
            PublicKey = configuration["APIS:StripeKeys:PublicKey"];
            SecretKey = configuration["APIS:StripeKeys:SecretKey"];
        }
        public async Task OnGetAsync(string? payment_intent)
        {
            if (string.IsNullOrEmpty(payment_intent))
            {
                RedirectToPage("Payment");
            }
            // �������� ����� � appsettings.json, �� �� �� ���� ����� � ����
            // ����� ��� �� ���������� � �������� ����������
            StripeConfiguration.ApiKey = SecretKey;
            var service = new PaymentIntentService();
            var paymentIntent = await service.GetAsync(payment_intent);
            if (paymentIntent.Status == "succeeded")
            {
                // ��� ��� �������� ����������� �� ������� �� �������� ��������, ��� ���� �� � ������� �� ����
            // ������ �� ��������� �� ���� �� ������ ���������� �������
 await _os.SetOrderPaid(User.Identity.Name);
            }
        }
    }

}
