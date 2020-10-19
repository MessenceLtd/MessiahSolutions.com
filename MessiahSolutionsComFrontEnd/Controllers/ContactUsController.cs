using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace MessiahSolutionsComFrontEnd.Controllers
{
    public class ContactUsController : UmbracoApiController
    {
        //[HttpGet]
        //[Route("umbraco/api/members")]
        public IEnumerable<string> GetAllMembers()
        {
            return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge" };
            
        }

        [HttpPost]
        public HttpResponseMessage ProcessDummyEmail()
        {
            HttpResponseMessage response;

            try
            {
                WebsiteOperationsTools.SendContactUsEmailDetails("bla", "sdfsfs", "ggwp@gmail.com", "what is dis wierd website!");

                response = Request.CreateResponse(HttpStatusCode.OK, "Sent successfully!");
            }
            catch (Exception exc)
            {
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response.Content = new StringContent("An error occured! Please try again later.");
            }

            //response.Content = new StringContent("hello", Encoding.Unicode);
            //response.Headers.CacheControl = new CacheControlHeaderValue()
            //{
            //    MaxAge = TimeSpan.FromMinutes(20)
            //};
            return response;
            //return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge" };
        }
    }

    
}