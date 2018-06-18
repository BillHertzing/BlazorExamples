using Funq;
using ServiceStack;
using ServiceStack.Testing;
using Ace.AceService.BaseServices.Interfaces;
using Ace.AceService.BaseServices.Models;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using System;

namespace Ace.AceService.BaseService.IntegrationTests
{
    class IntegrationTestingAppHost : AppSelfHostBase
    {
        public IntegrationTestingAppHost() : base(nameof(IntegrationTest), typeof(Ace.AceService.BaseServices.Interfaces.BaseServices).Assembly) { }

        public override void Configure(Container container)
        {
        }
    }
    public class Fixture : IDisposable
    {
        public const string BaseUri = "http://localhost:2000/";
        private readonly ServiceStackHost integrationTestingAppHost;
        #region MOQs
        // a MOQ for the async web calls used for Term1
        //public Mock<IWebGet> mockTerm1;
        #endregion

        //public ISSDataConcrete iSSData;

        public Fixture()
        {
            integrationTestingAppHost = new IntegrationTestingAppHost()
    .Init()
    .Start(BaseUri);
            /*
            mockTerm1 = new Mock<ITcp>();
            mockTerm1.Setup(webGet => webGet.AsyncWebGet<double>("A"))
                .Callback(() => Task.Delay(new TimeSpan(0, 0, 1)))
                .ReturnsAsync(100.0);
            mockTerm1.Setup(webGet => webGet.AsyncWebGet<double>("B"))
                .Callback(() => Task.Delay(new TimeSpan(0, 0, 1)))
                .ReturnsAsync(200.0);
                */
        }
        public void Dispose()
        {
            integrationTestingAppHost.Dispose();
        }
    }

    public class IntegrationTest : IClassFixture<Fixture>
    {
        protected Fixture _fixture;
        readonly ITestOutputHelper output;
        public IntegrationTest(ITestOutputHelper output, Fixture fixture)
        {
            this.output = output;
            this._fixture = fixture;
        }

        public IServiceClient CreateClient() => new JsonServiceClient(Fixture.BaseUri);

        [Fact]
        public void Can_call_Hello_Service()
        {
            var client = CreateClient();

            var response = client.Get(new BaseServiceIsAlive {  });

            response.Result.Should().Be("Hello!");

        }
    }
}
