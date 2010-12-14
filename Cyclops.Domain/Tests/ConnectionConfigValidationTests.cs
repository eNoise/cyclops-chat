using Cyclops.Core.Model;
using NUnit.Framework;

namespace Cyclops.Core.Tests
{
    [TestFixture]
    public class ConnectionConfigValidationTests : ValidationTestBase<ConnectionConfig>
    {
        //TODO: test regex

        /// <summary>
        /// Creates a simple valid data
        /// </summary>
        protected override ConnectionConfig CreateValidData()
        {
            return new ConnectionConfig
                       {
                           User = "User",
                           Server = "jabber.server.domain",
                           Password = "qwerty123",
                           NetworkHost = "127.0.0.1"
                       };
        }

        [Test]
        public void RequiredTest()
        {
            //arrange 
            var config = new ConnectionConfig {User = null, Server = null, Password = null};

            //act
            bool result = Validate(config);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ServerAddressValidation()
        {
            string[] servers = {
                                   "host.com",
                                   "host.host.host.host.host"
                               };
            ValidateDataCollection((data, value) => data.Server = value, servers);
        }
    }
}