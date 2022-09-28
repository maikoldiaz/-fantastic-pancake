// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeStorageLocationTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Api.Tests.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using Ecp.True.Core.Attributes;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The Node Storage Location Tests.
    /// </summary>
    [TestClass]
    public class NodeStorageLocationTests
    {
        /// <summary>
        /// Nodes the storage location should invoke validator to result error message store should have at least one product.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_ShouldInvokeValidator_ToResultErrorMessageStoreShouldHaveAtLeastOneProduct()
        {
            var nodeStorageLocation = new NodeStorageLocation
            {
                Name = "Node",
                Description = null,
                StorageLocationTypeId = 1,
                IsActive = true,
                NodeId = 1,
                SendToSap = true,
                StorageLocationId = "Storage Location",
            };

            var validationResults = this.ValidateModel(nodeStorageLocation);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.StoreShouldHaveAtLeastOneProduct);
        }

        /// <summary>
        /// Nodes the storage location should invoke validator to result logistic center required if send to sap is true.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_ShouldInvokeValidator_ToResultLogisticCenterRequired_IfSendToSap_IsTrue()
        {
            var nodeStorageLocation = new NodeStorageLocation
            {
                Name = "Node",
                Description = null,
                StorageLocationTypeId = 1,
                IsActive = true,
                NodeId = 1,
                SendToSap = true,
            };

            nodeStorageLocation.Products.Add(new StorageLocationProduct());

            var validationResults = this.ValidateModel(nodeStorageLocation);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.SapCodeRequired);
        }

        /// <summary>
        /// Nodes the storage location should invoke validator to result logistic center not required if send to sap is false.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_ShouldInvokeValidator_ToResultLogisticCenterNotRequired_IfSendToSap_IsFalse()
        {
            var nodeStorageLocation = new NodeStorageLocation
            {
                Name = "Node",
                Description = null,
                StorageLocationTypeId = 1,
                IsActive = true,
                NodeId = 1,
                SendToSap = true,
                StorageLocationId = "Storage Location",
            };

            nodeStorageLocation.Products.Add(new StorageLocationProduct());

            var validationResults = this.ValidateModel(nodeStorageLocation);
            Assert.AreEqual(0, validationResults.Count);
        }

        /// <summary>
        /// Nodes the storage location entity property name verify if name has data validation attributes.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_Name_VerifyIfNameHasDataValidationAttributes()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, string>> expression = nameProperty => nameProperty.Name;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;
            bool isRegularExpressionAttribute = member.GetCustomAttributes(typeof(RegularExpressionAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 3, "Three Attributes expected to be on Name attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on Name attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isMaxLengthAttribute, "Max Length attribute is expected on Name attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isRegularExpressionAttribute, "Regular Expression attribute is expected on Name attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity property description verify if description has data validation attributes.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_Description_VerifyIfDescriptionHasDataValidationAttributes()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, string>> expression = nameProperty => nameProperty.Description;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Two Attributes expected to be on Description attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isMaxLengthAttribute, "Max Length attribute is expected on Description attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity property storage location type identifier required is decorated.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_StorageLocationTypeId_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, int?>> expression = nameProperty => nameProperty.StorageLocationTypeId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on StorageLocationTypeId attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required Attribute attribute is expected on StorageLocationTypeId attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity property is active required is decorated.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_IsActive_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, bool?>> expression = nameProperty => nameProperty.IsActive;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on IsActive attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required Attribute attribute is expected on IsActive attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity property send to sap required is decorated.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_SendToSap_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, bool?>> expression = nameProperty => nameProperty.SendToSap;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on SendToSap attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required Attribute attribute is expected on SendToSap attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity property storage location identifier required if is decorated.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_StorageLocationId_RequiredIf_Is_Decorated()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, string>> expression = nameProperty => nameProperty.StorageLocationId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredIfAttribute = member.GetCustomAttributes(typeof(RequiredIfAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on StorageLocationId attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isRequiredIfAttribute, "Required If attribute is expected on StorageLocationId attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity property product locations must not be empty is decorated.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocation_Entity_Property_ProductLocations_MustNotBeEmpty_Is_Decorated()
        {
            // Arrange
            Expression<Func<NodeStorageLocation, ICollection<StorageLocationProduct>>> expression = nameProperty => nameProperty.Products;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMustNotBeEmptyAttribute = member.GetCustomAttributes(typeof(MustNotBeEmptyIfAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 2, "One Attributes expected to be on ProductLocations attribute of NodeStorageLocation entity.");
            Assert.IsTrue(isMustNotBeEmptyAttribute, "Must Not Be Empty attribute is expected on ProductLocations attribute of NodeStorageLocation entity.");
        }

        /// <summary>
        /// Nodes the storage location entity copy from should copy data when invoked.
        /// </summary>
        [TestMethod]
        public void NodeStorageLocationEntity_CopyFrom_ShouldCopyData_WhenInvoked()
        {
            var existing = new NodeStorageLocation() { StorageLocationId = "1000:M001", SendToSap = true };
            var newEntity = new NodeStorageLocation() { StorageLocationId = null, SendToSap = false };

            existing.CopyFrom(newEntity);

            // Assert
            Assert.AreEqual(existing.StorageLocationId, newEntity.StorageLocationId, "StorageLocationId should be copied.");
            Assert.AreEqual(existing.SendToSap, newEntity.SendToSap, "SendToSap should be copied.");
        }

        /// <summary>
        /// Validates the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The collection validation result.</returns>
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
