// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationTests.cs" company="Microsoft">
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
    using Ecp.True.Entities.Dto;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The Homologation Tests.
    /// </summary>
    [TestClass]
    public class HomologationTests
    {
        /// <summary>
        /// Homologations the should invoke validator to result error message homologation should have system id.
        /// </summary>
        [TestMethod]
        public void Homologation_ShouldInvokeValidator_ToResultErrorMessageHomologationShouldHaveSystemId()
        {
            var homologation = new Homologation
            {
                HomologationId = 1,
                SourceSystemId = null,
                DestinationSystemId = (int)SystemType.TRUE,
            };

            var validationResults = this.ValidateModel(homologation);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.SourceSystemIdRequired);
        }

        /// <summary>
        /// Homologations the should invoke validator to result error message homologation should have destination system id.
        /// </summary>
        [TestMethod]
        public void Homologation_ShouldInvokeValidator_ToResultErrorMessageHomologationShouldHaveDestinationId()
        {
            var homologation = new Homologation
            {
                HomologationId = 1,
                SourceSystemId = (int)SystemType.TRUE,
                DestinationSystemId = null,
            };

            var validationResults = this.ValidateModel(homologation);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.DestinationSystemIdRequired);
        }

        /// <summary>
        /// Homologation entity property source system id required is decorated.
        /// </summary>
        [TestMethod]
        public void Homologation_Entity_Property_SourceSystemId_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<Homologation, int?>> expression = nameProperty => nameProperty.SourceSystemId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on SourceSystemId attribute of Homologation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on SourceSystemId attribute of Homologation entity.");
        }

        /// <summary>
        /// Homologation entity property destination system id required is decorated.
        /// </summary>
        [TestMethod]
        public void Homologation_Entity_Property_DestinationSystemId_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<Homologation, int?>> expression = nameProperty => nameProperty.DestinationSystemId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on DestinationSystemId attribute of Homologation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on DestinationSystemId attribute of Homologation entity.");
        }

        /// <summary>
        /// Homologations the should invoke validator to result error message homologation should have at least one group.
        /// </summary>
        [TestMethod]
        public void Homologation_ShouldInvokeValidator_ToResultErrorMessageHomologationShouldHaveAtLeastOneGroup()
        {
            var homologation = new Homologation
            {
                HomologationId = 1,
                SourceSystemId = (int)SystemType.TRUE,
                DestinationSystemId = (int)SystemType.SINOPER,
            };

            var validationResults = this.ValidateModel(homologation);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.HomologationShouldHaveAtleastOneGroup);
        }

        /// <summary>
        /// Homologations the should invoke validator must not be empty attribute to result validator success if value is not empty collection.
        /// </summary>
        [TestMethod]
        public void Homologation_ShouldInvokeValidatorMustNotBeEmptyAttribute_ToResultValidatorSuccess_If_Value_Is_NotEmptyCollection()
        {
            var homologation = new Homologation
            {
                HomologationId = 1,
                SourceSystemId = (int)SystemType.TRUE,
                DestinationSystemId = (int)SystemType.SINOPER,
            };

            homologation.HomologationGroups.Add(new HomologationGroup());
            var attribute = new MustNotBeEmptyAttribute();
            var result = attribute.GetValidationResult(homologation.HomologationGroups, new ValidationContext(homologation));

            Assert.IsNull(result);
        }

        /// <summary>
        /// Homologations the should invoke validator for must not be empty attribute to result error message from validator if value is empty.
        /// </summary>
        [TestMethod]
        public void Homologation_ShouldInvokeValidatorForMustNotBeEmptyAttribute_ToResultValidatorError_If_Value_Is_EmptyCollection()
        {
            var homologation = new Homologation
            {
                HomologationId = 1,
                SourceSystemId = (int)SystemType.TRUE,
                DestinationSystemId = (int)SystemType.SINOPER,
            };
            var attribute = new MustNotBeEmptyAttribute();
            var result = attribute.GetValidationResult(homologation.HomologationGroups, new ValidationContext(homologation));

            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Homologations the entity property homologation groups must not be empty is decorated.
        /// </summary>
        [TestMethod]
        public void Homologation_Entity_Property_HomologationGroups_MustNotBeEmpty_Is_Decorated()
        {
            // Arrange
            Expression<Func<Homologation, ICollection<HomologationGroup>>> expression = nameProperty => nameProperty.HomologationGroups;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMustNotBeEmptyAttribute = member.GetCustomAttributes(typeof(MustNotBeEmptyAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on HomologationGroups attribute of Homologation entity.");
            Assert.IsTrue(isMustNotBeEmptyAttribute, "Must Not Be Empty attribute is expected on HomologationGroups attribute of Homologation entity.");
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
