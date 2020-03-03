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
        [ExpectedException(typeof(TypeAlreadyRegisteredException))]
        public void RegisterSecondTimeWithGenericTest()
        {
            _container.Register<BaseClass>();
            _container.Register<BaseClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeAlreadyRegisteredException))]
        public void RegisterSecondTimeWithFuncTest()
        {
            _container.Register(() => new BaseClass());
            _container.Register(() => new BaseClass());
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotRegisteredException))]
        public void TryGetNotCreatedInstanceTest()
        {
            _container.GetInstance<BaseClass>();
        }

        [TestMethod]
        public void ClassWithinClassGetInstanceTest()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            var obj = _container.GetInstance<SecondClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.BaseClass);
        }

        [TestMethod]
        public void SameInstancesTest()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            var objFirst = _container.GetInstance<SecondClass>();
            var objSecond = _container.GetInstance<SecondClass>();
            Assert.AreSame(objFirst, objSecond);
            Assert.AreSame(objFirst.BaseClass, objSecond.BaseClass);
        }

        [TestMethod]
        public void CreateNewInstanceTest()
        {
            _container.Register(() => new BaseClass());
            var firstBase = _container.GetNewInstance<BaseClass>();
            var secondBase = _container.GetNewInstance<BaseClass>();
            _container.Register<SecondClass>();
            var first = _container.GetNewInstance<SecondClass>();
            var second = _container.GetNewInstance<SecondClass>();
            Assert.AreNotSame(firstBase, secondBase);
            Assert.AreNotSame(first, second);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeNotRegisteredException))]
        public void TryGetNewEmptyInstanceTest()
        {
            _container.GetNewInstance<BaseClass>();
        }


        [TestMethod]
        public void TypeNotRegisteredExceptionTest()
        {
            try
            {
                _container.GetNewInstance<BaseClass>();
            }
            catch (TypeNotRegisteredException e)
            {
                Assert.AreEqual(typeof(BaseClass), e.Type);
                Assert.AreEqual($"Type {typeof(BaseClass).Name} was not been registered in the container!", e.Message);
                return;
            }
            Assert.Fail("Exception has not been thrown");
        }

        [TestMethod]
        public void TypeAlreadyRegisteredExceptionTest()
        {
            try
            {
                _container.Register<BaseClass>();
                _container.Register<BaseClass>();
            }
            catch (TypeAlreadyRegisteredException e)
            {
                Assert.AreEqual(typeof(BaseClass), e.Type);
                Assert.AreEqual($"Type {typeof(BaseClass).Name} has already been registered in the container!", e.Message);
                return;
            }
            Assert.Fail("Exception has not been thrown");
        }

        [TestMethod]
        public void TypeIsAbstractExceptionTest()
        {
            try
            {
                _container.Register<IBaseClass>();
            }
            catch (TypeIsAbstractException e)
            {
                Assert.AreEqual(typeof(IBaseClass), e.Type);
                Assert.AreEqual($"Type {typeof(IBaseClass).Name} is abstract, it cannot be registered!", e.Message);
                return;
            }
            Assert.Fail("Exception has not been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(TypeIsAbstractException))]
        public void TypeIsInterfaceTest()
        {
            _container.Register<IBaseClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(TypeIsAbstractException))]
        public void TypeIsAbstractTest()
        {
            _container.Register<BaseAbstractClass>();
        }

        [TestMethod]
        public void RegisterInterfaceTest()
        {
            _container.Register<IBaseClass, BaseClass>();
            var obj = _container.GetInstance<IBaseClass>();
            var obj2 = _container.GetNewInstance<IBaseClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        public void RegisterAbstractTest()
        {
            _container.Register<BaseAbstractClass, BaseClass>();
            var obj = _container.GetInstance<BaseAbstractClass>();
            var obj2 = _container.GetNewInstance<BaseAbstractClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        public void RegisterInterfaceFuncTest()
        {
            _container.Register<IBaseClass, BaseClass>(() => new BaseClass());
            var obj = _container.GetInstance<IBaseClass>();
            var obj2 = _container.GetNewInstance<IBaseClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        public void RegisterAbstractFuncTest()
        {
            _container.Register<BaseAbstractClass, BaseClass>(() => new BaseClass());
            var obj = _container.GetInstance<BaseAbstractClass>();
            var obj2 = _container.GetNewInstance<BaseAbstractClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        public void RegisterInterfaceFuncWithoutGenericTypeTest()
        {
            _container.Register<IBaseClass>(() => new BaseClass());
            var obj = _container.GetInstance<IBaseClass>();
            var obj2 = _container.GetNewInstance<IBaseClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        public void RegisterAbstractFuncWithoutGenericTypeTest()
        {
            _container.Register<BaseAbstractClass>(() => new BaseClass());
            var obj = _container.GetInstance<BaseAbstractClass>();
            var obj2 = _container.GetNewInstance<BaseAbstractClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj2);
            Assert.AreNotSame(obj, obj2);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeAlreadyRegisteredException))]
        public void TypeAlreadyRegisteredFuncInterface()
        {
            _container.Register<IBaseClass, BaseClass>(() => new BaseClass());
            _container.Register<IBaseClass, BaseClass>(() => new BaseClass());
        }

        [TestMethod]
        [ExpectedException(typeof(TypeAlreadyRegisteredException))]
        public void TypeAlreadyRegisteredAbstract()
        {
            _container.Register<BaseAbstractClass, BaseClass>();
            _container.Register<BaseAbstractClass, BaseClass>();
        }

        [TestMethod]
        [ExpectedException(typeof(MoreThanOneConstructorException))]
        public void SeveralConstructorsTest()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            _container.Register<SeveralConstructorsClass>();

            _container.GetInstance<SeveralConstructorsClass>();
        }
        
        [TestMethod]
        [ExpectedException(typeof(MoreThanOneConstructorException))]
        public void SeveralConstructorsWithSeveralPreferredAttributesClassTest()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            _container.Register<SeveralConstructorsWithSeveralPreferredAttributesClass>();

            _container.GetInstance<SeveralConstructorsWithSeveralPreferredAttributesClass>();
        }
        
        [TestMethod]
        public void SeveralConstructorsWithPreferredAttributeClassTest()
        {
            _container.Register<BaseClass>();
            _container.Register<SecondClass>();
            _container.Register<SeveralConstructorsWithPreferredAttributeClass>();

            var obj = _container.GetInstance<SeveralConstructorsWithPreferredAttributeClass>();
            Assert.IsNotNull(obj);
            Assert.IsNotNull(obj.Type);
            Assert.AreEqual(obj.Type, typeof(SecondClass));
        }


    }
}
