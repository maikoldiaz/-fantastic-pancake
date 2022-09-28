// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationTests.cs" company="Microsoft">
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
    /// The Storage Location Tests.
    /// </summary>
    [TestClass]
    public class StorageLocationTests
    {
        /// <summary>
        /// StorageLocation entity property logistic center identifier maximum length is decorated.
        /// </summary>
        [TestMethod]
        public void StorageLocation_Entity_Property_StorageLocationId_MaxLength_Is_Decorated()
        {
            // Arrange
            Expression<Func<StorageLocation, string>> expression = nameProperty => nameProperty.StorageLocationId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on StorageLocationId attribute of StorageLocation entity.");
            Assert.IsTrue(isMaxLengthAttribute, "String Length attribute is expected on StorageLocationId attribute of StorageLocation entity.");
        }

        /// <summary>
        /// StorageLocation entity property name is required.
        /// </summary>
        [TestMethod]
        public void StorageLocation_Entity_Property_Name_Is_Required()
        {
            // Arrange
            Expression<Func<LogisticCenter, string>> expression = nameProperty => nameProperty.Name;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;
            bool isMaxLengthAttribute = member.GetCustomAttributes(typeof(MaxLengthAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 2, "Two Attributes expected to be on Name attribute of StorageLocation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on Name attribute of StorageLocation entity.");
            Assert.IsTrue(isMaxLengthAttribute, "String Length attribute is expected on Name attribute of StorageLocation entity.");
        }

        /// <summary>
        /// StorageLocation entity property is active required is decorated.
        /// </summary>
        [TestMethod]
        public void StorageLocation_Entity_Property_IsActive_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<LogisticCenter, bool>> expression = nameProperty => nameProperty.IsActive;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "One Attributes expected to be on Name attribute of StorageLocation entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on IsActive attribute of StorageLocation entity.");
        }
    }
}
