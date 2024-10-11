# Domestic Data Library 
Set of projects to enable interaction with CRM entity data using the CRM SDK and databases

## Getting Started
Clone repo and open in Visual Studio. 
In order to run integration tests or use the classes in an app, you must be connected to the OCAS network

## Build and Test
Run default Cake target to build and deploy SSDT to (LocalDb)\MSSQLLocalDB.   

Run Cake target "Local" to build, deploy the SSDT, and generate the seed data lookups for testing.  
Run Cake target "Integration-Tests to run integration tests as default target only runs unit tests.  
Run Cake target "Create-Crm-Entities" to update CRM entities if they have changed.    
Run Cake target "Create-Crm-Dacpac" to update Dacpac referenced by SSDT if CRM database schema has changed.  
