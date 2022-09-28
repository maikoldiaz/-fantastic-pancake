# Welcome to Ecopetrol TRUE
<br/>

**TRUE** is Blockchain Platform for Operational Volumetric Management and Official Information.

For more than 15 years Ecopetrol has been making numerous efforts to solve the challenges of its Volumetric Management, which responds to a process with the involvement of multiple resources of the organization.
In 2016, an internal audit was conducted around the confidence of the volumetric information within the company.<br/><br/>
In the face of this situation, Ecopetrol's senior management decided to generate an initiative for the **Redesign of the Volumetric Management Model**

To learn more about TRUE and Ecopetrol, click [here](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/2921/Welcome).

# Getting Started
Kindly follow the below links for setting up the project in your machine.<br/>

- [Project Setup](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/863/Project-Setup)
- [Node Setup](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/114/Setup)
- [Automation Setup](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/99/Test-Automation)
- [Users](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/995/Users-Roles)

# Planning
In this project, we make use to Agile development Methodology where we run typically a 3 weeks sprint.<br/>
The project template followed is scrum and estimation is done using user story points (Fibonnaci series).<br/>
The reference story for estimation is [this](https://dev.azure.com/ecopetrolad/True/_sprints/backlog/TrueDev/True/TrueDev/Sprint%2001?workitem=3837).<br/>

Go through the below links for more details.

- [Program](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/62/Program)
- [Delivery Methodology](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/157/Delivery-Methodology)
- [Definition of Ready](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/92/Definition-of-Ready)
- [Definition of Done](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/91/Definition-of-Done)
- [Scrum](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/157/Delivery-Methodology)
- [Story Pointing & Estimation](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/158/Plan)

# Develop
The solution is based on .NET platform using .NET Standard 2.1 for libraries and .NET Core 3.1 for applications.<br/>
The web components are developed using ASP NET MVC Core 3.1 on server side and React-Redux on client side for SPA.<br/>
The Blockchain part of the solution is developed using Ethereum and Solidity is used for writing Smart Contracts.<br/>

Both server side and client side code makes use of several well known design patterns like Factory, Builder, Proxy, Repository, IoC, DI etc.<br/>
AutoFac is used for IoC, EF Core 2.2 is used for DataAccess, Polly is used for retries and Redis is used for caching.<br/>
The application makes use of Azure Active Directory for Identity management, Azure Key Vault for secret management and Azure Table Storage for Configuration management.<br/>

The entire solutions uses Microservice architecture and these Microservices are deployed as App Services, Azure Functions(Serverless) and AKS Pods<br/>
More details can be found in wiki about the development [here](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/66/Develop).

- [React](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/136/React)
- [Redux](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/138/Redux)
- [Blockchain](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/151/Blockchain)
- [Patterns](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/3623/Patterns)

# Testing
TRUE uses Behavior driven development using SpecFlow for automation & Gherkin language for test case specification.<br/>
The application makes use of Seleneum Web driver for UI automation.<br/>
Browser stack is used as cloud testing platform for cross browser test execution and API testing.

For unit testing, MS Test is used for server side unit testing and Jest & Enzyme for client side unit testing.
More details can be found in wiki about the testing [here](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/68/QA).

- [Automation Setup](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/99/Test-Automation)
- [Quality Gates](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/94/Quality-Gates)
- [Static Analysis](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/153/Static-Analysis)
- [Severities & Priorities](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/118/Severities-Priorities)

# DevOps
TRUE uses most of the DevOps best pratices including Continous Planning, MonoRepo, GIT, CI/CD, Conitnous quality, Conitnous Security and Continous Monitoring.<br/>
There are 5 Environments where the solution is deployed and all these deployments are fully automated.<br/>
The application makes use of Azure DevOps Multi-Stage Pipelines for deployment and all the pipelines are developed in YML.<br/>
The entire infrastructure is maintained as code in TRUE repository.<br/>

More details can be found in wiki about the testing [here](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/69/DevOps).
- [Environments](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/82/Environments)
- [Pipelines](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/2507/Pipelines)
- [Deployment](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/144/Deployment)
- [Infra as Code](https://dev.azure.com/ecopetrolad/True/_wiki/wikis/True.wiki/313/IaaC)

# Contribute
To contribute to the repository, ensure you are part of development team and have cloned the repository.<br/>
The application uses GIT as source control system and Pull Requests are the key mechanism to contribute.