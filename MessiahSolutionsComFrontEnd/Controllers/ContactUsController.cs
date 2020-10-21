using MessiahSolutionsComFrontEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using Umbraco.Web.WebApi;

namespace MessiahSolutionsComFrontEnd.Controllers
{
    public class ContactUsController : UmbracoApiController
    {
        [HttpPost]
        public HttpResponseMessage ProcessContactUsEmail( [FromBody] ContactUsRequestModel contactUsModel )
        {
            HttpResponseMessage response;

            try
            {
                bool test = ModelState.IsValid;


                string _requestFromUrl = this.Request.RequestUri.AbsoluteUri;
                string _fullName = contactUsModel.FullName;
                string _email = contactUsModel.Email;
                string _messageDetails = contactUsModel.Description;

                Thread.Sleep(2000); // simulate heavy email sending.. 
                //WebsiteOperationsTools.SendContactUsEmailDetails(
                //    requestFromUrl, fullName, email, messageDetails);

                //response = Request.CreateResponse(HttpStatusCode.OK, "MF000");
                if (string.IsNullOrEmpty(_fullName))
                    response = Request.CreateResponse(HttpStatusCode.OK, "MF255");
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, "MF000");
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