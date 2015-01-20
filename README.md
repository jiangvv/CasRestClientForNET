# CasRestClientForNET
A Cas Rest Client for .NET 

Get a service ticket from CAS (Jasig) server.

Validates a service ticket by CAS (Jasig) server.

It it written in C# and uses only RestSharp (restsharp.org)

Use CasTest.cs file to test your environment.

How to use

CasLogin casLogin = new CasLogin(userName, password, casUrl);

String serviceTicket = casLogin.getServiceTicket(serviceUrl);

CasValidate casValidate = new CasValidate(casUrl);

casValidate.validateServiceTicket(serviceUrl, serviceTicket);

Inspired by 
http://www.bmchild.com/2014/05/a-simple-cas-java-rest-client-example.html 

