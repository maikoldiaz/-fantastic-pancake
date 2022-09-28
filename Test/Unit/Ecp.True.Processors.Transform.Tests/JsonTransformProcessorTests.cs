// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonTransformProcessorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Tests
{
    using System.Threading.Tasks;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.TransportBalance;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Transform.Input.Interfaces;
    using Ecp.True.Processors.Transform.Services.Interfaces;
    using Ecp.True.Processors.Transform.Services.Json;
    using Ecp.True.Processors.Transform.Services.Json.Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The JsonTransformProcessorTests.
    /// </summary>
    [TestClass]
    public class JsonTransformProcessorTests
    {
        /// <summary>
        /// The mock logger.
        /// </summary>
        private Mock<ITrueLogger<JsonTransformProcessor>> mockLogger;

        /// <summary>
        /// The mock input factory.
        /// </summary>
        private Mock<IInputFactory> mockInputFactory;

        /// <summary>
        /// The mock data service.
        /// </summary>
        private Mock<IDataService> mockDataService;

        /// <summary>
        /// The mock json transformer.
        /// </summary>
        private Mock<IJsonTransformer> mockJsonTransformer;

        /// <summary>
        /// The json transform processor.
        /// </summary>
        private JsonTransformProcessor jsonTransformProcessor;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ITrueLogger<JsonTransformProcessor>>();
            this.mockInputFactory = new Mock<IInputFactory>();
            this.mockDataService = new Mock<IDataService>();
            this.mockJsonTransformer = new Mock<IJsonTransformer>();

            this.jsonTransformProcessor = new JsonTransformProcessor(
                this.mockLogger.Object,
                this.mockInputFactory.Object,
                this.mockDataService.Object,
                this.mockJsonTransformer.Object);
        }

        /// <summary>
        /// Jsons the transform processor should retry do transform asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task JsonTransformProcessor_Should_Retry_False_DoTransformAsync()
        {
            var trueMessage = new TrueMessage
            {
                IsRetry = false,
                SourceSystem = SystemType.TRUE,
                TargetSystem = SystemType.SINOPER,
                Message = MessageType.Inventory,
            };

            var jobject = this.GetJObjectWithValidDataTypes();
            jobject.Add("BlobPath", "test/sinoper/1");
            var array = new JArray();
            array.Add(jobject);

            this.mockInputFactory.Setup(a => a.GetFileRegistrationAsync(It.IsAny<string>())).ReturnsAsync(new FileRegistration());
            this.mockInputFactory.Setup(m => m.GetJsonInputAsync(It.IsAny<string>())).ReturnsAsync(array);

            var output = await this.jsonTransformProcessor.TransformAsync(trueMessage).ConfigureAwait(false);

            Assert.IsNotNull(output);
            this.mockInputFactory.Verify(a => a.GetFileRegistrationAsync(It.IsAny<string>()), Times.Once);
            this.mockInputFactory.Verify(m => m.GetJsonInputAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Jsons the transform processor should retry true do transform asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task JsonTransformProcessor_Should_Retry_True_DoTransformAsync()
        {
            var trueMessage = new TrueMessage
            {
                IsRetry = true,
                SourceSystem = SystemType.TRUE,
                TargetSystem = SystemType.SINOPER,
                Message = MessageType.Inventory,
            };

            var fileRegistration = new FileRegistration
            {
                SystemTypeId = SystemType.TRUE,
                UploadId = "UploadId",
                BlobPath = "Some path",
            };
            fileRegistration.SystemTypeId = SystemType.TRUE;

            var fileRegistrationTransaction = new FileRegistrationTransaction
            {
                FileRegistration = fileRegistration,
            };

            var jobject = this.GetJObjectWithValidDataTypes();
            jobject.Add("BlobPath", "test/sinoper/1");
            var array = new JArray();
            array.Add(jobject);
            var jtoken = JToken.Parse(JsonConvert.SerializeObject(array));

            this.mockInputFactory.Setup(a => a.GetFileRegistrationTransactionAsync(It.IsAny<int>())).ReturnsAsync(fileRegistrationTransaction);
            this.mockInputFactory.Setup(m => m.GetJsonInputAsync(It.IsAny<string>())).ReturnsAsync(array);
            this.mockJsonTransformer.Setup(z => z.TransformJsonAsync(It.IsAny<JToken>(), It.IsAny<TrueMessage>())).ReturnsAsync(jtoken);

            var output = await this.jsonTransformProcessor.TransformAsync(trueMessage).ConfigureAwait(false);

            Assert.IsNotNull(output);
            this.mockInputFactory.Verify(a => a.GetFileRegistrationTransactionAsync(It.IsAny<int>()), Times.Once);
            this.mockInputFactory.Verify(m => m.GetJsonInputAsync(It.IsAny<string>()), Times.Once);
        }

        /// <summary>
        /// Jsons the transform processor should complete asynchronous.
        /// </summary>
        /// <returns>The task.</returns>
        [TestMethod]
        public async Task JsonTransformProcessor_ShouldCompleteAsync()
        {
            var trueMessage = new TrueMessage();

            this.mockDataService.Setup(a => a.SaveAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()));

            await this.jsonTransformProcessor.CompleteAsync(new JArray(), trueMessage).ConfigureAwait(false);

            this.mockDataService.Verify(a => a.SaveAsync(It.IsAny<JArray>(), It.IsAny<TrueMessage>()), Times.Once);
        }

        /// <summary>
        /// Jsons the transform processor system type should return system type sap when invoked.
        /// </summary>
        [TestMethod]
        public void JsonTransformProcessor_SystemType_ShouldReturnSystemTypeSAP_WhenInvoked()
        {
            var result = this.jsonTransformProcessor.InputType;

            Assert.AreEqual(InputType.JSON, result);
        }

        /// <summary>
        /// Gets the j object with valid data types.
        /// </summary>
        /// <returns>The JObject.</returns>
        private JObject GetJObjectWithValidDataTypes()
        {
            var json = "{\r\n \"SourceSystem\": \"SINOPER\",\r\n \"OriginMessageId\": \"12345\",\r\n \"MovementId\": 600,\r\n\"MessageId\":\"600\",\r\n\"Type\":\"Movement\",\r\n \"OperationalDate\": \"2019-08-21T08:36:00\",\r\n \"GrossStandardVolume\": null,\r\n \"NetStandardVolume\": 195.45,\r\n \"Observations\": null,\r\n \"Classification\": \"Movimiento\",\r\n \"HasMovement\": false,\r\n \"Attributes\": [\r\n      {\r\n        \"AttributeId\": 11,\r\n        \"AttributeValue\": 200.2,\r\n        \"ValueAttributeUnit\": \"Bls\",\r\n        \"AttributeDescription\": null,\r\n        \"AttributeOperationalDate\": \"2019-08-21T08:36:00\",\r\n        \"HasAttribute\": true\r\n      },\r\n      {\r\n        \"AttributeId\": 12,\r\n        \"AttributeValue\": 201.2,\r\n        \"ValueAttributeUnit\": \"ADM\",\r\n        \"AttributeDescription\": \"Mayorista\",\r\n        \"AttributeOperationalDate\": \"2019-08-21T08:36:00\",\r\n        \"HasAttribute\": false\r\n      }\r\n ] ,\r\n \"MovementDestination\": {\r\n     \"DestinationNodeId\": \"SALGAR:17:ECOPETROL\",\r\n     \"DestinationStorageLocationId\": 11,\r\n     \"DestinationProductId\": 100,\r\n     \"DestinationProductTypeId\": 2,\r\n     \"DestinationOperationalDate\": \"2019-08-21T08:36:00\",\r\n     \"HasDestination\": true\r\n }\r\n}\r\n";
            return JObject.Parse(json);
        }
    }
}
