# The Online Pet Store Administration System
### Built by a noob, for noobs.

##How to properly port your ASP .NET 5 RC 1 Project to ASP Core

1. Make sure you copy the whole project.json I have.

90% of the file has changed and fixing it file by file has been proven impossible because
nuGet might fail sometimes.

2. Web.config

Replace the whole file with this

<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <handlers>
      <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
    </handlers>
    <aspNetCore processPath="%LAUNCHER_PATH%" arguments="%LAUNCHER_ARGS%" forwardWindowsAuthToken="false" stdoutLogEnabled="false" />
  </system.webServer>
</configuration>

3. launchSettings.json

Search for all of the "Hosting:Environment" and replace it with "ASPNETCORE_ENVIRONMENT"

4. global.json

Set the SDK version to "1.0.0-preview1-002702"


=== Once you're done with everything till here, you need to replace LOTS of imports from your various classes. ===

You’re going to want to replace the namespaces, in fact…I’d just do a search and replace from “using Microsoft.AspNet.” to “using Microsoft.AspNetCore.”. 

There will be other namespaces that needs to be changed, but on the whole, adding Core will solve many problems. For example:

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

You’ll find that other systems need renaming with Core but the main ones (Entity Framework and Identity) are covered next.

You'll want to apply the same think to

using Microsoft.EntityFramework to using Microsoft.EntityFrameworkCore.

Be sure to use the mass edit and replace function in Visual Studio
to make these changes. [Use Ctrl + H]

Make sure you are checking cases and once you have input the correct
text to replace, click Replace All.


For the last few segments for updating your project,
This link will help.

https://wildermuth.com/2016/05/17/Converting-an-ASP-NET-Core-RC1-Project-to-RC2
