using System;

namespace AsesoriaLegal
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect(Page.ResolveUrl("~/Login.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
                Response.SuppressContent = true;
                return;
            }

            Page.Title = "Inicio | Asesoría Legal Global";
        }
    }
}
