using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Layout.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitzCheckout.PDF
{
    class CheckboxCellRenderer : CellRenderer
    {
        // The name of the check box field
        protected String name;
        protected int checkboxTypeIndex;
        protected string value;

        public CheckboxCellRenderer(Cell modelElement, String name, int checkboxTypeIndex, string value)
            : base(modelElement)
        {
            this.name = name;
            this.checkboxTypeIndex = checkboxTypeIndex;
            this.value = value;
        }

        // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
        // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
        // renderer will be created
        public override IRenderer GetNextRenderer()
        {
            return new CheckboxCellRenderer((Cell)modelElement, name, checkboxTypeIndex, "Off");
        }

        public override void Draw(DrawContext drawContext)
        {
            Rectangle position = GetOccupiedAreaBBox();
            PdfAcroForm form = PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true);

            // Define the coordinates of the middle
            float x = (position.GetLeft() + position.GetRight()) / 2;
            float y = (position.GetTop() + position.GetBottom()) / 2;

            // Define the position of a check box that measures 20 by 20
            Rectangle rect = new Rectangle(x - 5, y - 5, 10, 10);

            // The 4th parameter is the initial value of checkbox: 'Yes' - checked, 'Off' - unchecked
            // By default, checkbox value type is cross.
            PdfButtonFormField checkBox =
                PdfFormField.CreateCheckBox(drawContext.GetDocument(), rect, this.name, this.value);
            switch (checkboxTypeIndex)
            {
                case 0:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_CHECK);
                        checkBox.SetVisibility(PdfFormField.VISIBLE);
                        checkBox.SetBorderWidth(1);
                        checkBox.SetBorderColor(ColorConstants.BLACK);
                        // Use this method if you changed any field parameters and didn't use setValue
                        //checkBox.RegenerateField();
                        break;
                    }

                case 1:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_CIRCLE);
                        checkBox.RegenerateField();
                        break;
                    }

                case 2:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_CROSS);
                        checkBox.RegenerateField();
                        break;
                    }

                case 3:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_DIAMOND);
                        checkBox.RegenerateField();
                        break;
                    }

                case 4:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_SQUARE);
                        checkBox.RegenerateField();
                        break;
                    }

                case 5:
                    {
                        checkBox.SetCheckType(PdfFormField.TYPE_STAR);
                        checkBox.RegenerateField();
                        break;
                    }
            }

            form.AddField(checkBox);
            //form.FlattenFields();
        }
    }
}
