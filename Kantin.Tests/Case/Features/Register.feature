Feature: Register

@basicflow
Scenario: As a user, i should be able to register successfully
	When I registered with <fullname>, <username> and <password>
	Then LoginResult should be successful
	And Database contains account with <fullname> and <username>
	And I can login successfully with <username> and <password>

	Examples: 
	| fullname    | username | password |
	| myphunguyen | jimmy    | -Test123 |

@exceptionflow
Scenario: As a user, i should not be able to register if username has taken
	Given I registered with <fullname>, <username> and <password>
	When I registered with <fullname>, <username> and <password>
	Then LoginResult should be conflicted

	Examples: 
	| fullname    | username | password | organisationName |
	| myphunguyen | jimmy    | -Test123 | plexure          |