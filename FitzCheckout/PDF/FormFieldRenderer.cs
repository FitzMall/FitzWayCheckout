using iText.Forms;
using iText.Forms.Fields;
using iText.Layout.Element;
using iText.Layout.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitzCheckout.PDF
{
     class CreateFormFieldRenderer : CellRenderer
    {
        protected String fieldName;

        public CreateFormFieldRenderer(Cell modelElement, String fieldName)
            : base(modelElement)
        {
            this.fieldName = fieldName;
        }

        // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
        // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
        // renderer will be created
        public override IRenderer GetNextRenderer()
        {
            return new CreateFormFieldRenderer((Cell)modelElement, fieldName);
        }

        public override void Draw(DrawContext drawContext)
        {
            base.Draw(drawContext);

            PdfTextFormField field = PdfFormField.CreateText(drawContext.GetDocument(),
                GetOccupiedAreaBBox(), fieldName, "");
            PdfAcroForm form = PdfAcroForm.GetAcroForm(drawContext.GetDocument(), true);
            form.AddField(field);
        }
    }
}
