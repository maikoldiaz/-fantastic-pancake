// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Administration.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Perf.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.WebTesting;

    [SuppressMessage("Design", "CA1010:Collections should implement generic interface", Justification = "Pending")]
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Pending")]
    [Priority(0)]
    //// [IncludeCodedWebTest(WebTestEx.IncludeClass, WebTestEx.IncludePath)]
    public class Administration : WebTest
    {
        public Administration()
        {
            this.Init();
        }

        public override IEnumerator<WebTestRequest> GetRequestEnumerator()
        {
            foreach (var login in this.SetupLogin())
            {
                yield return login;
            }

            string mainSeg = null;
            for (int j = 1; j <= 2; j++)
            {
                var dstr = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
                for (int i = 1; i <= 4; i++)
                {
                    yield return this.ExecutePostTransaction("Create Node Element", Routes.CategoryElements, $"{{\r\n  \"name\": \"perfelement_{this.Context.WebTestUserId}_{dstr}\",\r\n  \"description\": \"CategoryElementAutomation\",\r\n  \"isActive\": true,\r\n  \"categoryId\": \"{i}\"\r\n}}\r\n\r\n");
                }

                string cn = null;
                for (int i = 1; i <= 4; i++)
                {
                    cn = WebTestEx.CategoryNames[i - 1];
                    yield return this.ExecuteExtractTransaction("Search Node Element", string.Format(CultureInfo.InvariantCulture, Routes.SearchCategoryElements, dstr, cn), "\"elementId\":", $"param_ele_{i}");
                    if ((i == 2) && (j == 1))
                    {
                        mainSeg = this.Context["param_ele_2"].ToString();
                    }
                }

                yield return this.ExecutePostTransaction("Create Node", Routes.Nodes, $"{{\r\n\"name\": \"perfnode_{this.Context.WebTestUserId}_{dstr}\",\r\n\"description\": \"Nodeone\",\r\n\"isActive\": true,\r\n  \"nodeTypeId\": \"{this.Context["param_ele_1"]}\",\r\n  \"segmentId\": \"{mainSeg}\",\r\n  \"operatorId\": \"{this.Context["param_ele_3"]}\",\r\n  \"sendToSap\": false,\r\n   \"order\": 2, \r\n  \"logisticCenterId\": 4033,\r\n  \"controlLimit\": 1.96,\r\n  \"acceptableBalancePercentage\": 0.1,\r\n  \"nodeStorageLocations\": [\r\n{{\r\n\"name\": \"Location1\",\r\n\"description\": \"North\",\r\n\"isActive\": true,\r\n\"sendToSap\": true,\r\n\"storageLocationId\": \"1004:M001\",\r\n\"storageLocationTypeId\": \"{this.Context["param_ele_4"]}\",\r\n\"products\": [\r\n{{\r\n\"nodeStorageLocationId\": 0,\r\n\"productId\": \"10000002318\",\r\n\"uncertaintyPercentage\": 0.04,\r\n\"isActive\": true\r\n}},\r\n{{\r\n\"nodeStorageLocationId\": 0,\r\n\"productId\": \"10000002372\",\r\n\"uncertaintyPercentage\": 0.08,\r\n\"isActive\": true\r\n}}\r\n]\r\n}}\r\n]\r\n}}\r\n\r\n");
                yield return this.ExecuteExtractTransaction("Search Node", string.Format(CultureInfo.InvariantCulture, Routes.SearchNodes, this.Context.WebTestUserId, dstr), "\"nodeId\":", $"param_node_{j}");
            }

            for (int k = 1; k <= 1; k++)
            {
                string s1 = null;
                string d1 = null;
                if (k == 1)
                {
                    s1 = this.Context["param_node_2"].ToString();
                    d1 = this.Context["param_node_1"].ToString();
                }
                else if (k == 2)
                {
                    s1 = this.Context["param_node_1"].ToString();
                    d1 = this.Context["param_node_4"].ToString();
                }
                else if (k == 3)
                {
                    s1 = this.Context["param_node_3"].ToString();
                    d1 = this.Context["param_node_1"].ToString();
                }

                string bs = null;
                if (k == 1)
                {
                    bs = $"{{\r\n  \"sourceNodeId\": \"{s1}\",\r\n  \"destinationNodeId\": \"{d1}\",\r\n  \"description\": \"Automation_Connection_2Products\",\r\n  \"isActive\": true,\r\n  \"products\": [\r\n    {{\r\n\"productId\": \"10000002318\",\r\n\"uncertaintyPercentage\": \"0.07\"\r\n    }},\r\n    {{\r\n\"productId\": \"10000002372\",\r\n\"uncertaintyPercentage\": \"0.06\"\r\n    }}\r\n  ]\r\n}}\r\n\r\n";
                }
                else if (k == 2)
                {
                    bs = $"{{\r\n  \"sourceNodeId\": \"{s1}\",\r\n  \"destinationNodeId\": \"{d1}\",\r\n  \"description\": \"Automation_Connection_2Products\",\r\n  \"isActive\": true,\r\n  \"products\": [\r\n    {{\r\n\"productId\": \"10000002318\",\r\n\"uncertaintyPercentage\": \"0.05\"\r\n    }},\r\n    {{\r\n\"productId\": \"10000002372\",\r\n\"uncertaintyPercentage\": \"0.04\"\r\n    }}\r\n  ]\r\n}}\r\n\r\n";
                }
                else if (k == 3)
                {
                    bs = $"{{\r\n  \"sourceNodeId\": \"{s1}\",\r\n  \"destinationNodeId\": \"{d1}\",\r\n  \"description\": \"Automation_Connection_1Product\",\r\n  \"isActive\": true,\r\n  \"products\": [\r\n    {{\r\n\"productId\": \"10000002372\",\r\n\"uncertaintyPercentage\": \"0.02\"\r\n    }}\r\n  ]\r\n}}\r\n\r\n";
                }

                yield return this.ExecutePostTransaction("Create Node Connection", Routes.NodeConnections, bs);
            }
        }
    }
}
