using ApplyCheckESosial.Extensions;
using ApplyCheckESosial.Models;
using ApplyCheckESosial.Services;
using ApplyCheckESosial.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApplyCheckESosial.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IESosialService _eSosialService;
        public IConfiguration Configuration { get; }

        public ServicesController(IESosialService eSosialService, IConfiguration configuration)
        {
            Configuration = configuration;
            _eSosialService = eSosialService;
        }

        [HttpGet]
        public IActionResult SearchResult()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Passport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Passport(PassportVM passport)
        {
            string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
            var client = HttpClientFactory.Create();
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"secret", Configuration["reCAPTCHA:SecretKey"]},
                    {"response", recaptchaResponse},
                    {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
                };

                HttpResponseMessage result = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                result.EnsureSuccessStatusCode();

                string apiResponse = await result.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    this.ModelState.AddModelError(string.Empty, "Təhlükəsizlik qutusunu işarələyin.");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Sistemdə xəta var");
            }

            string passportSeria = passport.SeriyaType + passport.SeriyaNumber;

            if (!ModelState.IsValid)
            {
                return View(passport);
            }

            var response = await _eSosialService.GetAsync(passportSeria, passport.Birthdate);

            if (response == null || response.Count < 1)
            {
                ModelState.AddModelError("", "Məlumatlarınız sistemdə tapılmadı");
                return View(passport);
            }

            Dictionary<string, List<ESosialServiceData>> resultModel = GetResponseBinding(response);

            return View("SearchResult", resultModel);
        }

        [HttpGet]
        public IActionResult ApplyNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyNumber(ApplyNumberVM number)
        {
            string recaptchaResponse = this.Request.Form["g-recaptcha-response"];
            var client = HttpClientFactory.Create();
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"secret", Configuration["reCAPTCHA:SecretKey"]},
                    {"response", recaptchaResponse},
                    {"remoteip", this.HttpContext.Connection.RemoteIpAddress.ToString()}
                };

                HttpResponseMessage result = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", new FormUrlEncodedContent(parameters));
                result.EnsureSuccessStatusCode();

                string apiResponse = await result.Content.ReadAsStringAsync();
                dynamic apiJson = JObject.Parse(apiResponse);
                if (apiJson.success != true)
                {
                    this.ModelState.AddModelError(string.Empty, "Təhlükəsizlik qutusunu işarələyin.");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "Sistemdə xəta var");
            }

            if (!ModelState.IsValid)
            {
                return View(number);
            }
            var response = await _eSosialService.GetAsync(number.DocNumber);

            if (response == null)
            {
                ModelState.AddModelError("", "Məlumatlarınız sistemdə tapılmadı");
                return View(number);
            }

            Dictionary<string, List<ESosialServiceData>> resultModel = GetResponseBinding(response);

            return View("SearchResult", resultModel);
        }

        private Dictionary<string, List<ESosialServiceData>> GetResponseBinding(List<ESosialServiceData> response)
        {
            Dictionary<string, List<ESosialServiceData>> resultModel = new Dictionary<string, List<ESosialServiceData>>();

            List<ESosialServiceData> otherData = new List<ESosialServiceData>();

            for (int i = 0; i < response.Count; i++)
            {
                ESosialServiceData eSosialServiceData = new ESosialServiceData
                {
                    ExecutorDepartment = Config.CheckNull(response[i].ExecutorDepartment),
                    Content = Config.CheckNull(response[i].Content),
                    IsClosed = response[i].IsClosed,
                    ReceivedDate = response[i].ReceivedDate,
                    ExecutorFullName = Config.CheckNull(response[i].ExecutorFullName),
                    Number = Config.CheckNull(response[i].Number),
                    ReceptMethod = Config.CheckNull(response[i].ReceptMethod),
                    Status = Config.CheckNull(response[i].Status),
                    Subject = Config.CheckNull(response[i].Subject)
                };

                if (response[i].ExecutorDepartment == null)
                {
                    otherData.Add(eSosialServiceData);
                }
                else
                {
                    if (!resultModel.ContainsKey(response[i].ExecutorDepartment))
                    {
                        resultModel.Add(response[i].ExecutorDepartment, new List<ESosialServiceData>());
                    }

                    resultModel[response[i].ExecutorDepartment].Add(eSosialServiceData);
                }
            }

            //Diger varmi? Sort en axira dusmesi ucun
            if (otherData.Count > 0)
            {
                resultModel.Add("Digər", otherData);
            }

            return resultModel;
        }

      
    }
}
