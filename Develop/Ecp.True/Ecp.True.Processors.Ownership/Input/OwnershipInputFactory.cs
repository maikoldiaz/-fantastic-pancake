// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OwnershipInputFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Input
{
    using System;
    using System.IO;
    using Ecp.True.Core;
    using Ecp.True.Entities.Dto;
    using Ecp.True.Logging;
    using Ecp.True.Processors.Core.Input;
    using Ecp.True.Processors.Ownership.Input.Interfaces;
    using ExcelDataReader;

    /// <summary>
    /// The ownership input factory.
    /// </summary>
    public class OwnershipInputFactory : IOwnershipInputFactory
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ITrueLogger<OwnershipInputFactory> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="OwnershipInputFactory"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public OwnershipInputFactory(ITrueLogger<OwnershipInputFactory> logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc/>
        public ExcelInput GetExcelInput(TrueMessage message, Stream stream)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            this.logger.LogInformation(message.MessageId);
            ArgumentValidators.ThrowIfNull(message, nameof(message));
            ArgumentValidators.ThrowIfStreamEmpty(stream, nameof(stream));
            stream.Position = 0;
            using (var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream))
            {
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = x => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true,
                    },
                };

                return new ExcelInput(excelReader.AsDataSet(conf), message);
            }
        }
    }
}
