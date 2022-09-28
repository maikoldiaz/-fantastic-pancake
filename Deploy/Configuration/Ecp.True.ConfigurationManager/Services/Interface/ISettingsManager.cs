// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISettingsManager.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.ConfigurationManager.Services.Interface
{
    using Ecp.True.ConfigurationManager.Console.Settings.Interface;
    using Ecp.True.ConfigurationManager.Entities;

    /// <summary>
    /// The setting manager.
    /// </summary>
    public interface ISettingsManager
    {
        /// <summary>
        /// Initializes the specified json data.
        /// </summary>
        /// <param name="jsonData">The json data.</param>
        void Initialize(string[] jsonData);

        /// <summary>
        /// Transforms the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="input">The input.</param>
        void Transform(ISettings settings, CopyInput input);
    }
}
