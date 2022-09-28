// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGeneratorService.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Ownership.Interfaces
{
    using System.Data;
    using Ecp.True.Entities.Query;

    /// <summary>
    /// The data generator service interface.
    /// </summary>
    public interface IDataGeneratorService
    {
        /// <summary>
        /// Transforms the logistics data.
        /// </summary>
        /// <param name="logisticsInfo">The logistics details.</param>
        /// <returns>Returns data set.</returns>
        DataSet TransformLogisticsData(LogisticsInfo logisticsInfo);
    }
}
