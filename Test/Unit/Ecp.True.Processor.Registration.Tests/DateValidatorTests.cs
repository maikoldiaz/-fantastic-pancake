// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateValidatorTests.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processor.Registration.Tests
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Processors.Registration.Validation;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    /// <summary>
    /// The date validator tests.
    /// </summary>
    [TestClass]
    public class DateValidatorTests
    {
        /// <summary>
        /// The configuration handler mock.
        /// </summary>
        private readonly Mock<IConfigurationHandler> configurationHandlerMock = new Mock<IConfigurationHandler>();

        /// <summary>
        /// The date validator.
        /// </summary>
        private DateValidator<Movement> movementDateValidator;

        /// <summary>
        /// The date validator.
        /// </summary>
        private DateValidator<InventoryProduct> inventoryDateValidator;

        /// <summary>
        /// The date validator.
        /// </summary>
        private DateValidator<Event> eventDateValidator;

        /// <summary>
        /// The date validator.
        /// </summary>
        private DateValidator<Contract> contractDateValidator;

        /// <summary>
        /// The system configuration.
        /// </summary>
        private SystemSettings systemConfig;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.systemConfig = new SystemSettings { CurrentMonthValidDays = 4, PreviousMonthValidDays = 10 };
            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);
            this.movementDateValidator = new DateValidator<Movement>(this.configurationHandlerMock.Object);
            this.inventoryDateValidator = new DateValidator<InventoryProduct>(this.configurationHandlerMock.Object);
            this.eventDateValidator = new DateValidator<Event>(this.configurationHandlerMock.Object);
            this.contractDateValidator = new DateValidator<Contract>(this.configurationHandlerMock.Object);
        }

        /// <summary>
        /// Validates the movement for incorrect operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateWithError_ForInvalidLastMonthDateMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.OperationalDate = DateTime.UtcNow.ToTrue().AddMonths(-1);
            var currentDate = DateTime.UtcNow.ToTrue().Date;
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            var lastDayofPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            var daysDifference = Convert.ToInt32((currentDate - firstDayOfCurrentMonth).TotalDays, CultureInfo.InvariantCulture) + 1;
            var previousMonthDays = Convert.ToInt32((firstDayOfCurrentMonth - movementObject.OperationalDate).TotalDays, CultureInfo.InvariantCulture) - 1;
            this.systemConfig = new SystemSettings { CurrentMonthValidDays = daysDifference, PreviousMonthValidDays = previousMonthDays };
            var initialValidDate = firstDayOfCurrentMonth.Subtract(TimeSpan.FromDays(Convert.ToInt32(this.systemConfig.PreviousMonthValidDays, CultureInfo.InvariantCulture)));

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);

            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual($"La fecha operativa del movimiento debe estar entre {initialValidDate.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)} y {lastDayofPreviousMonth.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Validates the movement for previous month operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateWithError_ForValidLastMonthDateMovementDateAsync()
        {
            var currentDate = DateTime.UtcNow.ToTrue().Date;
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            var lastDayofPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            var daysDifference = Convert.ToInt32((currentDate - firstDayOfCurrentMonth).TotalDays, CultureInfo.InvariantCulture) + 1;
            var previousMonthDays = Convert.ToInt32((currentDate - lastDayofPreviousMonth).TotalDays, CultureInfo.InvariantCulture) + 1;
            this.systemConfig = new SystemSettings { CurrentMonthValidDays = daysDifference, PreviousMonthValidDays = previousMonthDays };

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.OperationalDate = DateTime.UtcNow.ToTrue().AddDays((daysDifference + 1) * -1);
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorInfo.Count);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the movement for incorrect operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateWithError_ForInvalidLastMonthDateInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.InventoryDate = DateTime.UtcNow.ToTrue().AddMonths(-1);
            var currentDate = DateTime.UtcNow.ToTrue().Date;
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            var lastDayofPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            var daysDifference = Convert.ToInt32((currentDate - firstDayOfCurrentMonth).TotalDays, CultureInfo.InvariantCulture) + 1;
            var previousMonthDays = Convert.ToInt32((firstDayOfCurrentMonth - inventoryObject.InventoryDate.Value).TotalDays, CultureInfo.InvariantCulture) - 1;
            this.systemConfig = new SystemSettings { CurrentMonthValidDays = daysDifference, PreviousMonthValidDays = previousMonthDays };
            var initialValidDate = firstDayOfCurrentMonth.Subtract(TimeSpan.FromDays(Convert.ToInt32(this.systemConfig.PreviousMonthValidDays, CultureInfo.InvariantCulture)));

            this.configurationHandlerMock.Setup(m => m.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings)).ReturnsAsync(this.systemConfig);

            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual($"La fecha del inventario debe estar entre {initialValidDate.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)} y {lastDayofPreviousMonth.ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture)}", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Validates the movement for incorrect operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [Ignore("Test needs to be fixed as it fails intermittently")]
        public async Task DateValidator_ShouldValidateWithError_ForInvalidLastMonthInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.InventoryDate = DateTime.UtcNow.ToTrue().AddMonths(-1);
            var currentDate = DateTime.UtcNow.ToTrue();
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);

            this.systemConfig.CurrentMonthValidDays = Convert.ToInt32((currentDate - firstDayOfCurrentMonth).TotalDays, CultureInfo.InvariantCulture) - 1;
            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Validates the movement for incorrect operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateSuccessfully_ForValidMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var currentDate = DateTime.UtcNow.ToTrue();
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            var lastDayofPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            movementObject.OperationalDate = lastDayofPreviousMonth.AddDays((this.systemConfig.PreviousMonthValidDays.GetValueOrDefault() + 1) * -1);
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
        }

        /// <summary>
        /// Validates the movement for incorrect operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForInvalidPeriod_ForValidMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.OperationalDate = DateTime.UtcNow.ToTrue().AddDays(-9);
            movementObject.Period.StartTime = DateTime.Now.ToTrue().AddDays(-1);
            movementObject.Period.EndTime = DateTime.Now.ToTrue().AddDays(-3);
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("La fecha final de la operación debe ser mayor o igual a la fecha inicial.", result.ErrorInfo[0].Message);
        }

        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForFutureDate_ForValidMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.OperationalDate = DateTime.UtcNow.ToTrue().AddDays(1);
            movementObject.Period.StartTime = DateTime.Now.ToTrue().AddDays(-3);
            movementObject.Period.EndTime = DateTime.Now.ToTrue().AddDays(-1);
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("La fecha operacional debe ser menor a la fecha actual.", result.ErrorInfo[0].Message);
        }

        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForFutureDate_ForValidInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.InventoryDate = DateTime.UtcNow.ToTrue().AddDays(1);
            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("La fecha del inventario debe ser menor a la fecha actual.", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Validates the inventory for correct date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateSuccessfully_ForValidInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.InventoryDate = DateTime.UtcNow.ToTrue().AddDays(-4);
            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorInfo.Count);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the inventory for null inventory date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task DateValidator_ShouldThrowArgumentNullException_ForNullDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.InventoryDate = null;
            await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates the event for correct date should pass.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateSuccessfully_ForValidEventDateAsync()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            eventObject.ActionType = "Insert";
            eventObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-2);
            eventObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(1);

            // Act
            var result = await this.eventDateValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorInfo.Count);
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Validates the event for invalid end date should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldFailWithInvalidEndDate_EventDateAsync()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            eventObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-2);
            eventObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-3);

            // Act
            var result = await this.eventDateValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Validates the event for invalid current date should fail.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldFailWithInvalidCurrentDate_EventDateAsync()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            eventObject.ActionType = "Insert";
            eventObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-3);
            eventObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-1);

            // Act
            var result = await this.eventDateValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Validates the official movement for correct operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateSuccessfully_ForValidOfficialMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var currentDate = DateTime.UtcNow.ToTrue();
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            var lastDayofPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            movementObject.OperationalDate = lastDayofPreviousMonth;
            movementObject.ScenarioId = ScenarioType.OFFICER;
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorInfo.Count);
        }

        /// <summary>
        /// Validates the official movement for incorrect operational date greater then the current date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForInvalidMonth_ForValidOfficialMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            movementObject.OperationalDate = DateTime.UtcNow.ToTrue().AddDays(1);
            movementObject.ScenarioId = ScenarioType.OFFICER;
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("No es posible registrar un movimiento oficial con fecha del mes actual.", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Validates the official movement for incorrect operational year greater then the current year .
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForInvalidYear_ForValidOfficialMovementDateAsync()
        {
            var movement = File.ReadAllText("MovementJson/SingleMovement.json");
            var movementObject = JsonConvert.DeserializeObject<Movement>(movement);
            var currentDate = DateTime.UtcNow.ToTrue();
            movementObject.OperationalDate = new DateTime(currentDate.Year + 1, currentDate.Month, 1, 0, 0, 0);
            movementObject.ScenarioId = ScenarioType.OFFICER;
            var result = await this.movementDateValidator.ValidateAsync(movementObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("No es posible registrar un movimiento oficial con fecha del mes actual.", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Validates the official inventory for correct operational date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldValidateSuccessfully_ForValidOfficialInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            var currentDate = DateTime.UtcNow.ToTrue();
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            var lastDayofPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
            inventoryObject.InventoryDate = lastDayofPreviousMonth;
            inventoryObject.ScenarioId = ScenarioType.OFFICER;
            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ErrorInfo.Count);
        }

        /// <summary>
        /// Validates the official inventory for incorrect operational date greater then the current date.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForInvalidMonth_ForValidOfficialInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            inventoryObject.InventoryDate = DateTime.UtcNow.ToTrue().AddDays(1);
            inventoryObject.ScenarioId = ScenarioType.OFFICER;
            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("No es posible registrar un inventario oficial con fecha del mes actual.", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Validates the official inventory for incorrect operational year greater then the current year .
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldRaiseErrorForInvalidYear_ForValidOfficialInventoryDateAsync()
        {
            var inventory = File.ReadAllText("InventoryJson/SingleInventory.json");
            var inventoryObject = JsonConvert.DeserializeObject<InventoryProduct>(inventory);
            var currentDate = DateTime.UtcNow.ToTrue();
            inventoryObject.InventoryDate = new DateTime(currentDate.Year + 1, currentDate.Month, 1, 0, 0, 0);
            inventoryObject.ScenarioId = ScenarioType.OFFICER;
            var result = await this.inventoryDateValidator.ValidateAsync(inventoryObject).ConfigureAwait(false);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ErrorInfo.Count);
            Assert.AreEqual("No es posible registrar un inventario oficial con fecha del mes actual.", result.ErrorInfo[0].Message);
        }

        /// <summary>
        /// Dates the validator should pass for event end date is earlier than current date case update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForEventEndDateIsEarlierThanCurrentDate_Case_Update_Async()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            eventObject.ActionType = "Update";
            eventObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-3);
            eventObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-1);

            // Act
            var result = await this.eventDateValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for event end date is earlier than current date case delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForEventEndDateIsEarlierThanCurrentDate_Case_Delete_Async()
        {
            // Arrange
            var eventElement = File.ReadAllText("EventJson/ValidEvent.json");
            var eventObject = JsonConvert.DeserializeObject<Event>(eventElement);
            eventObject.ActionType = "Delete";
            eventObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-3);
            eventObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-1);

            // Act
            var result = await this.eventDateValidator.ValidateAsync(eventObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for valid contract case insert asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForValidContract_Case_Insert_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Insert";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(1);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(2);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for valid contract case update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForValidContract_Case_Update_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Update";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(1);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(2);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for valid contract case delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForValidContract_Case_Delete_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Delete";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(1);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(2);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should fail for contract start date earlier than previous month first date case insert asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldFail_ForContractStartDateEarlierThanPreviousMonthFirstDate_Case_Insert_Async()
        {
            // Arrange
            var previousMonthDate = DateTime.Now.AddMonths(-1);
            var previousMonthFirstDate = new DateTime(previousMonthDate.Year, previousMonthDate.Month, 1);

            var contractObject = new Contract();
            contractObject.ActionType = "Insert";
            contractObject.StartDate = previousMonthFirstDate.AddDays(-1);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(2);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should fail for contract start date earlier than previous month first date case update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForContractStartDateEarlierThanPreviousMonthFirstDate_Case_Update_Async()
        {
            // Arrange
            var previousMonthDate = DateTime.Now.AddMonths(-1);
            var previousMonthFirstDate = new DateTime(previousMonthDate.Year, previousMonthDate.Month, 1);

            var contractObject = new Contract();
            contractObject.ActionType = "Update";
            contractObject.StartDate = previousMonthFirstDate.AddDays(-1);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(2);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for contract start date earlier than previous month first date case delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForContractStartDateEarlierThanPreviousMonthFirstDate_Case_Delete_Async()
        {
            // Arrange
            var previousMonthDate = DateTime.Now.AddMonths(-1);
            var previousMonthFirstDate = new DateTime(previousMonthDate.Year, previousMonthDate.Month, 1);

            var contractObject = new Contract();
            contractObject.ActionType = "Delete";
            contractObject.StartDate = previousMonthFirstDate.AddDays(-1);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(2);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should fail for contract start date earlier end date case insert asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldFail_ForContractStartDateEarlierEndDate_Case_Insert_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Insert";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(2);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(1);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should fail for contract start date earlier end date case update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldFail_ForContractStartDateEarlierEndDate_Case_Update_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Update";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(2);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(1);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should fail for contract start date earlier than current date case insert asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldFail_ForContractStartDateEarlierThanCurrentDate_Case_Insert_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Insert";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-3);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-1);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(!result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for contract start date earlier than current date case update asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForContractStartDateEarlierThanCurrentDate_Case_Update_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Update";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-3);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-1);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        /// <summary>
        /// Dates the validator should pass for contract end date earlier than current date case delete asynchronous.
        /// </summary>
        /// <returns>The Task.</returns>
        [TestMethod]
        public async Task DateValidator_ShouldPass_ForContractEndDateEarlierThanCurrentDate_Case_Delete_Async()
        {
            // Arrange
            var contractObject = new Contract();
            contractObject.ActionType = "Delete";
            contractObject.StartDate = DateTime.UtcNow.ToTrue().AddDays(-3);
            contractObject.EndDate = DateTime.UtcNow.ToTrue().AddDays(-1);

            // Act
            var result = await this.contractDateValidator.ValidateAsync(contractObject).ConfigureAwait(false);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }
    }
}
