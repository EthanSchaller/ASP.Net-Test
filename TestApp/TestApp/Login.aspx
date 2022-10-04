<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TestApp.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="col-8">
        <div class="form-group">
            Username
            <asp:TextBox ID="txtUsername" runat="server" class="form-control" />
        </div>
        <div class="form-group">
            Password
            <asp:TextBox ID="txtPassword" runat="server" class="form-control" TextMode="Password" />
        </div>
        <div class="form-group">
            <asp:LinkButton ID="btnLogin" runat="server" CssClass="btn btn-md btn-primary" onclick="btnLogin_Click"><i class="fas fa-user"></i>&nbsp;&nbsp;Login</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="btnSignup" runat="server" CssClass="btn btn-md btn-warning" onclick="btnSignup_Click"><i class="fas fa-user-plus"></i>&nbsp;&nbsp;Sign Up</asp:LinkButton>
        </div>
        <div class="form-group">
            <a href="User/PasswordRecovery.aspx">Did you forget your password?</a>
        </div>
    </div>
    <input id="hidUTCOffset" type="hidden" value="" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJS" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var d = new Date()
            var n = d.getTimezoneOffset();

            document.getElementById("<%= hidUTCOffset.ClientID %>").value = n;
        });
    </script>
</asp:Content>
