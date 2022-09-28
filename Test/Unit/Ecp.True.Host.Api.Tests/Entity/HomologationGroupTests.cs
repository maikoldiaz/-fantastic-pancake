// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationGroupTests.cs" company="Microsoft">
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
    /// The Homologation Group Tests.
    /// </summary>
    [TestClass]
    public class HomologationGroupTests
    {
        /// <summary>
        /// Homologations group the should invoke validator to result error message homologation group should have group type id.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidator_ToResultErrorMessageHomologationGroupShouldHaveGroupTypeId()
        {
            var validationResults = this.ValidateModel(new HomologationGroup());
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.GroupTypeIdRequired);
        }

        /// <summary>
        /// Homologation group entity property group type id required is decorated.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_Entity_Property_GroupTypeId_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<HomologationGroup, int?>> expression = nameProperty => nameProperty.GroupTypeId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on GroupTypeId attribute of HomologationGroup entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on GroupTypeId attribute of HomologationGroup entity.");
        }

        /// <summary>
        /// Homologations group the should invoke validator to result error message homologation group should have at least one objects.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidator_ToResultErrorMessageHomologationShouldHaveAtLeastOneObjects()
        {
            var homologationGroup = new HomologationGroup
            {
                GroupTypeId = 13,
            };

            var validationResults = this.ValidateModel(homologationGroup);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.HomologationShouldHaveAtleastOneHomologationObjects);
        }

        /// <summary>
        /// Homologations group the should invoke validator to result error message homologation group should have at least one data mapping.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidator_ToResultErrorMessageHomologationShouldHaveAtLeastOneDataMapping()
        {
            var homologationGroup = new HomologationGroup
            {
                GroupTypeId = 14,
            };
            homologationGroup.HomologationObjects.Add(new HomologationObject());
            var validationResults = this.ValidateModel(homologationGroup);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.HomologationShouldHaveAtleastOneDataMapping);
        }

        /// <summary>
        /// Homologations the should invoke validator must not be empty attribute to result validator success if homologation object value is not empty collection.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidatorMustNotBeEmptyAttribute_ToResultValidatorSuccess_If_HomologationObjectValue_Is_NotEmptyCollection()
        {
            var homologationGroup = new HomologationGroup
            {
                GroupTypeId = 15,
            };

            homologationGroup.HomologationObjects.Add(new HomologationObject());
            var attribute = new MustNotBeEmptyAttribute();
            var result = attribute.GetValidationResult(homologationGroup.HomologationObjects, new ValidationContext(homologationGroup));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Homologations the should invoke validator must not be empty attribute to result validator success if homologation data mapping value is not empty collection.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidatorMustNotBeEmptyAttribute_ToResultValidatorSuccess_If_HomologationDataMappingValue_Is_NotEmptyCollection()
        {
            var homologationGroup = new HomologationGroup
            {
                GroupTypeId = 14,
            };

            homologationGroup.HomologationDataMapping.Add(new HomologationDataMapping());
            var attribute = new MustNotBeEmptyAttribute();
            var result = attribute.GetValidationResult(homologationGroup.HomologationDataMapping, new ValidationContext(homologationGroup));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Homologations the should invoke validator must not be empty attribute to result validator success if homologation object value is empty collection.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidatorMustNotBeEmptyAttribute_ToResultValidatorSuccess_If_HomologationObjectValue_Is_EmptyCollection()
        {
            var homologationGroup = new HomologationGroup
            {
                GroupTypeId = 15,
            };

            var attribute = new MustNotBeEmptyAttribute();
            var result = attribute.GetValidationResult(homologationGroup.HomologationObjects, new ValidationContext(homologationGroup));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Homologations the should invoke validator must not be empty attribute to result validator success if homologation data mapping value is empty collection.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_ShouldInvokeValidatorMustNotBeEmptyAttribute_ToResultValidatorSuccess_If_HomologationDataMappingValue_Is_EmptyCollection()
        {
            var homologationGroup = new HomologationGroup
            {
                GroupTypeId = 13,
            };

            var attribute = new MustNotBeEmptyAttribute();
            var result = attribute.GetValidationResult(homologationGroup.HomologationDataMapping, new ValidationContext(homologationGroup));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// HomologationsGroup the entity property homologation objects must not be empty is decorated.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_Entity_Property_HomologationObjects_MustNotBeEmpty_Is_Decorated()
        {
            // Arrange
            Expression<Func<HomologationGroup, ICollection<HomologationObject>>> expression = nameProperty => nameProperty.HomologationObjects;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMustNotBeEmptyAttribute = member.GetCustomAttributes(typeof(MustNotBeEmptyAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on HomologationObjects attribute of HomologationGroup entity.");
            Assert.IsTrue(isMustNotBeEmptyAttribute, "Must Not Be Empty attribute is expected on HomologationObjects attribute of HomologationGroup entity.");
        }

        /// <summary>
        /// HomologationsGroup the entity property homologation data mapping must not be empty is decorated.
        /// </summary>
        [TestMethod]
        public void HomologationGroup_Entity_Property_HomologationDataMapping_MustNotBeEmpty_Is_Decorated()
        {
            // Arrange
            Expression<Func<HomologationGroup, ICollection<HomologationDataMapping>>> expression = nameProperty => nameProperty.HomologationDataMapping;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMustNotBeEmptyAttribute = member.GetCustomAttributes(typeof(MustNotBeEmptyAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on HomologationDataMapping attribute of HomologationGroup entity.");
            Assert.IsTrue(isMustNotBeEmptyAttribute, "Must Not Be Empty attribute is expected on HomologationDataMapping attribute of HomologationGroup entity.");
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
