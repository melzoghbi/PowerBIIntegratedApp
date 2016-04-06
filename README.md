# PowerBI Integrated App
Power BI integrated in ASP.NET MVC App

How to run the application:

1) Register your application in Azure AD, here are the configurations we need to update ASP.NET MVC app with:

Client Id

Secret Id

Return Url: If you want to change it from the original app code.


2) Login to your PowerBI (app.powerbi.com) and get the Report Id you want to embed in the app:

Select a report that you would like to embed, grab the report Id from the browser url.
Below screen show shows a selected report "RPT1" and highlighted ReportId we will embed in our app.

3) Open ASP.NET MVC Project settings in VS 2015 and update Client Id and Secret Id from Azure Portal after registering your app.



4) Open index.cshtml and update iframe src attribute by adding the Report Id we captured in step #2.

 <iframe ID="iFrameEmbedReport" src="https://app.powerbi.com/reportEmbed?reportId=11156ed2-ede7-4b60-ac26-10cda004bdee" height="768px" width="1024px" frameborder="1" seamless></iframe>
 5) Run the app, PowerBI report is showing after your login to your Office 365 tenant. 
 
 
 A detailed blog post has been posted here:
 http://www.mostafaelzoghbi.com/2016/04/integrate-power-bi-report-into-app.html 
