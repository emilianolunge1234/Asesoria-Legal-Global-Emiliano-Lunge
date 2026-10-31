using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsesoriaLegal
{
    public partial class Registro : Page
    {
        protected Panel pnlExito;
        protected Label lblExito;
        protected Label lblError;
        protected ValidationSummary vsRegistro;
        protected TextBox txtEmail;
        protected TextBox txtUsuario;
        protected TextBox txtEdad;
        protected TextBox txtPassword;
        protected TextBox txtConfirmar;
        protected TextBox txtNombre;
        protected Button btnRegistrar;

        protected void Page_Init(object sender, EventArgs e)
        {
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Registrarse | Asesoría Legal Global";
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string usuario = txtUsuario.Text.Trim();
            string nombreCompleto = txtNombre.Text.Trim();
            string clave = txtPassword.Text;

            int edad;
            if (!int.TryParse(txtEdad.Text.Trim(), out edad) || edad < 16 || edad > 120)
            {
                MostrarErrorLocal("Debe tener entre 16 y 120 años para registrarse.");
                return;
            }

            if (clave != txtConfirmar.Text)
            {
                MostrarErrorLocal("Las contraseñas no coinciden.");
                return;
            }

            AppUser nuevo;
            string motivo;
            if (!UserStore.TryRegister(email, usuario, edad, nombreCompleto, clave, out nuevo, out motivo))
            {
                MostrarErrorLocal(motivo);
                return;
            }

            Session["Usuario"] = nuevo.Username;
            Session["NombreCompleto"] = nuevo.FullName;

            HttpCookie cookieClave = new HttpCookie("ALG_Clave", Server.UrlEncode(clave));
            cookieClave.HttpOnly = true;
            cookieClave.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(cookieClave);

            lblExito.Text = Server.HtmlEncode(
                "¡Registro exitoso! Bienvenido/a, " + nuevo.FullName +
                ". Será redirigido/a a la página de inicio de sesión en unos segundos.");
            pnlExito.Visible = true;

            ClientScript.RegisterStartupScript(
                GetType(),
                "RedirigirALogin",
                "setTimeout(function () { window.location = 'Login.aspx'; }, 2500);",
                true);
        }

        private void MostrarErrorLocal(string mensaje)
        {
            lblError.Text = Server.HtmlEncode(mensaje);
            lblError.Visible = true;
            pnlExito.Visible = false;
        }
    }
}
