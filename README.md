# Playwright-CSharp-Web-Automation
BDD test automation Framework with Playwright and Extent Report

<b> Run the following commands in the terminal - </b>

Install - choco install powershell-core

dotnet tool update --global PowerShell

dotnet tool install --global Microsoft.Playwright.CLI

playwright install

<b> Viewing the trace - </b>

You can open the saved trace using Playwright CLI or in your browser on trace.playwright.dev.

To run the test in parallel -

dotnet test -- NUnit.Parallelize.Workers=4
