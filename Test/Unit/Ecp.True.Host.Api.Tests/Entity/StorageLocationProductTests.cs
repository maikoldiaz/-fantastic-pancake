// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageLocationProductTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
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
    /// The Storage Location Product Tests.
    /// </summary>
    [TestClass]
    public class StorageLocationProductTests
    {
        /// <summary>
        /// Storages the location product entity property product identifier required is decorated.
        /// </summary>
        [TestMethod]
        public void StorageLocationProduct_Entity_Property_ProductId_Required_Is_Decorated()
        {
            // Arrange
            Expression<Func<StorageLocationProduct, string>> expression = nameProperty => nameProperty.ProductId;
            var memberExpression = (MemberExpression)expression.Body;
            var member = memberExpression.Member;

            bool isRequiredAttribute = member.GetCustomAttributes(typeof(RequiredAttribute), false).Length > 0;

            // Assert
            Assert.IsTrue(member.CustomAttributes.Count() == 1, "Three Attributes expected to be on ProductId of Product entity.");
            Assert.IsTrue(isRequiredAttribute, "Required attribute is expected on ProductId of Product entity.");
        }
    }
}
