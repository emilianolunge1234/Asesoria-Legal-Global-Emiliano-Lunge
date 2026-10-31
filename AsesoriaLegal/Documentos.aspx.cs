using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AsesoriaLegal
{
    public class DocumentoInfo
    {
        public string Nombre { get; set; }
        public string TamanoTexto { get; set; }
        public string FechaTexto { get; set; }
    }

    public partial class Documentos : Page
    {
        private const int TamanoMaximoBytes = 5 * 1024 * 1024;

        private static readonly string[] ExtensionesPermitidas =
            { ".pdf", ".docx", ".txt", ".jpg", ".jpeg", ".png" };

        private static readonly string[] ExtensionesPeligrosas =
            { ".exe", ".bat", ".cmd", ".scr", ".msi", ".pif", ".vbs", ".js", ".jar" };

        protected Label lblUsuario;
        protected FileUpload fuDocumento;
        protected Button btnSubir;
        protected Label lblMensaje;
        protected GridView gvDocumentos;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect(Page.ResolveUrl("~/Login.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
                Response.SuppressContent = true;
                return;
            }

            lblUsuario.Text = Server.HtmlEncode(Session["Usuario"].ToString());

            BindFiles();
        }

        protected void btnSubir_Click(object sender, EventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect(Page.ResolveUrl("~/Login.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!fuDocumento.HasFile || fuDocumento.PostedFile == null || fuDocumento.PostedFile.ContentLength == 0)
            {
                MostrarMensaje("Debe seleccionar un archivo para subir.", false);
                return;
            }

            if (fuDocumento.PostedFile.ContentLength > TamanoMaximoBytes)
            {
                MostrarMensaje("El archivo supera el tamaño máximo permitido de 5 MB.", false);
                return;
            }

            string nombreCliente = Path.GetFileName(fuDocumento.FileName);

            string extension = Path.GetExtension(nombreCliente);
            if (string.IsNullOrEmpty(extension))
            {
                MostrarMensaje("El archivo no tiene extensión y no puede subirse.", false);
                return;
            }

            string extensionNormalizada = extension.ToLowerInvariant();
            if (Array.IndexOf(ExtensionesPermitidas, extensionNormalizada) < 0)
            {
                MostrarMensaje("Tipo de archivo no permitido. Formatos admitidos: PDF, DOCX, TXT, JPG, JPEG y PNG.", false);
                return;
            }

            string[] partes = nombreCliente.Split('.');
            for (int i = 0; i < partes.Length - 1; i++)
            {
                string parte = "." + partes[i].Trim().ToLowerInvariant();
                if (Array.IndexOf(ExtensionesPeligrosas, parte) >= 0)
                {
                    MostrarMensaje("El nombre del archivo contiene una extensión no permitida.", false);
                    return;
                }
            }

            string baseSegura = SanitizarNombre(Path.GetFileNameWithoutExtension(nombreCliente), "documento");
            string nombreFinal = baseSegura + extensionNormalizada;

            string carpeta = ObtenerCarpetaUsuario();
            string destino = EvitarSobreescritura(Path.Combine(carpeta, nombreFinal), extensionNormalizada);

            try
            {
                fuDocumento.SaveAs(destino);
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo guardar el archivo: " + ex.Message, false);
                return;
            }

            MostrarMensaje("Documento subido correctamente.", true);
            BindFiles();
        }

        protected void gvDocumentos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Session["Usuario"] == null)
            {
                Response.Redirect(Page.ResolveUrl("~/Login.aspx"), false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            string nombre = e.CommandArgument as string;
            string carpeta = ObtenerCarpetaUsuario();

            string ruta;
            string error;
            if (!IntentarResolverRuta(carpeta, nombre, out ruta, out error))
            {
                MostrarMensaje(error, false);
                return;
            }

            if (string.Equals(e.CommandName, "Descargar", StringComparison.OrdinalIgnoreCase))
            {
                DescargarArchivo(ruta, nombre);
            }
            else if (string.Equals(e.CommandName, "Eliminar", StringComparison.OrdinalIgnoreCase))
            {
                EliminarArchivo(ruta);
            }
        }

        private void DescargarArchivo(string ruta, string nombre)
        {
            if (!File.Exists(ruta))
            {
                MostrarMensaje("El archivo ya no existe.", false);
                BindFiles();
                return;
            }

            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = MimeMapping.GetMimeMapping(nombre);
            Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + nombre + "\"");
            Response.TransmitFile(ruta);
            Response.End();
        }

        private void EliminarArchivo(string ruta)
        {
            try
            {
                if (File.Exists(ruta))
                {
                    File.Delete(ruta);
                }
                MostrarMensaje("Archivo eliminado correctamente.", true);
            }
            catch (Exception ex)
            {
                MostrarMensaje("No se pudo eliminar el archivo: " + ex.Message, false);
            }
            BindFiles();
        }

        private void BindFiles()
        {
            string carpeta = ObtenerCarpetaUsuario();
            List<DocumentoInfo> archivos = new List<DocumentoInfo>();

            foreach (string ruta in Directory.GetFiles(carpeta))
            {
                FileInfo info = new FileInfo(ruta);
                archivos.Add(new DocumentoInfo
                {
                    Nombre = info.Name,
                    TamanoTexto = FormatearTamano(info.Length),
                    FechaTexto = info.LastWriteTime.ToString("dd/MM/yyyy HH:mm")
                });
            }

            archivos.Sort((a, b) => string.Compare(a.Nombre, b.Nombre, StringComparison.OrdinalIgnoreCase));

            gvDocumentos.DataSource = archivos;
            gvDocumentos.DataBind();
        }

        private string ObtenerCarpetaUsuario()
        {
            string usuario = Session["Usuario"].ToString();
            string carpetaNombre = SanitizarNombre(usuario, "usuario");
            string baseDir = Server.MapPath("~/App_Data/Documentos");
            string carpeta = Path.Combine(baseDir, carpetaNombre);
            Directory.CreateDirectory(carpeta);
            return carpeta;
        }

        private static bool IntentarResolverRuta(string carpetaUsuario, string nombreArchivo, out string rutaCompleta, out string error)
        {
            rutaCompleta = null;
            error = null;

            if (string.IsNullOrEmpty(nombreArchivo) || nombreArchivo.Trim().Length == 0)
            {
                error = "Nombre de archivo no válido.";
                return false;
            }

            bool inseguro = nombreArchivo.IndexOf("..", StringComparison.Ordinal) >= 0
                || nombreArchivo.IndexOf('/') >= 0
                || nombreArchivo.IndexOf('\\') >= 0
                || nombreArchivo.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0;

            if (inseguro)
            {
                error = "Nombre de archivo no válido.";
                return false;
            }

            string carpeta = Path.GetFullPath(carpetaUsuario);
            string completa = Path.GetFullPath(Path.Combine(carpeta, nombreArchivo));
            string prefijo = carpeta + Path.DirectorySeparatorChar;

            if (!completa.StartsWith(prefijo, StringComparison.OrdinalIgnoreCase))
            {
                error = "Ruta de archivo no válida.";
                return false;
            }

            rutaCompleta = completa;
            return true;
        }

        private static string SanitizarNombre(string valor, string respaldo)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in valor ?? string.Empty)
            {
                if (char.IsLetterOrDigit(c) || c == ' ' || c == '.' || c == '-' || c == '_')
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('_');
                }
            }

            string resultado = sb.ToString().Trim().Trim('.');
            if (resultado.Length == 0)
            {
                resultado = respaldo;
            }
            if (resultado.Length > 80)
            {
                resultado = resultado.Substring(0, 80);
            }
            return resultado;
        }

        private static string EvitarSobreescritura(string destino, string extension)
        {
            if (!File.Exists(destino))
            {
                return destino;
            }

            string carpeta = Path.GetDirectoryName(destino);
            string baseNombre = Path.GetFileNameWithoutExtension(destino);

            for (int i = 1; i < 1000; i++)
            {
                string candidato = Path.Combine(carpeta, baseNombre + " (" + i + ")" + extension);
                if (!File.Exists(candidato))
                {
                    return candidato;
                }
            }

            return Path.Combine(carpeta, baseNombre + "_" + Guid.NewGuid().ToString("N").Substring(0, 8) + extension);
        }

        private static string FormatearTamano(long bytes)
        {
            if (bytes < 1024)
            {
                return bytes + " B";
            }
            if (bytes < 1024 * 1024)
            {
                return (bytes / 1024.0).ToString("0.#") + " KB";
            }
            return (bytes / (1024.0 * 1024.0)).ToString("0.##") + " MB";
        }

        private void MostrarMensaje(string texto, bool exito)
        {
            lblMensaje.Text = Server.HtmlEncode(texto);
            lblMensaje.CssClass = exito ? "alert alert-exito" : "alert alert-error";
            lblMensaje.Visible = true;
        }
    }
}
