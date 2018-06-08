using System;
using Ace.AceService.BaseServicesInterface;
using Ace.AceService.BaseServicesModel;
using FluentAssertions;
using Funq;
using ServiceStack;
using Xunit;
using Xunit.Abstractions;

namespace Ace.AceService.BaseService.UnitTests {
    class UnitTestingAppHost : AppSelfHostBase {
        public UnitTestingAppHost() : base(nameof(BaseServicesUnitTest), typeof(BaseServices).Assembly) {
        }

        public override void Configure(Container container) {
        }
    }

    public class Fixture : IDisposable {
        public const string BaseUri = "http://localhost:2000/";

        public readonly ServiceStackHost unitTestingAppHost;

        #region MOQs
        // a MOQ for the async web calls used for Term1
        //public Mock<IWebGet> mockTerm1;
        #endregion

        //public ISSDataConcrete iSSData;

        public Fixture() {
            unitTestingAppHost = new UnitTestingAppHost()
    .Init()
                .Start(BaseUri);
        }

        public void Dispose() {
            unitTestingAppHost.Dispose();
        }
    }

    public class BaseServicesUnitTest : IClassFixture<Fixture> {
        readonly ITestOutputHelper output;
        protected Fixture _fixture;

        public BaseServicesUnitTest(ITestOutputHelper output, Fixture fixture) {
            this.output = output;
            this._fixture = fixture;
        }

        [Fact]
        public void Can_call_BaseServiceIsAlive() {
            var service = _fixture.unitTestingAppHost.Container.Resolve<BaseServices>();

            var response = (IsAliveResponse)service.Any(new BaseServiceIsAlive());
            response.Result.Should()
                .Be("Hello!");
        }
    }
}
