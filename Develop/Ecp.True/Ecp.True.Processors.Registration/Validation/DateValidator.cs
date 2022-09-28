// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateValidator.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Registration.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Core;
    using Ecp.True.Entities.Enumeration;
    using Ecp.True.Entities.Registration;

    /// <summary>
    /// The date Validator.
    /// </summary>
    /// <typeparam name="T">The type param.</typeparam>
    /// <seealso cref="Ecp.True.Processors.Api.Inventory.Validator.Interface.IValidator{T}" />
    public class DateValidator<T> : Validator<T>
        where T : class
    {
        /// <summary>
        /// The configuration handler.
        /// </summary>
        private readonly IConfigurationHandler configurationHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateValidator{T}"/> class.
        /// </summary>
        /// <param name="configurationHandler">The configuration handler.</param>
        public DateValidator(IConfigurationHandler configurationHandler)
        {
            this.configurationHandler = configurationHandler;
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateInventoryAsync(InventoryProduct inventoryProduct)
        {
            ArgumentValidators.ThrowIfNull(inventoryProduct, nameof(inventoryProduct));
            ArgumentValidators.ThrowIfNull(inventoryProduct.InventoryDate, nameof(inventoryProduct.InventoryDate));
            var systemConfig = await this.GetDateRangeAsync().ConfigureAwait(false);
            var errorResponse = new List<ErrorInfo>();

            if (inventoryProduct.ScenarioId == ScenarioType.OFFICER)
            {
                return ValidateOfficial(
                  inventoryProduct.InventoryDate.GetValueOrDefault(),
                  MessageType.Inventory,
                  errorResponse);
            }

            return ValidateOperational(
                inventoryProduct.InventoryDate.GetValueOrDefault(),
                systemConfig,
                MessageType.Inventory,
                errorResponse);
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateMovementAsync(Movement movement)
        {
            ArgumentValidators.ThrowIfNull(movement, nameof(movement));
            ArgumentValidators.ThrowIfNull(movement.Period, nameof(movement.Period));
            var systemConfig = await this.GetDateRangeAsync().ConfigureAwait(false);
            var errorResponse = new List<ErrorInfo>();

            if (movement.Period.StartTime > movement.Period.EndTime)
            {
                errorResponse.Add(new ErrorInfo(
                        Registration.Constants.Operations_EndDate_Validation));
                return new ValidationResult(errorResponse);
            }

            if (movement.ScenarioId == ScenarioType.OFFICER)
            {
                return ValidateOfficial(
                  movement.OperationalDate,
                  MessageType.Movement,
                  errorResponse);
            }

            return ValidateOperational(
                movement.OperationalDate,
                systemConfig,
                MessageType.Movement,
                errorResponse);
        }

        /// <inheritdoc/>
        protected override Task<ValidationResult> ValidateEventAsync(Event eventObj)
        {
            ArgumentValidators.ThrowIfNull(eventObj, nameof(eventObj));
            ArgumentValidators.ThrowIfNull(eventObj.StartDate, nameof(eventObj.StartDate));
            ArgumentValidators.ThrowIfNull(eventObj.EndDate, nameof(eventObj.EndDate));
            var errorResponse = new List<ErrorInfo>();

            var currentDate = DateTime.UtcNow.ToTrue().Date;

            if (eventObj.EndDate < eventObj.StartDate)
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.EventEndDateValidation));
                return Task.FromResult(new ValidationResult(errorResponse));
            }

            if (eventObj.ActionType.EqualsIgnoreCase(EventType.Insert.ToString("G")) && eventObj.EndDate < currentDate)
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.EventEndDateGreaterNowValidation));
                return Task.FromResult(new ValidationResult(errorResponse));
            }

            return Task.FromResult(ValidationResult.Success);
        }

        /// <inheritdoc/>
        protected override async Task<ValidationResult> ValidateContractAsync(Contract contractObj)
        {
            ArgumentValidators.ThrowIfNull(contractObj, nameof(contractObj));

            if (!contractObj.ActionType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                ArgumentValidators.ThrowIfNull(contractObj.StartDate, nameof(contractObj.StartDate));
                ArgumentValidators.ThrowIfNull(contractObj.EndDate, nameof(contractObj.EndDate));
            }

            var errorResponse = new List<ErrorInfo>();
            var systemConfig = await this.GetDateRangeAsync().ConfigureAwait(false);
            var currentDate = DateTime.UtcNow.ToTrue().Date;
            var currentDateLessContractDays = currentDate.AddDays(-systemConfig.ValidateContractDays);

            if (contractObj.ActionType.EqualsIgnoreCase(EventType.Delete.ToString("G")))
            {
                return ValidationResult.Success;
            }

            if (contractObj.EndDate < contractObj.StartDate)
            {
                errorResponse.Add(new ErrorInfo(Registration.Constants.ContractEndDateValidation));
                return new ValidationResult(errorResponse);
            }

            if (contractObj.ActionType.EqualsIgnoreCase(EventType.Insert.ToString("G")) && contractObj.EndDate < currentDateLessContractDays)
            {
                errorResponse.Add(new ErrorInfo(string.Format(
                            CultureInfo.InvariantCulture,
                            Registration.Constants.ContractEndDateGreaterNowLessParameterValidation,
                            currentDateLessContractDays.ToTrueString())));
            }

            if (contractObj.ActionType.EqualsIgnoreCase(EventType.Insert.ToString("G")) && contractObj.StartDate < currentDateLessContractDays)
            {
                errorResponse.Add(new ErrorInfo(string.Format(
                            CultureInfo.InvariantCulture,
                            Registration.Constants.ContractStartDateGreaterNowLessParameterValidation,
                            currentDateLessContractDays.ToTrueString())));

                return new ValidationResult(errorResponse);
            }

            return ValidationResult.Success;
        }

        private static ValidationResult ValidateOperational(
            DateTime operationalDateTime,
            SystemSettings systemConfig,
            MessageType entityType,
            List<ErrorInfo> errorResponse)
        {
            var currentDate = DateTime.UtcNow.ToTrue().Date;

            // Validate if operationalDate is GREATER than currentDate.
            if (operationalDateTime.Date >= currentDate.Date)
            {
                errorResponse.Add(new ErrorInfo(string.Format(
                      CultureInfo.InvariantCulture,
                      entityType == MessageType.Movement ? Registration.Constants.Movement_OperationalDate_InThePast_Validation : Registration.Constants.Inventory_ClosingDate_InThePast_Validation)));
                return new ValidationResult(errorResponse);
            }

            // Validate if entity to register is for the current month and current year.
            if (currentDate.Month != operationalDateTime.Month || currentDate.Year != operationalDateTime.Year)
            {
                var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
                var initialValidDate = firstDayOfCurrentMonth.AddDays(systemConfig.PreviousMonthValidDays.GetValueOrDefault() * -1);

                // Validate if current date is GREATER than the allowed CurrentMonthValidDays value (starting for first day of the current month.)
                if (currentDate > firstDayOfCurrentMonth.AddDays(systemConfig.CurrentMonthValidDays.GetValueOrDefault()))
                {
                    errorResponse.Add(new ErrorInfo(string.Format(
                            CultureInfo.InvariantCulture,
                            Registration.Constants.InventoryMovementCreateDateValidation,
                            systemConfig.CurrentMonthValidDays.GetValueOrDefault())));
                    return new ValidationResult(errorResponse);
                }

                // Validate if the operational date is less than the allowed date (previous month(s)/day(s)).
                if (initialValidDate > operationalDateTime.Date)
                {
                    var message = entityType == MessageType.Movement ? Registration.Constants.Movement_Create_DateValidation
                        : Registration.Constants.Inventory_Create_DateValidation;
                    errorResponse.Add(new ErrorInfo(string.Format(
                            CultureInfo.InvariantCulture,
                            message,
                            initialValidDate,
                            firstDayOfCurrentMonth.AddDays(-1))));
                    return new ValidationResult(errorResponse);
                }
            }

            return ValidationResult.Success;
        }

        private static ValidationResult ValidateOfficial(
            DateTime dateTime,
            MessageType entityType,
            List<ErrorInfo> errorResponse)
        {
            var currentDate = DateTime.UtcNow.ToTrue().Date;
            var firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1, 0, 0, 0);
            if (dateTime >= firstDayOfCurrentMonth)
            {
                errorResponse.Add(new ErrorInfo(string.Format(
                       CultureInfo.InvariantCulture,
                       entityType == MessageType.Movement ? Registration.Constants.OfficialMovementDateValidation : Registration.Constants.OfficialInventoryDateValidation)));
                return new ValidationResult(errorResponse);
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Getting the no of days since which the movement or inventory registration is valid.
        /// </summary>
        /// <returns>The number of days.</returns>
        private Task<SystemSettings> GetDateRangeAsync()
        {
            return this.configurationHandler.GetConfigurationAsync<SystemSettings>(ConfigurationConstants.SystemSettings);
        }
    }
}
