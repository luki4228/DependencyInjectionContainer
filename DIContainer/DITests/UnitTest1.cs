using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIContainer;
using DITests.TestClasses;

namespace DITests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void InitializeTest()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestReferenceTypeValidation()
        {
            var config = new DIConfig();
            config.Register<int, int>(true);
            var provider = new DIProvider(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestImplementationInheritenceValidation()
        {
            var config = new DIConfig();
            config.Register<String, Boolean>(true);
            var provider = new DIProvider(config);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestImplementationTypeValidation()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomeAbstractImplement>(true);
            var provider = new DIProvider(config);
        }

        [TestMethod]
        public void TestDuplicateImplementations()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomeImplementation>(true);
            config.Register<SomethingAbstract1, SomeImplementation>(true);
            var provider = new DIProvider(config);

            Assert.AreEqual(config.DContainer[typeof(SomethingAbstract1)].Count, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestUnregisteredDependencies()
        {
            var config = new DIConfig();
            var provider = new DIProvider(config);
            var obj = provider.Resolve<SomethingAbstract1>();
        }

        [TestMethod]
        public void TestResolvingBasicImplementation()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomeAbstractImplement>(true);
            var provider = new DIProvider(config);
            var obj = provider.Resolve<SomethingAbstract1>();
            Assert.IsInstanceOfType(obj[0], typeof(SomeAbstractImplement));
        }

        // Check for resolving dependencies that can't be created.
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestResolvingUncreatableDependencies()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomethingPrivate>(true);
            var provider = new DIProvider(config);
            var obj = provider.Resolve<SomethingAbstract1>();
        }


    }
}
