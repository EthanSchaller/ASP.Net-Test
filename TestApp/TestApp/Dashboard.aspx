<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TestApp.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="col-12">
        <h2>
            <span>Dashboard</span>
        </h2>
        <hr />
    </div>
    <div id="ownerDashboard" class="col-12" runat="server">
        <span>This is the owner's dashboard.</span>
        <br />
        <span>Click the menu options on the left to enter the various pages.</span>
    </div>
    <div id="adminDashboard" class="col-12" runat="server">
        <span>This is the admin's dashboard.</span>
        <br />
        <span>Click the menu options on the left to enter the various pages.</span>
    </div>
    <div id="guestDashboard" class="col-12" runat="server">
        <span>This is the dashboard for the site.</span>
        <br />
        <span>Click the menu options on the left to enter the products page.</span>
    </div>
</asp:Content>

