using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string ApiResponse { get; private set; }
        public string ApiUrl { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task OnPost()
        {
            this.ApiUrl = Request.Form["url"];
            var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(4);
            try
            {
                var response = await client.GetAsync(this.ApiUrl);
                this.ApiResponse = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                this.ApiResponse = JsonConvert.SerializeObject(ex, Formatting.Indented);   
            }
        }
    }
}