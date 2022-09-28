// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CertificateHelper.cs" company="Microsoft">
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//   THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//   OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//   OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Configuration
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// The certificate helper class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CertificateHelper
    {
        /// <summary>
        /// Gets the service certificate.
        /// </summary>
        /// <param name="thumbprint">The thumbprint.</param>
        /// <returns>
        /// The certificate.
        /// </returns>
        public static X509Certificate2 GetCertificateByThumbprint(string thumbprint)
        {
            X509Certificate2 certificate = null;
            using (var certStore = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint, thumbprint, false);

                if (certCollection.Count > 0)
                {
                    certificate = certCollection[0];
                    return certificate;
                }
            }

            // If the certificate is not found in local machine store check in current store
            using (var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint, thumbprint, false);

                // Get the first certificate
                if (certCollection.Count > 0)
                {
                    certificate = certCollection[0];
                }

                return certificate;
            }
        }
    }
}