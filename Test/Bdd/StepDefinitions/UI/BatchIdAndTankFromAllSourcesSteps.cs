// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BatchIdAndTankFromAllSourcesSteps.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.StepDefinitions.UI
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;
    using Ecp.True.Bdd.Tests.Utils;

    using global::Bdd.Core.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    using OfficeOpenXml;

    using TechTalk.SpecFlow;

    [Binding]
    public class BatchIdAndTankFromAllSourcesSteps : EcpWebStepDefinitionBase
    {
        [StepDefinition(@"I update the excel ""(.*)"" with both ""(.*)"" and ""(.*)""")]
        public void GivenIUpdateTheExcelWithBothAnd(string fileName, string field1, string field2)
        {
            this.GivenIUpdateTheExcelWithBothTankNameAndBatchId(fileName, field1, field2);
        }

        [Given(@"I update the excel ""(.*)"" with same ""(.*)"" for inventory, attributes, owners")]
        [Given(@"I update the excel ""(.*)"" with only ""(.*)""")]
        public void GivenIUpdateTheExcelWithOnly(string fileName, string field)
        {
            this.GivenIUpdateTheExcelWithOnlyTankName(fileName, field);
        }

        [Given(@"I update the excel ""(.*)"" with only ""(.*)"" but without ""(.*)"" in attributes and owners")]
        public void GivenIUpdateTheExcelWithOnlyButWithoutInAttributesAndOwners(string fileName, string field1, string field2)
        {
            if (string.IsNullOrEmpty(this.GetValue("InventoryId")))
            {
                this.InventoryIdAndInventoryDateUpdate(fileName);
            }

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in Excel
            if (field1.EqualsIgnoreCase("ProductVolume"))
            {
                var volume = 500.00;
                this.SetValue("ProductVolume", volume);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 12].Value = volume;
            }

            if (field2.EqualsIgnoreCase("TanKName"))
            {
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 4].Value = string.Empty;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 10].Value = string.Empty;
            }
            else if (field2.EqualsIgnoreCase("BatchId"))
            {
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 5].Value = string.Empty;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 11].Value = string.Empty;
            }

            package.Save();
            package.Dispose();
        }

        [Given(@"I update the excel ""(.*)"" with ""(.*)"" exceeds (.*) characters")]
        public void GivenIUpdateTheExcelWithExceedsCharacters(string fileName, string field, int length)
        {
            this.InventoryIdAndInventoryDateUpdate(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            if (field.EqualsIgnoreCase("TankName"))
            {
                var tankName = new Faker().Random.AlphaNumeric(length + 1);
                this.SetValue("TankName", tankName);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 5].Value = tankName;
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                var batchId = new Faker().Random.AlphaNumeric(length + 1);
                this.SetValue("TankName", batchId);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 6].Value = batchId;
            }

            package.Save();
            package.Dispose();
        }

        [Given(@"I update the excel ""(.*)"" but different ""(.*)""")]
        public void GivenIUpdateTheExcelButDifferent(string fileName, string field)
        {
            this.InventoryIdAndInventoryDateUpdate(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in excel
            if (field.EqualsIgnoreCase("TankName"))
            {
                worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                    this.SetValue("TankName" + (i - 1), tankName);
                    worksheet = package.Workbook.Worksheets[0];
                    worksheet.Cells[i, 5].Value = tankName;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 4].Value = tankName;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 10].Value = tankName;
                }
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                    this.SetValue("BatchId" + (i - 1), batchId);
                    worksheet = package.Workbook.Worksheets[0];
                    worksheet.Cells[i, 6].Value = batchId;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 5].Value = batchId;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 11].Value = batchId;
                }
            }

            package.Save();
            package.Dispose();
        }

        [Given(@"I update the excel ""(.*)"" but same ""(.*)""")]
        public void GivenIUpdateTheExcelButSame(string fileName, string field)
        {
            this.InventoryIdAndInventoryDateUpdate(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in excel
            if (field.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("TankName", tankName);
                worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    worksheet.Cells[i, 5].Value = tankName;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 4].Value = tankName;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 10].Value = tankName;
                }
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("BatchId", batchId);
                worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    worksheet.Cells[i, 6].Value = batchId;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 5].Value = batchId;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 11].Value = batchId;
                }
            }

            package.Save();
            package.Dispose();
        }

        [Given(@"I update the excel ""(.*)"" but same '(.*)' and '(.*)'")]
        public void GivenIUpdateTheExcelButSameAnd(string fileName, string field1, string field2)
        {
            this.GivenIUpdateTheExcelButSameTwoFields(fileName, field1, field2);
        }

        [Given(@"I update the excel ""(.*)"" with same ""(.*)"" for inventory, attributes but without owners")]
        public void GivenIUpdateTheExcelWithSameForInventoryAttributesButWithoutOwners(string fileName, string field)
        {
            this.InventoryIdAndInventoryDateUpdate(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in Excel
            if (field.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("TankName", tankName);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 5].Value = tankName;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 10].Value = tankName;
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("BatchId", batchId);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 6].Value = batchId;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 11].Value = batchId;
            }

            package.Save();
            package.Dispose();
        }

        [Given(@"I update the excel ""(.*)"" with same ""(.*)"" for inventory, owners but without attributes")]
        public void GivenIUpdateTheExcelWithSameForInventoryOwnersButWithoutAttributes(string fileName, string field)
        {
            this.InventoryIdAndInventoryDateUpdate(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in Excel
            if (field.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("TankName", tankName);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 5].Value = tankName;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 4].Value = tankName;
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("BatchId", batchId);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 6].Value = batchId;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 5].Value = batchId;
            }

            package.Save();
            package.Dispose();
        }

        [Given(@"I update the excel ""(.*)"" with same ""(.*)"" for inventory but without attributes and owners")]
        public void GivenIUpdateTheExcelWithSameForInventoryButWithoutAttributesAndOwners(string fileName, string field)
        {
            this.InventoryIdAndInventoryDateUpdate(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in Excel
            if (field.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("TankName", tankName);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 5].Value = tankName;
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                this.SetValue("BatchId", batchId);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 6].Value = batchId;
            }

            package.Save();
            package.Dispose();
        }

        [Then(@"validate ""(.*)"" and ""(.*)"" is updated as ""(.*)""")]
        public async Task ThenValidateAndIsUpdatedAsAsync(string field1, string field2, string value)
        {
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            int count = this.ActionTypeRowCounts(this.GetValue("ActionType"));
            if (value.EqualsIgnoreCase("Null"))
            {
                for (int i = 0; i < count; i++)
                {
                    Assert.IsTrue(string.IsNullOrEmpty(lastCreatedInventoryList[i][field1].ToString()));
                    Assert.IsTrue(string.IsNullOrEmpty(lastCreatedInventoryList[i][field2].ToString()));
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Assert.IsTrue(value.EqualsIgnoreCase(lastCreatedInventoryList[0][field1].ToString()));
                    Assert.IsTrue(value.EqualsIgnoreCase(lastCreatedInventoryList[0][field2].ToString()));
                }
            }
        }

        [Then(@"validate ""(.*)"" and '(.*)' is updated")]
        public async Task ThenValidateAndIsUpdatedAsync(string field1, string field2)
        {
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            int count = this.ActionTypeRowCounts(this.GetValue("ActionType"));
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(this.GetValue(field1).EqualsIgnoreCase(lastCreatedInventoryList[i][field1].ToString()));
                Assert.IsTrue(this.GetValue(field2).EqualsIgnoreCase(lastCreatedInventoryList[i][field2].ToString()));
            }
        }

        [StepDefinition(@"validate ""(.*)"" is inserted")]
        public async Task ThenValidateIsInsertedAsync(string field)
        {
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            int count = this.ActionTypeRowCounts(this.GetValue("ActionType"));
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(this.GetValue(field).EqualsIgnoreCase(lastCreatedInventoryList[i][field].ToString()));
            }
        }

        [Then(@"validate ""(.*)"" is inserted as ""(.*)""")]
        public async Task ThenValidateIsInsertedAsAsync(string field, string value)
        {
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            int count = this.ActionTypeRowCounts(this.GetValue("ActionType"));
            for (int i = 0; i < count; i++)
            {
                if (value.EqualsIgnoreCase("Null"))
                {
                    Assert.IsTrue(string.IsNullOrEmpty(lastCreatedInventoryList[i][field].ToString()));
                }
                else
                {
                    Assert.IsTrue(value.EqualsIgnoreCase(lastCreatedInventoryList[i][field].ToString()));
                }
            }
        }

        [Then(@"validate (.*) same products with different ""(.*)""")]
        public async Task ThenValidateSameProductsWithDifferentAsync(int totalProducts, string field)
        {
            Assert.IsNotNull(totalProducts);
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            if (this.GetValue("ActionType") == "Insert")
            {
                Assert.IsTrue(lastCreatedInventoryList[1]["ProductId"].ToString().EqualsIgnoreCase(lastCreatedInventoryList[0]["ProductId"].ToString()));
                Assert.IsFalse(lastCreatedInventoryList[1][field].ToString().EqualsIgnoreCase(lastCreatedInventoryList[0][field].ToString()));
            }
        }

        [Then(@"validate (.*) different products with same ""(.*)""")]
        public async Task ThenValidateDifferentProductsWithSameAsync(int totalProducts, string field)
        {
            Assert.IsNotNull(totalProducts);
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            if (this.GetValue("ActionType") == "Insert")
            {
                Assert.IsFalse(lastCreatedInventoryList[1]["ProductId"].ToString().EqualsIgnoreCase(lastCreatedInventoryList[0]["ProductId"].ToString()));
                Assert.IsTrue(lastCreatedInventoryList[1][field].ToString().EqualsIgnoreCase(lastCreatedInventoryList[0][field].ToString()));
            }
        }

        [Then(@"validate (.*) different products and ""(.*)""")]
        public async Task ThenValidateDifferentProductsAndAsync(int totalProducts, string field)
        {
            Assert.IsNotNull(totalProducts);
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            if (this.GetValue("ActionType") == "Insert")
            {
                Assert.IsFalse(lastCreatedInventoryList[1]["ProductId"].ToString().EqualsIgnoreCase(lastCreatedInventoryList[0]["ProductId"].ToString()));
                Assert.IsFalse(lastCreatedInventoryList[1][field].ToString().EqualsIgnoreCase(lastCreatedInventoryList[0][field].ToString()));
            }
        }

        [Then(@"validate (.*) different products with same '(.*)' and '(.*)'")]
        public async Task ThenValidateDifferentProductsWithSameAndAsync(int totalProducts, string field1, string field2)
        {
            Assert.IsNotNull(totalProducts);
            var lastCreatedRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedRows.ToDictionaryList();
            int count = this.ActionTypeRowCounts(this.GetValue("ActionType"));
            for (int i = 0; i < count * totalProducts; i += 2)
            {
                Assert.IsFalse(lastCreatedInventoryList[i + 1]["ProductId"].ToString().EqualsIgnoreCase(lastCreatedInventoryList[i]["ProductId"].ToString()));
                Assert.IsTrue(lastCreatedInventoryList[i + 1][field1].ToString().EqualsIgnoreCase(lastCreatedInventoryList[i][field1].ToString()));
                Assert.IsTrue(lastCreatedInventoryList[i + 1][field2].ToString().EqualsIgnoreCase(lastCreatedInventoryList[i][field2].ToString()));
            }
        }

        [Then(@"validate ""(.*)"" and ""(.*)"" details are inserted")]
        public async Task ThenValidateAndDetailsAreInsertedAsync(string entity1, string entity2)
        {
            Assert.IsNotNull(entity1);
            Assert.IsNotNull(entity2);
            var lastCreatedInventoryRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedInventoryRows.ToDictionaryList();
            var lastCreatedAttributeRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["AttributeByInventoryProductId"], args: new { inventoryProductId = lastCreatedInventoryList[0]["InventoryProductId"].ToString() }).ConfigureAwait(false);
            var lastCreatedAttributeList = lastCreatedAttributeRows.ToDictionaryList();
            var lastCreatedOwnerRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["OwnerByInventoryProductId"], args: new { inventoryProductId = lastCreatedInventoryList[0]["InventoryProductId"].ToString() }).ConfigureAwait(false);
            var lastCreatedOwnerList = lastCreatedAttributeRows.ToDictionaryList();
            Assert.IsTrue(lastCreatedAttributeList.Count.Equals(1));
            Assert.IsTrue(lastCreatedOwnerList.Count.Equals(1));
        }

        [Then(@"validate ""(.*)"" details is inserted")]
        public async Task ThenValidateDetailsIsInsertedAsync(string entity)
        {
            var lastCreatedInventoryRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedInventoryRows.ToDictionaryList();
            var lastCreatedEntityRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow[entity + "ByInventoryProductId"], args: new { inventoryProductId = lastCreatedInventoryList[0]["InventoryProductId"].ToString() }).ConfigureAwait(false);
            var lastCreatedEntityList = lastCreatedEntityRows.ToDictionaryList();
            Assert.IsTrue(lastCreatedEntityList.Count.Equals(1));
        }

        [Then(@"validate ""(.*)"" details is not inserted")]
        public async Task ThenValidateDetailsIsNotInsertedAsync(string entity)
        {
            var lastCreatedInventoryRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedInventoryRows.ToDictionaryList();
            var lastCreatedEntityRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow[entity + "ByInventoryProductId"], args: new { inventoryProductId = lastCreatedInventoryList[0]["InventoryProductId"].ToString() }).ConfigureAwait(false);
            var lastCreatedEntityList = lastCreatedEntityRows.ToDictionaryList();
            Assert.IsTrue(lastCreatedEntityList.Count.Equals(0));
        }

        [Then(@"validate ""(.*)"", ""(.*)"" details are not inserted")]
        public async Task ThenValidateDetailsAreNotInsertedAsync(string entity1, string entity2)
        {
            Assert.IsNotNull(entity1);
            Assert.IsNotNull(entity2);
            var lastCreatedInventoryRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedInventoryRows.ToDictionaryList();
            var lastCreatedAttributeRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["AttributeByInventoryProductId"], args: new { inventoryProductId = lastCreatedInventoryList[0]["InventoryProductId"].ToString() }).ConfigureAwait(false);
            var lastCreatedAttributeList = lastCreatedAttributeRows.ToDictionaryList();
            var lastCreatedOwnerRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["OwnerByInventoryProductId"], args: new { inventoryProductId = lastCreatedInventoryList[0]["InventoryProductId"].ToString() }).ConfigureAwait(false);
            var lastCreatedOwnerList = lastCreatedAttributeRows.ToDictionaryList();
            Assert.IsTrue(lastCreatedAttributeList.Count.Equals(0));
            Assert.IsTrue(lastCreatedOwnerList.Count.Equals(0));
        }

        [Then(@"""(.*)"" should be registered with (.*) products in the system")]
        public async Task ThenShouldBeRegisteredWithProductsInTheSystemAsync(string entity, int numberofProducts)
        {
            if (entity.EqualsIgnoreCase("Inventory"))
            {
                var lastCreatedRow = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
                int count = this.ActionTypeRowCounts(this.GetValue("ActionType"));
                Assert.AreEqual(numberofProducts * count, lastCreatedRow.Count());
            }
        }

        [Then(@"validate ""(.*)"" is updated")]
        public async Task ThenValidateIsUpdatedAsync(string field)
        {
            var lastCreatedInventoryRows = await this.ReadAllSqlAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
            var lastCreatedInventoryList = lastCreatedInventoryRows.ToDictionaryList();
            var valueinDB = lastCreatedInventoryList[2][field].ToString();
            Assert.IsTrue(valueinDB.EqualsIgnoreCase("500"));
        }

        [When(@"I have (.*) inventory with ""(.*)"" that exceeds (.*) characters")]
        public void WhenIHaveInventoryWithThatExceedsCharacters(int count, string field, int limit)
        {
            JArray inventoryArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < count; i++)
            {
                content = this.UpdateDefaultValuesForInventoryJson(content);

                var fieldValue = new Faker().Random.AlphaNumeric(limit + 1);
                this.SetValue(field, fieldValue);
                content = content.JsonChangePropertyValue(field, fieldValue);

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I have (.*) inventory with (.*) same product but different ""(.*)""")]
        public void WhenIHaveInventoryWithSameProductButDifferent(int count, int productCount, string field)
        {
            JArray inventoryArray = new JArray();
            string content = ApiContent.Creates["InventoriesWithTwoSameProduct"];
            for (int i = 0; i < count; i++)
            {
                content = this.UpdateDefaultValuesForInventoryJson(content);

                for (int j = 0; j < productCount; j++)
                {
                    var fieldValue = new Faker().Random.AlphaNumeric(10);
                    this.SetValue(field + (j + 1), fieldValue);
                    content = content.JsonChangePropertyValue("Product_" + (j + 1) + " " + field, fieldValue);
                }

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I have (.*) inventory with (.*) different products and ""(.*)""")]
        public void WhenIHaveInventoryWithDifferentProductsAnd(int count, int productCount, string field)
        {
            JArray inventoryArray = new JArray();
            string content = ApiContent.Creates["InventoriesWithMultipleProducts"];
            for (int i = 0; i < count; i++)
            {
                content = this.UpdateDefaultValuesForInventoryJson(content);

                for (int j = 0; j < productCount; j++)
                {
                    var fieldValue = new Faker().Random.AlphaNumeric(10);
                    this.SetValue(field + (j + 1), fieldValue);
                    content = content.JsonChangePropertyValue("Product_" + (j + 1) + " " + field, fieldValue);
                }

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I have (.*) inventory with (.*) different products but same ""(.*)""")]
        public void WhenIHaveInventoryWithDifferentProductsButSame(int count, int productCount, string field)
        {
            JArray inventoryArray = new JArray();
            string content = ApiContent.Creates["InventoriesWithMultipleProducts"];
            for (int i = 0; i < count; i++)
            {
                content = this.UpdateDefaultValuesForInventoryJson(content);

                var fieldValue = new Faker().Random.AlphaNumeric(10);
                this.SetValue(field, fieldValue);
                for (int j = 0; j < productCount; j++)
                {
                    content = content.JsonChangePropertyValue("Product_" + (j + 1) + " " + field, fieldValue);
                }

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I have (.*) inventory with ""(.*)""")]
        public void WhenIHaveInventoryWith(int count, string field)
        {
            JArray inventoryArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < count; i++)
            {
                content = this.UpdateDefaultValuesForInventoryJson(content);

                var fieldValue = new Faker().Random.AlphaNumeric(10);
                this.SetValue(field, fieldValue);
                content = content.JsonChangePropertyValue(field, fieldValue);

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
                this.SetValue("InventoryStringContent", content);
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I have (.*) inventory without ""(.*)""")]
        public void WhenIHaveInventoryWithout(int count, string field)
        {
            JArray inventoryArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();
            for (int i = 0; i < count; i++)
            {
                content = this.UpdateDefaultValuesForInventoryJson(content);

                content = content.JsonChangePropertyValue(field, string.Empty);

                inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
                this.SetValue("InventoryStringContent", content);
            }

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [Given(@"I have (.*) inventory with ""(.*)"" in the system")]
        public async Task GivenIHaveInventoryWithInTheSystemAsync(int count, string field)
        {
            this.WhenIHaveInventoryWith(count, field);
            ////this.When("I register \"Inventories\" in system");
            await this.IRegisterInventoriesOrMovementsInSystemThroughSappoAsync("Inventories").ConfigureAwait(false);
            ////this.Then("response should be successful");
            await this.ResponseShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [Given(@"I have (.*) inventory without ""(.*)"" in the system")]
        public async Task GivenIHaveInventoryWithoutInTheSystemAsync(int count, string field)
        {
            this.WhenIHaveInventoryWithout(count, field);
            ////this.When("I register \"Inventories\" in system");
            await this.IRegisterInventoriesOrMovementsInSystemThroughSappoAsync("Inventories").ConfigureAwait(false);
            ////this.Then("response should be successful");
            await this.ResponseShouldBeSuccessfulAsync().ConfigureAwait(false);
        }

        [When(@"I update ""(.*)"" as ""(.*)""")]
        public void WhenIUpdateAs(string field, string value)
        {
            this.SetValue(field, value);
            string content = this.GetValue("InventoryStringContent");
            content = content.JsonChangePropertyValue(field, value);
            JArray inventoryArray = new JArray();
            inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I update ""(.*)"" as ""(.*)"" with existing '(.*)'")]
        public void WhenIUpdateAsWithExisting(string field1, string value1, string field2)
        {
            Assert.IsNotNull(field2);
            this.SetValue(field1, value1);
            string content = this.GetValue("InventoryStringContent");
            content = content.JsonChangePropertyValue(field1, value1);
            JArray inventoryArray = new JArray();
            inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));
            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        [When(@"I update ""(.*)"" as ""(.*)"" with ""(.*)"" exceeds (.*) characters")]
        public void WhenIUpdateAsWithExceedsCharacters(string field1, string value1, string field2, int limit)
        {
            JArray inventoryArray = new JArray();
            string content = this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)].ToString();

            content = this.UpdateDefaultValuesForInventoryJson(content);
            var fieldValue = new Faker().Random.AlphaNumeric(limit + 1);
            this.SetValue(field2, fieldValue);
            content = content.JsonChangePropertyValue(field2, fieldValue);
            content = content.JsonChangePropertyValue(field1, value1);

            inventoryArray.Add(JsonConvert.DeserializeObject<JObject>(content));

            this.ScenarioContext[this.GetValue(Entities.Keys.EntityType)] = inventoryArray;
        }

        public string UpdateDefaultValuesForInventoryJson(string content)
        {
            content = content.JsonChangePropertyValue("scenarioId", 3);
            content = content.JsonChangePropertyValue("ProductId batchId", this.GetValue("BatchId"));
            this.SetValue(ConstantValues.InventoryId, new Faker().Random.Number(19999, 999999).ToString(CultureInfo.InvariantCulture));
            content = content.JsonChangePropertyValue("inventoryId", this.GetValue(ConstantValues.InventoryId));
            content = content.JsonChangePropertyValue(ConstantValues.InventoryDate, DateTime.Now.AddDays(-2).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture));
            if (this.GetValue(Entities.Keys.EntityType).ContainsIgnoreCase("Homologated") || this.GetValue("shouldHomologate").EqualsIgnoreCase("false"))
            {
                content = content.JsonChangePropertyValue("nodeId", this.GetValue("NodeId_1"));
                content = content.JsonChangePropertyValue("system", this.GetValue("SystemElementId"));
                content = content.JsonChangePropertyValue("segmentId", this.GetValue("SegmentId"));
                content = content.JsonChangePropertyValue("operatorId", this.GetValue("OperatorId"));
                content = content.JsonChangePropertyValue("ProductId productId", "10000002318");
                content = content.JsonChangePropertyValue("ProductId productType", this.GetValue("ProductTypeId"));
                content = content.JsonChangePropertyValue("ProductId measurementUnit", 31);
                content = content.JsonChangePropertyValue("Attribute attributeId", this.GetValue("AttributeId"));
                content = content.JsonChangePropertyValue("Attribute valueAttributeUnit", this.GetValue("ValueAttributeUnitId"));
                content = content.JsonChangePropertyValue("OwnerId ownerId", this.GetValue("Owner"));
            }

            return content;
        }

        [Given(@"I have ""(.*)"" flag set as '(.*)'")]
        public void GivenIHaveFlagSetAs(string field, string value)
        {
            this.SetValue(field, value);
        }

        [StepDefinition(@"validate ""(.*)"" should be ""(.*)"" in the system")]
        public async Task WhenValidateShouldBeInTheSystemAsync(string entity, string actionType)
        {
            if (entity.EqualsIgnoreCase("Inventory"))
            {
                var lastCreatedRow = await this.ReadAllSqlAsDictionaryAsync(input: ApiContent.GetRow["InventoryById"], args: new { inventoryId = this.GetValue("InventoryId") }).ConfigureAwait(false);
                if (actionType.EqualsIgnoreCase("Insert"))
                {
                    Assert.AreEqual(1, lastCreatedRow.Count());
                }
                else if (actionType.EqualsIgnoreCase("Update"))
                {
                    if (string.IsNullOrEmpty(this.GetValue("ActionTypeCount")))
                    {
                        Assert.AreEqual(3, lastCreatedRow.Count());
                        this.SetValue("ActionTypeCount", "3");
                    }
                    else
                    {
                        Assert.AreEqual(int.Parse(this.GetValue("ActionTypeCount"), CultureInfo.InvariantCulture) + 2, lastCreatedRow.Count());
                    }
                }
                else if (actionType.EqualsIgnoreCase("Delete"))
                {
                    if (string.IsNullOrEmpty(this.GetValue("ActionTypeCount")))
                    {
                        Assert.AreEqual(2, lastCreatedRow.Count());
                        this.SetValue("ActionTypeCount", "2");
                    }
                    else
                    {
                        Assert.AreEqual(int.Parse(this.GetValue("ActionTypeCount"), CultureInfo.InvariantCulture) + 1, lastCreatedRow.Count());
                    }
                }
            }
        }

        public int ActionTypeRowCounts(string actionType)
        {
            int count = 0;
            if (actionType.EqualsIgnoreCase("Insert"))
            {
                count = 1;
            }
            else if (actionType.EqualsIgnoreCase("Update"))
            {
                count = 3;
            }
            else if (actionType.EqualsIgnoreCase("Delete"))
            {
                count = 2;
            }

            return count;
        }

        public void InventoryIdAndInventoryDateUpdate(string fileName)
        {
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // Inventory id update
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            var id = "AUTOMATION " + new Faker().Random.Number(9999, 999999);
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[i, 2].Value = id;
                this.SetValue("InventoryId", id);
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[i, 1].Value = id;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            // Inventory date update
            worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1);
            }

            for (int j = 1; j <= 2; j++)
            {
                worksheet = package.Workbook.Worksheets[j];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet.Cells[i, 2].Value = DateTime.UtcNow.AddDays(-1);
                }
            }

            package.Save();
            package.Dispose();
        }
    }
}
