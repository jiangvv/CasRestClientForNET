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
    /// <summary>
    /// Creates a service ticket to login a service that uses CAS (Jasig) as authentication system.
    /// </summary>
    public class CasLogin
    {
        public static String CAS_TICKETS_PATH = "/v1/tickets";
        public static String HEADER_NAME_LOCATION = "Location";

        private String username;
        private String password;
        private String casUrl;
        private String casTicketsUrl;

        public CasLogin(String username, String password, String casUrl) {
            this.username = username;
            this.password = password;
            this.casUrl = casUrl;
            this.casTicketsUrl = casUrl + CAS_TICKETS_PATH;
        }

        /// <summary>
        /// Creates an ONE TIME ONLY service ticket for a serviceUrl.
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <returns></returns>
        public String getServiceTicket(String serviceUrl)  {
            
            String tgtLocation = getTicketGrantingTicketLocation(username, password);

            return getServiceTicketInternal(tgtLocation, serviceUrl);

        }

        /// <summary>
        /// Gets Ticket Granting Ticket (TGT) Location from CAS server.
        /// We will user TGT location to get service tickets from it.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private String getTicketGrantingTicketLocation(String username, String password)
        {
            System.Diagnostics.Debug.WriteLine("casTicketsUrl:" + casTicketsUrl);

            var client = new RestClient(casTicketsUrl);

            var request = new RestRequest(Method.POST);
            request.AddParameter("username", username);
            request.AddParameter("password", password);

            IRestResponse response = client.Execute(request);
            handleRemoteError(response, HttpStatusCode.Created, "An error occured while getting TGT from Cas Server");

            String tgtLocation = (String)response.Headers.Where(p => HEADER_NAME_LOCATION.Equals(p.Name)).SingleOrDefault().Value;
            System.Diagnostics.Debug.WriteLine("tgtLocation:" + tgtLocation);

            return tgtLocation;

        }

        /// <summary>
        /// It gets service tickets from TGT location for a serviceUrl.
        /// </summary>
        /// <param name="tgtLocation"></param>
        /// <param name="serviceUrl"></param>
        /// <returns></returns>
        private String getServiceTicketInternal(String tgtLocation, String serviceUrl)
        {
            System.Diagnostics.Debug.WriteLine("tgtLocation:" + tgtLocation);
            System.Diagnostics.Debug.WriteLine("serviceUrl:" + serviceUrl);

            var client = new RestClient(tgtLocation);
            
            var request = new RestRequest(Method.POST);
            request.AddParameter("service", serviceUrl);
            request.AddParameter("method", "POST");
            
            IRestResponse response = client.Execute(request);
            handleRemoteError(response, HttpStatusCode.OK,  "An error occured while getting Service Ticket from Cas Server");
            
            String serviceTicket = response.Content;
            System.Diagnostics.Debug.WriteLine("serviceTicket:" + serviceTicket);

            return serviceTicket;

        }

        private void handleRemoteError(IRestResponse response, HttpStatusCode expectedStatusCode, string message)
        {
            if (response.ErrorException != null)
            {
                throw new Exception(message, response.ErrorException);
            }

            bool success = response.StatusCode.Equals(expectedStatusCode);
            if (!success)
            {
                message = message + "[Http Status Code =" + response.StatusCode + "]";
                throw new Exception(message);
            }

        }
    
    }
   
}
