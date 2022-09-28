// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeTests.cs" company="Microsoft">
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
    /// The Node Tests.
    /// </summary>
    [TestClass]
    public class NodeTests
    {
        /// <summary>
        /// Nodes the should invoke validator to result error message node should have atleast one store.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidator_ToResultErrorMessageNodeShouldHaveAtleastOneStore()
        {
            var node = new Node
            {
                Name = "Node",
                Description = null,
                IsActive = true,
                NodeTypeId = 1,
                OperatorId = 2,
                SegmentId = 3,
                SendToSap = true,
                LogisticCenterId = "Center 1",
            };

            var validationResults = this.ValidateModel(node);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.NodeShouldHaveAtleastOneStore);
        }

        /// <summary>
        /// Nodes the should invoke validator for required if to result error message from validator if property does not exists.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorForRequiredIf_ToResultErrorMessageFromValidator_If_Property_DoesNotExists()
        {
            string input = "myteststring";
            var attribute = new RequiredIfAttribute("test", false);
            var result = attribute.GetValidationResult(input, new ValidationContext(input));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator for required if to result error message from validator if value is null.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorForRequiredIf_ToResultErrorMessageFromValidator_If_Value_Is_Null()
        {
            var node = new Node { SendToSap = true };
            var attribute = new RequiredIfAttribute("SendToSap", true);
            var result = attribute.GetValidationResult(null, new ValidationContext(node));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator for required if to result error message from validator if value is empty string.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorForRequiredIf_ToResultErrorMessageFromValidator_If_Value_Is_EmptyString()
        {
            var node = new Node { SendToSap = true };
            var attribute = new RequiredIfAttribute("SendToSap", true);
            var result = attribute.GetValidationResult(string.Empty, new ValidationContext(node));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator for required if attribute to result validator success if value is different from property value.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorForRequiredIfAttribute_ToResultValidatorSuccess_If_Value_Is_Different_FromPropertyValue()
        {
            var node = new Node { IsActive = false };
            var attribute = new RequiredIfAttribute("IsActive", true);
            var result = attribute.GetValidationResult("LogisticCenterId", new ValidationContext(node));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator for required if attribute to check requires validation context is true.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorForRequiredIfAttribute_ToCheck_RequiresValidationContext_Is_True()
        {
            var attribute = new RequiredIfAttribute("SendToSap", true);
            Assert.IsTrue(attribute.RequiresValidationContext);
        }

        /// <summary>
        /// Nodes the should invoke validator must not be empty if attribute to result error message from validator if value is null.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorMustNotBeEmptyIfAttribute_ToResultErrorMessageFromValidator_If_Value_Is_Null()
        {
            var node = new Node { IsActive = true };
            var attribute = new MustNotBeEmptyIfAttribute("IsActive", true);
            var result = attribute.GetValidationResult(null, new ValidationContext(node));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator must not be empty if attribute to result error message from validator if value is empty collection.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorMustNotBeEmptyIfAttribute_ToResultErrorMessageFromValidator_If_Value_Is_EmptyCollection()
        {
            var node = new Node { IsActive = true };
            var attribute = new MustNotBeEmptyIfAttribute("IsActive", true);
            var result = attribute.GetValidationResult(new List<NodeStorageLocation>(), new ValidationContext(node));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator must not be empty if attribute to result validation success if value is any object other than i collection.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorMustNotBeEmptyIfAttribute_ToResultValidationSuccess_If_Value_Is_AnyObject_OtherThanICollection()
        {
            var node = new Node { IsActive = true };
            var attribute = new MustNotBeEmptyIfAttribute("IsActive", true);
            var result = attribute.GetValidationResult(new NodeStorageLocation(), new ValidationContext(node));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator must not be empty if attribute to result validator success if value is not empty collection.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorMustNotBeEmptyIfAttribute_ToResultValidatorSuccess_If_Value_Is_NotEmptyCollection()
        {
            var node = new Node { IsActive = true };
            node.NodeStorageLocations.Add(new NodeStorageLocation());
            var attribute = new MustNotBeEmptyIfAttribute("IsActive", true);
            var result = attribute.GetValidationResult(node.NodeStorageLocations, new ValidationContext(node));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator for must not be empty if attribute to result error message from validator if property does not exists.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorForMustNotBeEmptyIfAttribute_ToResultErrorMessageFromValidator_If_Property_DoesNotExists()
        {
            string input = "myteststring";
            var attribute = new MustNotBeEmptyIfAttribute("test", false);
            var result = attribute.GetValidationResult(input, new ValidationContext(input));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator must not be empty if attribute to result validator success if value is different from property value.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorMustNotBeEmptyIfAttribute_ToResultValidatorSuccess_If_Value_Is_Different_FromPropertyValue()
        {
            var node = new Node { IsActive = false };
            var attribute = new MustNotBeEmptyIfAttribute("IsActive", true);
            var result = attribute.GetValidationResult("NodeStorageLocations", new ValidationContext(node));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Nodes the should invoke validator must not be empty if attribute to check requires validation context is true.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidatorMustNotBeEmptyIfAttribute_ToCheck_RequiresValidationContext_Is_True()
        {
            var attribute = new MustNotBeEmptyIfAttribute("IsActive", true);
            Assert.IsTrue(attribute.RequiresValidationContext);
        }

        /// <summary>
        /// Nodes the should invoke validator to result logistic center required if send to sap is true.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidator_ToResultLogisticCenterRequired_IfSendToSap_IsTrue()
        {
            var node = new Node
            {
                Name = "Node",
                Description = null,
                IsActive = true,
                NodeTypeId = 1,
                OperatorId = 2,
                SegmentId = 3,
                SendToSap = true,
            };

            node.NodeStorageLocations.Add(new NodeStorageLocation());

            var validationResults = this.ValidateModel(node);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.SapCodeRequired);
        }

        /// <summary>
        /// Nodes the should invoke validator to result logistic center not required if send to sap is false.
        /// </summary>
        [TestMethod]
        public void Node_ShouldInvokeValidator_ToResultLogisticCenterNotRequired_IfSendToSap_IsFalse()
        {
            var node = new Node
            {
                Name = "Node",
                Description = null,
                IsActive = true,
                NodeTypeId = 1,
                OperatorId = 2,
                SegmentId = 3,
                SendToSap = false,
            };

            node.NodeStorageLocations.Add(new NodeStorageLocation());

            var validationResults = this.ValidateModel(node);
            Assert.AreEqual(0, validationResults.Count);
        }

        /// <summary>
        /// Nodes the entity property name verify if name has data validation attributes.
        /// </summary>
        [TestMethod]
        public void Node_Entity_Property_Name_VerifyIfNameHasDataValidationAttributes()
        {
            // Arrange
            Expression<Func<Node, string>> expression = nameProperty => nameProperty.Name;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;
            bool isRegularExpressionAttribute = member.GetCustomAttributes(typeof(RegularExpressionAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 3, "Three Attributes expected to be on Name attribute of Node entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on Name attribute of Node entity.");
            Assert.IsTrue(isMaxLengthAttribute, "String Length attribute is expected on Name attribute of Node entity.");
            Assert.IsTrue(isRegularExpressionAttribute, "Regular Expression attribute is expected on Name attribute of Node entity.");
        }

        /// <summary>
        /// Nodes the entity property description maximum length is decorated.
        /// </summary>
        [TestMethod]
        public void Node_Entity_Property_Description_MaxLength_Is_Decorated()
        {
            // Arrange
            Expression<Func<Node, string>> expression = nameProperty => nameProperty.Description;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Two Attributes expected to be on Description attribute of Node entity.");
            Assert.IsTrue(isMaxLengthAttribute, "Max Length attribute is expected on Description attribute of Node entity.");
        }

        /// <summary>
        /// Nodes the entity property is active required is decorated.
        /// </summary>
        [TestMethod]
        public void Node_Entity_Property_IsActive_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<Node, bool?>> expression = nameProperty => nameProperty.IsActive;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on IsActive attribute of Node entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on IsActive attribute of Node entity.");
        }

        /// <summary>
        /// Nodes the entity property send to sap required is decorated.
        /// </summary>
        [TestMethod]
        public void Node_Entity_Property_SendToSap_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<Node, bool?>> expression = nameProperty => nameProperty.SendToSap;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on SendToSap attribute of Node entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on SendToSap attribute of Node entity.");
        }

        /// <summary>
        /// Nodes the entity property logistic center identifier required if is decorated.
        /// </summary>
        [TestMethod]
        public void Node_Entity_Property_LogisticCenterId_RequiredIf_Is_Decorated()
        {
            // Arrange
            Expression<Func<Node, string>> expression = nameProperty => nameProperty.LogisticCenterId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredIfAttribute = member.GetCustomAttributes(typeof(RequiredIfAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on LogisticCenterId attribute of Node entity.");
            Assert.IsTrue(isRequiredIfAttribute, "Required If attribute is expected on LogisticCenterId attribute of Node entity.");
        }

        /// <summary>
        /// Nodes the entity property node storage locations must not be empty is decorated.
        /// </summary>
        [TestMethod]
        public void Node_Entity_Property_NodeStorageLocations_MustNotBeEmpty_Is_Decorated()
        {
            // Arrange
            Expression<Func<Node, ICollection<NodeStorageLocation>>> expression = nameProperty => nameProperty.NodeStorageLocations;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMustNotBeEmptyAttribute = member.GetCustomAttributes(typeof(MustNotBeEmptyIfAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on NodeStorageLocations attribute of Node entity.");
            Assert.IsTrue(isMustNotBeEmptyAttribute, "Must Not Be Empty attribute is expected on NodeStorageLocations attribute of Node entity.");
        }

        /// <summary>
        /// Nodes the entity copy from should copy data when invoked.
        /// </summary>
        [TestMethod]
        public void NodeEntity_CopyFrom_ShouldCopyData_WhenInvoked()
        {
            var existing = new Node() { LogisticCenterId = "1000:M001", SendToSap = true };
            var newEntity = new Node() { LogisticCenterId = null, SendToSap = false };

            existing.CopyFrom(newEntity);

            // Assert
            Assert.AreEqual(existing.LogisticCenterId, newEntity.LogisticCenterId, "LogisticCenterId should be copied.");
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
