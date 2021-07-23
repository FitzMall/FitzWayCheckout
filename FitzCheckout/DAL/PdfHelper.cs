using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Web;

namespace FitzCheckout.DAL
{
    public static class PdfHelper
    {
        public static bool DoesPdfExist(string filename, string filepath)
        {

            string file = System.IO.Path.Combine(filepath, filename);
            var fileInfo = new FileInfo(file);
            if (fileInfo.Exists)
                return true;
            else
                return false;

        }
    }
}