IIS Settings
==============================
-> You need to turn on both Asp.Net Impersonation and Windows Authentication 
-> Please change app pool pipe line from integrated to classic.

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

