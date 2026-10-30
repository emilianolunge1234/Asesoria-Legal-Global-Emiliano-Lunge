using System;
using System.IO;
using System.Web;

namespace AsesoriaLegal
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            string baseFolder = HttpContext.Current.Server.MapPath("~/App_Data/Documentos");
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
        }
    }
}
