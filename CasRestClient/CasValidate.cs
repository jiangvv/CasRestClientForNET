using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace CasRestClient
{
    public class CasValidate
    {
        private String casUrl;

        public CasValidate(String casUrl)
        {
            this.casUrl = casUrl;
        }

        /// <summary>
        /// Validates serviceTicket against s
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <param name="serviceTicket"></param>
        public void validateServiceTicket(String serviceUrl, String serviceTicket)
        {
            String url = casUrl + "?ticket=" + serviceTicket + "&service=" + serviceUrl;

            var client = new RestClient(url);

            var request = new RestRequest(Method.GET);

            IRestResponse response = client.Execute(request);
            if (response.ErrorException != null)
            {
                throw new Exception("An error occured while validating service ticket from Cas Server", response.ErrorException);
            }

            bool success = response.StatusCode.Equals(HttpStatusCode.OK);
            if (!success) {
                String message = "An error occured while validating service ticket from Cas Server. " + 
                                 "[Http Status Code =" + response.StatusCode + "]";
                throw new Exception(message);
            }


        }
    
    }
   
}
