<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="AsesoriaLegal.Registro" MasterPageFile="~/Site.master" %>

<asp:Content ID="cphRegistro" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-card">
        <h1>Registrarse</h1>
        <p class="subtitulo">Cree su cuenta para gestionar sus documentos legales en línea.</p>

        <asp:Panel ID="pnlExito" runat="server" Visible="false" CssClass="alert alert-exito">
            <asp:Label ID="lblExito" runat="server" />
        </asp:Panel>

        <asp:Label ID="lblError" runat="server" Visible="false" CssClass="alert alert-error" />

        <asp:ValidationSummary ID="vsRegistro" runat="server"
            ValidationGroup="registro"
            ShowMessageBox="false"
            ShowSummary="true"
            HeaderText="Por favor corrija los siguientes errores:"
            CssClass="validacion-resumen" />

        <div class="grupo-control">
            <asp:Label ID="lblEmail" runat="server" AssociatedControlID="txtEmail" CssClass="control-label">Correo electrónico</asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="control-form" TextMode="Email" placeholder="ejemplo@correo.com" />
            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                ControlToValidate="txtEmail"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="El correo electrónico es obligatorio." />
            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                ControlToValidate="txtEmail"
                ValidationExpression="^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,}$"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="Ingrese un correo electrónico válido." />
        </div>

        <div class="grupo-control">
            <asp:Label ID="lblUsuario" runat="server" AssociatedControlID="txtUsuario" CssClass="control-label">Nombre de usuario</asp:Label>
            <asp:TextBox ID="txtUsuario" runat="server" CssClass="control-form" MaxLength="30" placeholder="Su nombre de usuario" />
            <asp:RequiredFieldValidator ID="rfvUsuario" runat="server"
                ControlToValidate="txtUsuario"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="El nombre de usuario es obligatorio." />
        </div>

        <div class="grupo-control">
            <asp:Label ID="lblEdad" runat="server" AssociatedControlID="txtEdad" CssClass="control-label">Edad</asp:Label>
            <asp:TextBox ID="txtEdad" runat="server" CssClass="control-form" TextMode="Number" placeholder="Ej.: 25" />
            <asp:RequiredFieldValidator ID="rfvEdad" runat="server"
                ControlToValidate="txtEdad"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="La edad es obligatoria." />
            <asp:RangeValidator ID="rvEdad" runat="server"
                ControlToValidate="txtEdad"
                Type="Integer"
                MinimumValue="16"
                MaximumValue="120"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="Debe tener entre 16 y 120 años para registrarse." />
        </div>

        <div class="grupo-control">
            <asp:Label ID="lblPassword" runat="server" AssociatedControlID="txtPassword" CssClass="control-label">Contraseña</asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="control-form" TextMode="Password" placeholder="Mínimo 6 caracteres" />
            <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                ControlToValidate="txtPassword"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="La contraseña es obligatoria." />
        </div>

        <div class="grupo-control">
            <asp:Label ID="lblConfirmar" runat="server" AssociatedControlID="txtConfirmar" CssClass="control-label">Confirmar contraseña</asp:Label>
            <asp:TextBox ID="txtConfirmar" runat="server" CssClass="control-form" TextMode="Password" placeholder="Repita su contraseña" />
            <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server"
                ControlToValidate="txtConfirmar"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="Debe confirmar su contraseña." />
            <asp:CompareValidator ID="cvPassword" runat="server"
                ControlToValidate="txtConfirmar"
                ControlToCompare="txtPassword"
                Operator="Equal"
                Type="String"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="Las contraseñas no coinciden." />
        </div>

        <div class="grupo-control">
            <asp:Label ID="lblNombre" runat="server" AssociatedControlID="txtNombre" CssClass="control-label">Nombre y apellido</asp:Label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="control-form" MaxLength="80" placeholder="Ej.: Ana García" />
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre"
                ValidationGroup="registro"
                Display="Dynamic"
                CssClass="text-danger"
                Text="*"
                ErrorMessage="El nombre y apellido es obligatorio." />
        </div>

        <asp:Button ID="btnRegistrar" runat="server"
            Text="Registrar"
            CssClass="btn btn-primario btn-bloque"
            ValidationGroup="registro"
            OnClick="btnRegistrar_Click" />

        <p class="form-enlace">¿Ya tiene cuenta? <asp:HyperLink ID="lnkALogin" runat="server" NavigateUrl="~/Login.aspx">Inicie sesión aquí</asp:HyperLink>.</p>
    </div>
</asp:Content>
