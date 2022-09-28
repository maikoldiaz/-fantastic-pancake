using Ecp.True.Stubs.Api.Processor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Ecp.True.Stubs.Api.Controllers
{

    [ApiController]
    public class OwnershipController
    {
        private readonly IOwnershipResponseProcessor ownershipResponseProcessor;

        private const string resultBlobName = "ownershipresultresponse";
        private const string errorBlobName = "ownershiperrorresponse";

        public OwnershipController(IOwnershipResponseProcessor ownershipResponseProcessor)
        {
            this.ownershipResponseProcessor = ownershipResponseProcessor;
        }

        [HttpPut]
        [Route("decisionExecutor/rest/service/processWithDecisionFlow/results")]
        public async Task<IActionResult> SaveResultResponse([FromBody] JObject request)
        {
            await ownershipResponseProcessor.SaveJsonToBlob(request, resultBlobName);
            return new OkResult();
        }

        [HttpPut]
        [Route("decisionExecutor/rest/service/processWithDecisionFlow/errors")]
        public async Task<IActionResult> SaveErrorResponse([FromBody] JObject request)
        {
            await ownershipResponseProcessor.SaveJsonToBlob(request, errorBlobName);
            return new OkResult();
        }

        [HttpPost]
        [Route("decisionExecutor/rest/service/processWithDecisionFlow/results")]
        public async Task<IActionResult> GetResultResponse([FromBody] JObject request)
        {
            var data = await ownershipResponseProcessor.GetResponseJson(request, false, resultBlobName);
            return new JsonResult(data);
        }

        [HttpPost]
        [Route("decisionExecutor/rest/service/processWithDecisionFlow/errors")]
        public async Task<IActionResult> GetErrorResponse([FromBody] JObject request)
        {
            var data = await ownershipResponseProcessor.GetResponseJson(request, true, errorBlobName);
            return new JsonResult(data);
        }

        [HttpDelete]
        [Route("decisionExecutor/rest/service/processWithDecisionFlow/results")]
        public async Task<IActionResult> DeleteResultResponse()
        {
            await ownershipResponseProcessor.DeleteResponseBlob(resultBlobName);
            return new OkResult();
        }

        [HttpDelete]
        [Route("decisionExecutor/rest/service/processWithDecisionFlow/errors")]
        public async Task<IActionResult> DeleteErrorResponse()
        {
            await ownershipResponseProcessor.DeleteResponseBlob(errorBlobName);
            return new OkResult();
        }

        [HttpPost]
        [Route("api/ownershiprule/process/results")]
        public JObject PostResults([FromBody] JObject request)
        {
            return this.ownershipResponseProcessor.BuildResponse(request, false);
        }

        [HttpPost]
        [Route("api/ownershiprule/process/errors")]
        public JObject PostErrors([FromBody] JObject request)
        {
            return this.ownershipResponseProcessor.BuildResponse(request, true);
        }
    }
}
