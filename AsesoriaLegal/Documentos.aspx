<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Documentos.aspx.cs" Inherits="AsesoriaLegal.Documentos" MasterPageFile="~/Site.master" %>

<asp:Content ID="cphDocumentos" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Mis Documentos</h1>
    <p class="subtitulo">Espacio privado de <asp:Label ID="lblUsuario" runat="server" />. Suba, descargue y elimine sus archivos legales.</p>

    <section class="panel-carga">
        <h2>Subir documento</h2>
        <asp:FileUpload ID="fuDocumento" runat="server" CssClass="control-archivo" />
        <asp:Button ID="btnSubir" runat="server"
            Text="Subir documento"
            CssClass="btn btn-primario"
            OnClick="btnSubir_Click" />
        <p class="nota">Formatos admitidos: PDF, DOCX, TXT, JPG, JPEG y PNG. Tamaño máximo: 5 MB por archivo.</p>
        <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="alert" />
    </section>

    <section>
        <h2>Archivos subidos</h2>
        <div class="tabla-scroll">
            <asp:GridView ID="gvDocumentos" runat="server"
                AutoGenerateColumns="False"
                CssClass="tabla-documentos"
                EmptyDataText="Todavía no subió documentos. ¡Comience subiendo el primero!"
                OnRowCommand="gvDocumentos_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre del archivo" />
                    <asp:BoundField DataField="TamanoTexto" HeaderText="Tamaño" />
                    <asp:BoundField DataField="FechaTexto" HeaderText="Fecha de subida" />
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <div class="acciones">
                                <asp:LinkButton ID="lnkDescargar" runat="server"
                                    CommandName="Descargar"
                                    CommandArgument='<%# Eval("Nombre") %>'
                                    CssClass="accion-link">Descargar</asp:LinkButton>
                                <asp:LinkButton ID="lnkEliminar" runat="server"
                                    CommandName="Eliminar"
                                    CommandArgument='<%# Eval("Nombre") %>'
                                    CssClass="accion-link accion-peligro"
                                    OnClientClick="return confirm('¿Seguro que desea eliminar este archivo? Esta acción no se puede deshacer.');">Eliminar</asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </section>
</asp:Content>
