// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationSystemTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Processors.Transform.Homologate.Entities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The homologation system tests.
    /// </summary>
    [TestClass]
    public class HomologationSystemTests
    {
        /// <summary>
        /// The homologation system.
        /// </summary>
        private HomologationSystem homologationSystem;

        /// <summary>
        /// Initilizes this instance.
        /// </summary>
        [TestInitialize]
        public void Initilize()
        {
            this.homologationSystem = this.CreateHomologationSystems();
        }

        /// <summary>
        /// Adds the objects should add object asynchronous.
        /// </summary>
        [TestMethod]
        public void AddObjects_Should_AddObject()
        {
            var homologationObjects = this.GetHomologationObjects();

            this.homologationSystem.AddObjects(homologationObjects);

            Assert.IsNotNull(this.homologationSystem.Objects);
            Assert.AreEqual(this.homologationSystem.Objects.Count, homologationObjects.Count);
            Assert.AreEqual(0, this.homologationSystem.Map.ConfigurationMaps.ToList().Count);
        }

        /// <summary>
        /// Adds the objects should add update objects when invoked.
        /// </summary>
        [TestMethod]
        public void AddObjects_Should_IgnoreDuplicateObjects()
        {
            var homologationObjects = this.GetHomologationObjects();

            this.homologationSystem.AddObjects(homologationObjects);
            this.homologationSystem.AddObjects(this.GetHomologationObjects());

            Assert.IsNotNull(this.homologationSystem.Objects);
            Assert.AreEqual(homologationObjects.Count, this.homologationSystem.Objects.Count);
            Assert.AreEqual(1, this.homologationSystem.Objects.Count(m => m.Key == "ProductId"));
            Assert.AreEqual(1, this.homologationSystem.Objects.Count(m => m.Key == "ProductName"));
            Assert.AreEqual(1, this.homologationSystem.Objects.Count(m => m.Key == "MovementTypeId"));
            Assert.AreEqual(0, this.homologationSystem.Map.ConfigurationMaps.ToList().Count);
        }

        /// <summary>
        /// Adds the mappings should add mappings when invoked.
        /// </summary>
        [TestMethod]
        public void AddMappings_Should_AddMappingsWhenInvoked()
        {
            var homologationDataMapping = this.GetHomologationDataMappings();

            this.homologationSystem.AddMappings(homologationDataMapping);

            Assert.IsNotNull(this.homologationSystem.Map);
            Assert.AreEqual(0, this.homologationSystem.Objects.Count);
            Assert.AreEqual(3, this.homologationSystem.Map.ConfigurationMaps.ToList().Count);
        }

        /// <summary>
        /// Adds the mappings should add mappings when invoked.
        /// </summary>
        [TestMethod]
        public void AddMappings_Should_IgnoreDuplicateMappings()
        {
            var homologationDataMapping = this.GetHomologationDataMappings();

            this.homologationSystem.AddMappings(homologationDataMapping);
            this.homologationSystem.AddMappings(this.GetHomologationDataMappings());

            Assert.IsNotNull(this.homologationSystem.Map);
            Assert.AreEqual(0, this.homologationSystem.Objects.Count);
            Assert.AreEqual(3, this.homologationSystem.Map.ConfigurationMaps.ToList().Count);
            Assert.AreEqual(1, this.homologationSystem.Map.ConfigurationMaps.Where(m => m.Name == "OriginalValue_1").ToList().Count);
            Assert.AreEqual(1, this.homologationSystem.Map.ConfigurationMaps.Where(m => m.Name == "OriginalValue_2").ToList().Count);
            Assert.AreEqual(1, this.homologationSystem.Map.ConfigurationMaps.Where(m => m.Name == "OriginalValue_3").ToList().Count);
        }

        /// <summary>
        /// Homologates the should homologate when invoked.
        /// </summary>
        [TestMethod]
        public void Homologate_Should_HomologateWhenInvoked()
        {
            var homologationDataMapping = this.GetHomologationDataMappings();
            this.homologationSystem.AddMappings(homologationDataMapping);
            var homologationObjects = this.GetHomologationObjects();
            this.homologationSystem.AddObjects(homologationObjects);

            var homologatedValue = this.homologationSystem.Homologate("ProductId", "OriginalValue_1", MessageType.Movement);
            Assert.IsNotNull(homologatedValue);
            Assert.AreEqual("UpdatedValue_1", homologatedValue);
        }

        /// <summary>
        /// Homologates the should homologate when invoked.
        /// </summary>
        [TestMethod]
        public void Homologate_ShouldReturnHomologateException_WhenHomologationNotValidkey()
        {
            var homologationDataMapping = this.GetHomologationDataMappings();
            this.homologationSystem.AddMappings(homologationDataMapping);
            var homologationObjects = this.GetHomologationObjects();
            this.homologationSystem.AddObjects(homologationObjects);
            var ex = Assert.ThrowsException<KeyNotFoundException>(() => this.homologationSystem.Homologate("MovementTypeId", "OriginalValue_1", MessageType.Movement));

            var message = string.Format(CultureInfo.InvariantCulture, Constants.ValueNotFound, "OriginalValue_1", "Movement");

            Assert.IsNotNull(ex);
            Assert.AreEqual(message, ex.Message);
        }

        /// <summary>
        /// Homologates the should return original value if not required when invoked.
        /// </summary>
        [TestMethod]
        public void Homologate_ShouldReturnHomologateValue_WhenHomologationNotRequired()
        {
            var homologationDataMapping = this.GetHomologationDataMappings();
            this.homologationSystem.AddMappings(homologationDataMapping);
            var homologationObjects = this.GetHomologationObjects();
            _ = homologationObjects.All(x => x.IsRequiredMapping = false);
            this.homologationSystem.AddObjects(homologationObjects);

            var homologatedValue = this.homologationSystem.Homologate("ProductId", "OriginalValue_1", MessageType.Movement);
            Assert.IsNotNull(homologatedValue);
            Assert.AreEqual("OriginalValue_1", homologatedValue);
        }

        /// <summary>
        /// Homologates the should return original value if object not exists when invoked.
        /// </summary>
        [TestMethod]
        public void Homologate_ShouldReturnOriginalValue_WhenObjectNotExists()
        {
            var homologationDataMapping = this.GetHomologationDataMappings();
            this.homologationSystem.AddMappings(homologationDataMapping);
            var homologationObjects = this.GetHomologationObjects();
            _ = homologationObjects.All(x => x.IsRequiredMapping = false);
            this.homologationSystem.AddObjects(homologationObjects);

            var homologatedValue = this.homologationSystem.Homologate("ProductId_1", "OriginalValue_1", MessageType.Movement);

            Assert.IsNotNull(homologatedValue);
            Assert.AreEqual("OriginalValue_1", homologatedValue);
        }

        /// <summary>
        /// Homologate should return null when original value is null.
        /// </summary>
        [TestMethod]
        public void Homologate_ShouldReturnNull_WhenOriginalValueIsNull()
        {
            var result = this.homologationSystem.Homologate("ProductId", null, MessageType.Movement);
            Assert.IsNull(result);
        }

        /// <summary>
        /// Homologates the should throw exception if object not exists when invoked.
        /// </summary>
        [TestMethod]
        public void Homologate_ShouldThrowException_WhenRequiredValueNotExistsWhenInvoked()
        {
            string originalValue = "OriginalValue_11";
            var homologationDataMapping = this.GetHomologationDataMappings();
            this.homologationSystem.AddMappings(homologationDataMapping);
            var homologationObjects = this.GetHomologationObjects();
            this.homologationSystem.AddObjects(homologationObjects);

            var ex = Assert.ThrowsException<KeyNotFoundException>(() => this.homologationSystem.Homologate("ProductId", originalValue, MessageType.Movement));

            var message = string.Format(CultureInfo.InvariantCulture, Constants.ValueNotFound, originalValue, "Product");

            Assert.IsNotNull(ex);
            Assert.AreEqual(message, ex.Message);
        }

        /// <summary>
        /// Homologates the should throw exception if movement type not exists when contract is message type and is invoked.
        /// </summary>
        [TestMethod]
        public void Homologate_ShouldThrowException_WhenRequiredMovementTypeValueAndMessageTypeIsContractNotExistsWhenInvoked()
        {
            string originalValue = "OriginalValue_11";
            var homologationDataMapping = this.GetHomologationDataMappings();
            this.homologationSystem.AddMappings(homologationDataMapping);
            var homologationObjects = this.GetHomologationObjects();
            this.homologationSystem.AddObjects(homologationObjects);

            var ex = Assert.ThrowsException<KeyNotFoundException>(() => this.homologationSystem.Homologate("MovementTypeId", originalValue, MessageType.Contract));

            var message = Constants.ValueNotFoundMovementType;

            Assert.IsNotNull(ex);
            Assert.AreEqual(message, ex.Message);
        }

        private HomologationSystem CreateHomologationSystems()
        {
            var homologationObjectTypes = new List<HomologationObjectType>();
            homologationObjectTypes.Add(new HomologationObjectType { HomologationObjectTypeId = 1, Name = "ProductId" });
            homologationObjectTypes.Add(new HomologationObjectType { HomologationObjectTypeId = 2, Name = "ProductName" });
            homologationObjectTypes.Add(new HomologationObjectType { HomologationObjectTypeId = 3, Name = "MovementTypeId" });
            return new HomologationSystem(homologationObjectTypes);
        }

        private IList<HomologationDataMapping> GetHomologationDataMappings()
        {
            var homologationDataMapping = new List<HomologationDataMapping>();
            homologationDataMapping.Add(new HomologationDataMapping { SourceValue = "OriginalValue_1", DestinationValue = "UpdatedValue_1", HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product", Description = "Product" } } });
            homologationDataMapping.Add(new HomologationDataMapping { SourceValue = "OriginalValue_2", DestinationValue = "UpdatedValue_2", HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product", Description = "Product" } } });
            homologationDataMapping.Add(new HomologationDataMapping { SourceValue = "OriginalValue_3", DestinationValue = "UpdatedValue_3", HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Movement", Description = "Movement" } } });
            return homologationDataMapping;
        }

        private IList<HomologationObject> GetHomologationObjects()
        {
            var homologationObjects = new List<HomologationObject>();
            homologationObjects.Add(new HomologationObject { HomologationObjectTypeId = 1, IsRequiredMapping = true, HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product", Description = "Product" } } });
            homologationObjects.Add(new HomologationObject { HomologationObjectTypeId = 2, IsRequiredMapping = false, HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Product", Description = "Product" } } });
            homologationObjects.Add(new HomologationObject { HomologationObjectTypeId = 3, IsRequiredMapping = true, HomologationGroup = new HomologationGroup { GroupTypeId = 14, Group = new Category { Name = "Movement", Description = "Movement" } } });
            return homologationObjects;
        }
    }
}
