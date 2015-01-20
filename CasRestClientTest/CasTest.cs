using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CasRestClient;

namespace CasRestClientTest
{
    [TestClass]
    public class CasTest
    {

        String userName = "admin";
        String password = "admin";
        String casUrl = "https://localhost/cas";
        String serviceUrl = "http://www.google.com";

        
        [TestMethod]
        public void test_get_service_ticket()
        {
            CasLogin casLogin = new CasLogin(userName, password, casUrl);
            String serviceTicket = casLogin.getServiceTicket(serviceUrl);

            Console.Out.WriteLine("serviceTicket:" + serviceTicket);

        }

        [TestMethod]
        public void test_get_service_ticket_and_validate()
        {
            CasLogin casLogin = new CasLogin(userName, password, casUrl);
            String serviceTicket = casLogin.getServiceTicket(serviceUrl);

            CasValidate casValidate = new CasValidate(casUrl);
            casValidate.validateServiceTicket(serviceUrl, serviceTicket);

        }


    }
}
