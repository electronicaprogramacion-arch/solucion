using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Domain.Repositories;
using CalibrationSaaS.Domain.Aggregates.ValueObjects;
using Helpers.Controls.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CalibrationSaaS.Application.UseCases
{
    public class EquipmentRecallUseCases
    {
        private readonly IEquipmentRecallRepository equipmentRecallRepository;

        public EquipmentRecallUseCases(IEquipmentRecallRepository equipmentRecallRepository)
        {
            this.equipmentRecallRepository = equipmentRecallRepository;
        }

        public async Task<IEnumerable<EquipmentRecall>> GetEquipmentRecalls(EquipmentRecallFilter filter)
        {
            return await equipmentRecallRepository.GetEquipmentRecalls(filter);
        }

        public async Task<ResultSet<EquipmentRecall>> GetEquipmentRecallsPaginated(Pagination<EquipmentRecall> pagination)
        {
            return await equipmentRecallRepository.GetEquipmentRecallsPaginated(pagination);
        }

        public async Task<int> GetEquipmentRecallsCount(EquipmentRecallFilter filter)
        {
            return await equipmentRecallRepository.GetEquipmentRecallsCount(filter);
        }

        public async Task<byte[]> ExportEquipmentRecallsToExcel(EquipmentRecallFilter filter)
        {
            var equipmentRecalls = await GetEquipmentRecalls(filter);
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Equipment Recalls");
                
                // Set headers
                var headers = new string[]
                {
                    "TECHNICIAN",
                    "REPORT NUMBER", 
                    "CALIBRATION DATE",
                    "DUE DATE",
                    "SERIAL NUMBER",
                    "DESCRIPTION",
                    "WO#",
                    "CMS#", // This should be ID# according to requirements
                    "PO#",
                    "QUOTE #",
                    "PT#"
                };

                // Add headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Add data
                int row = 2;
                foreach (var recall in equipmentRecalls.OrderBy(x => x.DueDate))
                {
                    worksheet.Cells[row, 1].Value = recall.Technician; // Empty
                    worksheet.Cells[row, 2].Value = recall.ReportNumber; // Empty
                    worksheet.Cells[row, 3].Value = recall.CalibrationDate.ToString("M/d/yyyy");
                    worksheet.Cells[row, 4].Value = recall.DueDate.ToString("M/d/yyyy");
                    worksheet.Cells[row, 5].Value = recall.SerialNumber;
                    worksheet.Cells[row, 6].Value = recall.Description;
                    worksheet.Cells[row, 7].Value = recall.WorkOrderNumber; // Empty
                    worksheet.Cells[row, 8].Value = recall.PieceOfEquipmentID; // ID# instead of CMS#
                    worksheet.Cells[row, 9].Value = recall.PONumber; // Empty
                    worksheet.Cells[row, 10].Value = recall.QuoteNumber; // Empty
                    worksheet.Cells[row, 11].Value = recall.PTNumber; // Empty
                    
                    row++;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Set date format for date columns
                worksheet.Cells[2, 3, row - 1, 3].Style.Numberformat.Format = "m/d/yyyy";
                worksheet.Cells[2, 4, row - 1, 4].Style.Numberformat.Format = "m/d/yyyy";

                return package.GetAsByteArray();
            }
        }
    }
}
