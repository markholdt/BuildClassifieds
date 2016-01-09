using System;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Testing;
using Classifieds.ServiceModel;
using Classifieds.ServiceInterface;

namespace Classifieds.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            var service = appHost.Container.Resolve<MyServices>();

            var response = (HelloResponse)service.Any(new ImageRoutes { Name = "World" });

            Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }
    }
}
