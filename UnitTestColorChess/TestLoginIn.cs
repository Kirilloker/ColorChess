using ColorChessModel;
using Moq;
using System.Net;

namespace UnitTest 
{
    [TestFixture]
    public class ServerTests
    {
        [Test]
        public async Task TryLoginIn_SuccessfulLogin()
        {
            var serverSenderMock = new Mock<IServerSender>();
            var server = Server.Instance;
            server.SetServerSender(serverSenderMock.Object);

            var httpClientMock = new Mock<IHttpClientForServer>();
            httpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                          .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
            server.SetHttpClient(httpClientMock.Object);

            bool result = await server.TryLoginIn("username", "password");

            Assert.IsTrue(result);
            Assert.IsTrue(server.IsLoginIn);
        }

        [Test]
        public async Task TryLoginIn_FailedLogin()
        {
            var serverSenderMock = new Mock<IServerSender>();
            var server = Server.Instance;
            server.SetServerSender(serverSenderMock.Object);

            var httpClientMock = new Mock<IHttpClientForServer>();
            httpClientMock.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                          .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Unauthorized });
            server.SetHttpClient(httpClientMock.Object);

            bool result = await server.TryLoginIn("username", "password");

            Assert.IsFalse(result);
            Assert.IsFalse(server.IsLoginIn);
        }
    }
}
