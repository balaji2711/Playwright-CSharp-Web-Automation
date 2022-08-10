set DIR=%~dp0
chdir /d %DIR% 

livingdoc test-assembly PlaywrightAutomator.dll -t TestExecution.json

LivingDoc.html

Timeout 10