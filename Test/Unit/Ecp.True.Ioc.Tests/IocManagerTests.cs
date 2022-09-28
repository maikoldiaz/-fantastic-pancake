// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IocManagerTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ecp.True.Ioc.Tests.Types;
    using Ecp.True.Ioc.Tests.Types.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The IOC tests.
    /// </summary>
    [TestClass]
    [CLSCompliant(false)]
    public class IocManagerTests
    {
        /// <summary>
        /// The container.
        /// </summary>
        private static IContainer container;

        /// <summary>
        /// Cleanups the specified test context.
        /// </summary>
        [ClassCleanup]
        public static void Cleanup()
        {
            container.Dispose();
        }

        /// <summary>
        /// Initializes the specified test context.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            if (testContext != null)
            {
                container = IoCManager.RegisterByConvention();
            }
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [TestMethod]
        public void ResolveShouldBuildHierarichalInstanceWhenResolvingHierarichalType()
        {
            var component = container.Resolve<IHierarichal>();

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Hierarichal));

            var state = component.State;

            component = container.Resolve<IHierarichal>();
            Assert.AreEqual(state, component.State);
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [TestMethod]
        public void ResolveShouldBuildHierarichalInstanceWhenResolvingHierarichalTypeFromChildContainer()
        {
            var childContainer = container.BuildChildContainer();
            var component = childContainer.Resolve<IHierarichal>();

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Hierarichal));

            var state = component.State;

            component = childContainer.Resolve<IHierarichal>();
            Assert.AreEqual(state, component.State);

            var anotherChildContainer = container.BuildChildContainer();
            component = anotherChildContainer.Resolve<IHierarichal>();

            Assert.AreNotEqual(state, component.State);
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [TestMethod]
        public void ResolveShouldBuildNewInstanceWhenResolvingTransientType()
        {
            var component = container.Resolve<IAnother>();

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Another));
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [TestMethod]
        public void ResolveShouldBuildNewInstanceWhenResolvingTransientTypeByName()
        {
            var component = container.Resolve<IBase>("CONCRETE");

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Concrete));
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [Ignore]
        public void ResolveShouldBuildSingletonInstanceWhenResolvingSingletonType()
        {
            var component = container.Resolve<ISingleton>();

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Singleton));

            var state = component.State;

            component = container.Resolve<ISingleton>();
            Assert.AreEqual(state, component.State);
        }

        /// <summary>
        /// Resolve should build new instance when resolving transient type.
        /// </summary>
        [Ignore]
        public void ResolveShouldBuildSingletonInstanceWhenResolvingSingletonTypeFromChildContainer()
        {
            var component = container.Resolve<ISingleton>();

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Singleton));

            var state = component.State;

            component = container.BuildChildContainer().Resolve<ISingleton>();
            Assert.AreEqual(state, component.State);
        }

        /// <summary>
        /// Resolve should inject all dependent types in factory.
        /// </summary>
        [TestMethod]
        public void ResolveShouldInjectAllDependentTypesInFactory()
        {
            var factory = container.Resolve<IFactory>();

            Assert.IsNotNull(factory);
            Assert.IsInstanceOfType(factory, typeof(Factory));

            var instance = (Factory)factory;

            Assert.IsNotNull(instance.Generic);
            Assert.IsNotNull(instance.Specialized);
            Assert.IsNotNull(instance.Singleton);
            Assert.IsNotNull(instance.Hierarichal);
        }

        /// <summary>
        /// Resolve should not resolve array injection.
        /// </summary>
        [TestMethod]
        public void ResolveShouldNotResolveArrayInjection()
        {
            var arrayBase = container.Resolve<IArrayBase>();

            Assert.IsNotNull(arrayBase);
            Assert.IsTrue(arrayBase.Components.Any());
        }

        /// <summary>
        /// Resolves the should resolve closed generic types.
        /// </summary>
        [TestMethod]
        public void ResolveShouldResolveClosedGenericTypes()
        {
            var closedGeneric = container.Resolve<IClosed>();

            Assert.IsNotNull(closedGeneric);
            Assert.IsInstanceOfType(closedGeneric, typeof(Closed));
        }

        /// <summary>
        /// Resolve should resolve open generic types.
        /// </summary>
        [TestMethod]
        public void ResolveShouldResolveOpenGenericTypes()
        {
            var generic = container.Resolve<IRepo<Exception>>();

            Assert.IsNotNull(generic);
            Assert.IsInstanceOfType(generic, typeof(Repo<Exception>));
        }

        /// <summary>
        /// Resolve should build new instance when resolving named parameter by name and value.
        /// </summary>
        [TestMethod]
        public void ResolveShouldBuildNewInstanceWhenResolvingNamedParameterByNameAndValue()
        {
            var component = container.Resolve<ISingleton>("State", new object());

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Singleton));
        }

        /// <summary>
        /// Resolve should build new instance when resolving by type.
        /// </summary>
        [TestMethod]
        public void ResolveShouldBuildNewInstanceWhenResolvingByType()
        {
            var component = container.Resolve(typeof(ISingleton));

            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Singleton));
        }

        /// <summary>
        /// Resolve container with explicit types.
        /// </summary>
        [TestMethod]
        public void ResolveContainerWithExplicitTypes()
        {
            IDictionary<Type, Type> explicitTypes = new Dictionary<Type, Type>();
            explicitTypes.Add(typeof(ISingleton), typeof(Singleton));
            container = IoCManager.RegisterByConvention(explicitTypes);
            Assert.IsNotNull(container);

            var component = container.Resolve(typeof(ISingleton));
            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Singleton));
        }

        /// <summary>
        /// Resolve container with container builder.
        /// </summary>
        [TestMethod]
        public void ResolveContainerWithContainerBuilder()
        {
            container = IoCManager.RegisterByConvention(new Autofac.ContainerBuilder());
            Assert.IsNotNull(container);

            var component = container.Resolve(typeof(ISingleton));
            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Singleton));
        }

        /// <summary>
        /// Resolve container with explicit instances.
        /// </summary>
        [TestMethod]
        public void ResolveContainerWithExplicitInstances()
        {
            IDictionary<Type, object> explicitInstances = new Dictionary<Type, object>();
            explicitInstances.Add(typeof(ISingleton), new object());
            container = IoCManager.RegisterByConvention(explicitInstances);
            Assert.IsNotNull(container);

            var component = container.Resolve(typeof(IAnother));
            Assert.IsNotNull(component);
            Assert.IsInstanceOfType(component, typeof(Another));
        }

        /// <summary>
        /// Get empty container registrations if scope is null.
        /// </summary>
        [TestMethod]
        public void GetEmptyContainerRegistrationsIfScopeIsNull()
        {
            var result = container.Registrations;
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Get container registrations via Inner Container.
        /// </summary>
        [TestMethod]
        public void GetContainerRegistrationsViaInnerContainer()
        {
            var result = container.InnerContainer;
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Should ignore type test.
        /// </summary>
        [TestMethod]
        public void ShouldIgnoreTypeTest()
        {
            var result = IoCHelper.ShouldIgnoreType(null);
            Assert.IsTrue(result);
        }
    }
}