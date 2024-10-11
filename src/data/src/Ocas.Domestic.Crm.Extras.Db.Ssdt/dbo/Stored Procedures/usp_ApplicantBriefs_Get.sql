CREATE PROCEDURE [dbo].[usp_ApplicantBriefs_Get]
    @ContactType INT = 1,
    @UserType INT = NULL,
    @PartnerId NVARCHAR(50) = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @AccountNumber NVARCHAR(12) = NULL,
    @ApplicationCycleId UNIQUEIDENTIFIER = NULL,
    @ApplicationNumber NVARCHAR(9) = NULL,
    @ApplicationStatusId UNIQUEIDENTIFIER = NULL,
    @BirthDate DATETIME = NULL,
    @Email NVARCHAR(100) = NULL,
    @FirstName VARCHAR(50) = NULL,
    @LastName VARCHAR(50) = NULL,
    @MiddleName VARCHAR(50) = NULL,
    @Mident VARCHAR(8) = NULL,
    @OntarioEducationNumber VARCHAR(9) = NULL,
    @PaymentLocked BIT = NULL,
    @PhoneNumber VARCHAR(15) = NULL,
    @PreviousLastName VARCHAR(50) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 100,
    @SortBy NVARCHAR(50) = '[AccountNumber]',
    @SortDirection NVARCHAR(5) = 'ASC',
    @TotalCount INT OUTPUT
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max)
       ,@filters nvarchar(max)
       ,@fromClause nvarchar(max)
       ,@sqlCommandCount nvarchar(max);

DECLARE @ActiveAccountStatusId UNIQUEIDENTIFIER = (SELECT ocaslr_accountstatusId AS Id FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_accountstatusBase] WHERE ocaslr_code = 1)

