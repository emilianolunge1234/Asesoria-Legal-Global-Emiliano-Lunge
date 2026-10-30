using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsesoriaLegal
{
    public partial class Login : Page
    {
        protected Label lblError;
        protected ValidationSummary vsLogin;
        protected TextBox txtUsuario;
        protected TextBox txtPassword;
        protected CheckBox chkRecordar;
        protected Button btnIngresar;

        protected void Page_Init(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Iniciar SesiÃ³n | AsesorÃ­a Legal Global";

            if (Session["Usuario"] != null)
            {
                Response.Redirect(Page.ResolveUrl("~/Documentos.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                HttpCookie cookie = Request.Cookies["ALG_Usuario"];
                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    txtUsuario.Text = Server.UrlDecode(cookie.Value);
                    chkRecordar.Checked = true;
                }
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtPassword.Text;

            AppUser encontrado = UserStore.Authenticate(usuario, clave);
            if (encontrado == null)
            {
                lblError.Text = "Usuario o contraseÃ±a incorrectos. Verifique sus datos o regÃ­strese.";
                lblError.Visible = true;
                return;
            }

            Session["Usuario"] = encontrado.Username;
            Session["NombreCompleto"] = encontrado.FullName;

            if (chkRecordar.Checked)
            {
                HttpCookie cookie = new HttpCookie("ALG_Usuario", Server.UrlEncode(encontrado.Username));
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(30);
                Response.Cookies.Add(cookie);
            }
            else if (Request.Cookies["ALG_Usuario"] != null)
            {
                HttpCookie vieja = new HttpCookie("ALG_Usuario");
                vieja.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(vieja);
            }

            Response.Redirect(Page.ResolveUrl("~/Documentos.aspx"));
        }
    }
}
