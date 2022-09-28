// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateSapRequestFilterAttribute.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.Sap.Api.Filter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Ecp.True.Configuration;
    using Ecp.True.Core;
    using Ecp.True.Core.Entities;
    using Ecp.True.Entities;
    using Ecp.True.Entities.Configuration;
    using Ecp.True.Entities.Registration;
    using Ecp.True.Entities.Sap;
    using Ecp.True.Entities.Sap.Purchases;
    using Ecp.True.Host.Core;
    using Ecp.True.Processors.Ownership.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// The ADAL token acquisition exception filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [CLSCompliant(false)]
    public sealed class ValidateSapRequestFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// The argument name.
        /// </summary>
        private readonly string argumentName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateSapRequestFilterAttribute" /> class.
        /// </summary>
        /// <param name="argumentName">Name of the argument.</param>
        public ValidateSapRequestFilterAttribute(string argumentName)
        {
            this.argumentName = argumentName;
        }

        /// <inheritdoc/>
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            ArgumentValidators.ThrowIfNull(context, nameof(context));
            ArgumentValidators.ThrowIfNull(next, nameof(next));

            var argument = context.ActionArguments[this.argumentName];
            var error = string.Empty;
            var errorList = new List<ErrorResponse>();
            switch (this.argumentName)
            {
                case "movements":
                    error = await ValidateItemsAsync<SapMovement>(argument, context).ConfigureAwait(false);
                    break;
                case "inventories":
                    error = await ValidateItemsAsync<SapInventory>(argument, context).ConfigureAwait(false);
                    break;
                case "sales":
                    errorList = await ValidateSalesDataAsync(argument, context).ConfigureAwait(false);
                    break;
                case "purchase":
                    error = await ValidatePurchasesPositionsAsync(argument, context).ConfigureAwait(false);
                    break;
                case "LogisticMovement":
                    error = await ValidateLogisticMovementAsync(argument, context).ConfigureAwait(false);
                    break;
                default:
                    error = string.Empty;
                    break;
            }

            await this.EvaluateErrorsAsync(context, next, error, errorList).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the result context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="error">The string with error.</param>
        /// <param name="errorList">The error list.</param>
        /// <returns>The context result.</returns>
        private static IActionResult GetContextResult(ActionExecutingContext context, string error, List<ErrorResponse> errorList)
        {
            if (!string.IsNullOrEmpty(error))
            {
                return context.HttpContext.BuildErrorResult(error);
            }

            if (errorList != null)
            {
                return context.HttpContext.BuildErrorResult(errorList);
            }

            return null;
        }

        /// <summary>
        /// Validate the sales data.
        /// </summary>
        /// <param name="argument">The sales argument.</param>
        /// <param name="context">the context.</param>
        /// <returns>The result validation.</returns>
        private static async Task<List<ErrorResponse>> ValidateSalesDataAsync(object argument, ActionExecutingContext context)
        {
            var sale = (Sale)argument;
            EventType eventSales = GetEventSale(sale);
            List<string> errorList;

            errorList = eventSales == EventType.Insert || eventSales == EventType.Update
                ? await ValidateInsertOrUpdateSalesDataAsync(sale, context).ConfigureAwait(false)
                : await ValidateDeleteSalesDataAsync(sale, context).ConfigureAwait(false);

            if (errorList.Any())
            {
                return BuildSalesModelError(errorList);
            }

            return null;
        }

        /// <summary>
        /// Build the model error with data validation.
        /// </summary>
        /// <param name="errorList">The error list.</param>
        /// <returns>The errors.</returns>
        private static List<ErrorResponse> BuildSalesModelError(List<string> errorList)
        {
            List<ErrorResponse> errors = new List<ErrorResponse>();

            foreach (var item in errorList)
            {
                errors.Add(new ErrorResponse(item));
            }

            return errors;
        }

        /// <summary>
        /// Validate sales data based on action.
        /// </summary>
        /// <param name="sale">the sale to validate.</param>
        /// <param name="context">The context.</param>
        /// <returns>The error list.</returns>
        private static async Task<List<string>> ValidateInsertOrUpdateSalesDataAsync(Sale sale, ActionExecutingContext context)
        {
            List<string> errorList = new List<string>();

            // Validate Positions
            await ValidateSalesPositionsAsync(sale, context, errorList).ConfigureAwait(false);

            // Validate ControlData
            ValidateControlData(sale, errorList);

            // Validate Header
            ValidateHeaderData(sale, errorList);

            // Validate Positions Data
            for (int i = 0; i < sale.OrderSale.PositionObject.Positions.Count(); i++)
            {
                errorList.AddRange(ValidatePositions(sale.OrderSale.PositionObject.Positions.ToList()[i], i));
            }

            return errorList;
        }

        /// <summary>
        /// Validate sales data based on delete action.
        /// </summary>
        /// <param name="sale">the sale to validate.</param>
        /// <param name="context">The context.</param>
        /// <returns>The error list.</returns>
        private static async Task<List<string>> ValidateDeleteSalesDataAsync(Sale sale, ActionExecutingContext context)
        {
            List<string> errorList = new List<string>();

            // Validate Positions
            await ValidateSalesPositionsAsync(sale, context, errorList).ConfigureAwait(false);

            return errorList;
        }

        /// <summary>
        /// Validate positions.
        /// </summary>
        /// <param name="sale">The sale object to validate.</param>
        /// <param name="context">The context.</param>
        /// <param name="errorList">The error list.</param>
        /// <returns>The errors.</returns>
        private static async Task ValidateSalesPositionsAsync(Sale sale, ActionExecutingContext context, List<string> errorList)
        {
            var items = sale.OrderSale.PositionObject.Positions;
            if (!items.Any())
            {
                errorList.Add(Entities.Constants.NoRecordsFound);
            }

            var sapSettings = await GetMaxPositionsAsync(context).ConfigureAwait(false);
            if (items.Count() > sapSettings.SalesPositionsMaxLimit)
            {
                errorList.Add(string.Format(CultureInfo.InvariantCulture, SapConstants.MoreThanMaxLimitPositionsFound, sapSettings.SalesPositionsMaxLimit));
            }
        }

        /// <summary>
        /// Validate the control data.
        /// </summary>
        /// <param name="sale">The sale object to validate.</param>
        /// <param name="errorList">The error list.</param>
        private static void ValidateControlData(Sale sale, List<string> errorList)
        {
            if (sale.OrderSale.ControlData.DateReceivedPo == null)
            {
                errorList.Add(SapConstants.DateReceivedPoRequired);
            }

            if (string.IsNullOrEmpty(sale.OrderSale.ControlData.SourceSystem))
            {
                errorList.Add(SapConstants.SourceSystemRequired);
            }
            else
            {
                if (sale.OrderSale.ControlData.SourceSystem.Length > 20)
                {
                    errorList.Add(SapConstants.SourceSystemLengthExceededPurchaseSale);
                }
            }

            if (!string.IsNullOrEmpty(sale.OrderSale.ControlData.DestinationSystem) &&
                sale.OrderSale.ControlData.DestinationSystem.Length > 10)
            {
                errorList.Add(SapConstants.SaleDestinationSystemLengthExceeded);
            }

            if (string.IsNullOrEmpty(sale.OrderSale.ControlData.MessageId))
            {
                errorList.Add(SapConstants.MessageIdRequired);
            }
        }

        /// <summary>
        /// Validate the header data.
        /// </summary>
        /// <param name="sale">The sale object to validate.</param>
        /// <param name="errorList">The error list.</param>
        private static void ValidateHeaderData(Sale sale, List<string> errorList)
        {
            if (string.IsNullOrEmpty(sale.OrderSale.Header.TypeOrder))
            {
                errorList.Add(SapConstants.TypeOrderRequired);
            }
            else
            {
                if (sale.OrderSale.Header.TypeOrder.Length > 4)
                {
                    errorList.Add(SapConstants.TypeOrderLengthExceeded);
                }
            }

            if (!string.IsNullOrEmpty(sale.OrderSale.Header.OrganizationId) &&
                sale.OrderSale.Header.OrganizationId.Length > 40)
            {
                errorList.Add(SapConstants.OrganizationIdLengthExceeded);
            }

            if (!string.IsNullOrEmpty(sale.OrderSale.Header.DescriptionStatus) &&
                sale.OrderSale.Header.DescriptionStatus.Length > 20)
            {
                errorList.Add(SapConstants.DescriptionStatusLengthExceeded);
            }

            if (string.IsNullOrEmpty(sale.OrderSale.Header.ClientId))
            {
                errorList.Add(SapConstants.ClientIdRequired);
            }
        }

        /// <summary>
        /// Validate list of positions.
        /// </summary>
        /// <param name="position">The position object to validate.</param>
        /// <param name="index">The index in list.</param>
        /// <returns>The errors.</returns>
        private static List<string> ValidatePositions(Position position, int index)
        {
            List<string> errors = new List<string>();

            if (string.IsNullOrWhiteSpace(position.Frequency))
            {
                position.Frequency = SapConstants.DefaultFrequency;
            }

            ValidateQuantity(position, index, errors);

            ValidateDate(position, index, errors);

            ValidateLocation(position, index, errors);

            if (string.IsNullOrEmpty(position.Material))
            {
                errors.Add($"{SapConstants.MaterialRequired} en la posición {index + 1}");
            }
            else
            {
                if (position.Material.Length > 40)
                {
                    errors.Add($"{SapConstants.MaterialLengthExceeded} en la posición {index + 1}");
                }
            }

            return errors;
        }

        /// <summary>
        /// Validate location data.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="index">The index in list.</param>
        /// <param name="errors">The errors.</param>
        private static void ValidateLocation(Position position, int index, List<string> errors)
        {
            if (string.IsNullOrEmpty(position.DestinationStorageLocationId))
            {
                errors.Add($"{SapConstants.DestinationStorageLocationIdRequired} en la posición {index + 1}");
            }
            else
            {
                if (position.DestinationStorageLocationId.Length > 4)
                {
                    errors.Add($"{SapConstants.DestinationStorageLocationIdLengthExceededSale} en la posición {index + 1}");
                }
            }

            if (string.IsNullOrEmpty(position.DestinationLocationId))
            {
                errors.Add($"{SapConstants.DestinationLocationIdRequired} en la posición {index + 1}");
            }
            else
            {
                if (position.DestinationLocationId.Length > 40)
                {
                    errors.Add($"{SapConstants.DestinationLocationLengthExceeded} en la posición {index + 1}");
                }
            }
        }

        /// <summary>
        /// Validate quantity data.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="index">The index in list.</param>
        /// <param name="errors">The errors.</param>
        private static void ValidateQuantity(Position position, int index, List<string> errors)
        {
            if (string.IsNullOrEmpty(position.Quantity))
            {
                errors.Add($"{SapConstants.QuantityRequired} en la posición {index + 1}");
            }
            else
            {
                if (position.Quantity.Length > 19)
                {
                    errors.Add($"{SapConstants.QuantityLengthExceeded} en la posición {index + 1}");
                }
            }

            if (string.IsNullOrEmpty(position.QuantityUom))
            {
                errors.Add($"{SapConstants.QuantityOumRequired} en la posición {index + 1}");
            }
            else
            {
                if (position.QuantityUom.Length > 3)
                {
                    errors.Add($"{SapConstants.QuantityOumLengthExceeded} en la posición {index + 1}");
                }
            }
        }

        /// <summary>
        /// Validate dates in position data.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="index">The index in list.</param>
        /// <param name="errors">The errors.</param>
        private static void ValidateDate(Position position, int index, List<string> errors)
        {
            if (position.StartTime == null)
            {
                errors.Add($"{SapConstants.StartTimeRequired} en la posición {index + 1}");
            }

            if (position.EndTime == null)
            {
                errors.Add($"{SapConstants.EndTimeRequired} en la posición {index + 1}");
            }

            if ((position.StartTime != null && position.EndTime != null) && position.EndTime < position.StartTime)
            {
                errors.Add($"{SapConstants.EndPeriodGreaterThanStartPeriod} en la posición {index + 1}");
            }
        }

        /// <summary>
        /// Gets the event.
        /// </summary>
        /// <param name="sale">The sale.</param>
        /// <returns>The EventType.</returns>
        private static EventType GetEventSale(Sale sale)
        {
            if (sale.OrderSale.ControlData.EventSapPo.EqualsIgnoreCase("CREAR"))
            {
                return EventType.Insert;
            }
            else
            {
                if (sale.OrderSale.PositionObject.Positions.Any(s => string.IsNullOrEmpty(s.PositionStatus)))
                {
                    return EventType.Update;
                }

                return EventType.Delete;
            }
        }

        /// <summary>
        /// Validate purchase positions.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The error.</returns>
        private static async Task<string> ValidatePurchasesPositionsAsync(object argument, ActionExecutingContext context)
        {
            var purchase = (SapPurchase)argument;
            var items = purchase.PurchaseOrder.PurchaseOrder.PurchaseItem.PurchaseItem;
            if (!items.Any())
            {
                return Entities.Constants.NoRecordsFound;
            }

            var sapSettings = await GetMaxPositionsAsync(context).ConfigureAwait(false);
            if (items.Count() > sapSettings.PurchasesPositionsMaxLimit)
            {
                return string.Format(CultureInfo.InvariantCulture, SapConstants.MoreThanMaxLimitPositionsFound, sapSettings.PurchasesPositionsMaxLimit);
            }

            return null;
        }

        /// <summary>
        /// Validate items on movements and inventories.
        /// </summary>
        /// <typeparam name="T">The Type.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="context">The context.</param>
        /// <returns>The error.</returns>
        private static async Task<string> ValidateItemsAsync<T>(object argument, ActionExecutingContext context)
        {
            var items = (IEnumerable<T>)argument;
            if (!items.Any())
            {
                return Entities.Constants.NoRecordsFound;
            }

            var sapRecordsMaxLimit = await GetMaxRecordsAsync(context).ConfigureAwait(false);
            if (items.Count() > sapRecordsMaxLimit)
            {
                return string.Format(CultureInfo.InvariantCulture, SapConstants.MoreThanMaxLimitRecordsFound, sapRecordsMaxLimit);
            }

            return null;
        }

        /// <summary>
        /// ets the maximum records for movements and inventories.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The maximum records.</returns>
        private static async Task<int> GetMaxRecordsAsync(ActionExecutingContext context)
        {
            var configurationHandler = (IConfigurationHandler)context.HttpContext.RequestServices.GetService(typeof(IConfigurationHandler));
            var sapConfig = await configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            var sapRecordsMaxLimit = sapConfig.SapRecordsMaxLimit.GetValueOrDefault();
            return sapRecordsMaxLimit > 0 ? sapRecordsMaxLimit : 2000;
        }

        /// <summary>
        /// ets the maximum records for movements and inventories.
        /// </summary>
        /// <param name="argument">The model.</param>
        /// <param name="context">The context.</param>
        /// <returns>The maximum records.</returns>
        private static async Task<string> ValidateLogisticMovementAsync(object argument, ActionExecutingContext context)
        {
            ArgumentValidators.ThrowIfNull(argument, nameof(argument));
            var result = string.Empty;
            var request = (LogisticMovementResponse)argument;
            var service = (ILogisticsService)context.HttpContext.RequestServices.GetService(typeof(ILogisticsService));

            var notExistLogisticMovement = await service.ExistLogisticMovementByMovementIdAsync(request.MovementId).ConfigureAwait(false);
            if (notExistLogisticMovement)
            {
                result = string.Format(CultureInfo.InvariantCulture, SapConstants.LogisticMomeventNotFound, request.MovementId);
            }
            else
            {
                var logisticMovement = await service.GetLogisticMovementByMovementIdAsync(request.MovementId).ConfigureAwait(false);
                if (logisticMovement == null)
                {
                    result = SapConstants.LogisticMomeventIsInvalidStatus;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the maximum records for purchases and sales.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The SapSettings.</returns>
        private static async Task<SapSettings> GetMaxPositionsAsync(ActionExecutingContext context)
        {
            var configurationHandler = (IConfigurationHandler)context.HttpContext.RequestServices.GetService(typeof(IConfigurationHandler));
            var sapConfig = await configurationHandler.GetConfigurationAsync<SapSettings>(ConfigurationConstants.SapSettings).ConfigureAwait(false);
            return sapConfig;
        }

        /// <summary>
        /// Evaluate the error result.
        /// </summary>
        /// <param name="context">The contexto.</param>
        /// <param name="next">The next action.</param>
        /// <param name="error">The error string.</param>
        /// <param name="errorList">The error list.</param>
        /// <returns>The action result.</returns>
        private async Task EvaluateErrorsAsync(ActionExecutingContext context, ActionExecutionDelegate next, string error, List<ErrorResponse> errorList)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                context.Result = GetContextResult(context, error, errorList);
            }
            else if (errorList != null && errorList.Any())
            {
                context.Result = GetContextResult(context, error, errorList);
            }
            else
            {
                await base.OnActionExecutionAsync(context, next).ConfigureAwait(false);
            }
        }
    }
}