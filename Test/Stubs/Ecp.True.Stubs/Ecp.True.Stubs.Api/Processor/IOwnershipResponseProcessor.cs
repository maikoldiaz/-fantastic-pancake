using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecp.True.Stubs.Api.Processor
{
    public interface IOwnershipResponseProcessor
    {
        Task SaveJsonToBlob(JObject ownershipResponse, string blobName);

        Task<JObject> GetResponseJson(JObject request, bool hasErrors, string blobName);

        Task<bool> DeleteResponseBlob(string blobName);

        JObject BuildResponse(JObject request, bool hasErrors);
    }
}
