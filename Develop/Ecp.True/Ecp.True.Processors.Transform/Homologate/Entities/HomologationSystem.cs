// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HomologationSystem.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Processors.Transform.Homologate.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Ecp.True.Core;
    using Ecp.True.Entities.Admin;
    using Ecp.True.Entities.Core;

    /// <summary>
    /// The homologations.
    /// </summary>
    public class HomologationSystem
    {
        /// <summary>
        /// The object types.
        /// </summary>
        private readonly IDictionary<int, string> objectTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomologationSystem" /> class.
        /// </summary>
        /// <param name="objectTypes">The homologation object types.</param>
        public HomologationSystem(IEnumerable<HomologationObjectType> objectTypes)
        {
            this.Map = new HomologationMap();
            this.Objects = new ObjectsDictionary();
            this.objectTypes = objectTypes.ToDictionary(s => s.HomologationObjectTypeId, s => s.Name);
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>
        /// The map.
        /// </value>
        public HomologationMap Map { get; }

        /// <summary>
        /// Gets the objects.
        /// </summary>
        /// <value>
        /// The objects.
        /// </value>
        public ObjectsDictionary Objects { get; }

        /// <summary>
        /// Error homologate the specified Movement Type.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="messageType">Message Type.</param>
        public static void HomologateMovementTypeError(string objectName, MessageType messageType)
        {
            if (objectName == Constants.MovementTypeId && messageType == MessageType.Contract)
            {
                throw new KeyNotFoundException(Transform.Constants.ValueNotFoundMovementType);
            }
        }

        /// <summary>
        /// Homologate the specified value.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="original">The original.</param>
        /// <param name="messageType">Message Type.</param>
        /// <returns>
        /// Return the homologated value.
        /// </returns>
        public object Homologate(string objectName, object original, MessageType messageType)
        {
            if (original == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(original.ToString()))
            {
                return original;
            }

            var homologationObjectEntity = this.Objects.TryGetValue(objectName, out Tuple<string, bool> homologationObject);
            if (!homologationObjectEntity || !homologationObject.Item2)
            {
                return original;
            }

            var value = this.Map.GetHomologationValue(original.ToString(), homologationObject.Item1);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }

            HomologateMovementTypeError(objectName, messageType);
            throw new KeyNotFoundException(string.Format(CultureInfo.InvariantCulture, Transform.Constants.ValueNotFound, original, homologationObject.Item1));
        }

        /// <summary>
        /// Try the homologation and eat up all the failures.
        /// </summary>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="original">The original.</param>
        /// <param name="messageType">Message Type.</param>
        /// <returns>Returns homologated value.</returns>
        public object TryHomologate(string objectName, object original, MessageType messageType)
        {
            try
            {
                return this.Homologate(objectName, original, messageType);
            }
            catch (KeyNotFoundException)
            {
                return original;
            }
        }

        /// <summary>
        /// Adds the object.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public void AddObjects(IEnumerable<HomologationObject> objects)
        {
            ArgumentValidators.ThrowIfNull(objects, nameof(objects));

            objects.ForEach(o =>
            {
                if (this.objectTypes.TryGetValue(o.HomologationObjectTypeId.Value, out string keyName) && o?.HomologationGroup?.GroupTypeId != null)
                {
                    var groupTypes = Tuple.Create(o.HomologationGroup.Group.Name, o.IsRequiredMapping);
                    this.Objects.AddOrUpdate(keyName, groupTypes, (a, b) => groupTypes);
                }
            });
        }

        /// <summary>
        /// Adds the mappings.
        /// </summary>
        /// <param name="maps">The maps.</param>
        public void AddMappings(IEnumerable<HomologationDataMapping> maps)
        {
            var configuration = new List<ConfigurationMap>();
            maps.ForEach(x => configuration.Add(this.SetconfigurationMap(x)));
            this.Map.ConfigurationMaps = configuration;
        }

        private ConfigurationMap SetconfigurationMap(HomologationDataMapping homologationDataMapping)
        {
            return new ConfigurationMap
            {
                Name = homologationDataMapping.SourceValue,
                Value = homologationDataMapping.DestinationValue,
                CategoryName = homologationDataMapping.HomologationGroup.Group.Description,
            };
        }
    }
}