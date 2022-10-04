<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="TestApp.Signup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="col-8">
        <div class="form-group">
            First Name
            <asp:TextBox ID="txtFName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                First name field is required.
            </div>
        </div>
        <div class="form-group">
            Middle Name(s)
            <asp:TextBox ID="txtMName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                Middle name field is required
            </div>
        </div>
        <div class="form-group">
            Last Name
            <asp:TextBox ID="txtLName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                Last name field is required.
            </div>
        </div>
    
        <div class="form-group">
            Username
            <asp:TextBox ID="txtUsrName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                Username field is required.
            </div>
        </div>
        <div class="form-group">
            Email
            <asp:TextBox ID="txtEmail" runat="server" class="form-control" />
            <div class="invalid-feedback">
                <asp:label ID="lblEmailError" Text="" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <h2>                    
                <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" onclick="btnSave_Click" onclientclick="return CheckIsRepeatPostback();"><i class="fas fa-save"></i>&nbsp;&nbsp;Save</asp:LinkButton>
                <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-warning" onclick="btnCancel_Click"><i class="fas fa-times"></i>&nbsp;&nbsp;Cancel</asp:LinkButton>
            </h2>
        </div>
    </div>
</asp:Content>
