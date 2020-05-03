Feature: Organisation

@basic
Scenario: As a user, i should be able to create organisation after register
	Given I registered with <fullname>, <username> and <password>
	When I created an organisation with name <organisation>
	Then response code should be 201

	Examples: 
	| fullname    | username | password | organisation |
	| myphunguyen | jimmy    | -Test123 | nphumy98     |

@exception
Scenario: As a user, i should not be able to create organisation if name is taken
	Given I registered with <fullname>, <username> and <password>
	And I created an organisation with name <organisation>
	When I created an organisation with name <organisation>
	Then response code should be 409

	Examples: 
	| fullname    | username | password | organisation |
	| myphunguyen | jimmy    | -Test123 | nphumy98     |

@exception
Scenario: As a user, i should not be able to create organisation without access token
	When I created an organisation with name <organisation>
	Then response code should be 401

	Examples: 
	| organisation |
	| nphumy98     |

@exception
Scenario: As a user, i should not be able to create organisation with no organisation name
	Given I registered with <fullname>, <username> and <password>
	When I created an organisation with name <organisation>
	Then response code should be 400

	Examples: 
	| fullname    | username | password | organisation |
	| myphunguyen | jimmy    | -Test123 |              |