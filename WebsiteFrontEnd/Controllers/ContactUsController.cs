using MessenceComFrontEnd.Models;
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

namespace MessenceComFrontEnd.Controllers
{
    [RoutePrefix("api/ContactUs")]
    public class ContactUsController : UmbracoApiController
    {
        [HttpPost]
        [Route("ProcessQuickContactUsEmail")]
        public IHttpActionResult ProcessQuickContactUsEmail( [FromBody] ContactUsRequestModel contactUsModel )
        {
            IHttpActionResult response;

            try
            {
                bool test = ModelState.IsValid;

                string _requestFromUrl = this.Request.Headers.Referrer.AbsoluteUri;
                string _fullName = contactUsModel.FullName;
                string _email = contactUsModel.Email;
                string _messageDetails = contactUsModel.Description;

                if (ValidateContactUs(_requestFromUrl, _fullName, _email, null, _messageDetails))
                {
                    try
                    {
                        WebsiteOperationsTools.SendContactUsEmailDetails(
                                                _requestFromUrl, _fullName, _email, null, _messageDetails);

                        response = Content<string>(HttpStatusCode.OK, "MF255");
                    }
                    catch (Exception exc)
                    {
                        response = Content<string>(HttpStatusCode.OK, "MF000");
                    }
                }
                else
                {
                    response = Content<string>(HttpStatusCode.OK, "MF000");
                }
            }
            catch (Exception exc)
            {
                response = Content<string>(HttpStatusCode.BadRequest, "Error.. " + exc.Message);
            }

            return response;
        }

        [HttpPost]
        [Route("ProcessFullContactUsEmail")]
        public IHttpActionResult ProcessFullContactUsEmail([FromBody] ContactUsRequestModel contactUsModel)
        {
            IHttpActionResult response;

            try
            {
                bool test = ModelState.IsValid;

                string _requestFromUrl = this.Request.Headers.Referrer.AbsoluteUri;
                string _fullName = string.Empty;
                if (string.IsNullOrEmpty(contactUsModel.FullName) && !string.IsNullOrEmpty(contactUsModel.FirstName))
                {
                    _fullName = contactUsModel.FirstName + " " + contactUsModel.LastName;
                }

                string _email = contactUsModel.Email;
                string _phone = contactUsModel.Phone;
                string _messageDetails = contactUsModel.Description;
                if (string.IsNullOrEmpty(contactUsModel.Description) && !string.IsNullOrEmpty(contactUsModel.Message))
                {
                    _messageDetails = contactUsModel.Message;
                }

                if (ValidateContactUs(_requestFromUrl, _fullName, _email , _phone, _messageDetails))
                {
                    try
                    {
                        WebsiteOperationsTools.SendContactUsEmailDetails(
                                                _requestFromUrl, _fullName, _email, _phone, _messageDetails);

                        response = Content<string>(HttpStatusCode.OK, "MF255");
                    }
                    catch (Exception exc)
                    {
                        response = Content<string>(HttpStatusCode.OK, "MF000");
                    }
                }
                else
                {
                    response = Content<string>(HttpStatusCode.OK, "MF000");
                }
            }
            catch (Exception exc)
            {
                response = Content<string>(HttpStatusCode.BadRequest, "Error.. " + exc.Message);
                //response.Content = new StringContent("An error occured! Please try again later.");
            }

            //response.Content = new StringContent("hello", Encoding.Unicode);
            //response.Headers.CacheControl = new CacheControlHeaderValue()
            //{
            //    MaxAge = TimeSpan.FromMinutes(20)
            //};
            return response;
            //return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge" };
        }

        private bool ValidateContactUs(string requestFromUrl, string fullName, string email, string phone, string messageDetails)
        {
            return true;
        }
    }
}