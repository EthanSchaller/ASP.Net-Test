<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="TestApp.PasswordRecovery" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
        <div class="col-12">
            <h2>
                <span><asp:Label ID="lblHeader" runat="server" Text="Password Recovery" /></span>
            </h2>
            <hr />
        </div>
    </div>
    <div id="divPasswordRecovery" runat="server">
        <div class="row">
            <div class="col-6">
                Enter your Email
                <asp:Textbox id="txtEmail" class="form-control" runat="server" AutoCompleteType="Disabled" MaxLength="512" />
                <div class="invalid-feedback">
                    Email field is required.
                </div>
            </div>   
        </div> 
        <div class="row">
            <div class="col-6" style="text-align:right; padding-top: 5px;">
                <asp:LinkButton ID="btnResetPassword" runat="server" CssClass="btn btn-md btn-primary" onclick="btnResetPassword_Click"><i class="fas fa-sync-alt"></i>&nbsp;&nbsp;Reset Password</asp:LinkButton>
            </div>
        </div>
    </div>
    <div id="divEmailSent" runat="server" visible="false">
        <div class="col alert-success">
            <br />
            A new password has been emailed to the address
            <br /><br />
        </div>
    </div>
    <div id="divResetPassword" runat="server" visible="false">
        <div class="col-6">
            <div class="form-group">
                New Password<br />
                <asp:Textbox id="txtNewPassword" class="form-control" runat="server" MaxLength="64" TextMode="Password" />
                <div class="invalid-feedback">
                    A new password must be entered.
                </div>
            </div>
            <div class="form-group">
               Confirm New Password<br />
                <asp:Textbox id="txtConfirmPassword" class="form-control" runat="server" MaxLength="64" TextMode="Password" />
                <div class="invalid-feedback">
                    You must confirm the new password.
                </div>
            </div>
            <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-md btn-primary pull-right" onclick="btnReset_Click"><i class="fas fa-sync-alt"></i>&nbsp;&nbsp;Reset</asp:LinkButton>
        </div>
    </div>
    <input id="hidHash" type="hidden" value="" runat="server" />
    <input id="hidCreate" type="hidden" value="" runat="server" />
</asp:Content>
