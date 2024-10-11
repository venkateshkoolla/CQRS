using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.TestFramework.Models;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class OfferLoader
    {
        public const string DateFormat = "yyyyMMdd";
        public const string OfferOutputPath = @"\\filesharedev\filesharedev\uploads\OCFileUpload";

        public Task LoadOffer(NewTestOfferInfo offer)
        {
            return LoadOffers(new List<NewTestOfferInfo>
            {
                offer
            });
        }

        public async Task LoadOffers(IList<NewTestOfferInfo> allOffers)
        {
            var estNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Constants.TimeZone.Est);

            foreach (var collegeOffers in allOffers.GroupBy(x => x.CollegeCode))
            {
                var collegeCode = collegeOffers.Key;
                var originalFile = await XunitInjectionCollection.CrmDatabaseFixture.GetOcFileProcessHeader(collegeCode);
                var newFileNumber = (int.Parse(originalFile.FileNumber) + 1).ToString().PadLeft(5, '0');
                var newFilename = originalFile.FileName.Replace(originalFile.FileNumber, newFileNumber);
                var numOffers = collegeOffers.Count().ToString().PadLeft(6, '0');

                var lines = new List<string>();
                var sb = new StringBuilder();

                sb.Append("BCT")
                    .Append(estNow.ToString(DateFormat))
                    .Append("01000000")
                    .Append(collegeCode)
                    .Append(newFileNumber)
                    .Append(numOffers)
                    .Append(originalFile.FileName)
                    .Append(string.Empty.PadRight(42, 'X'));

                lines.Add(sb.ToString());

                foreach (var offer in collegeOffers)
                {
                    var estEffectiveDate = TimeZoneInfo.ConvertTimeFromUtc(offer.EffectiveDate, Constants.TimeZone.Est);
                    var estSoftExpiryDate = TimeZoneInfo.ConvertTimeFromUtc(offer.SoftExpiryDate, Constants.TimeZone.Est);
                    var estHardExpiryDate = TimeZoneInfo.ConvertTimeFromUtc(offer.HardExpiryDate, Constants.TimeZone.Est);

                    sb.Clear()
                        .Append("OCI")
                        .Append(estNow.ToString(DateFormat))
                        .Append("01000000")
                        .Append(collegeCode)
                        .Append(offer.ApplicationNumber)
                        .Append(offer.ProgramCode.PadRight(8, ' '))
                        .Append(offer.CampusCode.PadRight(4, ' '))
                        .Append(offer.SequenceNumber)
                        .Append(offer.EntryLevelCode.PadLeft(2, '0'))
                        .Append(offer.StartDate)
                        .Append(offer.OfferStudyMethodCode)
                        .Append(estEffectiveDate.ToString(DateFormat))
                        .Append(estSoftExpiryDate.ToString(DateFormat))
                        .Append(estHardExpiryDate.ToString(DateFormat))
                        .Append(offer.OfferStateCode)
                        .Append(offer.IsLateAdmit ? 'Y' : 'N')
                        .Append(offer.TermIdentifier)
                        .Append(string.Empty.PadRight(68, 'X'));

                    lines.Add(sb.ToString());
                }

                File.WriteAllLines(OfferOutputPath + Path.DirectorySeparatorChar + newFilename, lines);

#if DEBUG
                File.WriteAllLines("Logs" + Path.DirectorySeparatorChar + newFilename, lines);
#endif
            }
        }
    }
}
