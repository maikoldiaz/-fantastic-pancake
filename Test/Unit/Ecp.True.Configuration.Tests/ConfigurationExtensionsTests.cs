// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationExtensionsTests.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration.Tests
{
    using System;
    using System.Text;
    using Ecp.True.Entities.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Newtonsoft.Json;

    /// <summary>
    /// Configuration Extensions Tests.
    /// </summary>
    [TestClass]
    public class ConfigurationExtensionsTests
    {
        /// <summary>
        /// Should Return Error Details If Parse To Boolean Fails.
        /// </summary>
        [TestMethod]
        public void ShouldReturnErrorDetailsIfParseToBooleanFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(bool));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Should Return Error Details If Parse To Byte Array Fails.
        /// </summary>
        [TestMethod]
        public void ShouldReturnErrorDetailsIfParseToByteArrayFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(byte[]));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Should Return Error Details If Parse To User Defined Object Fails.
        /// </summary>
        [TestMethod]
        public void ShouldReturnErrorDetailsIfParseToEnumFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(ConfigurationStoreType));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Should Return Error Details If Parse To User Defined Object Fails.
        /// </summary>
        [TestMethod]
        public void ShouldReturnErrorDetailsIfParseToUserDefinedObjectTypeFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(ServiceBusMessagingConfig));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Should Return Exception Details If Parse To Datetime offset Fails.
        /// </summary>
        [TestMethod]
        public void ShouldReturnExceptionDetailsIfParseToDateTimeOffsetFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(DateTimeOffset));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Should Return Exception Details If Parse To Guid Fails.
        /// </summary>
        [TestMethod]
        public void ShouldReturnExceptionDetailsIfParseToGuidFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(Guid));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Should Successfully Parse To Boolean.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToBoolean()
        {
            // Arrange
            object value = "true";

            // Act
            var parseResult = value.ParseType(typeof(bool));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(bool));
            Assert.IsTrue((bool)parseResult.Item1);
        }

        /// <summary>
        /// Should Successfully Parse To Byte Array.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToByteArray()
        {
            // Arrange
            object value = Convert.ToBase64String(Encoding.ASCII.GetBytes("Value"));

            // Act
            var parseResult = value.ParseType(typeof(byte[]));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(byte[]));
            Assert.IsTrue(Encoding.ASCII.GetString((byte[])parseResult.Item1).Equals("Value", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Should Successfully Parse To DateTime Offset.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToDateTimeOffset()
        {
            // Arrange
            var value = new DateTimeOffset(DateTime.Now.Date);

            // Act
            var parseResult = value.ParseType(typeof(DateTimeOffset));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(DateTimeOffset));
        }

        /// <summary>
        /// Should Successfully Parse To Enum.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToEnum()
        {
            // Arrange
            object value = ConfigurationStoreType.File;

            // Act
            var parseResult = value.ParseType(typeof(ConfigurationStoreType));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(ConfigurationStoreType));
        }

        /// <summary>
        /// Should Successfully Parse To Guid.
        /// </summary>
        [TestMethod]
        public void ShouldSUccessfullyParseToGuid()
        {
            // Arrange
            var value = Guid.NewGuid();

            // Act
            var parseResult = value.ParseType(typeof(Guid));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(Guid));
            Assert.IsTrue((Guid)parseResult.Item1 == value);
        }

        /// <summary>
        /// Should Successfully Parse To String Type.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToStringType()
        {
            // Arrange
            var value = "value";

            // Act
            var parseResult = value.ParseType(typeof(string));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(string));
            Assert.IsTrue(((string)parseResult.Item1).Equals(value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Should Successfully Parse To Time Span.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToTimeSpan()
        {
            // Arrange
            var value = new TimeSpan(DateTime.Now.Ticks);

            // Act
            var parseResult = value.ParseType(typeof(TimeSpan));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(TimeSpan));
            Assert.IsTrue((TimeSpan)parseResult.Item1 == value);
        }

        /// <summary>
        /// Should Successfully Parse To User Defined Object Type.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToUserDefinedObjectType()
        {
            // Arrange
            var parseTestObj = new ServiceBusMessagingConfig { QueueName = "Queue" };
            object value = JsonConvert.SerializeObject(parseTestObj);

            // Act
            var parseResult = value.ParseType(typeof(ServiceBusMessagingConfig));

            // Assert
            Assert.IsNull(parseResult.Item2);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(ServiceBusMessagingConfig));
            Assert.IsTrue(((ServiceBusMessagingConfig)parseResult.Item1).QueueName.Equals(parseTestObj.QueueName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Shoulds the return tuple of nulls when type is null.
        /// </summary>
        [TestMethod]
        public void ShouldReturnTupleOfNullsWhenTypeIsNull()
        {
            object value = "Value";

            var parseResult = value.ParseType(null);

            Assert.IsNull(parseResult.Item1);
            Assert.IsNull(parseResult.Item2);
        }

        /// <summary>
        /// Should successfully parse to specified type.
        /// </summary>
        [TestMethod]
        public void ShouldSuccessfullyParseToSpecifiedType()
        {
            object value = "{Key:\"somekey\", Value:\"somevalue\"}";

            var parseResult = value.ParseType(typeof(ConfigurationSetting));

            Assert.IsNotNull(parseResult.Item1);
            Assert.IsInstanceOfType(parseResult.Item1, typeof(ConfigurationSetting));
        }

        /// <summary>
        /// Should Throw Exception If Parse To Time Span Fails.
        /// </summary>
        [TestMethod]
        public void ShouldThrowExceptionIfParseToTimeSpanFails()
        {
            // Arrange
            object value = "Value";

            // Act
            var parseResult = value.ParseType(typeof(TimeSpan));

            // Assert
            Assert.IsNotNull(parseResult.Item2);
            Assert.IsNull(parseResult.Item1);
        }

        /// <summary>
        /// Shoulds the return tuple of nulls when object and type is null.
        /// </summary>
        [TestMethod]
        public void ShouldReturnTupleOfNullsWhenObjectAndTypeIsNull()
        {
            object value = null;

            var parseResult = value.ParseType(null);

            Assert.IsNull(parseResult.Item1);
            Assert.IsNull(parseResult.Item2);
        }
    }
}