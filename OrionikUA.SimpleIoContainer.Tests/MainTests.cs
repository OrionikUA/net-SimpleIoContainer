using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrionikUA.SimpleIoContainer.Exceptions;
using OrionikUA.SimpleIoContainer.Tests.Helpers;

namespace OrionikUA.SimpleIoContainer.Tests
{
    [TestClass]
    public class MainTests
    {
        private IoContainer _container;

        [TestInitialize]
        public void Start()
        {
            _container = new IoContainer();
        }

        [TestMethod]
        public void BaseTest()
        {
            _container.Register<BaseClass>();
            var obj = _container.GetInstance<BaseClass>();
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void RegisterSecondTime()
        {
            _container.Register<BaseClass>();
            _container.Register<BaseClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotRegisteredException))]
        public void TryGetNotCreatedInstance()
        {
            _container.GetInstance<BaseClass>();
        }

        [TestMethod]
        public void ClassWithinClassGetInstance()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            var obj = _container.GetInstance<SecondClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.BaseClass);
        }

        [TestMethod]
        public void SameInstances()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            var objFirst = _container.GetInstance<SecondClass>();
            var objSecond = _container.GetInstance<SecondClass>();
            Assert.AreSame(objFirst, objSecond);
            Assert.AreSame(objFirst.BaseClass, objSecond.BaseClass);
        }

        [TestMethod]
        public void NotSameInstances()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            var objFirst = _container.GetInstance<SecondClass>();
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            var objSecond = _container.GetInstance<SecondClass>();
            Assert.AreNotSame(objFirst, objSecond);
            Assert.AreNotSame(objFirst.BaseClass, objSecond.BaseClass);
        }
    }
}
