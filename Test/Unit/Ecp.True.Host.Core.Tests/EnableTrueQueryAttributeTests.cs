// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnableTrueQueryAttributeTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Core.Tests
{
    using System;
    using System.IO;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Host.Core.Tests.OData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The enable true query attribute tests.
    /// </summary>
    [TestClass]
    public class EnableTrueQueryAttributeTests
    {
        /// <summary>
        /// The attribute.
        /// </summary>
        private EnableTrueQueryAttribute attribute;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            var systemSettings = new SystemSettings
            {
                TransportFileUpload = new DateConfig { DateRange = 40 },
            };
            EnableTrueQueryAttribute.Initialize(systemSettings);
        }

        /// <summary>
        /// Validate should throw error when date range is more than configuration.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void Validate_ShouldThrowError_WhenDateRangeIsMoreThanConfiguration()
        {
            var uri = new Uri("http://localhost:44372/api/v1.0/fileregistrations?$count=true&$top=10&$filter=createdDate%20ge%202018-04-27T00%3A00%3A00.000Z%20and%20createdDate%20le%202018-12-27T23%3A59%3A59.000Z%20and%20systemTypeId%20eq%20%27CONTRACT%27");
            var options = ODataHelper.SetUpQueryOptions<FileRegistration>(uri);

            this.attribute = new EnableTrueQueryAttribute("fileregistrations");
            this.attribute.ValidateQuery(options.Request, options);
        }

        /// <summary>
        /// Validate should throw error when date range is more than configuration.
        /// </summary>
        [TestMethod]
        public void Validate_ShouldThrowError_WhenDateRangeIsWithinThanConfiguration()
        {
            var uri = new Uri("http://localhost:44372/api/v1.0/fileregistrations?$count=true&$top=10&$filter=createdDate%20ge%202018-12-27T00%3A00%3A00.000Z%20and%20createdDate%20le%202018-12-28T23%3A59%3A59.000Z%20and%20systemTypeId%20eq%20%27CONTRACT%27");
            var options = ODataHelper.SetUpQueryOptions<FileRegistration>(uri);

            this.attribute = new EnableTrueQueryAttribute("fileregistrations");
            this.attribute.ValidateQuery(options.Request, options);
            Assert.IsNotNull(this.attribute);
        }

        /// <summary>
        /// Validate should throw error when date range is more than configuration.
        /// </summary>
        [TestMethod]
        public void Validate_ShouldNotThrowError_WhenUrlHasNoValidationSetup()
        {
            var uri = new Uri("http://localhost:44372/api/v1.0/categories?$count=true&$top=10");
            var options = ODataHelper.SetUpQueryOptions<Category>(uri);

            this.attribute = new EnableTrueQueryAttribute("categories");
            this.attribute.ValidateQuery(options.Request, options);
            Assert.IsNotNull(this.attribute);
        }

        /// <summary>
        /// Validate should throw error when date range is more than configuration.
        /// </summary>
        [TestMethod]
        public void Validate_ShouldNotThrowError_WhenUrlHasNoFilters()
        {
            var uri = new Uri("http://localhost:44372/api/v1.0/fileregistrations?$count=true&$top=10");
            var options = ODataHelper.SetUpQueryOptions<FileRegistration>(uri);

            this.attribute = new EnableTrueQueryAttribute("fileregistrations");
            this.attribute.ValidateQuery(options.Request, options);
            Assert.IsNotNull(this.attribute);
        }
    }
}
