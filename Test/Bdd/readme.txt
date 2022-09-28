BDD.CORE.API
------------

- AzDevOps Sync
  - Use Spex

- Code files
	- `Core\AppTestBase.cs`: Used for "Hooks"

- Configuration
	- `app.config`: Change values under `<vstsDocTarget>` node for VSTS-sync
	- Make sure the values of AssignedTo (in .config) / @owner tag (in .feature) are valid.

- Main classes to use
    - `ApiStepDefinitionBase`: To add additional functionality, inherit this class and add/override methods
    - `ApiExecutor`: To add additional functionality, inherit this class and add/override methods

- Scenario specific Packages
    - `Bdd.Core.Web`: For Web Tests
    - `Bdd.Core.Api`: For Api Tests