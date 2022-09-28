// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryEntityTests.cs" company="Microsoft">
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
    /// The Category entity tests.
    /// </summary>
    [TestClass]
    public sealed class CategoryEntityTests
    {
        /// <summary>
        /// Categories entity, name attribute verify if name attribute have data validation attributes.
        /// </summary>
        [TestMethod]
        public void CategoryEntity_NameAttribute_VerifyIfNameAttributeHaveDataValidationAttributes()
        {
            Expression<Func<Category, string>> expression = nameProperty => nameProperty.Name;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isStringLengthAttribute = member.GetCustomAttributes(typeof(StringLengthAttribute), false).Length > 0;
            bool isRegularExpressionAttribute = member.GetCustomAttributes(typeof(RegularExpressionAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 3, "Three Attributes expected to be on Name attribute of Category entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on Name attribute of Category entity.");
            Assert.IsTrue(isStringLengthAttribute, "String Length attribute is expected on Name attribute of Category entity.");
            Assert.IsTrue(isRegularExpressionAttribute, "Regular expression attribute is expected on Name attribute of Category entity.");
        }

        /// <summary>
        /// Categories entity, verify if description attribute have data validation attributes.
        /// </summary>
        [TestMethod]
        public void CategoryEntity_DescriptionAttribute_VerifyIfDescriptionAttributeHaveDataValidationAttributes()
        {
            Expression<Func<Category, string>> expression = nameProperty => nameProperty.Description;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool isStringLengthAttribute = member.GetCustomAttributes(typeof(StringLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Two Attributes expected to be on Description attribute of Category entity.");
            Assert.IsTrue(isStringLengthAttribute, "String Length attribute is expected on Description attribute of Category entity.");
        }

        /// <summary>
        /// Categories entity, verify if active attributes have data validation attribute.
        /// </summary>
        [TestMethod]
        public void CategoryEntity_IsActiveAttribute_VerifyIfActiveAttributesHaveDataValidationAttribute()
        {
            Expression<Func<Category, bool?>> expression = nameProperty => nameProperty.IsActive;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool hasIsActiveAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Only one Attributes expected to be on Description attribute of Category entity.");
            Assert.IsTrue(hasIsActiveAttribute, "Required attribute is expected on IsActive attribute of Category entity.");
        }

        /// <summary>
        /// Categories entity, verify if Grouper attributes have data validation attribute.
        /// </summary>
        [TestMethod]
        public void CategoryEntity_IsActiveAttribute_VerifyIfGrouperAttributesHaveDataValidationAttribute()
        {
            Expression<Func<Category, bool?>> expression = nameProperty => nameProperty.IsGrouper;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;
            bool hasIsGrouperAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Only one Attributes aspected to be on Grouper attribute of Category entity.");
            Assert.IsTrue(hasIsGrouperAttribute, "Required attribute is expected on Grouper attribute of Category entity.");
        }

        /// <summary>
        /// Categories the entity copy from positive test.
        /// </summary>
        [TestMethod]
        public void CategoryEntity_CopyFrom_ShouldCopyData_WhenInvoked()
        {
            var firstCategory = new Category();
            var secondCategory = new Category() { CategoryId = 1, Name = "Segment", IsActive = true, IsGrouper = true, Description = "Description test" };

            firstCategory.CopyFrom(secondCategory);

            // Assert
            Assert.AreEqual(secondCategory.Name, firstCategory.Name, "Name should be copied.");
            Assert.AreEqual(secondCategory.Description, firstCategory.Description, "Description should be copied.");
            Assert.AreEqual(secondCategory.IsActive, firstCategory.IsActive, "Name IsActive be copied.");
            Assert.AreEqual(secondCategory.IsGrouper, firstCategory.IsGrouper, "IsGrouper should be copied.");
        }

        /// <summary>
        /// Categories the entity copy from positive test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CategoryEntity_CopyFrom_ThrowArgumentNullException_IfInvoked()
        {
            var firstCategory = new Category();
            firstCategory.CopyFrom(null);
        }
    }
}
