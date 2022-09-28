using Ecp.True.Stubs.Api.Processor;
using Ecp.True.Stubs.Api.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ecp.True.Stubs.Api.Controllers
{

    [ApiController]
    public class SapResponseController : ControllerBase
    {

        public SapResponseController()
        {

        }

        [HttpPost]
        [Route("sappo/transferpoint")]
        public  IActionResult TransferPointValidation([FromBody] JObject request)
        {
            var headers = this.Request.Headers;
            StringValues chaosValues;
            if (headers.TryGetValue("chaos", out chaosValues))
            {
                var chaosValue = chaosValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(chaosValue))
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet]
        [Route("sappo/mappings")]
        public ActionResult<IEnumerable<SapMappingResponse>> GetMappingsAsync([FromHeader] string chaos)
        {
            if (string.Equals(chaos, "Sap", StringComparison.OrdinalIgnoreCase))
            {
                return new StatusCodeResult(500);
            }

            if (string.Equals(chaos, "Blank", StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new List<SapMappingResponse>());
            }

            var responses = this.GetSapMappingResponse();
            return Ok(responses);
        }

        private IEnumerable<SapMappingResponse> GetSapMappingResponse()
        {
            return new List<SapMappingResponse>
            {
                new SapMappingResponse
            {
                DestinationProductId = "10000003006",
                DestinationMovementTypeId = 42,
                DestinationSystemDestinationNodeId = 225,
                DestinationSystemId = 9,
                DestinationSystemSourceNodeId = 40,
                OfficialSystem = 9,
                SourceMovementTypeId = 42,
                SourceProductId = "10000003006",
                SourceSystemDestinationNodeId = 225,
                SourceSystemId = 1,
                SourceSystemSourceNodeId = 40
            },
                 new SapMappingResponse
            {
                DestinationProductId = "10000003010",
                DestinationMovementTypeId = 40,
                DestinationSystemDestinationNodeId = 220,
                DestinationSystemId = 19,
                DestinationSystemSourceNodeId = 80,
                OfficialSystem = 3,
                SourceMovementTypeId = 42,
                SourceProductId = "10000003006",
                SourceSystemDestinationNodeId = 230,
                SourceSystemId = 1,
                SourceSystemSourceNodeId = 34
            },
            };
        }
    }
}
