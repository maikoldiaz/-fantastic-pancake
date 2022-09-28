// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlobExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Specialized;
    using System.Configuration;

    using System.Threading.Tasks;
    using System.Xml;

    using Ecp.True.Bdd.Tests.Entities;

    using global::Bdd.Core.Utils;

    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    using NLog;

    public static class BlobExtensions
    {
        private const string SinoperXml = "sinoper/xml/";
        private const string SinoperJson = "sinoper/json/";
        private const string TestContainer = "true-test";
        private const string True = "true";
        private const string Ownership = "ownership";
        private const string SapProcessResult = "sap/out";
        private const string Delta = "delta";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly ConcurrentDictionary<string, (CloudStorageAccount StorageAccount, string ConnectionString)> StorageAccounts = new ConcurrentDictionary<string, (CloudStorageAccount StorageAccount, string ConnectionString)>();
        private static readonly ConcurrentDictionary<string, CloudBlobClient> Clients = new ConcurrentDictionary<string, CloudBlobClient>();
        private static readonly NameValueCollection Settings = ConfigurationManager.GetSection("blobStorage") as NameValueCollection;

        private static string DefaultKeyPrefix => ConfigurationManager.AppSettings[nameof(DefaultKeyPrefix)];

        public static string GetMessageId(this string fileName, string folder, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(TestContainer);
            string directoryPath = SinoperXml + folder;
            var directory = cloudBlobContainer.GetDirectoryReference(@directoryPath);
            var blobs = directory.ListBlobs();
            foreach (var blob in blobs)
            {
                if (blob.Uri.ToString().ContainsIgnoreCase(fileName))
                {
                    string uri = blob.Uri.ToString();
                    return uri.Split('_')[1].Split('.')[0];
                }
            }

            return null;
        }

        public static async Task<bool> GetBlobFromMessageIdAsync(string blobContainer, string blobId, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(True + "/" + blobContainer);
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(blobId);
            try
            {
                string contents = await blob.DownloadTextAsync().ConfigureAwait(false);
                return contents != null;
            }
            catch (StorageException)
            {
                Logger.Info("Blob does not exist in the Container");
                return false;
            }
        }

        public static async Task UploadFileAsync(string containerName, string fileName, string uploadFileName, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            string filePath = fileName + ".csv";
            uploadFileName = (@"TestData\Input\" + uploadFileName + ".csv").GetFullPath();
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filePath);
            await cloudBlockBlob.UploadFromFileAsync(uploadFileName).ConfigureAwait(false);
        }

        public static async Task UploadExcelFileAsync(string containerName, string fileName, string uploadFileName, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            string filePath = fileName + ".xlsx";
            uploadFileName = (@"TestData\Input\ExcelUpload\" + uploadFileName + ".xlsx").GetFullPath();
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            await cloudBlockBlob.UploadFromFileAsync(uploadFileName).ConfigureAwait(false);
        }

        public static async Task UploadXmlFileAsync(string containerName, string fileName, string uploadFileName, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            string filePath = fileName + ".xml";
            if (uploadFileName.ContainsIgnoreCase(ConstantValues.InventoryQueueName))
            {
                uploadFileName = (@"TestData\Input\Inventory\" + uploadFileName + ".xml").GetFullPath();
            }
            else if (uploadFileName.ContainsIgnoreCase(ConstantValues.MovementQueueName))
            {
                uploadFileName = (@"TestData\Input\Movements\" + uploadFileName + ".xml").GetFullPath();
            }

            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = "application/xml";
            await cloudBlockBlob.UploadFromFileAsync(uploadFileName).ConfigureAwait(false);
        }

        public static async Task<string> DeltafromBlobAsync(this string fileName, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Delta);
            string folderPath = fileName;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(@folderPath);
            string contents = await blob.DownloadTextAsync().ConfigureAwait(false);
            return contents;
        }

        public static async Task<string> DownloadBlobDataAsync(this string fileName, string folder, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(True);
            string folderPath = SinoperJson + folder + "/" + fileName;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(@folderPath);
            string contents = await blob.DownloadTextAsync().ConfigureAwait(false);
            return contents;
        }

        public static async Task<string> DownloadSapProcessResultBlobDataAsync(this string fileName, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(True);
            string folderPath = SapProcessResult + "/" + fileName + ".json";
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(@folderPath);
            string contents = await blob.DownloadTextAsync().ConfigureAwait(false);
            return contents;
        }

        public static async Task UploadXmlAsync(this string fileName, string foldername, string uploadFileName, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(TestContainer);
            string filePath = SinoperXml + foldername + "/" + fileName;
            uploadFileName = (@"TestData\Input\" + uploadFileName + ".xml").GetFullPath();
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(@filePath);
            await cloudBlockBlob.UploadFromFileAsync(uploadFileName).ConfigureAwait(false);
        }

        public static void UpdateXmlData(string fileName, string field, string value)
        {
            fileName = AppDomain.CurrentDomain.BaseDirectory + "TestData\\Input\\" + fileName + ".xml";
            var xmlDoc = new XmlDocument() { XmlResolver = null };
#pragma warning disable CA3075 // Insecure DTD processing in XML
            xmlDoc.Load(fileName);
#pragma warning restore CA3075 // Insecure DTD processing in XML
            XmlElement formData = (XmlElement)xmlDoc.SelectSingleNode(ApiContent.UpdateXML[field]);
            if (field == ConstantValues.BATCHID + "_1" || field == ConstantValues.BATCHID + "_2")
            {
                field = ConstantValues.BATCHID;
            }

            if ((field.EqualsIgnoreCase("Inventory") || field.EqualsIgnoreCase("Movement")) && field != null)
            {
                formData.SelectSingleNode(field.ToUpperInvariant()).Attributes["ACTION"].InnerText = value;
            }
            else if (formData != null && field != null)
            {
                field = field.ContainsIgnoreCase("Movement ") || field.ContainsIgnoreCase("Inventory ") ? field.Split(' ')[1] : field;
                formData.SelectSingleNode(field.ToUpperInvariant()).InnerText = value;
            }

            xmlDoc.Save(fileName);
        }

        public static async Task<string> OwnershipdatafromBlobAsync(this string fileName, string folder, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Ownership);
            string folderPath = folder + "/" + fileName;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(@folderPath);
            string contents = await blob.DownloadTextAsync().ConfigureAwait(false);
            return contents;
        }

        public static async Task<string> OfficialDeltafromBlobAsync(this string fileName, string folder, string keyPrefix = null)
        {
            var cloudBlobClient = GetBlobClient(keyPrefix);
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(Delta);
            string folderPath = Delta + "/" + folder + "/" + fileName;
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(@folderPath);
            string contents = await blob.DownloadTextAsync().ConfigureAwait(false);
            return contents;
        }

        private static CloudBlobClient GetBlobClient(string keyPrefix = null)
        {
            var key = string.IsNullOrWhiteSpace(keyPrefix) ? DefaultKeyPrefix : keyPrefix;
            var client = Clients.GetOrAdd(key, k =>
            {
                var storageAccount = SetConnection(key, Settings);
                return storageAccount.StorageAccount.CreateCloudBlobClient();
            });

            return client;
        }

        private static (CloudStorageAccount StorageAccount, string ConnectionString) SetConnection(string key, NameValueCollection settings)
        {
            var storageAccount = StorageAccounts.GetOrAdd(key, k =>
            {
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
                var connectionString = GetStorageConnectionStringAsync(key, settings).GetAwaiter().GetResult();
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
                CloudStorageAccount account = null;
                try
                {
                    account = CloudStorageAccount.Parse(connectionString); // GetStorageAccount()
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception ex)
                {
                    Logger.Warn(ex);
                }
#pragma warning restore CA1031 // Do not catch general exception types

                return (account, connectionString);
            });

            return storageAccount;
        }

        private static async Task<string> GetStorageConnectionStringAsync(string keyPrefix, NameValueCollection settings)
        {
            var connectionStringSecretName = settings.GetValue("ConnectionStringKey", string.IsNullOrWhiteSpace(keyPrefix) ? DefaultKeyPrefix : keyPrefix);
            if (connectionStringSecretName.ContainsIgnoreCase("Endpoint"))
            {
                return connectionStringSecretName;
            }

            var connectionString = (await KeyVaultHelper.GetKeyVaultSecretAsync(connectionStringSecretName).ConfigureAwait(false)).Value;
            return connectionString;
        }
    }
}