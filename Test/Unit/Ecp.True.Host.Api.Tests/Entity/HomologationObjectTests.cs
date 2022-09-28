// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationObjectTests.cs" company="Microsoft">
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
    public class HomologationObjectTests
    {
        /// <summary>
        /// Homologations object the should invoke validator to result error message homologation object should have SourceObjectName.
        /// </summary>
        [TestMethod]
        public void HomologationObject_ShouldInvokeValidator_ToResultErrorMessageHomologationObjectShouldHaveSourceObjectName()
        {
            var validationResults = this.ValidateModel(new HomologationObject());
            Assert.IsTrue(validationResults.Count > 0);
            Assert.AreEqual(validationResults.First().ErrorMessage, Constants.HomologationObjectTypeIdRequired);
        }

        /// <summary>
        /// Homologation object entity property source object name required is decorated.
        /// </summary>
        [TestMethod]
        public void HomologationObject_Entity_Property_HomologationObjectTypeId_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<HomologationObject, int?>> expression = nameProperty => nameProperty.HomologationObjectTypeId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Three Attributes expected to be on HomologationObjectTypeId attribute of HomologationObject entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on HomologationObjectTypeId attribute of HomologationObject entity.");
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
