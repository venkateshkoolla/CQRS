IF DATABASE_PRINCIPAL_ID('db_executor') IS NULL
BEGIN
  PRINT 'Creating db_executor role...'
  CREATE ROLE db_executor;
END

PRINT 'Granting execute (dbo) to db_executor...'
GRANT EXECUTE ON SCHEMA::dbo TO db_executor;
