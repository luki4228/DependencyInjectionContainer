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
            config.Register<SomethingAbstract1, SomeImplementation>(true);
            var provider = new DIProvider(config);
            var obj = provider.Resolve<SomethingAbstract1>();
            Assert.IsInstanceOfType(obj[0], typeof(SomeImplementation));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestResolvingUncreatableDependencies()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomethingPrivate>(true);
            var provider = new DIProvider(config);
            var obj = provider.Resolve<SomethingAbstract1>();
        }

        [TestMethod]
        public void TestResolvingMultipleImplementations()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomeImplementation>(true);
            config.Register<SomethingAbstract1, Something2>(true);
            var provider = new DIProvider(config);
            var resolved = provider.Resolve<SomethingAbstract1>();
            Assert.AreEqual(resolved.Count, 2);

            Type[] actual = new Type[] { resolved[0].GetType(), resolved[1].GetType() };
            Type[] expected = new Type[] { typeof(Something2), typeof(SomeImplementation) };

            CollectionAssert.AreEquivalent(actual, expected);
        }

        [TestMethod]
        public void TestSingletonDependencies()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, SomeImplementation>(true);
            config.Register<ISomeKek, SomeKek>(false);
            var provider = new DIProvider(config);

            var obj1 = provider.Resolve<SomethingAbstract1>()[0];
            var obj2 = provider.Resolve<SomethingAbstract1>()[0];

            Assert.AreSame(obj1, obj2);

            var obj3 = provider.Resolve<ISomeKek>()[0];
            var obj4 = provider.Resolve<ISomeKek>()[0];

            Assert.AreNotSame(obj3, obj4);
        }

        [TestMethod]
        public void TestRecursiveDependencies()
        {
            var config = new DIConfig();
            config.Register<SomethingAbstract1, Something2>(true);
            config.Register<ISomeLol, SomeLol>(true);
            config.Register<ISomeLol, AnotherLol>(true);
            config.Register<ISomeKek, SomeKek>(true);
            var provider = new DIProvider(config);

            var complexImplement = (Something2)provider.Resolve<SomethingAbstract1>()[0];

            Type[] actual = new Type[] { complexImplement.smthng2[0].GetType(), complexImplement.smthng2[1].GetType() };
            Type[] expected = new Type[] { typeof(SomeLol), typeof(AnotherLol) };

            CollectionAssert.AreEquivalent(actual, expected);

            // Check that IAnimal dependency is created.
            Assert.AreEqual(typeof(SomeKek), complexImplement.smthng1.GetType());

        }
        
    }
}
