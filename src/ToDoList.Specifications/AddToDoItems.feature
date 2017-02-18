Feature: AddToDoItems
	In order remind me what to do
	As a user
	I want to add to-do items

Scenario: Add an item
	Given I have an empty to-do list
	When I add the item "buy some milk"
	Then the to-do items list should contain "buy some milk"

Scenario: Remove an item
	Given I have a list with the items "buy some milk,feed the dog,prepare the lunch"	
	When I remove the item "buy some milk"
	Then the to-do items list should not contain "buy some milk"

Scenario: Add same item twice
	Given I have a list with the items "buy some milk,feed the dog,prepare the lunch"	
	When I add the item "buy some milk"
	Then the to-do items list should contain "buy some milk" 2 times

