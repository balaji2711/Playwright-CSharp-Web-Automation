﻿@Logout
Feature: Logout
	Check logout scenario in sauce demo application

@Sanity
@Regression
Scenario Outline: Verify the user is able to logout into sauce demo application
	When I enter the username <userName> and password <password>
	And I click on login
	Then I user logged in to the application successfully

	Examples: 
	| userName        | password       |
	| "standard_user" | "secret_sauce" |