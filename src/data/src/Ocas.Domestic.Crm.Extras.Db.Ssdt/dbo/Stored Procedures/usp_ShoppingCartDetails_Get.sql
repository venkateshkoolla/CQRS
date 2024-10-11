CREATE PROCEDURE [dbo].[usp_ShoppingCartDetails_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand = 'SELECT [Id]
        ,[ShoppingCartId]
        ,CASE 
            WHEN ([DescriptionEnglish] IS NULL) AND ([DescriptionFrench] IS  NULL) THEN [Description]                
            ELSE 
                CASE @Locale
                    WHEN 0 THEN [DescriptionEnglish] 
                    WHEN 1 THEN [DescriptionFrench]
                    ELSE NULL
                END 
         END AS [Description]
        ,[Type]
        ,[ReferenceId]
        ,[Amount]
        ,[Quantity]
        ,[UomId]
        ,[ProductId]
        ,[ApplicationId]
        ,[ProgramChoiceId]
        ,[VoucherId]
        ,[TranscriptRequestId]
        ,[ManualDiscountAmount]
        ,[PricePerUnit]
        ,[ProductName]
        ,[VoucherProductId]
        ,[statecode]
        ,[ApplicantId]
    FROM [dbo].[view_ShoppingCartDetails] WHERE 1=1 ';

IF (@ApplicationId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [CartApplicationId] = @ApplicationId';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicantId] = @ApplicantId';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [statecode] = @StateCode'

SET @paramListTerms = '@ApplicationId UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @Id UNIQUEIDENTIFIER, @StateCode BIT, @Locale INT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @ApplicationId = @ApplicationId    
    , @ApplicantId = @ApplicantId
    , @Id = @Id
    , @StateCode = @StateCode
    , @Locale = @Locale
