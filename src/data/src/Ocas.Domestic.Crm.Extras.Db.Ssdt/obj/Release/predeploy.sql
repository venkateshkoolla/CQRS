/*
 Pre-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.    
 Use SQLCMD syntax to include a file in the pre-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the pre-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

-- Run linked server script
IF '$(Environment)' IN('dev') AND NOT EXISTS(SELECT * FROM SYS.SERVERS WHERE [Name] = N'$(SERVER)')
BEGIN
    PRINT 'Creating $(SERVER) link server...'

    USE [master]
    
    EXECUTE sp_addlinkedserver @server = N'$(SERVER)', @srvproduct=N'SQL Server'

    EXECUTE sp_addlinkedsrvlogin @rmtsrvname=N'$(SERVER)',@useself=N'False',@locallogin=NULL,@rmtuser=N'cbui',@rmtpassword=N'Ocas2014'
    
    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'collation compatible', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'data access', @optvalue=N'true'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'dist', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'pub', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'rpc', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'rpc out', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'sub', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'connect timeout', @optvalue=N'0'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'collation name', @optvalue=null

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'lazy schema validation', @optvalue=N'false'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'query timeout', @optvalue=N'0'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'use remote collation', @optvalue=N'true'

    EXECUTE sp_serveroption @server=N'$(SERVER)', @optname=N'remote proc transaction promotion', @optvalue=N'true'

    USE [$(DatabaseName)]
END

GO
