using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Ocas.Domestic.Crm.Db.Cli.Services
{
    public class DbPurger
    {
        private readonly string _connectionString;

        public Action<string> Log { get; set; } = s => { };

        public DbPurger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task Purge()
        {
            await DropTableConstraints();
            await DropTables();
            await DropStoredProcs();
            await DropViews();
            await DropTriggers();
            await DropSequences();
            await UpdateStats();
        }

        private Task DropTableConstraints()
        {
            Log("Dropping table constraints");

            return ExecuteSqlCommand(@"
                while(exists(select 1 from INFORMATION_SCHEMA.TABLE_CONSTRAINTS where CONSTRAINT_TYPE='FOREIGN KEY'))
                begin
                    declare @sql nvarchar(2000)
                    SELECT TOP 1 @sql=('ALTER TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME
                    + '] DROP CONSTRAINT [' + CONSTRAINT_NAME + ']')
                    FROM information_schema.table_constraints
                    WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'
                    exec (@sql)
                    PRINT @sql
                end");
        }

        private Task DropTables()
        {
            Log("Dropping tables");

            return ExecuteSqlCommand(@"
                while (exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_TYPE='BASE TABLE'))
                begin
                    declare @sql nvarchar(2000)
                    SELECT TOP 1 @sql=('DROP TABLE ' + TABLE_SCHEMA + '.[' + TABLE_NAME
                    + ']')
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE TABLE_TYPE='BASE TABLE'
                exec (@sql)
                    PRINT @sql
                end");
        }

        private Task DropStoredProcs()
        {
            Log("Dropping stored procs");

            return ExecuteSqlCommand(@"
                Declare @procName varchar(500) 
                Declare cur Cursor For Select [name] From sys.objects where type = 'p' And is_ms_shipped = 0
                Open cur 
                Fetch Next From cur Into @procName 
                While @@fetch_status = 0 
                Begin 
                    Exec('drop procedure ' + @procName) 
                    Fetch Next From cur Into @procName 
                End
                Close cur 
                Deallocate cur");
        }

        private Task DropViews()
        {
            Log("Dropping views");

            return ExecuteSqlCommand(@"
                Declare @viewName varchar(500) 
                Declare cur Cursor For Select [name] From sys.objects where type = 'v' And is_ms_shipped = 0
                Open cur 
                Fetch Next From cur Into @viewName 
                While @@fetch_status = 0 
                Begin 
                    Exec('drop view ' + @viewName) 
                    Fetch Next From cur Into @viewName 
                End
                Close cur 
                Deallocate cur");
        }

        private Task DropTriggers()
        {
            Log("Dropping triggers");

            return ExecuteSqlCommand(@"
                Declare @trgName varchar(500) 
                Declare cur Cursor For Select [name] From sys.objects where type = 'tr' And is_ms_shipped = 0
                Open cur 
                Fetch Next From cur Into @trgName 
                While @@fetch_status = 0 
                Begin 
                    Exec('drop trigger ' + @trgName) 
                    Fetch Next From cur Into @trgName 
                End
                Close cur 
                Deallocate cur");
        }

        private Task DropSequences()
        {
            Log("Dropping sequences");

            return ExecuteSqlCommand(@"
                Declare @seqName varchar(500) 
                Declare cur Cursor For Select [name] From sys.objects where type = 'SO' And is_ms_shipped = 0
                Open cur 
                Fetch Next From cur Into @seqName 
                While @@fetch_status = 0 
                Begin 
                    Exec('drop sequence ' + @seqName) 
                    Fetch Next From cur Into @seqName 
                End
                Close cur 
                Deallocate cur");
        }

        private Task UpdateStats()
        {
            Log("Updating stats");

            return ExecuteSqlCommand("EXEC sp_updatestats;");
        }

        private async Task ExecuteSqlCommand(string cmdText)
        {
            Log(cmdText);

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(cmdText, conn))
            {
                conn.InfoMessage += (sender, args) => Log(args.Message);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
