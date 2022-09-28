// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExcelExtensions.cs" company="Microsoft">
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;

    using Bogus;

    using Ecp.True.Bdd.Tests.Entities;

    using global::Bdd.Core.StepDefinitions;
    using global::Bdd.Core.Utils;

    using NUnit.Framework;

    using OfficeOpenXml;

    public static class ExcelExtensions
    {
        public static void ChangeValue(string fileName, string sheetNumber, string oldValue, string newValue)
        {
            ExcelPackage package = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetNumber];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                for (int j = 1; j <= worksheet.Dimension.Columns; j++)
                {
#pragma warning disable CA1305 // Specify IFormatProvider
                    if (oldValue.ToString() == worksheet.Cells[i, j].Value.ToString())
#pragma warning restore CA1305 // Specify IFormatProvider
                    {
                        worksheet.Cells[i, j].Value = newValue;
                        break;
                    }
                }
            }

            package.Save();
            Assert.IsTrue(true);
            package.Dispose();
        }

        public static void InventoryIdAndInventoryDateUpdateInExcel(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // Inventory id update
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            var id = "AUTOMATION " + new Faker().Random.Number(9999, 999999);
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["InventoryId"] = id;
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

        public static void GivenIUpdateTheExcelWithBothTankNameAndBatchId(this StepDefinitionBase step, string fileName, string field1, string field2)
        {
            step.ThrowIfNull(nameof(step));
            step.InventoryIdAndInventoryDateUpdateInExcel(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in Excel
            if (field1.EqualsIgnoreCase("TankName") || field2.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["TankName"] = tankName;
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 5].Value = tankName;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 4].Value = tankName;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 10].Value = tankName;
            }

            if (field1.EqualsIgnoreCase("BatchId") || field2.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["BatchId"] = batchId;
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 6].Value = batchId;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 5].Value = batchId;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 11].Value = batchId;
            }

            package.Save();
            package.Dispose();
        }

        public static void GivenIUpdateTheExcelWithOnlyTankName(this StepDefinitionBase step, string fileName, string field)
        {
            step.ThrowIfNull(nameof(step));
            if (string.IsNullOrEmpty(step.GetValueInternal("InventoryId")))
            {
                step.InventoryIdAndInventoryDateUpdateInExcel(fileName);
            }

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in Excel
            if (field.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["TankName"] = tankName;
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 5].Value = tankName;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 4].Value = tankName;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 10].Value = tankName;
            }
            else if (field.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["BatchId"] = batchId;
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 6].Value = batchId;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 5].Value = batchId;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 11].Value = batchId;
            }
            else if (field.EqualsIgnoreCase("ProductVolume"))
            {
                var volume = 500.00;
                step.ScenarioContext["ProductVolume"] = volume;
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 12].Value = volume;
            }

            package.Save();
            package.Dispose();
        }

        public static void GivenIUpdateTheExcelButSameTwoFields(this StepDefinitionBase step, string fileName, string field1, string field2)
        {
            step.ThrowIfNull(nameof(step));
            step.InventoryIdAndInventoryDateUpdateInExcel(fileName);

            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            ExcelWorksheet worksheet;

            // Updating field value in excel
            if (field1.EqualsIgnoreCase("TankName") || field2.EqualsIgnoreCase("TankName"))
            {
                var tankName = "Tank_ " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["TankName"] = tankName;
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

            if (field1.EqualsIgnoreCase("BatchId") || field2.EqualsIgnoreCase("BatchId"))
            {
                var batchId = "Batch_ " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["BatchId"] = batchId;
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

        public static void WhenIUpdateTheExcelFileWithDaywiseData(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "DEFECT " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["InventoryId"] = id;
                step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
                if (fileName != "TestData_WithInitialInventoryWithoutOwnersMovements")
                {
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                }
            }

            if (fileName == "TestDataCutOff_Daywise"
                || fileName == "TestData_logistic"
                || fileName == "TestData_AnalyticalModel")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[4, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[5, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[6, 3].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[7, 3].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[8, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[9, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[10, 3].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[11, 3].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[12, 3].Value = DateTime.UtcNow;
                worksheet.Cells[13, 3].Value = DateTime.UtcNow;
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[4, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[5, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[6, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[7, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[8, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[9, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[10, 2].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[11, 2].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[12, 2].Value = DateTime.UtcNow;
                    worksheet.Cells[13, 2].Value = DateTime.UtcNow;
                }
            }
            else if (fileName == "TestData_NodeStatus")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-10);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-10);
                worksheet.Cells[4, 3].Value = DateTime.UtcNow.AddDays(-10);
                worksheet.Cells[5, 3].Value = DateTime.UtcNow.AddDays(-10);
                worksheet.Cells[6, 3].Value = DateTime.UtcNow.AddDays(-9);
                worksheet.Cells[7, 3].Value = DateTime.UtcNow.AddDays(-9);
                worksheet.Cells[8, 3].Value = DateTime.UtcNow.AddDays(-8);
                worksheet.Cells[9, 3].Value = DateTime.UtcNow.AddDays(-8);
                worksheet.Cells[10, 3].Value = DateTime.UtcNow.AddDays(-7);
                worksheet.Cells[11, 3].Value = DateTime.UtcNow.AddDays(-7);
                worksheet.Cells[12, 3].Value = DateTime.UtcNow.AddDays(-6);
                worksheet.Cells[13, 3].Value = DateTime.UtcNow.AddDays(-6);
                worksheet.Cells[14, 3].Value = DateTime.UtcNow.AddDays(-5);
                worksheet.Cells[15, 3].Value = DateTime.UtcNow.AddDays(-5);
                worksheet.Cells[16, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[17, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[18, 3].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[19, 3].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[20, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[21, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[22, 3].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[23, 3].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[24, 3].Value = DateTime.UtcNow;
                worksheet.Cells[25, 3].Value = DateTime.UtcNow;
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-10);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-10);
                    worksheet.Cells[4, 2].Value = DateTime.UtcNow.AddDays(-10);
                    worksheet.Cells[5, 2].Value = DateTime.UtcNow.AddDays(-10);
                    worksheet.Cells[6, 2].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[7, 2].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[8, 2].Value = DateTime.UtcNow.AddDays(-8);
                    worksheet.Cells[9, 2].Value = DateTime.UtcNow.AddDays(-8);
                    worksheet.Cells[10, 2].Value = DateTime.UtcNow.AddDays(-7);
                    worksheet.Cells[11, 2].Value = DateTime.UtcNow.AddDays(-7);
                    worksheet.Cells[12, 2].Value = DateTime.UtcNow.AddDays(-6);
                    worksheet.Cells[13, 2].Value = DateTime.UtcNow.AddDays(-6);
                    worksheet.Cells[14, 2].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[15, 2].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[16, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[17, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[18, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[19, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[20, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[21, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[22, 2].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[23, 2].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[24, 2].Value = DateTime.UtcNow;
                    worksheet.Cells[25, 2].Value = DateTime.UtcNow;
                }
            }
            else if (fileName == "SingleDay")
            {
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[4, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[5, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[6, 3].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[7, 3].Value = DateTime.UtcNow.AddDays(-1);
            }
            else if (fileName == "Testdata_20215")
            {
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-2).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[2, 11].Value = string.Empty;
            }
            else if (fileName == "Testdata_20215_0Volume")
            {
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-2).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
            }
            else if (fileName == "TestDataCutOff_singleInventory")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-2);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-2);
                }
            }
            else if (fileName == "SingleInventory_DateLessThanNodeDate")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-31);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-31);
                }
            }
            else if (fileName == "TestData_InitialCutOff")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-5);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-5);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-5);
                }
            }
            else if (fileName == "TestData_WithInitialInventoryWithoutOwnersMovements")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-4);
            }
            else if (fileName == "TestData_WithInitialInventoryWithOwnersMovements")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-4);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-4);
                }
            }
            else if (fileName == "BatchIdTankNameValidation")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-1);
                }
            }
            else if (fileName == "TestData_TransferPointMovements")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-3);
            }
            else if (fileName == "TestData_Ownership")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-1);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-2);
                }
            }
            else if (fileName == "TestData_FICO")
            {
                worksheet = package.Workbook.Worksheets[0];
                for (int z = 2; z <= 41; z++)
                {
                    if (z < 10)
                    {
                        worksheet.Cells[z, 3].Value = DateTime.UtcNow.AddDays(-5);
                    }
                    else if (z >= 10 && z < 18)
                    {
                        worksheet.Cells[z, 3].Value = DateTime.UtcNow.AddDays(-4);
                    }
                    else if (z >= 18 && z < 26)
                    {
                        worksheet.Cells[z, 3].Value = DateTime.UtcNow.AddDays(-3);
                    }
                    else if (z >= 26 && z < 34)
                    {
                        worksheet.Cells[z, 3].Value = DateTime.UtcNow.AddDays(-2);
                    }
                    else
                    {
                        worksheet.Cells[z, 3].Value = DateTime.UtcNow.AddDays(-1);
                    }
                }

                for (int y = 1; y <= 2; y++)
                {
                    worksheet = package.Workbook.Worksheets[y];
                    for (int z = 2; z <= 41; z++)
                    {
                        if (z < 10)
                        {
                            worksheet.Cells[z, 2].Value = DateTime.UtcNow.AddDays(-5);
                        }
                        else if (z >= 10 && z < 18)
                        {
                            worksheet.Cells[z, 2].Value = DateTime.UtcNow.AddDays(-4);
                        }
                        else if (z >= 18 && z < 26)
                        {
                            worksheet.Cells[z, 2].Value = DateTime.UtcNow.AddDays(-3);
                        }
                        else if (z >= 26 && z < 34)
                        {
                            worksheet.Cells[z, 2].Value = DateTime.UtcNow.AddDays(-2);
                        }
                        else
                        {
                            worksheet.Cells[z, 2].Value = DateTime.UtcNow.AddDays(-1);
                        }
                    }
                }
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                if (fileName != "TestData_WithInitialInventoryWithoutOwnersMovements")
                {
                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = id;
                }
            }
            //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

            if (fileName == "TestDataCutOff_Daywise"
                || fileName == "TestData_logistic"
                || fileName == "TestData_AnalyticalModel"
                || fileName == "TestData_FICO")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[4, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[5, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[6, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[7, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[8, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[9, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[10, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[11, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[12, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[13, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[14, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[15, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[16, j].Value = DateTime.UtcNow.AddDays(-1);
                }
            }
            else if (fileName == "TestData_Ownership")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-1);
                }
            }
            else if (fileName == "TestData_NodeStatus")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[4, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[5, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[6, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[7, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[8, j].Value = DateTime.UtcNow.AddDays(-9);
                    worksheet.Cells[9, j].Value = DateTime.UtcNow.AddDays(-8);
                    worksheet.Cells[10, j].Value = DateTime.UtcNow.AddDays(-8);
                    worksheet.Cells[11, j].Value = DateTime.UtcNow.AddDays(-8);
                    worksheet.Cells[12, j].Value = DateTime.UtcNow.AddDays(-8);
                    worksheet.Cells[13, j].Value = DateTime.UtcNow.AddDays(-7);
                    worksheet.Cells[14, j].Value = DateTime.UtcNow.AddDays(-7);
                    worksheet.Cells[15, j].Value = DateTime.UtcNow.AddDays(-7);
                    worksheet.Cells[16, j].Value = DateTime.UtcNow.AddDays(-7);
                    worksheet.Cells[17, j].Value = DateTime.UtcNow.AddDays(-6);
                    worksheet.Cells[18, j].Value = DateTime.UtcNow.AddDays(-6);
                    worksheet.Cells[19, j].Value = DateTime.UtcNow.AddDays(-6);
                    worksheet.Cells[20, j].Value = DateTime.UtcNow.AddDays(-6);
                    worksheet.Cells[21, j].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[22, j].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[23, j].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[24, j].Value = DateTime.UtcNow.AddDays(-5);
                    worksheet.Cells[25, j].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[26, j].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[27, j].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[28, j].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[29, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[30, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[31, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[32, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[33, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[34, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[35, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[36, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[37, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[38, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[39, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[40, j].Value = DateTime.UtcNow.AddDays(-1);
                }
            }
            else if (fileName == "SingleDay")
            {
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[4, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[5, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[6, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[7, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[8, j].Value = DateTime.UtcNow.AddDays(-1);
                }
            }
            else if (fileName == "Testdata_20215_0Volume")
            {
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 6].Value = DateTime.UtcNow.AddDays(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, 7].Value = DateTime.UtcNow.AddDays(-5).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
            }
            else if (fileName == "Testdata_20215")
            {
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[2, 14].Value = string.Empty;
            }
            else if (fileName == "TestDataCutOff_singleMovement")
            {
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-2);
            }
            else if (fileName == "SingleMovement_DateLessThanNodeDate")
            {
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-31);
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-31);
            }
            else if (fileName == "TestData_InitialCutOff")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-4);
                }
            }
            else if (fileName == "TestData_WithInitialInventoryWithoutOwnersMovements" || fileName == "TestData_WithInitialInventoryWithOwnersMovements" || fileName == "TestData_WithoutInitialInventoryMovements" || fileName == "TestData_WithInitialInventoryWithOwnersMovements")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-3);
                }
            }
            else if (fileName == "TestData_TransferPointMovements")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[4, j].Value = DateTime.UtcNow.AddDays(-1);
                }
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileWithOfficialDeltaConsolidationData(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            List<string> inventoryIds = new List<string>();
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "Official_Inventory " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                inventoryIds.Add(id);
                step.ScenarioContext["InventoryId"] = id;
                step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;

                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            int indexOfCurrentInventoryId = 0;
            int updatedRows = 1;
            worksheet = package.Workbook.Worksheets[1];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                if (updatedRows > 6)
                {
                    if (i % 2 == 0)
                    {
                        worksheet.Cells[i, 1].Value = inventoryIds[indexOfCurrentInventoryId];
                        worksheet.Cells[i + 1, 1].Value = inventoryIds[indexOfCurrentInventoryId];
                        indexOfCurrentInventoryId++;
                    }
                }
                else
                {
                    worksheet.Cells[i, 1].Value = inventoryIds[indexOfCurrentInventoryId];
                    indexOfCurrentInventoryId++;
                }

                updatedRows++;
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            List<string> movementIds = new List<string>();
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = "Official_Movement " + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                movementIds.Add(id);
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;

                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[i, 1].Value = id;
            }

            int indexOfCurrentMovementId = 0;
            int percentageRows = 1;
            worksheet = package.Workbook.Worksheets[4];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                if (percentageRows > 8)
                {
                    worksheet.Cells[i, 1].Value = movementIds[indexOfCurrentMovementId];
                    indexOfCurrentMovementId++;
                }
                else
                {
                    if (i % 2 == 0)
                    {
                        worksheet.Cells[i, 1].Value = movementIds[indexOfCurrentMovementId];
                        worksheet.Cells[i + 1, 1].Value = movementIds[indexOfCurrentMovementId];
                        indexOfCurrentMovementId++;
                    }
                }

                percentageRows++;
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileWithOfficialDeltaData(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            List<string> inventoryIds = new List<string>();
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "Official_Inventory " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                inventoryIds.Add(id);
                step.ScenarioContext["InventoryId"] = id;
                step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;

                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            int indexOfCurrentInventoryId = 0;
            worksheet = package.Workbook.Worksheets[1];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                if (i % 2 == 0)
                {
                    worksheet.Cells[i, 1].Value = inventoryIds[indexOfCurrentInventoryId];
                    worksheet.Cells[i + 1, 1].Value = inventoryIds[indexOfCurrentInventoryId];
                    indexOfCurrentInventoryId++;
                }
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            List<string> movementIds = new List<string>();
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = "Official_Movement " + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                movementIds.Add(id);
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;

                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[i, 1].Value = id;
            }

            int indexOfCurrentMovementId = 0;
            worksheet = package.Workbook.Worksheets[4];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                if (i % 2 == 0)
                {
                    worksheet.Cells[i, 1].Value = movementIds[indexOfCurrentMovementId];
                    worksheet.Cells[i + 1, 1].Value = movementIds[indexOfCurrentMovementId];
                    indexOfCurrentMovementId++;
                }
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileWithConsolidationData(this StepDefinitionBase step, string fileName, int numberOfPreviousMonth = 1)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            ExcelWorksheet worksheet;
            if (string.IsNullOrEmpty(step.GetValueInternal("InventoryId1")))
            {
                DateTime date = DateTime.Now.AddMonths(-numberOfPreviousMonth);
                var firstDayOfConsolidationMonth = new DateTime(date.Year, date.Month, 1);
                var anotherPeriod = firstDayOfConsolidationMonth.AddDays(-1);
                var lastDayOfConsolidationMonth = firstDayOfConsolidationMonth.AddMonths(1).AddDays(-1);
                step.ScenarioContext["PreviousMonthStartDate"] = firstDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"] = firstDayOfConsolidationMonth.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
                step.ScenarioContext["PreviousMonthName"] = step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"].ToString().Split('-')[1].Trim('.');
                step.ScenarioContext["YearForPeriod"] = step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"].ToString().Split('-')[2];
                step.ScenarioContext["PreviousMonthEndDate"] = lastDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                step.ScenarioContext["BeforeOperativeDelta_CutoffEndDate"] = lastDayOfConsolidationMonth.AddDays(-5).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                step.ScenarioContext["AnotherPeriod_Consolidation"] = anotherPeriod.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                step.ScenarioContext["AnotherPeriodDetailsInColumbianCulture"] = anotherPeriod.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
                step.ScenarioContext["AnotherPeriodMonthName"] = step.ScenarioContext["AnotherPeriodDetailsInColumbianCulture"].ToString().Split('-')[1].Trim('.');
                step.ScenarioContext["YearForAnotherPeriod"] = step.ScenarioContext["AnotherPeriodDetailsInColumbianCulture"].ToString().Split('-')[2];

                //// For Inventories
                for (int i = 2; i <= 8; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = "Consolidated_Inventory " + new Faker().Random.Number(9999, 999999);
                    worksheet.Cells[i, 2].Value = id;
                    worksheet.Cells[i, 3].Value = lastDayOfConsolidationMonth;
                    step.ScenarioContext["InventoryId" + (i - 1)] = id;
                    step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;

                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet.Cells[i, 2].Value = lastDayOfConsolidationMonth;
                }

                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[7, 3].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[8, 3].Value = anotherPeriod;

                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[7, 2].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[8, 2].Value = anotherPeriod;

                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[12, 2].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[13, 2].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[14, 2].Value = anotherPeriod;
                worksheet.Cells[15, 2].Value = anotherPeriod;
                for (int i = 2; i <= 11; i++)
                {
                    worksheet.Cells[i, 2].Value = lastDayOfConsolidationMonth;
                }

                worksheet.Cells[2, 1].Value = step.ScenarioContext["InventoryId1"];
                worksheet.Cells[3, 1].Value = step.ScenarioContext["InventoryId1"];
                worksheet.Cells[4, 1].Value = step.ScenarioContext["InventoryId2"];
                worksheet.Cells[5, 1].Value = step.ScenarioContext["InventoryId2"];
                worksheet.Cells[6, 1].Value = step.ScenarioContext["InventoryId3"];
                worksheet.Cells[7, 1].Value = step.ScenarioContext["InventoryId3"];
                worksheet.Cells[8, 1].Value = step.ScenarioContext["InventoryId4"];
                worksheet.Cells[9, 1].Value = step.ScenarioContext["InventoryId4"];
                worksheet.Cells[10, 1].Value = step.ScenarioContext["InventoryId5"];
                worksheet.Cells[11, 1].Value = step.ScenarioContext["InventoryId5"];
                worksheet.Cells[12, 1].Value = step.ScenarioContext["InventoryId6"];
                worksheet.Cells[13, 1].Value = step.ScenarioContext["InventoryId6"];
                worksheet.Cells[14, 1].Value = step.ScenarioContext["InventoryId7"];
                worksheet.Cells[15, 1].Value = step.ScenarioContext["InventoryId7"];

                //// For Movements
                for (int i = 2; i <= 42; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    var id = "Consolidated_Movement " + new Faker().Random.Number(11111, 9999999);
                    worksheet.Cells[i, 2].Value = id;
                    step.ScenarioContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;
                    step.FeatureContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;

                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[4];
                worksheet.Cells[2, 1].Value = step.ScenarioContext["MovementId1"];
                worksheet.Cells[3, 1].Value = step.ScenarioContext["MovementId1"];
                worksheet.Cells[4, 1].Value = step.ScenarioContext["MovementId2"];
                worksheet.Cells[5, 1].Value = step.ScenarioContext["MovementId2"];
                worksheet.Cells[6, 1].Value = step.ScenarioContext["MovementId3"];
                worksheet.Cells[7, 1].Value = step.ScenarioContext["MovementId3"];
                worksheet.Cells[8, 1].Value = step.ScenarioContext["MovementId4"];
                worksheet.Cells[9, 1].Value = step.ScenarioContext["MovementId4"];
                worksheet.Cells[10, 1].Value = step.ScenarioContext["MovementId5"];
                worksheet.Cells[11, 1].Value = step.ScenarioContext["MovementId5"];
                worksheet.Cells[12, 1].Value = step.ScenarioContext["MovementId6"];
                worksheet.Cells[13, 1].Value = step.ScenarioContext["MovementId6"];
                worksheet.Cells[14, 1].Value = step.ScenarioContext["MovementId7"];
                worksheet.Cells[15, 1].Value = step.ScenarioContext["MovementId7"];
                worksheet.Cells[16, 1].Value = step.ScenarioContext["MovementId8"];
                worksheet.Cells[17, 1].Value = step.ScenarioContext["MovementId8"];
                worksheet.Cells[18, 1].Value = step.ScenarioContext["MovementId9"];
                worksheet.Cells[19, 1].Value = step.ScenarioContext["MovementId9"];
                worksheet.Cells[20, 1].Value = step.ScenarioContext["MovementId10"];
                worksheet.Cells[21, 1].Value = step.ScenarioContext["MovementId10"];

                for (int k = 22; k <= 52; k++)
                {
                    worksheet.Cells[k, 1].Value = step.ScenarioContext["MovementId" + (k - 11)];
                }

                //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

                worksheet = package.Workbook.Worksheets[3];
                worksheet.Cells[2, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[3, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[4, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[5, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[6, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[7, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[8, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[9, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[10, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[11, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[2, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[3, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[4, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[5, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[6, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[7, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[8, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[9, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[10, 7].Value = lastDayOfConsolidationMonth;
                worksheet.Cells[11, 7].Value = lastDayOfConsolidationMonth;

                for (int z = 12; z <= 42; z++)
                {
                    firstDayOfConsolidationMonth = firstDayOfConsolidationMonth.AddDays(1);
                    worksheet.Cells[z, 6].Value = firstDayOfConsolidationMonth;
                    worksheet.Cells[z, 7].Value = firstDayOfConsolidationMonth;

                    if (z > 40)
                    {
                        worksheet.Cells[z, 6].Value = anotherPeriod;
                        worksheet.Cells[z, 7].Value = anotherPeriod;
                    }
                }

                if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.MovementsWithoutOwners)))
                {
                    //// Deleting both Inventories and Movements rows form owners sheet
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.DeleteRow(2, 15);
                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.DeleteRow(2, 52);
                }
            }
            else
            {
                //// For Inventories
                worksheet = package.Workbook.Worksheets[0];
                //// Negative case
                worksheet.Cells[2, 12].Value = 50000; // old value - 1234567,89
                worksheet.Cells[3, 12].Value = 10000; // old value - 9876543,21
                //// Positive case
                worksheet.Cells[4, 12].Value = 9234567; // old value - 1234567,89
                worksheet.Cells[5, 12].Value = 9976543; // old value - 9876543,21

                //// For Movements
                worksheet = package.Workbook.Worksheets[3];
                //// Negative case
                worksheet.Cells[2, 15].Value = 50000; // old value - 442558,55
                worksheet.Cells[3, 15].Value = 10000; // old value - 432000,00
                worksheet.Cells[4, 15].Value = 10000; // old value - 442558,55
                //// Positive case
                worksheet.Cells[5, 15].Value = 542558; // old value - 442558,55
                worksheet.Cells[6, 15].Value = 532000; // old value - 432000,00
                worksheet.Cells[7, 15].Value = 546786; // old value - 432000,00
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileWithOfficialData(this StepDefinitionBase step, string fileName, int numberOfPreviousMonth = 1)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            ExcelWorksheet worksheet;
            DateTime date = DateTime.Now.AddMonths(-numberOfPreviousMonth);
            var firstDayOfConsolidationMonth = new DateTime(date.Year, date.Month, 1);
            var anotherPeriod = firstDayOfConsolidationMonth.AddDays(-1);
            var lastDayOfConsolidationMonth = firstDayOfConsolidationMonth.AddMonths(1).AddDays(-1);
            step.ScenarioContext["PreviousMonthStartDate"] = firstDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"] = firstDayOfConsolidationMonth.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            step.ScenarioContext["PreviousMonthName"] = step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"].ToString().Split('-')[1].Trim('.');
            step.ScenarioContext["YearForPeriod"] = step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"].ToString().Split('-')[2];
            step.ScenarioContext["PreviousMonthEndDate"] = lastDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["BeforeOperativeDelta_CutoffEndDate"] = lastDayOfConsolidationMonth.AddDays(-5).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["AnotherPeriod_Consolidation"] = anotherPeriod.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["AnotherPeriodDetailsInColumbianCulture"] = anotherPeriod.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            step.ScenarioContext["AnotherPeriodMonthName"] = step.ScenarioContext["AnotherPeriodDetailsInColumbianCulture"].ToString().Split('-')[1].Trim('.');
            step.ScenarioContext["YearForAnotherPeriod"] = step.ScenarioContext["AnotherPeriodDetailsInColumbianCulture"].ToString().Split('-')[2];

            //// For Inventories
            if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithoutAnnulation))
                || !string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForInvalidScenarioWithAnnulation)))
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "OfficialBalance_Inventory " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[2, 2].Value = id;

                step.ScenarioContext["InventoryId1"] = id;
                step.FeatureContext["Inventory1"] = worksheet.Cells[2, 2].Value;

                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 1].Value = id;

                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 1].Value = id;

                for (int i = 0; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    if (i == 0)
                    {
                        worksheet.Cells[2, 3].Value = lastDayOfConsolidationMonth;
                    }
                    else
                    {
                        worksheet.Cells[2, 2].Value = lastDayOfConsolidationMonth;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
            {
                for (int i = 2; i <= 3; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = "OfficialBalance_Inventory " + new Faker().Random.Number(9999, 999999);
                    worksheet.Cells[i, 2].Value = id;

                    step.ScenarioContext["InventoryId" + (i - 1)] = id;
                    step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;

                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = id;

                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                }

                for (int i = 0; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    if (i == 1)
                    {
                        worksheet.Cells[2, 3].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[3, 3].Value = lastDayOfConsolidationMonth;
                    }
                    else
                    {
                        worksheet.Cells[2, 2].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[3, 2].Value = lastDayOfConsolidationMonth;
                    }
                }
            }
            else
            {
                for (int i = 2; i <= 7; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = "OfficialBalance_Inventory " + new Faker().Random.Number(9999, 999999);
                    worksheet.Cells[i, 2].Value = id;

                    step.ScenarioContext["InventoryId" + (i - 1)] = id;
                    step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;

                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = id;

                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                }

                for (int i = 0; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    if (i == 0)
                    {
                        worksheet.Cells[2, 3].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[3, 3].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[4, 3].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[5, 3].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[6, 3].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[7, 3].Value = lastDayOfConsolidationMonth;
                    }
                    else
                    {
                        worksheet.Cells[2, 2].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[3, 2].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[4, 2].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[5, 2].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[6, 2].Value = lastDayOfConsolidationMonth;
                        worksheet.Cells[7, 2].Value = lastDayOfConsolidationMonth;
                    }
                }
            }

            //// For Movements
            if (!fileName.ContainsIgnoreCase("ErrorScenario"))
            {
                if (!string.IsNullOrEmpty(step.GetValueInternal(ConstantValues.TestdataForOfficialBalanceFileWithAnnulationAsPerRelationshipTable)))
                {
                    for (int i = 2; i <= 127; i++)
                    {
                        worksheet = package.Workbook.Worksheets[3];
                        var id = "Transformation_Movement " + new Faker().Random.Number(11111, 9999999);
                        worksheet.Cells[i, 2].Value = id;
                        step.ScenarioContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;
                        step.FeatureContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;

                        worksheet = package.Workbook.Worksheets[4];
                        worksheet.Cells[i, 1].Value = id;

                        worksheet = package.Workbook.Worksheets[5];
                        worksheet.Cells[i, 1].Value = id;
                    }

                    //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

                    worksheet = package.Workbook.Worksheets[3];
                    for (int z = 2; z <= 64; z++)
                    {
                        worksheet.Cells[z, 6].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[z, 7].Value = lastDayOfConsolidationMonth;
                    }

                    firstDayOfConsolidationMonth = firstDayOfConsolidationMonth.AddDays(20);
                    step.ScenarioContext["LogisticOfficialBalanceCutoffAndOwnershipDate"] = firstDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    for (int z = 65; z <= 127; z++)
                    {
                        worksheet.Cells[z, 6].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[z, 7].Value = firstDayOfConsolidationMonth;
                    }
                }
                else
                {
                    for (int i = 2; i <= 8; i++)
                    {
                        worksheet = package.Workbook.Worksheets[3];
                        var id = "OfficialBalance_Movement " + new Faker().Random.Number(11111, 9999999);
                        worksheet.Cells[i, 2].Value = id;
                        step.ScenarioContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;
                        step.FeatureContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;

                        worksheet = package.Workbook.Worksheets[4];
                        worksheet.Cells[i, 1].Value = id;

                        worksheet = package.Workbook.Worksheets[5];
                        worksheet.Cells[i, 1].Value = id;
                    }

                    //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

                    worksheet = package.Workbook.Worksheets[3];
                    for (int z = 2; z <= 8; z++)
                    {
                        worksheet.Cells[z, 6].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[z, 7].Value = lastDayOfConsolidationMonth;
                    }
                }

                if (fileName.EqualsIgnoreCase("TestData_OfficialBalanceFileWithAnnulation") || fileName.EqualsIgnoreCase("TestData_OfficialBalanceFileWithAnnulationCalculations"))
                {
                    firstDayOfConsolidationMonth = firstDayOfConsolidationMonth.AddDays(20);
                    step.ScenarioContext["LogisticOfficialBalanceCutoffAndOwnershipDate"] = firstDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                    for (int i = 9; i <= 15; i++)
                    {
                        worksheet = package.Workbook.Worksheets[3];
                        var id = "Consolidated_Movement " + new Faker().Random.Number(11111, 9999999);
                        worksheet.Cells[i, 2].Value = id;
                        step.ScenarioContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;
                        step.FeatureContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;

                        worksheet = package.Workbook.Worksheets[4];
                        worksheet.Cells[i, 1].Value = id;

                        worksheet = package.Workbook.Worksheets[5];
                        worksheet.Cells[i, 1].Value = id;
                    }

                    worksheet = package.Workbook.Worksheets[3];
                    for (int z = 9; z <= 15; z++)
                    {
                        worksheet.Cells[z, 6].Value = firstDayOfConsolidationMonth;
                        worksheet.Cells[z, 7].Value = firstDayOfConsolidationMonth;
                    }
                }
            }
            else if (fileName.EqualsIgnoreCase("ErrorScenarioTestData_OfficialBalanceFileWithAnnulation"))
            {
                for (int i = 2; i <= 3; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    var id = "Transformation_Movement " + new Faker().Random.Number(11111, 9999999);
                    worksheet.Cells[i, 2].Value = id;
                    step.ScenarioContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;
                    step.FeatureContext["MovementId" + (i - 1)] = worksheet.Cells[i, 2].Value;

                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.Cells[i, 1].Value = id;

                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = id;
                }

                //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

                worksheet = package.Workbook.Worksheets[3];
                worksheet.Cells[2, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[2, 7].Value = lastDayOfConsolidationMonth;

                firstDayOfConsolidationMonth = firstDayOfConsolidationMonth.AddDays(20);
                step.ScenarioContext["LogisticOfficialBalanceCutoffAndOwnershipDate"] = firstDayOfConsolidationMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
                worksheet.Cells[3, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[3, 7].Value = firstDayOfConsolidationMonth;
            }
            else if (fileName.EqualsIgnoreCase("ErrorScenarioTestData_OfficialBalanceFileWithoutAnnulation"))
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = "Transformation_Movement " + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[2, 2].Value = id;
                step.ScenarioContext["MovementId1"] = worksheet.Cells[2, 2].Value;
                step.FeatureContext["MovementId1"] = worksheet.Cells[2, 2].Value;

                worksheet = package.Workbook.Worksheets[4];
                worksheet.Cells[2, 1].Value = id;

                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[2, 1].Value = id;

                //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns
                worksheet = package.Workbook.Worksheets[3];
                worksheet.Cells[2, 6].Value = firstDayOfConsolidationMonth;
                worksheet.Cells[2, 7].Value = lastDayOfConsolidationMonth;
            }

            package.Save();
            package.Dispose();
        }

        public static void IUpdateTheExcelFile(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[i, 2].Value = "DEFECT " + new Faker().Random.Number(99999, 999999);
                ////this.FeatureContext.Add("Inventory" + (i - 1), worksheet.Cells[i, 2].Value);
            }

            worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
            worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-4).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
            worksheet.Cells[4, 3].Value = DateTime.UtcNow.AddDays(-4).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            worksheet.Cells[5, 3].Value = DateTime.UtcNow.AddDays(+2).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            worksheet.Cells[6, 3].Value = DateTime.UtcNow.AddDays(-6).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";

            // For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[i, 2].Value = new Faker().Random.Number(11111, 99999);
                ////this.FeatureContext.Add("Movement" + (i - 1), worksheet.Cells[i, 2].Value);
            }

            // Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns
            for (int j = 6; j <= 7; j++)
            {
                worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";
                worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-4).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:01";
                worksheet.Cells[4, j].Value = DateTime.UtcNow.AddDays(-4).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
                worksheet.Cells[5, j].Value = DateTime.UtcNow.AddDays(+2).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
                worksheet.Cells[6, j].Value = DateTime.UtcNow.AddDays(-6).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 00:00:00";
            }

            package.Save();
            package.Dispose();
        }

        public static async Task ExcelInformationOfEventsAsync(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));

            var actualEventsInformation = new Dictionary<string, string>()
            {
                { "Column1", "IdentificadorEvento" },
                { "Column2", "EventoPropiedad" },
                { "Column3", "IdNodoOrigen" },
                { "Column4", "NodoOrigen" },
                { "Column5", "IdNodoDestino" },
                { "Column6", "NodoDestino" },
                { "Column7", "IdProductoOrigen" },
                { "Column8", "ProductoOrigen" },
                { "Column9", "IdProductoDestino" },
                { "Column10", "ProductoDestino" },
                { "Column11", "FechaInicio" },
                { "Column12", "FechaFin" },
                { "Column13", "IdPropietario" },
                { "Column14", "Propietario" },
                { "Column15", "ValorDiario" },
                { "Column16", "Unidad" },
            };

            var eventIdForEvent = await step.ReadSqlAsDictionaryAsync(SqlQueries.GetTopProcessedEvent).ConfigureAwait(false);
            actualEventsInformation.Add("row[2,1]", eventIdForEvent[ConstantValues.EventId].ToString());
            actualEventsInformation.Add("row[2,2]", "EventoPlaneacion");
            actualEventsInformation.Add("row[2,3]", step.ScenarioContext["NodeId_1"].ToString());
            actualEventsInformation.Add("row[2,4]", step.ScenarioContext["NodeName_1"].ToString());
            actualEventsInformation.Add("row[2,5]", step.ScenarioContext["NodeId_2"].ToString());
            actualEventsInformation.Add("row[2,6]", step.ScenarioContext["NodeName_2"].ToString());
            actualEventsInformation.Add("row[2,7]", "10000002318");
            actualEventsInformation.Add("row[2,8]", "CRUDO CAMPO MAMBO");
            actualEventsInformation.Add("row[2,10]", "CRUDO CAMPO CUSUCO");
            actualEventsInformation.Add("row[2,9]", "10000002372");
            actualEventsInformation.Add("row[2,11]", eventIdForEvent[ConstantValues.StartDate].ToString());
            actualEventsInformation.Add("row[2,12]", eventIdForEvent[ConstantValues.EndDate].ToString());
            actualEventsInformation.Add("row[2,13]", "30");
            actualEventsInformation.Add("row[2,14]", "ECOPETROL");
            actualEventsInformation.Add("row[2,15]", "10000");
            actualEventsInformation.Add("row[2,16]", "Bbl");

            var expectedEventsInformation = new Dictionary<string, string>();
            ExcelPackage package = new ExcelPackage(new FileInfo(FilePaths.DownloadedOwnershipExcelPath.GetFullPath()));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[6];
            for (int i = 1; i <= worksheet.Dimension.Columns; i++)
            {
                expectedEventsInformation.Add("Column" + i, worksheet.Cells[1, i].Value.ToString());
                expectedEventsInformation.Add("row[2," + i + "]", worksheet.Cells[2, i].Value.ToString());
            }

            package.Dispose();
            Assert.IsTrue(step.VerifyDiffs(expectedEventsInformation, actualEventsInformation));
        }

        public static void ExcelInformationWithoutEvents(this StepDefinitionBase step)
        {
            step.ThrowIfNull(nameof(step));

            var actualEventsInformation = new Dictionary<string, object>()
            {
                { "Column1", "IdentificadorEvento" },
                { "Column2", "EventoPropiedad" },
                { "Column3", "IdNodoOrigen" },
                { "Column4", "NodoOrigen" },
                { "Column5", "IdNodoDestino" },
                { "Column6", "NodoDestino" },
                { "Column7", "IdProductoOrigen" },
                { "Column8", "ProductoOrigen" },
                { "Column9", "IdProductoDestino" },
                { "Column10", "ProductoDestino" },
                { "Column11", "FechaInicio" },
                { "Column12", "FechaFin" },
                { "Column13", "IdPropietario" },
                { "Column14", "Propietario" },
                { "Column15", "ValorDiario" },
                { "Column16", "Unidad" },
            };

            var expectedEventsInformation = new Dictionary<string, object>();
            ExcelPackage package = new ExcelPackage(new FileInfo(FilePaths.DownloadedOwnershipExcelPath.GetFullPath()));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[6];
            for (int i = 1; i <= worksheet.Dimension.Columns; i++)
            {
                expectedEventsInformation.Add("Column" + i, worksheet.Cells[1, i].Value);
            }

            // Verifying whether excel contains without Events information
            Assert.AreEqual(1, worksheet.Dimension.Rows);
            package.Dispose();
            Assert.IsTrue(step.VerifyDiffs(expectedEventsInformation, actualEventsInformation));
        }

        public static void IUpdateTheExcelWithNewData(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[i, 2].Value = "DEFECT " + new Faker().Random.Number(9999, 99999);
                step.ScenarioContext["InventoryId"] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
            }

            worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";

            // For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet.Cells[i, 2].Value = new Faker().Random.Number(11111, 99999);
                step.ScenarioContext["MovementId"] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
            }

            // Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns
            worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-1).ToShortDateString().ToString(CultureInfo.InvariantCulture) + " 23:59:59";

            package.Save();
            package.Dispose();
        }

        public static void IUpdateDataInExcel(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            if (fileName.EqualsIgnoreCase("MovementExcel"))
            {
                var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                // For Movements
                ExcelWorksheet worksheet = package.Workbook.Worksheets[3];
                var id = string.Empty + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[2, 2].Value = id;
                step.ScenarioContext["MovementId"] = id;
                worksheet = package.Workbook.Worksheets[4];
                worksheet.Cells[2, 1].Value = id;
                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[2, 1].Value = id;
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1);
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("InventoryExcel"))
            {
                var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                //// For Inventories
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var id = "DEFECT " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[2, 2].Value = id;
                step.ScenarioContext["InventoryId"] = id;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 1].Value = id;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 1].Value = id;
                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("InventoryWithManyProducts"))
            {
                var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                //// For Inventories
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var id = "DEFECT " + new Faker().Random.Number(9999, 999999);
                step.ScenarioContext["InventoryId"] = id;
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    worksheet.Cells[i, 2].Value = id;
                    worksheet.Cells[i, 3].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet.Cells[i, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet.Cells[i, 2].Value = DateTime.UtcNow.AddDays(-2);
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("UpdateMovementExcel"))
            {
                var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                // For Movements
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    var id = step.ScenarioContext["Movement" + (i - 1)].ToString();
                    worksheet.Cells[i, 2].Value = id;
                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-1);
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("UpdateInventoryExcel"))
            {
                var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                //// For Inventories
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = step.ScenarioContext["Inventory" + (i - 1)];
                    worksheet.Cells[i, 2].Value = id;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-1);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-1);
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("InsertEventExcel"))
            {
                var uploadFileName = @"TestData\Input\Planning, Programming and Collaboration agreements\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                var startTime = DateTime.UtcNow;
                step.ScenarioContext["StartTime"] = startTime;
                var endTime = DateTime.UtcNow.AddDays(2);
                step.ScenarioContext["EndTime"] = endTime;
                worksheet.Cells[2, 9].Value = startTime;
                worksheet.Cells[2, 10].Value = endTime;
                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("DeleteEventExcel") || fileName.EqualsIgnoreCase("UpdateEventExcel"))
            {
                var uploadFileName = @"TestData\Input\Planning, Programming and Collaboration agreements\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 9].Value = step.ScenarioContext["StartTime"];
                worksheet.Cells[2, 10].Value = step.ScenarioContext["EndTime"];
                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("InsertContractExcel"))
            {
                var uploadFileName = @"TestData\Input\PurchaseAndSales\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = new Faker().Random.Number(1000000, 9999999);
                    worksheet.Cells[i, 1].Value = id;
                    step.ScenarioContext["Contract" + (i - 1)] = id;
                    var startTime = DateTime.UtcNow;
                    step.ScenarioContext["StartTime" + (i - 1)] = startTime;
                    var endTime = DateTime.UtcNow.AddDays(2);
                    step.ScenarioContext["EndTime" + (i - 1)] = endTime;
                    worksheet.Cells[i, 7].Value = startTime;
                    worksheet.Cells[i, 8].Value = endTime;
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("UpdateContractExcel") || fileName.EqualsIgnoreCase("DeleteContractExcel"))
            {
                var uploadFileName = @"TestData\Input\PurchaseAndSales\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = step.ScenarioContext["Contract" + (i - 1)];
                    var startTime = step.ScenarioContext["StartTime" + (i - 1)];
                    var endTime = step.ScenarioContext["EndTime" + (i - 1)];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet.Cells[i, 7].Value = startTime;
                    worksheet.Cells[i, 8].Value = endTime;
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("ValidFicoEvents"))
            {
                var uploadFileName = @"TestData\Input\Planning, Programming and Collaboration agreements\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                // For Events
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Updating Date Range of Events
                for (var i = 2; i < 10; i++)
                {
                    worksheet.Cells[i, 9].Value = DateTime.Now.AddDays(-7);
                    worksheet.Cells[i, 10].Value = DateTime.Now.AddDays(+3);
                }

                package.Save();
                package.Dispose();
            }
            else if (fileName.EqualsIgnoreCase("MovementValidation"))
            {
                var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
                ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

                // For Movements
                ExcelWorksheet worksheet = package.Workbook.Worksheets[3];
                var id = string.Empty + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[2, 2].Value = id;
                step.ScenarioContext["MovementId"] = id;
                worksheet = package.Workbook.Worksheets[4];
                worksheet.Cells[2, 1].Value = id;
                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[2, 1].Value = id;
                worksheet = package.Workbook.Worksheets[3];
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-1);
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-2);
                package.Save();
                package.Dispose();
            }
        }

        public static void IUpdateTheEventsExcel(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\Planning, Programming and Collaboration agreements\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            // For Events
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            // Updating Date Range of Events
            worksheet.Cells[2, 9].Value = DateTime.UtcNow.AddDays(-5).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[2, 10].Value = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[3, 9].Value = DateTime.UtcNow.AddDays(-2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[3, 10].Value = DateTime.UtcNow.AddDays(+2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[4, 9].Value = DateTime.UtcNow.AddDays(-1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[4, 10].Value = DateTime.UtcNow.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[5, 9].Value = DateTime.UtcNow.AddDays(+2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            worksheet.Cells[5, 10].Value = DateTime.UtcNow.AddDays(+4).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            package.Save();
            package.Dispose();
        }

        public static void IUpdateTheExcelForOfficialMovements(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-32);
            for (int i = 1; i <= 2; i++)
            {
                worksheet = package.Workbook.Worksheets[i];
                worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-32);
            }

            if (fileName == "TestData_Official")
            {
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var concatenatedvalue = new Faker().Random.AlphaNumeric(149);
                    worksheet.Cells[i, 2].Value = concatenatedvalue;
                    step.ScenarioContext["InventoryData"] = worksheet.Cells[i, 2].Value;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                }
            }
            //// For Movements
            if (fileName == "TestData_Official")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    var concatenatedvalue = new Faker().Random.AlphaNumeric(149);
                    worksheet.Cells[i, 2].Value = concatenatedvalue;
                    step.ScenarioContext["MovementData"] = worksheet.Cells[i, 2].Value;
                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = concatenatedvalue;
                }
            }

            //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns

            worksheet = package.Workbook.Worksheets[3];
            for (int j = 6; j <= 7; j++)
            {
                worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-32);
            }

            package.Save();
            package.Dispose();
        }

        public static void IUpdateTheExcelDataForInventories(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

            var id = "DEFECT " + new Faker().Random.Number(9999, 999999);
            worksheet.Cells[2, 2].Value = id;
            worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-3);
            step.ScenarioContext["Inventory1"] = worksheet.Cells[2, 2].Value;
            worksheet = package.Workbook.Worksheets[1];
            worksheet.Cells[2, 1].Value = id;
            worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-3);

            package.Save();
            package.Dispose();
        }

        public static void IUpdateTheExcelDataForAdjustmentsInCutoff(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            if (string.IsNullOrEmpty(step.GetValueInternal("Inventory1")))
            {
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[0];
                    var id = "DEFECT " + new Faker().Random.Number(9999, 999999);
                    worksheet.Cells[i, 2].Value = id;
                    step.ScenarioContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
                    worksheet = package.Workbook.Worksheets[1];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet = package.Workbook.Worksheets[2];
                    worksheet.Cells[i, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[0];
                step.SetValueInternal("FirstCutoff", DateTime.UtcNow.AddDays(-3).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-3);
                step.SetValueInternal("SecondCutoff", DateTime.UtcNow.AddDays(-2).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture));
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-2);

                for (int j = 2; j <= 3; j++)
                {
                    worksheet = package.Workbook.Worksheets[j - 1];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-2);
                }

                worksheet = package.Workbook.Worksheets[3];
                for (int i = 2; i <= worksheet.Dimension.Rows; i++)
                {
                    worksheet = package.Workbook.Worksheets[3];
                    var id = new Faker().Random.Number(11111, 9999999);
                    worksheet.Cells[i, 2].Value = id;
                    step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                    worksheet = package.Workbook.Worksheets[4];
                    worksheet.Cells[i, 1].Value = id;
                    worksheet = package.Workbook.Worksheets[5];
                    worksheet.Cells[i, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[3];
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[3, 6].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[3, 7].Value = DateTime.UtcNow.AddDays(-2);
            }
            else
            {
                worksheet.Cells[2, 12].Value = 90000;
                worksheet.Cells[3, 12].Value = 100000;
            }

            package.Save();
            package.Dispose();
        }

        public static void IHaveExcelHavingMovementsWithTransferPoint(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "DEFECT " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[i, 1].Value = id;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            if (fileName == "TestData_TransferPoint")
            {
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[3, 3].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[4, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[5, 3].Value = DateTime.UtcNow.AddDays(+2);
                worksheet.Cells[6, 3].Value = DateTime.UtcNow.AddDays(-32);
                worksheet.Cells[7, 3].Value = DateTime.UtcNow.AddDays(-32);
                worksheet.Cells[8, 3].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[9, 3].Value = DateTime.UtcNow.AddDays(-3);
                worksheet.Cells[10, 3].Value = DateTime.UtcNow.AddDays(-2);
                worksheet.Cells[11, 3].Value = DateTime.UtcNow.AddDays(+2);
                for (int i = 1; i <= 2; i++)
                {
                    worksheet = package.Workbook.Worksheets[i];
                    worksheet.Cells[2, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[3, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[4, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[5, 2].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[6, 2].Value = DateTime.UtcNow.AddDays(-32);
                    worksheet.Cells[7, 2].Value = DateTime.UtcNow.AddDays(-32);
                    worksheet.Cells[8, 2].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[9, 2].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[10, 2].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[11, 2].Value = DateTime.UtcNow.AddDays(-1);
                }
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[4];
                worksheet.Cells[i, 1].Value = id;
                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[i, 1].Value = id;
            }

            //// Modifying data in FechaHoraInicial(j=6) and FechaHoraFinal(j=7) columns
            if (fileName == "TestData_TransferPoint")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    worksheet.Cells[2, j].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[3, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[4, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[5, j].Value = DateTime.UtcNow.AddDays(-1);
                    worksheet.Cells[6, j].Value = DateTime.UtcNow.AddDays(-32);
                    worksheet.Cells[7, j].Value = DateTime.UtcNow.AddDays(-32);
                    worksheet.Cells[8, j].Value = DateTime.UtcNow.AddDays(-4);
                    worksheet.Cells[9, j].Value = DateTime.UtcNow.AddDays(-3);
                    worksheet.Cells[10, j].Value = DateTime.UtcNow.AddDays(-2);
                    worksheet.Cells[11, j].Value = DateTime.UtcNow.AddDays(-2);
                }
            }
            else if (fileName == "SapPo_TransferPoint")
            {
                worksheet = package.Workbook.Worksheets[3];
                worksheet.Cells[2, 6].Value = DateTime.UtcNow.AddDays(-4);
                worksheet.Cells[2, 7].Value = DateTime.UtcNow.AddDays(-4);

                DateTime date = DateTime.Now.AddMonths(-1);
                var firstDayOfPreviousMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfPreviousMonth = firstDayOfPreviousMonth.AddMonths(1).AddDays(-1);

                worksheet.Cells[3, 6].Value = firstDayOfPreviousMonth;
                worksheet.Cells[3, 7].Value = lastDayOfPreviousMonth;
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileWithTwoOwners(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int l = 2;
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "INV_OFF " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["InventoryId"] = id;
                step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[1];
                int j;
                for (j = l; j <= (l + 1); j++)
                {
                    worksheet.Cells[j, 1].Value = id;
                }

                l = j;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            if (fileName == "TestData_OfficialMovementsInventories")
            {
                DateTime date = DateTime.Now.AddMonths(-1);
                var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                worksheet = package.Workbook.Worksheets[0];
                worksheet.Cells[2, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[3, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[4, 3].Value = firstDayOfMonth;
                worksheet.Cells[5, 3].Value = firstDayOfMonth;
                for (int i = 6, j = 1; i <= 63; i += 2, j++)
                {
                    worksheet.Cells[i, 3].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 3].Value = firstDayOfMonth.AddDays(j);
                }

                worksheet.Cells[64, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[65, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[66, 3].Value = firstDayOfMonth;
                worksheet.Cells[67, 3].Value = firstDayOfMonth;
                for (int i = 68, j = 1; i <= 125; i += 2, j++)
                {
                    worksheet.Cells[i, 3].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 3].Value = firstDayOfMonth.AddDays(j);
                }

                worksheet.Cells[126, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[127, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[128, 3].Value = firstDayOfMonth;
                worksheet.Cells[129, 3].Value = firstDayOfMonth;
                for (int i = 130, j = 1; i <= 187; i += 2, j++)
                {
                    worksheet.Cells[i, 3].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 3].Value = firstDayOfMonth.AddDays(j);
                }

                worksheet.Cells[188, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[189, 3].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[190, 3].Value = firstDayOfMonth;
                worksheet.Cells[191, 3].Value = firstDayOfMonth;
                for (int i = 192, j = 1; i <= 249; i += 2, j++)
                {
                    worksheet.Cells[i, 3].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 3].Value = firstDayOfMonth.AddDays(j);
                }

                // Updating Owner Sheet
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[2, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[3, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[4, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[5, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[6, 2].Value = firstDayOfMonth;
                worksheet.Cells[7, 2].Value = firstDayOfMonth;
                worksheet.Cells[8, 2].Value = firstDayOfMonth;
                worksheet.Cells[9, 2].Value = firstDayOfMonth;
                for (int i = 10, k = 1; i <= 125; i += 4, k++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 2, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 3, 2].Value = firstDayOfMonth.AddDays(k);
                }

                worksheet.Cells[126, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[127, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[128, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[129, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[130, 2].Value = firstDayOfMonth;
                worksheet.Cells[131, 2].Value = firstDayOfMonth;
                worksheet.Cells[132, 2].Value = firstDayOfMonth;
                worksheet.Cells[133, 2].Value = firstDayOfMonth;
                for (int i = 134, k = 1; i <= 249; i += 4, k++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 2, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 3, 2].Value = firstDayOfMonth.AddDays(k);
                }

                worksheet.Cells[250, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[251, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[252, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[253, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[254, 2].Value = firstDayOfMonth;
                worksheet.Cells[255, 2].Value = firstDayOfMonth;
                worksheet.Cells[256, 2].Value = firstDayOfMonth;
                worksheet.Cells[257, 2].Value = firstDayOfMonth;
                for (int i = 258, k = 1; i <= 373; i += 4, k++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 2, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 3, 2].Value = firstDayOfMonth.AddDays(k);
                }

                worksheet.Cells[374, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[375, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[376, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[377, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[378, 2].Value = firstDayOfMonth;
                worksheet.Cells[379, 2].Value = firstDayOfMonth;
                worksheet.Cells[380, 2].Value = firstDayOfMonth;
                worksheet.Cells[381, 2].Value = firstDayOfMonth;
                for (int i = 382, k = 1; i <= 497; i += 4, k++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 2, 2].Value = firstDayOfMonth.AddDays(k);
                    worksheet.Cells[i + 3, 2].Value = firstDayOfMonth.AddDays(k);
                }

                // Updating Attribute Sheet
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[2, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[3, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[4, 2].Value = firstDayOfMonth;
                worksheet.Cells[5, 2].Value = firstDayOfMonth;
                for (int i = 6, j = 1; i <= 63; i += 2, j++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(j);
                }

                worksheet.Cells[64, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[65, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[66, 2].Value = firstDayOfMonth;
                worksheet.Cells[67, 2].Value = firstDayOfMonth;
                for (int i = 68, j = 1; i <= 125; i += 2, j++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(j);
                }

                worksheet.Cells[126, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[127, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[128, 2].Value = firstDayOfMonth;
                worksheet.Cells[129, 2].Value = firstDayOfMonth;
                for (int i = 130, j = 1; i <= 187; i += 2, j++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(j);
                }

                worksheet.Cells[188, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[189, 2].Value = firstDayOfMonth.AddDays(-1);
                worksheet.Cells[190, 2].Value = firstDayOfMonth;
                worksheet.Cells[191, 2].Value = firstDayOfMonth;
                for (int i = 192, j = 1; i <= 249; i += 2, j++)
                {
                    worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(j);
                    worksheet.Cells[i + 1, 2].Value = firstDayOfMonth.AddDays(j);
                }
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            l = 2;
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = "MOV_OFF " + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[4];
                int j;
                for (j = l; j <= (l + 1); j++)
                {
                    worksheet.Cells[j, 1].Value = id;
                }

                l = j;
                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[i, 1].Value = id;
            }

            if (fileName == "TestData_OfficialMovementsInventories")
            {
                worksheet = package.Workbook.Worksheets[3];
                for (int j = 6; j <= 7; j++)
                {
                    DateTime date = DateTime.Now.AddMonths(-1);
                    var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                    var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                    worksheet.Cells[2, j].Value = firstDayOfMonth;
                    worksheet.Cells[3, j].Value = firstDayOfMonth;
                    for (int i = 4, k = 1; i <= 61; i += 2, k++)
                    {
                        worksheet.Cells[i, j].Value = firstDayOfMonth.AddDays(k);
                        worksheet.Cells[i + 1, j].Value = firstDayOfMonth.AddDays(k);
                    }

                    worksheet.Cells[62, j].Value = firstDayOfMonth;
                    worksheet.Cells[63, j].Value = firstDayOfMonth;
                    for (int i = 64, k = 1; i <= 121; i += 2, k++)
                    {
                        worksheet.Cells[i, j].Value = firstDayOfMonth.AddDays(k);
                        worksheet.Cells[i + 1, j].Value = firstDayOfMonth.AddDays(k);
                    }

                    worksheet.Cells[122, j].Value = firstDayOfMonth;
                    worksheet.Cells[123, j].Value = firstDayOfMonth;
                    for (int i = 124, k = 1; i <= 181; i += 2, k++)
                    {
                        worksheet.Cells[i, j].Value = firstDayOfMonth.AddDays(k);
                        worksheet.Cells[i + 1, j].Value = firstDayOfMonth.AddDays(k);
                    }

                    worksheet.Cells[182, j].Value = firstDayOfMonth;
                    worksheet.Cells[183, j].Value = firstDayOfMonth;
                    for (int i = 184, k = 1; i <= 241; i += 2, k++)
                    {
                        worksheet.Cells[i, j].Value = firstDayOfMonth.AddDays(k);
                        worksheet.Cells[i + 1, j].Value = firstDayOfMonth.AddDays(k);
                    }
                }
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileWithInventoryAndMovementNetVolumes(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));

            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            int net = 500;
            var row1 = worksheet.Cells[2, 12].Value.ToInt();
            worksheet.Cells[2, 12].Value = row1 - net;

            var row2 = worksheet.Cells[3, 12].Value.ToInt();
            worksheet.Cells[3, 12].Value = row2 - net;

            var row3 = worksheet.Cells[4, 12].Value.ToInt();
            worksheet.Cells[4, 12].Value = row3 - net;

            var row4 = worksheet.Cells[5, 12].Value.ToInt();
            worksheet.Cells[5, 12].Value = row4 - net;

            var row5 = worksheet.Cells[6, 12].Value.ToInt();
            worksheet.Cells[6, 12].Value = row5 - net;

            var row6 = worksheet.Cells[7, 12].Value.ToInt();
            worksheet.Cells[7, 12].Value = row6 + net;

            var row7 = worksheet.Cells[8, 12].Value.ToInt();
            worksheet.Cells[8, 12].Value = row7 + net;

            var row8 = worksheet.Cells[9, 12].Value.ToInt();
            worksheet.Cells[9, 12].Value = row8 + net;

            //// For Movements
            ExcelWorksheet worksheetMovement = package.Workbook.Worksheets[3];
            int netAmt = 500;
            var r1 = worksheetMovement.Cells[2, 15].Value.ToInt();
            worksheetMovement.Cells[2, 15].Value = r1 - netAmt;

            var r2 = worksheetMovement.Cells[3, 15].Value.ToInt();
            worksheetMovement.Cells[3, 15].Value = r2 - netAmt;

            var r3 = worksheetMovement.Cells[4, 15].Value.ToInt();
            worksheetMovement.Cells[4, 15].Value = r3 - netAmt;

            var r4 = worksheetMovement.Cells[5, 15].Value.ToInt();
            worksheetMovement.Cells[5, 15].Value = r4 - netAmt;

            var r5 = worksheetMovement.Cells[6, 15].Value.ToInt();
            worksheetMovement.Cells[6, 15].Value = r5 - netAmt;

            var r6 = worksheetMovement.Cells[7, 15].Value.ToInt();
            worksheetMovement.Cells[7, 15].Value = r6 + netAmt;

            var r7 = worksheetMovement.Cells[8, 15].Value.ToInt();
            worksheetMovement.Cells[8, 15].Value = r7 + netAmt;

            var r8 = worksheetMovement.Cells[9, 15].Value.ToInt();
            worksheetMovement.Cells[9, 15].Value = r8 + netAmt;

            package.Save();
            package.Dispose();
        }

        public static void IUpdateTheExcelForOfficialMovementsAndInventories(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "Official_Inventory " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[1];
                worksheet.Cells[i, 1].Value = id;
                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = "Official_Movement" + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[4];
                worksheet.Cells[i, 1].Value = id;
                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[i, 1].Value = id;
            }

            package.Save();
            package.Dispose();
        }

        public static void WhenIUpdateTheExcelFileForOfficialDeltaPerNodeReport(this StepDefinitionBase step, string fileName)
        {
            step.ThrowIfNull(nameof(step));
            var uploadFileName = @"TestData\Input\ExcelUpload\" + fileName + ".xlsx";
            ExcelPackage package = new ExcelPackage(new FileInfo(uploadFileName.GetFullPath()));
            //// For Inventories
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[0];
                var id = "INV_OFF " + new Faker().Random.Number(9999, 999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["InventoryId"] = id;
                step.FeatureContext["Inventory" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[1];
                if (i == 2)
                {
                    worksheet.Cells[2, 1].Value = id;
                }
                else if (i == 3)
                {
                    worksheet.Cells[3, 1].Value = id;
                }
                else if (i == 4)
                {
                    worksheet.Cells[4, 1].Value = id;
                    worksheet.Cells[5, 1].Value = id;
                }
                else if (i == 5)
                {
                    worksheet.Cells[6, 1].Value = id;
                    worksheet.Cells[7, 1].Value = id;
                }
                else if (i == 6)
                {
                    worksheet.Cells[8, 1].Value = id;
                }
                else if (i == 7)
                {
                    worksheet.Cells[9, 1].Value = id;
                }
                else if (i == 8)
                {
                    worksheet.Cells[10, 1].Value = id;
                    worksheet.Cells[11, 1].Value = id;
                }
                else if (i == 9)
                {
                    worksheet.Cells[12, 1].Value = id;
                    worksheet.Cells[13, 1].Value = id;
                }
                else if ((i >= 10) && (i <= 38))
                {
                    worksheet.Cells[i + 4, 1].Value = id;
                }
                else if (i == 39)
                {
                    worksheet.Cells[43, 1].Value = id;
                    worksheet.Cells[44, 1].Value = id;
                }
                else if (i == 40)
                {
                    worksheet.Cells[45, 1].Value = id;
                    worksheet.Cells[46, 1].Value = id;
                }
                else if (i == 41)
                {
                    worksheet.Cells[47, 1].Value = id;
                }
                else if (i == 42)
                {
                    worksheet.Cells[48, 1].Value = id;
                }
                else if (i == 43)
                {
                    worksheet.Cells[49, 1].Value = id;
                    worksheet.Cells[50, 1].Value = id;
                }
                else if (i == 44)
                {
                    worksheet.Cells[51, 1].Value = id;
                    worksheet.Cells[52, 1].Value = id;
                }
                else if (i == 45)
                {
                    worksheet.Cells[53, 1].Value = id;
                }
                else if (i == 46)
                {
                    worksheet.Cells[54, 1].Value = id;
                }
                else if (i == 47)
                {
                    worksheet.Cells[55, 1].Value = id;
                    worksheet.Cells[56, 1].Value = id;
                }
                else if (i == 48)
                {
                    worksheet.Cells[57, 1].Value = id;
                    worksheet.Cells[58, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[2];
                worksheet.Cells[i, 1].Value = id;
            }

            ////Updating Date Range

            // Updating Inventory Sheet
            DateTime date = DateTime.Now.AddMonths(-1);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            step.ScenarioContext["PreviousMonthStartDate"] = firstDayOfMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["PreviousMonthEndDate"] = lastDayOfMonth.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"] = firstDayOfMonth.ToString("dd-MMM-yyyy", CultureInfo.CreateSpecificCulture("es-CO"));
            step.ScenarioContext["PreviousMonthName"] = step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"].ToString().Split('-')[1].Trim('.');
            step.ScenarioContext["YearForPeriod"] = step.ScenarioContext["PreviousMonthStartDateDetailsInColumbianCulture"].ToString().Split('-')[2];
            worksheet = package.Workbook.Worksheets[0];
            worksheet.Cells[2, 3].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[3, 3].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[4, 3].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[5, 3].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[6, 3].Value = firstDayOfMonth;
            worksheet.Cells[7, 3].Value = firstDayOfMonth;
            worksheet.Cells[8, 3].Value = firstDayOfMonth;
            worksheet.Cells[9, 3].Value = firstDayOfMonth;
            for (int i = 10; i <= 36; i++)
            {
                worksheet.Cells[i, 3].Value = firstDayOfMonth.AddDays(i - 9);
            }

            worksheet.Cells[37, 3].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[38, 3].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[39, 3].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[40, 3].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[41, 3].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[42, 3].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[43, 3].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[44, 3].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[45, 3].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[46, 3].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[47, 3].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[48, 3].Value = firstDayOfMonth.AddDays(30);

            // Updating Owner Sheet
            worksheet = package.Workbook.Worksheets[1];
            worksheet.Cells[2, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[3, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[4, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[5, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[6, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[7, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[8, 2].Value = firstDayOfMonth;
            worksheet.Cells[9, 2].Value = firstDayOfMonth;
            worksheet.Cells[10, 2].Value = firstDayOfMonth;
            worksheet.Cells[11, 2].Value = firstDayOfMonth;
            worksheet.Cells[12, 2].Value = firstDayOfMonth;
            worksheet.Cells[13, 2].Value = firstDayOfMonth;
            for (int i = 14; i <= 40; i++)
            {
                worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(i - 13);
            }

            worksheet.Cells[41, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[42, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[43, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[44, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[45, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[46, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[47, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[48, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[49, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[50, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[51, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[52, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[53, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[54, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[55, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[56, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[57, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[58, 2].Value = firstDayOfMonth.AddDays(30);

            // Updating Attribute Sheet
            worksheet = package.Workbook.Worksheets[2];
            worksheet.Cells[2, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[3, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[4, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[5, 2].Value = firstDayOfMonth.AddDays(-1);
            worksheet.Cells[6, 2].Value = firstDayOfMonth;
            worksheet.Cells[7, 2].Value = firstDayOfMonth;
            worksheet.Cells[8, 2].Value = firstDayOfMonth;
            worksheet.Cells[9, 2].Value = firstDayOfMonth;
            for (int i = 10; i <= 36; i++)
            {
                worksheet.Cells[i, 2].Value = firstDayOfMonth.AddDays(i - 9);
            }

            worksheet.Cells[37, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[38, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[39, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[40, 2].Value = firstDayOfMonth.AddDays(28);
            worksheet.Cells[41, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[42, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[43, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[44, 2].Value = firstDayOfMonth.AddDays(29);
            worksheet.Cells[45, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[46, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[47, 2].Value = firstDayOfMonth.AddDays(30);
            worksheet.Cells[48, 2].Value = firstDayOfMonth.AddDays(30);

            //// For Movements
            worksheet = package.Workbook.Worksheets[3];
            for (int i = 2; i <= worksheet.Dimension.Rows; i++)
            {
                worksheet = package.Workbook.Worksheets[3];
                var id = "MOV_OFF " + new Faker().Random.Number(11111, 9999999);
                worksheet.Cells[i, 2].Value = id;
                step.ScenarioContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                step.FeatureContext["Movement" + (i - 1)] = worksheet.Cells[i, 2].Value;
                worksheet = package.Workbook.Worksheets[4];
                if ((i >= 2) && (i <= 17))
                {
                    worksheet.Cells[i, 1].Value = id;
                }
                else if (i == 18)
                {
                    worksheet.Cells[18, 1].Value = id;
                    worksheet.Cells[19, 1].Value = id;
                }
                else if (i == 19)
                {
                    worksheet.Cells[20, 1].Value = id;
                    worksheet.Cells[21, 1].Value = id;
                }
                else if (i == 20)
                {
                    worksheet.Cells[22, 1].Value = id;
                    worksheet.Cells[23, 1].Value = id;
                }
                else if (i == 21)
                {
                    worksheet.Cells[24, 1].Value = id;
                    worksheet.Cells[25, 1].Value = id;
                }

                worksheet = package.Workbook.Worksheets[5];
                worksheet.Cells[i, 1].Value = id;
            }

            ////Update Date Range
            worksheet = package.Workbook.Worksheets[3];
            for (int j = 6; j <= 7; j++)
            {
                worksheet.Cells[2, j].Value = firstDayOfMonth;
                worksheet.Cells[3, j].Value = firstDayOfMonth;
                worksheet.Cells[4, j].Value = firstDayOfMonth;
                worksheet.Cells[5, j].Value = firstDayOfMonth;
                worksheet.Cells[6, j].Value = firstDayOfMonth.AddDays(1);
                worksheet.Cells[7, j].Value = firstDayOfMonth.AddDays(1);
                worksheet.Cells[8, j].Value = firstDayOfMonth.AddDays(1);
                worksheet.Cells[9, j].Value = firstDayOfMonth.AddDays(1);
                worksheet.Cells[10, j].Value = firstDayOfMonth.AddDays(29);
                worksheet.Cells[11, j].Value = firstDayOfMonth.AddDays(29);
                worksheet.Cells[12, j].Value = firstDayOfMonth.AddDays(29);
                worksheet.Cells[13, j].Value = firstDayOfMonth.AddDays(29);
                worksheet.Cells[14, j].Value = firstDayOfMonth.AddDays(30);
                worksheet.Cells[15, j].Value = firstDayOfMonth.AddDays(30);
                worksheet.Cells[16, j].Value = firstDayOfMonth.AddDays(30);
                worksheet.Cells[17, j].Value = firstDayOfMonth.AddDays(30);
            }

            worksheet.Cells[18, 6].Value = firstDayOfMonth;
            worksheet.Cells[19, 6].Value = firstDayOfMonth;
            worksheet.Cells[20, 6].Value = firstDayOfMonth;
            worksheet.Cells[21, 6].Value = firstDayOfMonth;
            worksheet.Cells[18, 7].Value = lastDayOfMonth;
            worksheet.Cells[19, 7].Value = lastDayOfMonth;
            worksheet.Cells[20, 7].Value = lastDayOfMonth;
            worksheet.Cells[21, 7].Value = lastDayOfMonth;

            package.Save();
            package.Dispose();
        }
    }
}