using System;
using Ocas.Domestic.Apply.Settings;

namespace Ocas.Domestic.AppSettings.Extras
{
    public class AppSettingsExtras : IAppSettingsExtras
    {
        private readonly IAppSettingsBase _appSettings;

        public AppSettingsExtras(IAppSettingsBase appSettings)
        {
            _appSettings = appSettings;
        }

        public DateTime GetApplicationCycleEqualConsiderationDate(int year)
        {
            var considerationTime = _appSettings.GetAppSetting(Apply.Constants.AppSettings.AppCycleEqualConsiderTime);
            var considerationMonth = _appSettings.GetAppSetting(Apply.Constants.AppSettings.AppCycleEqualConsiderMonth);
            var considerationDay = _appSettings.GetAppSetting(Apply.Constants.AppSettings.AppCycleEqualConsiderDay);

            var isValidconsiderationTime = TimeSpan.TryParse(considerationTime, out var validconsiderationTime);
            var isValidconsiderationMonth = int.TryParse(considerationMonth, out var validconsiderationMonth);
            var isValidconsiderationDay = int.TryParse(considerationDay, out var validconsiderationDay);

            if (!isValidconsiderationTime) throw new ArgumentException($"{nameof(considerationTime)} is not a valid time: {considerationTime}");
            if (!isValidconsiderationMonth || validconsiderationMonth < 1 || validconsiderationMonth > 12) throw new ArgumentException($"{nameof(considerationMonth)} is not a valid month: {considerationMonth}");
            if (!isValidconsiderationDay || validconsiderationDay < 1 || validconsiderationDay > DateTime.DaysInMonth(year, validconsiderationMonth)) throw new ArgumentException($"{nameof(considerationDay)} is not a valid day: {considerationDay}");

            var estconsiderdationDateTime = new DateTime(year, validconsiderationMonth, validconsiderationDay, validconsiderationTime.Hours, validconsiderationTime.Minutes, validconsiderationTime.Seconds);
            return TimeZoneInfo.ConvertTimeToUtc(estconsiderdationDateTime, Apply.Constants.TimeZone.Est);
        }

        // From CBA: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/domesticapi?path=%2Fsrc%2FOCAS.Connector.CRM.WCF%2FCustom%2FProviders%2FOffer%2FApplicantOffersProviderSQL.cs&version=GBmaster&line=118&lineStyle=plain&lineEnd=147&lineStartColumn=16&lineEndColumn=17
        // From A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FOffer%2FOfferService.cs&version=GBmaster&line=153&lineStyle=plain&lineEnd=239&lineStartColumn=9&lineEndColumn=10
        public DateTime? GetNextSendDate(DateTime utcMaxLock)
        {
            var lockStartTimeConfigValue = _appSettings.GetAppSetting(Apply.Constants.AppSettings.LockStartTime);
            var lockEndTimeConfigValue = _appSettings.GetAppSetting(Apply.Constants.AppSettings.LockEndTime);

            var isValidLockStartTime = TimeSpan.TryParse(lockStartTimeConfigValue, out var lockStartTime);
            var isValidLockEndTime = TimeSpan.TryParse(lockEndTimeConfigValue, out var lockEndTime);

            if (!isValidLockStartTime) throw new ArgumentException($"{nameof(lockStartTimeConfigValue)} is not a valid time: {lockStartTimeConfigValue}");
            if (!isValidLockEndTime) throw new ArgumentException($"{nameof(lockEndTimeConfigValue)} is not a valid time: {lockEndTimeConfigValue}");

            var estNow = TimeZoneInfo.ConvertTime(DateTime.UtcNow, Apply.Constants.TimeZone.Est);

            // assuming maxLock is UTC
            var estMaxLock = TimeZoneInfo.ConvertTime(utcMaxLock, Apply.Constants.TimeZone.Est);
            var estMaxLockDate = estMaxLock.Date;

            var estEndDateTime = new DateTime(estMaxLockDate.Year, estMaxLockDate.Month, estMaxLockDate.Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);

            var estNextSendDate = (DateTime?)null;
            if (estMaxLock.TimeOfDay >= lockStartTime)
            {
                // Anything after lock start time
                switch (estMaxLockDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                        if (estEndDateTime.AddDays(1) >= estNow)
                        {
                            estNextSendDate = new DateTime(estMaxLock.AddDays(1).Year, estMaxLock.AddDays(1).Month, estMaxLock.AddDays(1).Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);
                        }

                        break;

                    case DayOfWeek.Thursday:
                        if (estEndDateTime.AddDays(3) >= estNow)
                        {
                            estNextSendDate = new DateTime(estMaxLock.AddDays(3).Year, estMaxLock.AddDays(3).Month, estMaxLock.AddDays(3).Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);
                        }

                        break;

                    case DayOfWeek.Friday:
                        if (estEndDateTime.AddDays(2) >= estNow)
                        {
                            estNextSendDate = new DateTime(estMaxLock.AddDays(2).Year, estMaxLock.AddDays(2).Month, estMaxLock.AddDays(2).Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);
                        }

                        break;
                }
            }
            else
            {
                // Before lock start time
                switch (estMaxLockDate.DayOfWeek)
                {
                    case DayOfWeek.Sunday:
                    case DayOfWeek.Monday:
                    case DayOfWeek.Tuesday:
                    case DayOfWeek.Wednesday:
                    case DayOfWeek.Thursday:
                        if (estEndDateTime >= estNow)
                        {
                            estNextSendDate = new DateTime(estMaxLock.Year, estMaxLock.Month, estMaxLock.Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);
                        }

                        break;

                    case DayOfWeek.Friday:
                        if (estEndDateTime.AddDays(2) >= estNow)
                        {
                            estNextSendDate = new DateTime(estMaxLock.AddDays(2).Year, estMaxLock.AddDays(2).Month, estMaxLock.AddDays(2).Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);
                        }

                        break;

                    case DayOfWeek.Saturday:
                        if (estEndDateTime.AddDays(1) >= estNow)
                        {
                            estNextSendDate = new DateTime(estMaxLock.AddDays(1).Year, estMaxLock.AddDays(1).Month, estMaxLock.AddDays(1).Day, lockEndTime.Hours, lockEndTime.Minutes, lockEndTime.Seconds);
                        }

                        break;
                }
            }

            if (estNextSendDate is null) return null;

            return TimeZoneInfo.ConvertTimeToUtc(estNextSendDate.Value, Apply.Constants.TimeZone.Est);
        }
    }
}
