<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AsesoriaLegal.Default" MasterPageFile="~/Site.master" %>

<asp:Content ID="cphPrincipal" ContentPlaceHolderID="MainContent" runat="server">
    <section class="hero">
        <img class="hero-imagen" src="<%: ResolveUrl("~/img/banner-legal.png") %>" alt="Banner de Asesoría Legal Global" />
        <div class="hero-texto">
            <h1>Asesoría Legal Global</h1>
            <p class="hero-subtitulo">Protegemos sus derechos con experiencia, transparencia y compromiso. Asesoramiento jurídico para personas y empresas, de principio a fin.</p>
            <div class="hero-acciones">
                <asp:HyperLink ID="lnkCtaRegistro" runat="server" NavigateUrl="~/Registro.aspx" CssClass="btn btn-primario">Crear mi cuenta</asp:HyperLink>
                <asp:HyperLink ID="lnkCtaLogin" runat="server" NavigateUrl="~/Login.aspx" CssClass="btn btn-secundario">Ya tengo cuenta</asp:HyperLink>
            </div>
        </div>
    </section>

    <section>
        <h2>Nuestros servicios</h2>
        <div class="card-grid">
            <article class="card">
                <h3>Asesoría Corporativa</h3>
                <p>Constitución de sociedades, actas, libros societarios y acompañamiento legal continuo para su empresa.</p>
            </article>
            <article class="card">
                <h3>Derecho Laboral</h3>
                <p>Contratos de trabajo, convenios, liquidaciones y defensa ante conflictos laborales, tanto para empleadores como para trabajadores.</p>
            </article>
            <article class="card">
                <h3>Contratos y Acuerdos</h3>
                <p>Redacción, revisión y negociación de contratos civiles y comerciales con cláusulas claras y a medida.</p>
            </article>
        </div>
    </section>

    <section>
        <h2>¿Por qué elegirnos?</h2>
        <div class="card-grid">
            <article class="card">
                <h3>Confidencialidad</h3>
                <p>Cada cliente cuenta con un espacio privado donde guardar y gestionar sus documentos legales.</p>
            </article>
            <article class="card">
                <h3>Transparencia</h3>
                <p>Presupuestos claros, sin sorpresas. Le explicamos cada paso del proceso en un lenguaje sencillo.</p>
            </article>
            <article class="card">
                <h3>Atención personalizada</h3>
                <p>Un asesor asignado a su caso, disponible para responder todas sus consultas.</p>
            </article>
        </div>
        <p class="nota" style="text-align: center; margin-top: 1.5rem;">Regístrese hoy y comience a gestionar sus documentos en línea desde "Mis Documentos".</p>
    </section>
</asp:Content>
