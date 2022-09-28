// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataGeneratorFactory.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Host.DataGenerator.Console.Generators
{
    using Ecp.True.DataAccess.Interfaces;
    using Ecp.True.Host.DataGenerator.Console.Interfaces;

    /// <summary>
    /// The data generator factory.
    /// </summary>
    /// <seealso cref="Ecp.True.Host.DataGenerator.Console.Interfaces.IDataGeneratorFactory" />
    public class DataGeneratorFactory : IDataGeneratorFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataGeneratorFactory"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public DataGeneratorFactory(IUnitOfWork unitOfWork)
        {
            this.NodeTagDataGenerator = new NodeTagDataGenerator(unitOfWork);
            this.NodeConnectionDataGenerator = new NodeConnectionDataGenerator(unitOfWork);
            this.AnnulationDataGenerator = new AnnulationDataGenerator(unitOfWork);
            this.MovementDataGenerator = new MovementDataGenerator(unitOfWork);
            this.InventoryProductDataGenerator = new InventoryProductDataGenerator(unitOfWork);
            this.CategoryElementDataGenerator = new CategoryElementDataGenerator(unitOfWork);
            this.NodeDataGenerator = new NodeDataGenerator(unitOfWork);
            this.TicketDataGenerator = new TicketDataGenerator(unitOfWork);
            this.ConsolidatedInventoryProductDataGenerator = new ConsolidatedInventoryProductDataGenerator(unitOfWork);
            this.ConsolidatedMovementDataGenerator = new ConsolidatedMovementDataGenerator(unitOfWork);
            this.DeltaNodeDataGenerator = new DeltaNodeDataGenerator(unitOfWork);
            this.HomologationDataGenerator = new HomologationDataGenerator(unitOfWork);
        }

        /// <summary>
        /// Gets the node tag data generator.
        /// </summary>
        /// <value>
        /// The node tag data generator.
        /// </value>
        public IDataGenerator NodeTagDataGenerator { get; }

        /// <summary>
        /// Gets the node connection data generator.
        /// </summary>
        /// <value>
        /// The node connection data generator.
        /// </value>
        public IDataGenerator NodeConnectionDataGenerator { get; }

        /// <summary>
        /// Gets annulation data generator.
        /// </summary>
        /// <value>
        /// The annulation data generator.
        /// </value>
        public IDataGenerator AnnulationDataGenerator { get; }

        /// <summary>
        /// Gets movement data generator.
        /// </summary>
        /// <value>
        /// The movement data generator.
        /// </value>
        public IDataGenerator MovementDataGenerator { get; }

        /// <summary>
        /// Gets the inventory product data generator.
        /// </summary>
        /// <value>
        /// The inventory product data generator.
        /// </value>
        public IDataGenerator InventoryProductDataGenerator { get; }

        /// <summary>
        /// Gets the category element data generator.
        /// </summary>
        /// <value>
        /// The category element data generator.
        /// </value>
        public IDataGenerator CategoryElementDataGenerator { get; }

        /// <summary>
        /// Gets the node data generator.
        /// </summary>
        /// <value>
        /// The node data generator.
        /// </value>
        public IDataGenerator NodeDataGenerator { get; }

        /// <summary>
        /// Gets the ticket data generator.
        /// </summary>
        /// <value>
        /// The ticket data generator.
        /// </value>
        public IDataGenerator TicketDataGenerator { get; }

        /// <summary>
        /// Gets the consolidated movement data generator.
        /// </summary>
        /// <value>
        /// The consolidated movement data generator.
        /// </value>
        public IDataGenerator ConsolidatedMovementDataGenerator { get; }

        /// <summary>
        /// Gets the consolidated inventory product data generator.
        /// </summary>
        /// <value>
        /// The consolidated inventory product data generator.
        /// </value>
        public IDataGenerator ConsolidatedInventoryProductDataGenerator { get; }

        /// <summary>
        /// Gets the DeltaNodeDataGenerator.
        /// </summary>
        /// <value>
        /// The DeltaNodeDataGenerator.
        /// </value>
        public IDataGenerator DeltaNodeDataGenerator { get; }

        /// <summary>
        /// Gets the homologation data generator.
        /// </summary>
        /// <value>
        /// The homologation data generator.
        /// </value>
        public IDataGenerator HomologationDataGenerator { get; }
    }
}
