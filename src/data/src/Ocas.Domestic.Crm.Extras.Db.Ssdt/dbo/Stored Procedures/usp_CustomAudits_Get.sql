CREATE PROCEDURE [dbo].[usp_CustomAudits_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @FromDate DATETIME = NULL,
    @ToDate DATETIME = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT [CustomAudits].*
                        , CASE @Locale
                            WHEN 0 THEN [CustomAudits].[CustomEntityLabelEnglish]
                            WHEN 1 THEN [CustomAudits].[CustomEntityLabelFrench]
                            ELSE NULL
                          END AS [CustomEntityLabel]
                        , [CustomAuditDetails].*
                    FROM [dbo].[view_CustomAudits] [CustomAudits]
                    LEFT OUTER JOIN [dbo].[view_CustomAuditDetails] [CustomAuditDetails] ON [CustomAudits].[Id] = [CustomAuditDetails].[CustomAuditId]
                    WHERE 1=1 ';

IF(@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [CustomAudits].[Id] = @Id';

IF (@ApplicationId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [CustomAudits].[ApplicationId] = @ApplicationId';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [CustomAudits].[ApplicantId] = @ApplicantId';

IF (@FromDate IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [CustomAudits].[CreatedOn] >= @FromDate ';

IF (@ToDate IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [CustomAudits].[CreatedOn] < @ToDate ';

SET @sqlCommand = @sqlCommand + ' ORDER BY [CustomAudits].[CreatedOn] DESC'

SET @paramListTerms = '@Id UNIQUEIDENTIFIER,
                      @ApplicantId UNIQUEIDENTIFIER,
                      @ApplicationId UNIQUEIDENTIFIER,
                      @FromDate DATETIME,
                      @ToDate DATETIME,
                      @Locale INT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ApplicantId = @ApplicantId
    ,@ApplicationId = @ApplicationId
    ,@FromDate = @FromDate
    ,@ToDate = @ToDate
    ,@Locale = @Locale