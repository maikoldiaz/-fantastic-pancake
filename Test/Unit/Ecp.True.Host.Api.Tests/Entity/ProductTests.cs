// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductTests.cs" company="Microsoft">
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
    using Ecp.True.Entities;
    using Ecp.True.Entities.Admin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The Product Tests.
    /// </summary>
    [TestClass]
    public class ProductTests
    {
        /// <summary>
        /// Product entity property logistic center identifier maximum length is decorated.
        /// </summary>
        [TestMethod]
        public void Product_Entity_Property_ProductId_MaxLength_Is_Decorated()
        {
            // Arrange
            var propertyInfo = typeof(Product).GetProperty("ProductId");

            // Assert
            Assert.IsNotNull(propertyInfo);

            // Act
            var attribute = propertyInfo.GetCustomAttributes(true).ToArray();
            var rmaxLengthAttribute = (MaxLengthAttribute)attribute[0];

            // Assert
            Assert.IsNotNull(rmaxLengthAttribute);
            Assert.AreEqual(rmaxLengthAttribute.ErrorMessage, Constants.Max20Characters);
        }

        /// <summary>
        /// Product entity property name is required.
        /// </summary>
        [TestMethod]
        public void Product_Entity_Property_Name_Is_Required()
        {
            // Arrange
            Expression<Func<Product, string>> expression = nameProperty => nameProperty.Name;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 2, "Three Attributes expected to be on Name of Product entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on Name of Product entity.");
            Assert.IsTrue(isMaxLengthAttribute, "String Length attribute is expected on Name of Product entity.");
        }

        /// <summary>
        /// Product entity property is active required is decorated.
        /// </summary>
        [TestMethod]
        public void Product_Entity_Property_IsActive_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<Product, bool>> expression = nameProperty => nameProperty.IsActive;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attribute expected to be on IsActive of Product entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on IsActive of Product entity.");
        }
    }
}
