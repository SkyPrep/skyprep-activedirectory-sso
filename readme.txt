SkyPrep ActiveDirectory Connector for Windows Server
----------------------------------------------------

The connector is provided as-is. 

Due to the complex and differing nature of Windows Server environments, SkyPrep Inc cannot provide support for this package. We provide the source code so that you may see how we are authenticating users.

The ActiveDirectory Connector (ADC) makes use of Windows Pass-through Authentication. The application will get the users email from ActiveDirectory and use that to make an API call to SkyPrep (https://api.skyprep.io/admin/api/get_otl_key) to get a one-time login URL.

At no point are your AD passwords passed to the ADC.

The server/controller must have access to the AD, and the IIS application must be running as a user with these permissions.

Requirements
-------------

IIS 7.0, Windows Server 2012+, .NET 4.5


IIS Settings
==============================
-> You need to turn on both ASP.NET Impersonation and Windows Authentication 
-> Change app pool pipe line from integrated to classic.

Edit Application Settings

set acct_key as your primary SkyPrep Domain
set api_key as your SkyPrep API key
set autocreate to 1 if you'd like users to be automatically created

Overview
---------

The package is an IIS package that uses the SkyPrep API to automatically enroll users into your training portal.

1. You need to turn on both ASP.NET Impersonation and Windows Authentication in the package settings
2. You need to change the Application Pool to .NET 4.5 Classic (Classic Pipeline) 
3. Edit the Application Settings with your SkyPrep acct_key (i.e. domain), API key and whether to auto-create users (set to 1/0 for true/false)
4. Host it at a URL on your Intranet

If the user is connecting to the Intranet URL and is already signed into Windows using AD, it should automatically log them in.

