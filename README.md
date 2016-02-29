# open-data
The application is built using C# programming language with REST Windows Communication Foundation (WCF) Serviec which is a set of APIs used in building in service-oriented applications.  Microsoft has a good Getting Started tutorial on WCF at: https://msdn.microsoft.com/en-us/library/ms734712(v=vs.110).aspx 
and on how to create a WCF data service at: https://msdn.microsoft.com/en-us/library/ee373841(v=vs.110).aspx
The instructions below are to configure the WCF for a service called Service1 under Visual Studio 2012 in c#.
Start Visual Studio
Click File ->New Project 

Once the new project is created, under the root of the Solution Explorer right click to create a folder called Service
Right click the Service folder and click Add and select WCF Service, and call it Service1 (you can, of course, call it anything you like)


It creates the interface file IService1.cs and the actual implementation fiiles Service1.svc and Service1.svc.cs
Right click the Service folder and create Web.config file
Copy the contents from the Web.config file found under the Service folder that I have uploaded in my source code  and then modify the following code in Web.config based on whatever  your project namd and service name are: 
<service name="pensions.Service.Service1">
        <endpoint address="" binding="webHttpBinding" contract="pensions.Service.IService1"
                  bindingConfiguration="ApiQuotaBinding" behaviorConfiguration="webHttpBehavior"/>
      </service>
Once WCF is configured, compile the project and run the URL similar to http://localhost:50570/Service/Service1.svc/p11XMLEn/2014 in your browser.  The resulting XML output  will look like:

On the Open Data portal, all the resulting XML and CSV outputs of Service1 can be found at:  http://open.canada.ca/data/en/dataset/2897cef1-4270-4546-80fc-e38d43758164


