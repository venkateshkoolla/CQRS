using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Models;
using Ocas.Domestic.Transactions;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class DomesticContextTest : BaseTest
    {
        private readonly Guid _programId = new Guid("c511e8fc-2dc9-4cc6-979e-ef6ba0000c12"); // Humber 3071R (Electromechanical Engineering Technology - Automation And Robotics)

        [Fact]
        public async Task Transaction_Should_Rollback()
        {
            var program = await Context.GetProgram(_programId);

            Func<Task> a = async () =>
            {
                await Context.BeginTransaction();
                var result = await Context.UpdateProgram(program); //Enlist in transactions
                await Context.RollbackTransaction();
            };

            a.Should().Throw<RollbackException>().WithMessage("Unable to rollback");
        }

        [Fact]
        public async Task Transaction_Should_RollbackWithInnerException()
        {
            var program = await Context.GetProgram(_programId);

            Func<Task> a = async () =>
            {
                await Context.BeginTransaction();
                var result = await Context.UpdateProgram(program); //Enlist in transactions
                await Context.RollbackTransaction(new DivideByZeroException());
            };

            a.Should().Throw<RollbackException>().WithMessage("Unable to rollback").WithInnerException<DivideByZeroException>();
        }
    }
}
