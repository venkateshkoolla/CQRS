/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

-- Run security scripts
IF DATABASE_PRINCIPAL_ID('db_executor') IS NULL
BEGIN
  PRINT 'Creating db_executor role...'
  CREATE ROLE db_executor;
END

PRINT 'Granting execute (dbo) to db_executor...'
GRANT EXECUTE ON SCHEMA::dbo TO db_executor;

GO
