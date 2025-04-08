Configuration files:
.\Verivox\sharedAppSettings.json – config file to specify ports form electricity provider APIs
.\Verivox\Backend\CalculationService\appsettings.json – config file to specify urls of electricity provider APIs and Redis cache parameters.

Run application:
.\Verivox\runApp.ps1 – PowerShell script to run all electricity provider APIs, Calculation Service, Redis in Docker and Frontend application.
Stop application:
.\Verivox\stopApp.ps1 – PowerShell script to stop all backend and frontend applications.

Further improvements:
1.	Add more tests to increase code coverage, e.g. for CalculateAnnualCost
2.	Refactor ElectricityProviderService to perform all calculations here and not in CalculationController
3.	Pick up electricity provider name from configuration.
4.	Use shared configuration file for whole backend.
5.	Stop (delete) redis docker container in stopApp.ps1.
