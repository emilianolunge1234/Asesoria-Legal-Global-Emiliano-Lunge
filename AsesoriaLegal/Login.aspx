<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AsesoriaLegal.Login" MasterPageFile="~/Site.master" %>

<asp:Content ID="cphLogin" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-card">
        <h1>Iniciar Sesión</h1>
        <p class="subtitulo">Acceda a su espacio personal de documentos.</p>

        <asp:Label ID="lblError" runat="server" Visible="false" CssClass="alert alert-error" />

        <asp:ValidationSummary ID="vsLogin" runat="server"
            ValidationGroup="login"
            ShowMessageBox="false"
            ShowSummary="true"
            HeaderText="Complete los siguientes campos:"
            CssClass="validacion-resumen" />

        <div class="grupo-control">
            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" CssClass="control-label">Nombre de usuario</asp:Label>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="control-form" MaxLength="30" placeholder="Su nombre de usuario" />
            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server"
                ControlToValidate="txtUsuario"
                ValidationGroup="login"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="El nombre de usuario es obligatorio." />
        </div>

        <div class="grupo-control">
            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="control-label">Contraseña</asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="control-form" TextMode="Password" placeholder="Su contraseña" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                ControlToValidate="txtPassword"
                ValidationGroup="login"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="La contraseña es obligatoria." />
        </div>

        <div class="casilla-recordar">
            <asp:CheckBox ID="chkRecordar" runat="server" />
            <asp:Label ID="lblRecordar" runat="server" AssociatedControlID="chkRecordar">Recordarme en este equipo</asp:Label>
        </div>

        <asp:Button ID="btnIngresar" runat="server"
            Text="Iniciar Sesión"
            CssClass="btn btn-primario btn-bloque"
            ValidationGroup="login"
            OnClick="btnIngresar_Click" />

        <p class="form-enlace">¿Todavía no tiene cuenta? <asp:HyperLink ID="lnkARegistro" runat="server" NavigateUrl="~/Registro.aspx">Regístrese aquí</asp:HyperLink>.</p>
    </div>
</asp:Content>
