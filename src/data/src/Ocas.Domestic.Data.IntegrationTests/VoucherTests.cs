using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class VoucherTests : BaseTest
    {
        [Fact]
        public async Task GetVoucher_ShouldPass_WithApplication()
        {
            // Act
            var voucher = await Context.GetVoucher(new GetVoucherOptions
            {
                ApplicationId = TestConstants.Vouchers.ApplicationWithVoucher
            });

            var voucherById = await Context.GetVoucher(new GetVoucherOptions
            {
                Id = voucher.Id
            });

            var voucherByCode = await Context.GetVoucher(new GetVoucherOptions
            {
                Code = voucher.Code
            });

            // Assert
            voucher.Should().NotBeNull();
            voucher.ApplicationId.Should().Be(TestConstants.Vouchers.ApplicationWithVoucher);
            voucher.Should().BeEquivalentTo(voucherById);
            voucher.Should().BeEquivalentTo(voucherByCode);
        }
    }
}
