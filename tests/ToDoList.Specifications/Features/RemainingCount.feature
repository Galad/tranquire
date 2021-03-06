﻿Feature: RemainingCount	
	In order remind me how many task is left
	As a user
	I want to see the number of remaining items

@UI
Scenario: Remaining count when having items
	Given I have a list with the items "buy some milk,feed the dog,prepare the lunch"		
	When I open the application
	Then I should have 3 items remaining

@UI
Scenario: Remaining count when adding an item
	Given I have a list with the items "buy some milk,feed the dog,prepare the lunch"
	And the application in opened
	When I add the item "wash the dishes"
	Then I should have 4 items remaining
