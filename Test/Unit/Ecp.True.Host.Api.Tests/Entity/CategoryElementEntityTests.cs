// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryElementEntityTests.cs" company="Microsoft">
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
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Linq.Expressions;
    using Ecp.True.Entities.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The category element entity test.
    /// </summary>
    [TestClass]
    public sealed class CategoryElementEntityTests
    {
        [TestMethod]
        public void CategoryEleEntity_NameAttribute_VerifyIfNameAttributeHaveDataValidationAttributes()
        {
            Expression<Func<CategoryElement, string>> expression = nameProperty => nameProperty.Name;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isStringLengthAttribute = member.GetCustomAttributes(typeof(StringLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 2, "Three Attributes expected to be on Name attribute of CategoryElement entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on Name attribute of CategoryElement entity.");
            Assert.IsTrue(isStringLengthAttribute, "String Length attribute is expected on Name attribute of CategoryElement entity.");
        }

        /// <summary>
        /// Categories element entity, verify if description attribute have data validation attributes.
        /// </summary>
        [TestMethod]
        public void CategoryElementEntity_DescriptionAttribute_VerifyIfDescriptionAttributeHaveDataValidationAttributes()
        {
            Expression<Func<CategoryElement, string>> expression = nameProperty => nameProperty.Description;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool isStringLengthAttribute = member.GetCustomAttributes(typeof(StringLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Two Attributes aspected to be on Description attribute of Category entity.");
            Assert.IsTrue(isStringLengthAttribute, "String Length attribute is expected on Description attribute of Category entity.");
        }

        /// <summary>
        /// Categories element entity, verify if active attributes have data validation attribute.
        /// </summary>
        [TestMethod]
        public void CategoryElementEntity_IsActiveAttribute_VerifyIfActiveAttributesHaveDataValidationAttribute()
        {
            Expression<Func<CategoryElement, bool?>> expression = nameProperty => nameProperty.IsActive;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool hasIsActiveAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Only one Attributes aspected to be on IsActive attribute of Category element entity.");
            Assert.IsTrue(hasIsActiveAttribute, "Required attribute is expected on IsActive attribute of Category element entity.");
        }

        /// <summary>
        /// Categories the element entity is active attribute verify if category identifier attributes have data validation attribute.
        /// </summary>
        [TestMethod]
        public void CategoryElementEntity_IsActiveAttribute_VerifyIfCategoryIdAttributesHaveDataValidationAttribute()
        {
            Expression<Func<CategoryElement, int?>> expression = nameProperty => nameProperty.CategoryId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool hasIsActiveAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Only one Attributes aspected to be on CategoryId attribute of Category element entity.");
            Assert.IsTrue(hasIsActiveAttribute, "Required attribute is expected on CategoryId attribute of Category element entity.");
        }

        /// <summary>
        /// Categories the element entity copy from should copy data when invoked.
        /// </summary>
        [TestMethod]
        public void CategoryElementEntity_CopyFrom_ShouldCopyData_WhenInvoked()
        {
            var firstCategoryElement = new CategoryElement();
            var secondCategoryElement = new CategoryElement() { CategoryId = 1, Name = "Segment", IsActive = true, Description = "Description test" };

            firstCategoryElement.CopyFrom(secondCategoryElement);

            // Assert
            Assert.AreEqual(secondCategoryElement.Name, firstCategoryElement.Name, "Name should be copied.");
            Assert.AreEqual(secondCategoryElement.Description, firstCategoryElement.Description, "Description should be copied.");
            Assert.AreEqual(secondCategoryElement.IsActive, firstCategoryElement.IsActive, "Name IsActive be copied.");
        }

        /// <summary>
        /// Categories the element entity copy from throw argument null exception if invoked.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CategoryElementEntity_CopyFrom_ThrowArgumentNullException_IfInvoked()
        {
            var firstCategory = new Category();
            firstCategory.CopyFrom(null);
        }
    }
}
