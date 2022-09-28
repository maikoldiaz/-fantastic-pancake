// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationDataMappingTests.cs" company="Microsoft">
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
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The Homologation Tests.
    /// </summary>
    [TestClass]
    public class HomologationDataMappingTests
    {
        /// <summary>
        /// Homologations data mapping the should invoke validator to result error message homologation data mapping should have SourceValue.
        /// </summary>
        [TestMethod]
        public void HomologationDataMapping_ShouldInvokeValidator_ToResultErrorMessageHomologationDataMappingShouldHaveSourceValue()
        {
            var validationResults = this.ValidateModel(new HomologationDataMapping());
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.SourceValueRequired);
        }

        /// <summary>
        /// Homologation data mapping entity property source value required is decorated.
        /// </summary>
        [TestMethod]
        public void HomologationDataMapping_Entity_Property_SourceValue_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<HomologationDataMapping, string>> expression = nameProperty => nameProperty.SourceValue;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 2, "Three Attributes expected to be on SourceValue attribute of HomologationDataMapping entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on SourceValue attribute of HomologationDataMapping entity.");
            Assert.IsTrue(isMaxLengthAttribute, "String Length attribute is expected on SourceValue attribute of HomologationDataMapping entity.");
        }

        /// <summary>
        /// Homologations data mapping the should invoke validator to result error message homologation data mapping should have max length SourceValue.
        /// </summary>
        [TestMethod]
        public void HomologationDataMapping_ShouldInvokeValidator_ToResultErrorMessageHomologationDataMappingShouldHaveMaxLengthSourceValue()
        {
            var homologationDataMapping = new HomologationDataMapping
            {
                SourceValue = "TestStringTestStringTestStringTestStringTestStringTestStringTestStringTestStringTestStringTestStringTestString",
            };

            var validationResults = this.ValidateModel(homologationDataMapping);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.SourceValueMaxLength100);
        }

        /// <summary>
        /// Homologations data mapping the should invoke validator to result error message homologation data mapping should have DestinationValue.
        /// </summary>
        [TestMethod]
        public void HomologationDataMapping_ShouldInvokeValidator_ToResultErrorMessageHomologationDataMappingShouldHaveDestinationValue()
        {
            var homologationDataMapping = new HomologationDataMapping
            {
                SourceValue = "TestString",
            };

            var validationResults = this.ValidateModel(homologationDataMapping);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.DestinationValueRequired);
        }

        /// <summary>
        /// Homologation data mapping entity property destination value required is decorated.
        /// </summary>
        [TestMethod]
        public void HomologationDataMapping_Entity_Property_DestinationValue_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<HomologationDataMapping, string>> expression = nameProperty => nameProperty.DestinationValue;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 2, "Three Attributes expected to be on DestinationValue attribute of HomologationDataMapping entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on DestinationValue attribute of HomologationDataMapping entity.");
            Assert.IsTrue(isMaxLengthAttribute, "String Length attribute is expected on DestinationValue attribute of HomologationDataMapping entity.");
        }

        /// <summary>
        /// Homologations data mapping the should invoke validator to result error message homologation data mapping should have max length DestinationValue.
        /// </summary>
        [TestMethod]
        public void HomologationDataMapping_ShouldInvokeValidator_ToResultErrorMessageHomologationDataMappingShouldHaveMaxLengthDestinationValue()
        {
            var homologationDataMapping = new HomologationDataMapping
            {
                SourceValue = "TestString",
                DestinationValue = "TestStringTestStringTestStringTestStringTestStringTestStringTestStringTestStringTestStringTestStringTestString",
            };

            var validationResults = this.ValidateModel(homologationDataMapping);
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.DestinationValueMaxLength100);
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
