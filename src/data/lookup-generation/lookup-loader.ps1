param(
  [Parameter(Mandatory = $true)]
  [string]$BinPath,
  [Parameter(Mandatory = $true)]
  [string]$ConnectionString)

Get-ChildItem -Recurse $BinPath |
Where-Object { $_.Extension -eq ".dll" } |
ForEach-Object {
  $AssemblyName = $_.FullName
  try
  {
    #Load assembly without locking it during the duration of the PowerShell session
    $Bytes = [System.IO.File]::ReadAllBytes($AssemblyName)
    [System.Reflection.Assembly]::Load($Bytes)
  }
  catch
  {
    "***ERROR*** Error when loading assembly: " + $AssemblyName
  }
}

$Instance = New-Object Ocas.Domestic.Crm.Extras.Provider.CrmExtrasProvider -ArgumentList $ConnectionString, 60
$BindingFlags = [Reflection.BindingFlags]"Public,Instance"

$Instance.GetType().GetMethods($BindingFlags) | Where-Object { $_.MemberType -eq "Method" -and $_.Name -ilike "Get*" -and $_.ToString() -like "*System.Collections.Generic.IList*" } `
   | ForEach-Object {
  $Method = $_
  $ResourceBasePath = ".\src\Ocas.Domestic.Data.TestFramework.SeedData\Resources\";
  $ResourcePath =  $ResourceBasePath + $Method.Name.SubString(3) + ".json"
  if ($Method.ToString() -ilike "*(Ocas.Domestic.Enums.Locale)*") {
    $Method.Name
    ConvertTo-Json @($Method.Invoke($Instance,0).GetAwaiter().GetResult()) | Out-File FileSystem::$ResourcePath
  } elseif ($Method.ToString() -ilike "*Ocas.Domestic.Models.ApplicationCycle]*") {
    $Method.Name
    ConvertTo-Json @($Method.Invoke($Instance,$null).GetAwaiter().GetResult()) | Out-File FileSystem::$ResourcePath
  } elseif ($Method.ToString() -ilike "*GetCollegeApplicationCycles()*"`
            -or $Method.ToString() -ilike "*GetColleges()*"`
            -or $Method.ToString() -ilike "*GetCollegeInformations()*"`
            -or $Method.ToString() -ilike "*GetCredentialEvaluationAgencies()*"`
            -or $Method.ToString() -ilike("*GetShsmCompletions()*")`
            -or $Method.ToString() -ilike("*GetCurrencies()*")`
            -or $Method.ToString() -ilike("*GetDocumentPrints()*")`
            -or $Method.ToString() -ilike("*GetGenders()*")`
            -or $Method.ToString() -ilike("*GetInstitutes()*")`
            -or $Method.ToString() -ilike("*GetPaymentResults()*")`
            -or $Method.ToString() -ilike("*GetProgramSpecialCodes()*")`
            -or $Method.ToString() -ilike("*GetUniversities()*")) {
    $Method.Name
    ConvertTo-Json @($Method.Invoke($Instance,$null).GetAwaiter().GetResult()) | Out-File FileSystem::$ResourcePath
  } elseif ($Method.ToString() -ilike "*(Ocas.Domestic.Models.GetAccountsOptions)*") {

    #GetColleges
    $CollegesResourcePath = $ResourceBasePath + "Colleges.json"
    $CollegeMethod = $Method.MakeGenericMethod([Ocas.Domestic.Models.College])
    $CollegeAccountOptions = [Ocas.Domestic.Models.GetAccountsOptions]@{ AccountType = 3 }
    $CollegeMethod.ToString()
    ConvertTo-Json @($CollegeMethod.Invoke($Instance,$CollegeAccountOptions).GetAwaiter().GetResult()) | Out-File FileSystem::$CollegesResourcePath

    #GetCampuses
    $CampusResourcePath = $ResourceBasePath + "Campuses.json"
    $CampusMethod = $Method.MakeGenericMethod([Ocas.Domestic.Models.Campus])
    $CampusAccountOptions = [Ocas.Domestic.Models.GetAccountsOptions]@{ AccountType = 5 }
    $CampusMethod.ToString()
    ConvertTo-Json @($CampusMethod.Invoke($Instance,$CampusAccountOptions).GetAwaiter().GetResult()) | Out-File FileSystem::$CampusResourcePath

    #GetReferralPartners
    $ReferralPartnersResourcePath = $ResourceBasePath + "ReferralPartners.json"
    $ReferralPartnersMethod = $Method.MakeGenericMethod([Ocas.Domestic.Models.ReferralPartner])
    $ReferralPartnersOptions = [Ocas.Domestic.Models.GetAccountsOptions]@{ AccountType = 6 }
    $ReferralPartnersMethod.ToString()
    ConvertTo-Json @($ReferralPartnersMethod.Invoke($Instance,$ReferralPartnersOptions).GetAwaiter().GetResult()) | Out-File FileSystem::$ReferralPartnersResourcePath

  } elseif($Method.ToString() -ilike "*(Ocas.Domestic.Enums.ProductServiceType)*") {
    #GetProducts
    $Method.Name
    ConvertTo-Json @($Method.Invoke($Instance,4).GetAwaiter().GetResult()) | Out-File FileSystem::$ResourcePath
  } else {
    "Skipping: " + $Method.Name
  }
}
