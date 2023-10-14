using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;

namespace JohnPenny.MSFS.SimConnectManager.REST.Pages
{
    public class RemoteModel : PageModel
    {
        public string IPInfo { get; set; }
        public System.Guid guid = System.Guid.NewGuid();

        public void OnGet()
        {
            IPInfo = $"Server IP info {Request.HttpContext.Connection.LocalIpAddress}  Port 4380 for HTTPS (8747 for HTTP)";
        }

        [HttpPost]
        public static ActionResult Submit()
        {
            //// Get the posted form values and add to list using model binding
            //IList<string> postData = new List<string> { m.Value1, m.Value2, m.Value3, m.Value4 };

            //using (var client = new HttpClient())
            //{
            //    // Assuming the API is in the same web application. 
            //    string baseUrl = HttpContext.Current
            //                                .Request
            //                                .Url
            //                                .GetComponents(UriComponents.SchemeAndServer, UriFormat.SafeUnescaped);
            //    client.BaseAddress = new Uri(baseUrl);
            //    int result = client.PostAsync("/api/values",
            //                                  postData,
            //                                  new JsonMediaTypeFormatter())
            //                        .Result
            //                        .Content
            //                        .ReadAsAsync<int>()
            //                        .Result;

            //    // add to viewmodel
            //    var model = new ViewModel
            //    {
            //        intValue = result
            //    };

            //return View(model);
            return new OkResult();
        }
    }
}