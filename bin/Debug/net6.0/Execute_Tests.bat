set DIR=%~dp0
chdir /d %DIR% 

SETLOCAL
set BROWSER=webkit
set HEADED=0
dotnet test PlaywrightAutomator.dll --filter "TestCategory=Login | TestCategory=Logout"

Timeout 10