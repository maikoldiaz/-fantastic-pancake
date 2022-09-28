// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataGeneratorFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Interfaces
{
    /// <summary>
    /// The data generator factory.
    /// </summary>
    public interface IDataGeneratorFactory
    {
        /// <summary>
        /// Gets the node tag data generator.
        /// </summary>
        /// <value>
        /// The node tag data generator.
        /// </value>
        IDataGenerator NodeTagDataGenerator { get; }

        /// <summary>
        /// Gets the node connection data generator.
        /// </summary>
        /// <value>
        /// The node connection data generator.
        /// </value>
        IDataGenerator NodeConnectionDataGenerator { get; }

        /// <summary>
        /// Gets annulation data generator.
        /// </summary>
        /// <value>
        /// The annulation data generator.
        /// </value>
        IDataGenerator AnnulationDataGenerator { get; }

        /// <summary>
        /// Gets movement data generator.
        /// </summary>
        /// <value>
        /// The movement data generator.
        /// </value>
        IDataGenerator MovementDataGenerator { get; }

        /// <summary>
        /// Gets the inventory product data generator.
        /// </summary>
        /// <value>
        /// The inventory product data generator.
        /// </value>
        IDataGenerator InventoryProductDataGenerator { get; }

        /// <summary>
        /// Gets the category element data generator.
        /// </summary>
        /// <value>
        /// The category element data generator.
        /// </value>
        IDataGenerator CategoryElementDataGenerator { get; }

        /// <summary>
        /// Gets the node data generator.
        /// </summary>
        /// <value>
        /// The node data generator.
        /// </value>
        IDataGenerator NodeDataGenerator { get; }

        /// <summary>
        /// Gets the ticket data generator.
        /// </summary>
        /// <value>
        /// The ticket data generator.
        /// </value>
        IDataGenerator TicketDataGenerator { get; }

        /// <summary>
        /// Gets the consolidated movement data generator.
        /// </summary>
        /// <value>
        /// The consolidated movement data generator.
        /// </value>
        IDataGenerator ConsolidatedMovementDataGenerator { get; }

        /// <summary>
        /// Gets the consolidated inventory product data generator.
        /// </summary>
        /// <value>
        /// The consolidated inventory product data generator.
        /// </value>
        IDataGenerator ConsolidatedInventoryProductDataGenerator { get; }

        /// <summary>
        /// Gets the DeltaNodeDataGenerator.
        /// </summary>
        /// <value>
        /// The consolidated DeltaNodeDataGenerator.
        /// </value>
        IDataGenerator DeltaNodeDataGenerator { get; }

        /// <summary>
        /// Gets the homologation data generator.
        /// </summary>
        /// <value>
        /// The homologation data generator.
        /// </value>
        IDataGenerator HomologationDataGenerator { get; }
    }
}
