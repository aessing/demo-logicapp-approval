using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Html;

namespace DemoLogicAppApprovalWeb.Pages
{
    public class IndexModel : PageModel
    {
        private IConfiguration _configuration;
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Required(ErrorMessage = "First name required")]
        [StringLength(40, ErrorMessage = "First name is too long")]
        [BindProperty]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Last name required")]
        [StringLength(40, ErrorMessage = "Last name is too long")]
        [BindProperty]
        public string name { get; set; }

        [Required(ErrorMessage = "E-Mail-Adress required")]
        [EmailAddress(ErrorMessage = "Invalid E-Mail-Adress")]
        [BindProperty]
        public string mail { get; set; }

        [Required(ErrorMessage = "Cost Center Code required")]
        [StringLength(10, ErrorMessage = "Cost Center Code must between 4-10 numbers long", MinimumLength = 4)]
        [BindProperty]
        public string costcenter { get; set; }

        [Required(ErrorMessage = "Reason required")]
        [StringLength(4000, ErrorMessage = "Reason is too long")]
        [BindProperty]
        public string reason { get; set; }

        public HtmlString submitResponse { get; set; }
        
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid) { 
                var FormData = new Dictionary<string, string>();
                FormData.Add("firstname", Request.Form[nameof(firstname)]);
                FormData.Add("name", Request.Form[nameof(name)]);
                FormData.Add("mail", Request.Form[nameof(mail)]);
                FormData.Add("costcenter", Request.Form[nameof(costcenter)]);
                FormData.Add("reason", Request.Form[nameof(reason)]);

                string statusMessage;

                // requires using System.Net.Http;
                var client = new HttpClient();
                // requires using System.Text.Json;
                var jsonData = JsonSerializer.Serialize(new
                {
                    form = FormData
                });

                HttpResponseMessage result = await client.PostAsync(
                    // Requires DI configuration to access app settings. See https://docs.microsoft.com/azure/app-service/configure-language-dotnetcore#access-environment-variables
                    _configuration["LOGIC_APP_URL"],
                    new StringContent(jsonData, Encoding.UTF8, "application/json"));

                var statusCode = result.StatusCode.ToString();

                if (statusCode == "OK")
                {
                    statusMessage = $"<div class=\"form-return-success\">Success! Your request was sent to an approver.</div>";
                }
                else
                {
                    statusMessage = $"<div class=\"form-return-failure\">Failure! Please try again!</div>";
                }

                var statusMessageHTML = new HtmlString(statusMessage);
                this.submitResponse = statusMessageHTML;
            }
            return Page();
        }
    }

}
