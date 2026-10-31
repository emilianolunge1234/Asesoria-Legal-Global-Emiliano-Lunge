using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsesoriaLegal
{
    public partial class Site : MasterPage
    {
        protected ContentPlaceHolder MainContent;
        protected HyperLink lnkInicio;
        protected HyperLink lnkRegistro;
        protected HyperLink lnkLogin;
        protected HyperLink lnkDocumentos;
        protected Label lblBienvenido;
        protected LinkButton lnkSalir;

        protected void Page_Load(object sender, EventArgs e)
        {
            string usuario = Session["Usuario"] as string;
            bool logueado = !string.IsNullOrEmpty(usuario);

            lnkInicio.Visible = logueado;
            lnkDocumentos.Visible = logueado;
            lblBienvenido.Visible = logueado;
            lnkSalir.Visible = logueado;

            lnkRegistro.Visible = !logueado;
            lnkLogin.Visible = !logueado;

            if (logueado)
            {
                lblBienvenido.Text = Server.HtmlEncode("Hola, " + usuario);
            }
        }

        protected void lnkSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            ExpireCookie("ALG_Usuario");
            ExpireCookie("ALG_Clave");

            Response.Redirect(Page.ResolveUrl("~/Login.aspx"));
        }

        private void ExpireCookie(string nombre)
        {
            HttpCookie vieja = Request.Cookies[nombre];
            if (vieja != null)
            {
                HttpCookie expirada = new HttpCookie(nombre);
                expirada.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(expirada);
            }
        }
    }
}
