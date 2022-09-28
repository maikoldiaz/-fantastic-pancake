// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Entities.Query;
    using Ecp.True.Processors.Ownership.Interfaces;
    using EntityConstants = Ecp.True.Entities.Constants;

    /// <summary>
    /// The Data Generator Service.
    /// </summary>
    /// <seealso cref="Ecp.True.Processors.Ownership.Calculation.Interfaces.IDataGeneratorService" />
    public class DataGeneratorService : IDataGeneratorService
    {
        /// <summary>
        /// Transforms the logistics data.
        /// </summary>
        /// <param name="logisticsInfo">The logistics details.</param>
        /// <returns>
        /// Returns data set.
        /// </returns>
        public DataSet TransformLogisticsData(LogisticsInfo logisticsInfo)
        {
            ArgumentValidators.ThrowIfNull(logisticsInfo, nameof(logisticsInfo));

            if (!logisticsInfo.LogisticMovementDetail.Any())
            {
                throw new ArgumentException(EntityConstants.NoDataFoundForLogisticFile);
            }

            if (!logisticsInfo.LogisticInventoryDetail.Any())
            {
                throw new ArgumentException(EntityConstants.NoInventoryDataFoundForLogisticFile);
            }

            logisticsInfo.LogisticMovementDetail.ForEach(a =>
            {
                a.SetDefaultValues();
            });

            logisticsInfo.LogisticInventoryDetail.ForEach(item =>
            {
                item.SetDefaultValues();
            });

            var dataSet = new DataSet { Locale = CultureInfo.InvariantCulture };
            var logisticsMovementDataTable = Extensions.ToDataTable<OperativeLogisticsMovement>(logisticsInfo.LogisticMovementDetail, string.Empty);
            var logisticsInventoryDataTable = Extensions.ToDataTable<LogisticsInventoryDetail>(logisticsInfo.LogisticInventoryDetail, string.Empty);

            dataSet.Tables.Add(logisticsMovementDataTable);
            dataSet.Tables.Add(logisticsInventoryDataTable);

            return dataSet;
        }
    }
}
