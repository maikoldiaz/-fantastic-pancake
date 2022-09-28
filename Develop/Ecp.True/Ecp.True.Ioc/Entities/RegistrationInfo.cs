// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationInfo.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Ioc.Entities
{
    /// <summary>
    /// The registration info.
    /// </summary>
    public class RegistrationInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationInfo" /> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="component">The component.</param>
        /// <param name="name">The name.</param>
        /// <param name="lifetime">The lifetime.</param>
        public RegistrationInfo(string service, string component, string name, string lifetime)
        {
            this.Service = service;
            this.Component = component;
            this.Name = name;
            this.Lifetime = lifetime;
        }

        /// <summary>
        /// Gets the component.
        /// </summary>
        /// <value>
        /// The component.
        /// </value>
        public string Component { get; }

        /// <summary>
        /// Gets the lifetime.
        /// </summary>
        /// <value>
        /// The lifetime.
        /// </value>
        public string Lifetime { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public string Service { get; }
    }
}