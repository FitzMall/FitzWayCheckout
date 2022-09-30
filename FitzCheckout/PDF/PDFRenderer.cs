using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.Kernel.Geom;
using System.IO;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Forms.Fields;
using iText.Forms;
using iText.Layout.Renderer;
using iText.Kernel.Colors;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Image;
using FitzCheckout.Models;
using iText.License;
using FitzCheckout.BizObjects;
using System.Configuration;
using FitzCheckout.DAL;

namespace FitzCheckout.PDF
{
    public interface IPdfRenderer
    {
        string CreatePdf(int checklistRecordId);
        
    }
    public class PdfRenderer : IPdfRenderer
    {
        private readonly IChecklistVM _checklistVM;

        public PdfRenderer()
        {

        }

        public PdfRenderer(IChecklistVM checklistVM)
        {
            _checklistVM = checklistVM;
        }
        public string CreatePdf(int checklistRecordID)
        {
            var checklistRecord = _checklistVM.GetChecklistVMByChecklistRecordID(checklistRecordID);

            var fileName = ConfigurationManager.AppSettings["PdfFilenameRoot"] + checklistRecord.MetaDataValue7 + ".pdf";
            //var path = "J:/inetpub/wwwroot/production/FITZWAY/pictures/UCPDFS/";
            //var fileName = path + ConfigurationManager.AppSettings["PdfFilenameRoot"] + checklistRecord.MetaDataValue7 + ".pdf";
            
            try
            {
                var file = System.IO.Path.Combine(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PdfLocation"]),
                                            fileName);
                //var file = System.IO.Path.Combine(ConfigurationManager.AppSettings["PdfLocation"],
                //                            fileName);
                
                SqlMapperUtil.SqlWithParams<int>("insert into ztemp (info) values ('this is the filename- " + file + "')", null);
                var writer = new PdfWriter(file);

                var pdf = new PdfDocument(writer);
                var form = PdfAcroForm.GetAcroForm(pdf, true);

                var pageSize = PageSize.LETTER.Rotate();
                var document = new Document(pdf, pageSize);

                //separate document into 3 columns
                float offset = 20;
                float columnWidth = (pageSize.GetWidth() - offset + 2 + 10) / 3;
                float columnHeight = pageSize.GetHeight() - offset * 2;

                Rectangle[] columns = new Rectangle[]
                {
                new Rectangle(offset - 5, offset, columnWidth, columnHeight),
                new Rectangle(offset + columnWidth, offset, columnWidth, columnHeight),
                new Rectangle(offset + columnWidth * 2 + 5, offset,columnWidth, columnHeight)
                };
                document.SetRenderer(new ColumnDocumentRenderer(document, columns));



                //set up fonts for document

                var sectionFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                var itemFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);


                //loop through checklist data
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 5, 1, 1, 1, 1, 1 }));

                // adding page # etc.
           
                string Page1Info = ("" + checklistRecord.MetaDataValue7 + " / " + checklistRecord.DateUpdated + " / 1 of 2");
                Paragraph ParagraphPage1 = new Paragraph()
                                                    .Add(Page1Info)
                                                    .SetFontSize(8)
                                                    .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                                                    .SetFixedPosition(columnWidth + 35, 5, 250);

                document.Add(ParagraphPage1);

                foreach (var section in checklistRecord.sections)
                {

                    var optionHeaderCount = 0;

                    if (String.IsNullOrEmpty(section.OptionName1))
                        optionHeaderCount++;
                    if (String.IsNullOrEmpty(section.OptionName2))
                        optionHeaderCount++;
                    if (String.IsNullOrEmpty(section.OptionName3))
                        optionHeaderCount++;
                    if (String.IsNullOrEmpty(section.OptionName4))
                        optionHeaderCount++;


                    Cell sectionHeader = new Cell(1, 2 + optionHeaderCount)
                        .Add(new Paragraph(section.SectionLabel)
                            .SetFont(sectionFont)
                            .SetFontSize(10)
                            .SetFixedLeading(8))
                            .SetBorder(Border.NO_BORDER);
                    table.AddCell(sectionHeader);

                    if (optionHeaderCount < 4)
                    {
                        if (!String.IsNullOrEmpty(section.OptionName1))
                        {
                            Cell headerCell1 = new Cell(1, 1)
                                .Add(new Paragraph(section.OptionName1)
                                .SetFont(sectionFont)
                                .SetFontSize(6))
                                .SetBorder(Border.NO_BORDER)
                                .SetTextAlignment(TextAlignment.CENTER);
                            table.AddCell(headerCell1);
                        }
                        if (!String.IsNullOrEmpty(section.OptionName2))
                        {
                            Cell headerCell2 = new Cell(1, 1)
                                .Add(new Paragraph(section.OptionName2)
                                .SetFont(sectionFont)
                                .SetFontSize(6))
                                .SetBorder(Border.NO_BORDER)
                                .SetTextAlignment(TextAlignment.CENTER);
                            table.AddCell(headerCell2);
                        }
                        if (!String.IsNullOrEmpty(section.OptionName3))
                        {
                            Cell headerCell3 = new Cell(1, 1)
                                .Add(new Paragraph(section.OptionName3)
                                .SetFont(sectionFont)
                                .SetFontSize(6))
                                .SetBorder(Border.NO_BORDER)
                                .SetTextAlignment(TextAlignment.CENTER);
                            table.AddCell(headerCell3);
                        }
                        if (!String.IsNullOrEmpty(section.OptionName4))
                        {
                            Cell headerCell4 = new Cell(1, 1)
                                .Add(new Paragraph(section.OptionName4)
                                .SetFont(sectionFont)
                                .SetFontSize(6))
                                .SetBorder(Border.NO_BORDER)
                                .SetTextAlignment(TextAlignment.CENTER);
                            table.AddCell(headerCell4);
                        }

                    }
                    Cell spacer = new Cell(1, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER);
                    table.AddCell(spacer);

                    foreach (var item in section.ChecklistItemRecords)
                    {
                        var optionCount = 0;
                        if (!item.HasCheckbox)
                            optionCount++;

                        if (item.OptionType1 == null)
                            optionCount++;

                        if (item.OptionType2 == null)
                            optionCount++;

                        if (item.OptionType3 == null)
                            optionCount++;

                        if (item.OptionType4 == null)
                            optionCount++;

                        if (item.HasCheckbox)
                        {
                            Cell itemCbox = SetCellValue(pdf, "Checkbox", item.IsChecked.ToString(), "", item.ItemId, 0);
                            table.AddCell(itemCbox);
                        }

                        string itemCellText = item.ItemText;
                        if (!String.IsNullOrEmpty(item.ITDropDownText1))
                            itemCellText = itemCellText + ": " + item.ITDropDownText1;

                        if (!String.IsNullOrEmpty(item.ITDropDownText2))
                            itemCellText = itemCellText + " - " + item.ITDropDownText2;

                        Cell itemCell = new Cell(1, 1 + optionCount).
                            Add(new Paragraph(itemCellText)
                                .SetFont(itemFont)
                                .SetFontSize(8)
                                .SetFixedLeading(7))
                                .SetBorder(Border.NO_BORDER);

                        table.AddCell(itemCell);

                        if (!String.IsNullOrEmpty(item.OptionType1))
                        {
                            Cell cbox1 = SetCellValue(pdf, item.OptionType1, item.IsOption1Selected.ToString(), item.Option1Text, item.ItemId, 1);
                            table.AddCell(cbox1);
                        }

                        if (!String.IsNullOrEmpty(item.OptionType2))
                        {
                            Cell cbox2 = SetCellValue(pdf, item.OptionType2, item.IsOption2Selected.ToString(), item.Option2Text, item.ItemId, 2);
                            table.AddCell(cbox2);
                        }

                        if (!String.IsNullOrEmpty(item.OptionType3))
                        {
                            Cell cbox3 = SetCellValue(pdf, item.OptionType3, item.IsOption3Selected.ToString(), item.Option3Text, item.ItemId, 3);
                            table.AddCell(cbox3);
                        }

                        if (!String.IsNullOrEmpty(item.OptionType4))
                        {
                            Cell cbox4 = SetCellValue(pdf, item.OptionType4, item.IsOption4Selected.ToString(), item.Option4Text, item.ItemId, 4);
                            table.AddCell(cbox4);
                        }

                        //prevent spacing problem in third column
                        Cell cboxSpacer = new Cell(1, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER);
                        cboxSpacer.SetHeight(5);
                        table.AddCell(cboxSpacer);

                    }
                }

                document.Add(table);

                AddFoldLines(pdf, offset, columnWidth, pageSize);

                InsertLogo(document, offset, columnWidth, pageSize);

                AddTitle(document, offset, columnWidth, pageSize, sectionFont);

                AddMetaData(document, checklistRecordID, offset, columnWidth, pageSize);  // dealership, mileage, year, model, VIN

                InsertFooter(document, offset, columnWidth, pageSize);

                Table inspectionPlateTable = new Table(UnitValue.CreatePercentArray(1));

                inspectionPlateTable.SetFixedPosition(offset + columnWidth * 2 + 10, offset, columnWidth - (offset + 20));

                 // adding page # etc.
                Cell inspectionCell2 = new Cell(1, 100)
                    .Add(new Paragraph("INSPECTION FORM"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(sectionFont).SetFontSize(9)
                    .SetBorder(Border.NO_BORDER);
                inspectionPlateTable.AddCell(inspectionCell2);

                document.Add(inspectionPlateTable);

                form.FlattenFields();
                document.Close();

                SqlMapperUtil.SqlWithParams<int>("insert into ztemp (info) values ('file created?')", null);
                return fileName;
            } catch (Exception ex)
            {
                
                SqlMapperUtil.SqlWithParams<int>("insert into ztemp (info) values ('Error below:')", null);
                string fixedErrorMessage = ex.Message.Replace("'", "");
//                string fixedErrorMessage = ex.Message.Replace(":", "-colon-");
                SqlMapperUtil.SqlWithParams<int>("insert into ztemp (info) values ('message: " + fixedErrorMessage + "')", null);
                SqlMapperUtil.SqlWithParams<int>("insert into ztemp (info) values ('stack trace: " + ex.StackTrace + "')", null);
 //               SqlMapperUtil.SqlWithParams<int>("insert into ztemp (info) values ('full: " + ex + "')", null);
            }

            return "";
        }

        private Cell SetCellValue(PdfDocument pdf, string optionType, string isOptionSelected, string optionValue, int id, int position)
        {
            Cell newCell = new Cell(1, 1).SetBorder(Border.NO_BORDER);
            switch (optionType)
            {
                case "Checkbox":
                    if (isOptionSelected.Equals("True", StringComparison.CurrentCultureIgnoreCase))
                    {
                        newCell.SetNextRenderer(new CheckboxCellRenderer(newCell, "cb" + id + "O" + position, 0, "Yes"));
                    }
                    else
                    {
                        newCell.SetNextRenderer(new CheckboxCellRenderer(newCell, "cb" + id + "O" + position, 0, "Off"));
                    }
                    break;
                case "Spare":
                case "Tread":
                    string fieldName = optionType + id + "O" + position;
                    //newCell.SetNextRenderer(new CreateFormFieldRenderer(newCell, fieldName));
                    //PdfAcroForm form = PdfAcroForm.GetAcroForm(pdf, true);
                    //PdfFormField tf = form.GetField(fieldName); ;
                    if (!String.IsNullOrEmpty(optionValue))
                    {
                        newCell.Add(new Paragraph(optionValue));
                    }
                    else
                    {
                        newCell.Add(new Paragraph(""));
                    }
                    break;
                case "Empty":
                    newCell.Add(new Paragraph(""));
                    break;
            }


            return newCell;
        }

        private void InsertLogo(Document document, float offset, float columnWidth, PageSize pageSize)
        {
            string imageFile = HttpContext.Current.Server.MapPath("~/Content/images/TandT_Fitzway_Logo_RED ON WHITE.png");
            ImageData data = ImageDataFactory.Create(imageFile);

            Image img = new Image(data);
            img.SetFixedPosition(offset + columnWidth * 2 + 36, (pageSize.GetHeight() * .96f) - img.GetImageHeight());
            document.Add(img);

        }

        private void InsertFooter(Document document, float offset, float columnWidth, PageSize pageSize)
        {
            string imageFile = HttpContext.Current.Server.MapPath("~/Content/images/FitzgeraldAutoMall1966_RETRO_VINTAGE_Logo_no tag.png");
            ImageData data = ImageDataFactory.Create(imageFile);

            Image img = new Image(data);
            //            img.SetFixedPosition(offset + columnWidth * 2 + 5, pageSize.GetHeight() - img.GetImageHeight());
            img.SetFixedPosition(offset + columnWidth * 2 + 34, offset + 20);
            document.Add(img);


        }


        private void AddFoldLines(PdfDocument pdf, float offset, float columnWidth, PageSize pageSize)
        {
            PdfCanvas canvas = new PdfCanvas(pdf.GetLastPage());

            canvas
                .SetStrokeColor(ColorConstants.LIGHT_GRAY)
                .MoveTo(offset + columnWidth - 5, offset)
                .LineTo(offset + columnWidth - 5, pageSize.GetHeight() - offset)
                .SetLineDash(5, 5, 2)
                .ClosePathStroke();

            canvas
                .SetStrokeColor(ColorConstants.LIGHT_GRAY)
                .MoveTo(offset + columnWidth * 2, offset)
                .LineTo(offset + columnWidth * 2, pageSize.GetHeight() - offset)
                .ClosePathStroke();
        }

        private void AddTitle(Document document, float offset, float columnWidth, PageSize pageSize, PdfFont sectionFont)
        {
            Table fitzwayTable = new Table(UnitValue.CreatePercentArray(1));
            fitzwayTable.SetFixedPosition(offset + columnWidth * 2 + 10, pageSize.GetHeight() / 2 + 65, columnWidth - (offset + 20));
            Cell fitzwayCell = new Cell(1, 100)
                .Add(new Paragraph("FITZWAY USED VEHICLE"))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(sectionFont).SetFontSize(16)
                .SetBorder(Border.NO_BORDER);
            fitzwayTable.AddCell(fitzwayCell);
            document.Add(fitzwayTable);

            fitzwayTable = new Table(UnitValue.CreatePercentArray(1));
            fitzwayTable.SetFixedPosition(offset + columnWidth * 2 + 10, pageSize.GetHeight() / 2 + 40, columnWidth - (offset + 20));
            fitzwayCell = new Cell(1, 100)
                            .Add(new Paragraph("INSPECTION"))
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(sectionFont).SetFontSize(24)
                            .SetBorder(Border.NO_BORDER);
            fitzwayTable.AddCell(fitzwayCell);
            document.Add(fitzwayTable);

        }

        private void AddMetaData(Document document, int checklistRecordID, float offset, float columnWidth, PageSize pageSize)
        {
            var inspectionMetadata = new InspectionMetadata();
            var theData = inspectionMetadata.GetInspectionMetadata(checklistRecordID);

            Table table = new Table(UnitValue.CreatePercentArray(10));
            table.SetFixedPosition(offset + columnWidth * 2 + 10, pageSize.GetHeight() / 2 - 80, columnWidth - (offset + 20));
            //table.SetWidth(UnitValue.CreatePercentValue(100));

            var metadataFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);
            var metadataFontBold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
            var dingbatsFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.ZAPFDINGBATS);

            var cellBackgroundGray = new DeviceRgb(0xEB, 0xEB, 0xEB);


            table.AddCell(new Cell(1, 1).Add(new Paragraph("F")).SetFont(dingbatsFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Dealership: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 7).Add(new Paragraph(theData.Dealership)).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("F")).SetFont(dingbatsFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Mileage: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 7).Add(new Paragraph(theData.Mileage)).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("F")).SetFont(dingbatsFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Year: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 7).Add(new Paragraph(theData.Year)).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("F")).SetFont(dingbatsFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("Model: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 7).Add(new Paragraph(theData.Model)).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));

            table.AddCell(new Cell(1, 1).Add(new Paragraph("F")).SetFont(dingbatsFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 2).Add(new Paragraph("VIN: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 7).Add(new Paragraph(theData.Vin)).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));

            document.Add(table);

            table = new Table(UnitValue.CreatePercentArray(20));
            table.SetFixedPosition(offset + columnWidth * 2 + 10, pageSize.GetHeight() / 4 - 50, columnWidth - (offset + 20));
            table.AddCell(new Cell(1, 3).Add(new Paragraph("Technician: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));

            var signature = GetSignature(checklistRecordID);
            table.AddCell(new Cell(1, 17).Add(new Paragraph(signature)).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));

            table.AddCell(new Cell(1, 2).Add(new Paragraph("Date: ")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 5).Add(new Paragraph(theData.InspectionDate.ToShortDateString())).SetFont(metadataFontBold).SetFontSize(8).SetBackgroundColor(cellBackgroundGray).SetBorder(Border.NO_BORDER));
            table.AddCell(new Cell(1, 13).Add(new Paragraph("")).SetBorder(Border.NO_BORDER));

            string techCertify = "I certify the information on this form was accurate at the time I inspected the vehicle and completed this form.";
            table.AddCell(new Cell(1, 20).Add(new Paragraph(techCertify)).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));

            table.AddCell(new Cell(1, 20).Add(new Paragraph("")).SetFont(metadataFont).SetFontSize(8).SetBorder(Border.NO_BORDER));

            Text asterix = new Text("*").SetFont(metadataFont).SetTextRise(1).SetFontSize(6);

            string exceptionText = "Except safety items, vehicle inspection results are adjusted to refelct the relative condition of a similar make and model vehicle with similar mileage";

            Paragraph exceptionParagraph = new Paragraph()
                                                .Add(asterix)
                                                .Add(exceptionText);
            table.AddCell(new Cell(1, 20).Add(exceptionParagraph).SetFont(metadataFont).SetFontSize(6).SetBorder(Border.NO_BORDER));

            document.Add(table);
            string Page2Info = ("" + theData.Vin + " / " + theData.InspectionDate + " / 2 of 2");
            Paragraph ParagraphPage2 = new Paragraph()
                                                .Add(Page2Info)
                                                .SetFontSize(8)
                                                .SetVerticalAlignment(VerticalAlignment.BOTTOM)
                                                .SetFixedPosition(columnWidth + 35,10,250);


            document.Add(ParagraphPage2);

        }

        public string GetSignature(int checklistRecordID)
        {
            var qs = @"SELECT 
                        LastName + ', ' + FirstName 
                    FROM [FITZDB].[dbo].[users] 
                    WHERE ID = (SELECT TOP 1 UserID 
                                    FROM[Checklists].[dbo].[ChecklistRecordHistory]
                                    WHERE ID = @ID AND Status = 2 
                                    ORDER BY DateUpdated DESC)";
            var signature = SqlMapperUtil.SqlWithParams<string>(qs, new { @ID = checklistRecordID }).FirstOrDefault();

            if (String.IsNullOrEmpty(signature))
            {
                qs = @"SELECT 
                        LastName + ', ' + FirstName 
                    FROM [FITZDB].[dbo].[users] 
                    WHERE ID = (SELECT UserID 
                                    FROM[Checklists].[dbo].[ChecklistRecord]
                                    WHERE ID = @ID)";
                signature = SqlMapperUtil.SqlWithParams<string>(qs, new { @ID = checklistRecordID }).FirstOrDefault();
            }
            return signature;
        }

        private void DisplayPDF()
        {

        }
    }
}