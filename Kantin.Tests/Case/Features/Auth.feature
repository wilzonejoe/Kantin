Feature: Auth

@basic
Scenario: As a user, i should be able to register successfully
	When I registered with <fullname>, <username> and <password>
	Then response code should be 200
	And LoginResult should be successful
	And Database contains account with <fullname> and <username>
	And I can login successfully with <username> and <password>

	Examples: 
	| fullname    | username | password |
	| myphunguyen | jimmy    | -Test123 |

@exception
Scenario: As a user, i should not be able to register if username has taken
	Given I registered with <fullname>, <username> and <password>
	When I registered with <fullname>, <username> and <password>
	Then response code should be 409

	Examples: 
	| fullname    | username | password |
	| myphunguyen | jimmy    | -Test123 |

@exception
Scenario: As a user, i should not be able to register if one of the field is not filled
	When I registered with <fullname>, <username> and <password>
	Then response code should be 400

	Examples: 
	| fullname    | username | password |
	| myphunguyen | jimmy    |          |
	| myphunguyen |          | -Test123 |
	|             | jimmy    | -Test123 |