SET @sqlCommand = 'SELECT [Applicant].[Id]
  , [AccountNumber]
  , [FirstName]
  , [MiddleName]
  , [LastName]
  , [PreviousLastName]
  , [PreferredName]
  , [BirthDate]
  , [Email]
  , [HomePhone]
  , [MobilePhone]
  , [OntarioEducationNumber]
  , [Applicant].[CreatedOn]
  , [Applicant].[ModifiedOn]
  , [Applicant].[CountryOfCitizenshipId]
  , [PaymentLocked]
  , [ApplicationNumber]
  , [App].[ApplicationStatusId]'

  SET @fromClause = ' FROM [dbo].[view_Contacts] AS [Applicant]
  LEFT JOIN [dbo].[view_Applications] AS [App] ON [Applicant].[Id] = [App].[ApplicantId] AND [App].[Code] = ''A'''  

  --CollegeUser
  IF(@UserType = 2)
    SET @fromClause = @fromClause + ' INNER JOIN [dbo].[view_Applicants_College] AS AppCollege ON AppCollege.ApplicantId = Applicant.Id AND AppCollege.CollegeCode = @PartnerId AND [AccountStatusId] = @ActiveAccountStatusId'

  --HighSchoolUser
  IF(@UserType = 1)
    SET @fromClause = @fromClause + ' INNER JOIN [dbo].[view_Applicants_HighSchool] AS AppHighSchool ON AppHighSchool.ApplicantId = Applicant.Id AND AppHighSchool.Mident = @PartnerId AND [AccountStatusId] = @ActiveAccountStatusId';

  --HighSchoolBoardUser
  IF(@UserType = 4)
    SET @fromClause = @fromClause + ' INNER JOIN [dbo].[view_Applicants_HighSchool] AS AppHighSchool ON AppHighSchool.ApplicantId = Applicant.Id AND AppHighSchool.BoardMident = @PartnerId AND [AccountStatusId] = @ActiveAccountStatusId';

  IF(@UserType = 2 OR @UserType = 3)
    SET @fromClause = @fromClause + ' LEFT JOIN [dbo].[view_Applicants_HighSchool] AS AppHighSchool ON AppHighSchool.ApplicantId = Applicant.Id AND [AccountStatusId] = @ActiveAccountStatusId'

  SET @filters = ' WHERE ContactType = @ContactType';
  
  IF(@AccountNumber IS NOT NULL)
    SET @filters = @filters + ' AND AccountNumber = @AccountNumber'

  IF(@ApplicationCycleId IS NOT NULL)
    SET @filters = @filters + ' AND [App].ApplicationCycleId = @ApplicationCycleId'

  IF(@ApplicationNumber IS NOT NULL)
    SET @filters = @filters + ' AND ApplicationNumber = @ApplicationNumber'

  IF(@ApplicationStatusId IS NOT NULL)
    SET @filters = @filters + ' AND [ApplicationStatusId] = @ApplicationStatusId'

  IF(@BirthDate IS NOT NULL)
    SET @filters = @filters + ' AND (DATEPART(YY, [Applicant].[BirthDate]) = DATEPART(YY, @BirthDate) AND 
        DATEPART(MM, [Applicant].[BirthDate]) = DATEPART(MM, @BirthDate) AND 
        DATEPART(DD, [Applicant].[BirthDate]) = DATEPART(DD, @BirthDate))'

  IF(NULLIF(@Email, '') IS NOT NULL)
    SET @filters = @filters + ' AND Email = @Email'

  IF (NULLIF(@FirstName, '') IS NOT NULL)
    SET @filters = @filters + ' AND FirstName LIKE @FirstName'

  IF (NULLIF(@LastName, '') IS NOT NULL)
    SET @filters = @filters + ' AND LastName LIKE @LastName'

  IF (NULLIF(@MiddleName, '') IS NOT NULL)
    SET @filters = @filters + ' AND MiddleName LIKE @MiddleName'

  IF(NULLIF(@Mident, '') IS NOT NULL)
      SET @filters = @filters + ' AND AppHighSchool.[Mident] LIKE @Mident'

  IF(@OntarioEducationNumber IS NOT NULL)
    SET @filters = @filters + ' AND OntarioEducationNumber = @OntarioEducationNumber'

  IF(@PaymentLocked IS NOT NULL)
    SET @filters = @filters + ' AND PaymentLocked = @PaymentLocked'

  IF(NULLIF(@PhoneNumber, '') IS NOT NULL)
    SET @filters = @filters + ' AND ([Applicant].[HomePhone] = @PhoneNumber OR [Applicant].[MobilePhone] = @PhoneNumber )'

  IF (NULLIF(@PreviousLastName, '') IS NOT NULL)
    SET @filters = @filters + ' AND PreviousLastName LIKE @PreviousLastName'

  IF (@StateCode IS NOT NULL)
    SET @filters = @filters + ' AND [Applicant].[StateCode] = @StateCode'

  IF (@StatusCode IS NOT NULL)
    SET @filters = @filters + ' AND [Applicant].[StatusCode] = @StatusCode'

  SET @sqlCommand = @sqlCommand + @fromClause + @filters + ' ORDER BY ' + @SortBy + ' ' + @SortDirection
  SET @sqlCommand = @sqlCommand + ' OFFSET (@PageNumber-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY;';
   
  SET @sqlCommand = @sqlCommand + ' SELECT @TotalCount  = Count(1) ' + @fromClause + @filters;

  SET @paramListTerms = '@ContactType INT,
                        @UserType INT,
                        @PartnerId NVARCHAR(50),
                        @StateCode BIT = 0,
                        @StatusCode TINYINT = 1,
                        @AccountNumber NVARCHAR(12),
                        @ApplicationCycleId UNIQUEIDENTIFIER,
                        @ApplicationNumber NVARCHAR(9),
                        @ApplicationStatusId UNIQUEIDENTIFIER,
                        @BirthDate DATETIME,
                        @Email NVARCHAR(100),
                        @FirstName VARCHAR(50),
                        @LastName VARCHAR(50),
                        @MiddleName VARCHAR(50),
                        @Mident VARCHAR(8),
                        @OntarioEducationNumber VARCHAR(9),
                        @PaymentLocked BIT,
                        @PhoneNumber VARCHAR(15),
                        @PreviousLastName VARCHAR(50),
                        @PageNumber INT = 1,
                        @PageSize INT = 100,
                        @TotalCount INT OUTPUT,
                        @ActiveAccountStatusId UNIQUEIDENTIFIER';
    
EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @ContactType = @ContactType
    , @UserType = @UserType
    , @PartnerId = @PartnerId
    , @StateCode = @StateCode
    , @StatusCode = @StatusCode
    , @AccountNumber = @AccountNumber
    , @ApplicationCycleId = @ApplicationCycleId
    , @ApplicationNumber = @ApplicationNumber
    , @ApplicationStatusId = @ApplicationStatusId
    , @BirthDate = @BirthDate
    , @Email = @Email
    , @FirstName = @FirstName
    , @LastName = @LastName
    , @MiddleName = @MiddleName
    , @Mident = @Mident
    , @OntarioEducationNumber = @OntarioEducationNumber
    , @PaymentLocked = @PaymentLocked
    , @PhoneNumber = @PhoneNumber
    , @PreviousLastName = @PreviousLastName
    , @PageNumber = @PageNumber
    , @PageSize = @PageSize
    , @TotalCount = @TotalCount OUTPUT
    , @ActiveAccountStatusId = @ActiveAccountStatusId