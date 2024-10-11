using System;
using Ocas.Domestic.Data.Mappers;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.UnitTests.Mappers
{
    [Collection(nameof(XunitInjectionCollection))]
    public partial class CrmMapperTest
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly CrmMapper _mapper;

        public CrmMapperTest(DataFakerFixture dataFakerFixture)
        {
            _dataFakerFixture = dataFakerFixture ?? throw new ArgumentNullException(nameof(dataFakerFixture));
            _mapper = new CrmMapper();
        }
    }
}
