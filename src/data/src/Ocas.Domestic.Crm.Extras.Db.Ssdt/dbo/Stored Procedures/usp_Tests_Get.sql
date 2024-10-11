CREATE PROCEDURE [dbo].[usp_Tests_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0,
    @IsOfficial BIT = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT [Tests].*
                        , CASE @Locale
                            WHEN 0 THEN [Tests].[EnglishNormingGroupName]
                            WHEN 1 THEN [Tests].[FrenchNormingGroupName]
                            ELSE NULL
                          END AS [NormingGroupName]
                        -- following fields are mapped to Test.Details
                        , [Details].*
                        , CASE @Locale
                            WHEN 0 THEN [Details].[EnglishDescription]
                            WHEN 1 THEN [Details].[FrenchDescription]
                            ELSE NULL
                          END AS [Description]
                    FROM [dbo].[view_Tests] [Tests]
                        LEFT OUTER JOIN [dbo].[view_TestDetails] [Details]
                            ON [Tests].[Id] = [Details].[TestId]
                    WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Tests].Id = @Id';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Tests].[ApplicantId] = @ApplicantId';

IF (@IsOfficial IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Tests].[IsOfficial] = @IsOfficial';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Tests].[StateCode] = @StateCode';

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Tests].[StatusCode] = @StatusCode';

SET @sqlCommand = @sqlCommand + ' ORDER BY [Details].[SortOrder], [Tests].[CreatedOn], [Tests].[Id]';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @IsOfficial BIT, @StateCode BIT, @StatusCode TINYINT, @Locale INT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ApplicantId = @ApplicantId
    ,@IsOfficial = @IsOfficial
    ,@StateCode = @StateCode
    ,@StatusCode = @StatusCode
    ,@Locale = @Locale
