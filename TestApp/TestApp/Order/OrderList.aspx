<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderList.aspx.cs" Inherits="TestApp.OrderList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="~/Dashboard.aspx" id="lnkDashboard" runat="server">Dashboard</a></li>
            <li class="breadcrumb-item active" aria-current="page">Order List</li>
        </ol>
    </nav>
    <div class="col-12">
        <h2>
            <span>Order Search</span>
        </h2>
        <hr />
    </div>
    <div class="col-12">
        <div class="card">
            <div class="card-header">
                <h4 class="float-left">Search Criteria</h4>
                <div class="float-right">
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-warning" onclick="btnClear_Click"><i class="fas fa-eraser"></i>&nbsp;&nbsp;Clear</asp:LinkButton>
                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-primary" onclick="btnSearch_Click"><i class="fas fa-search"></i>&nbsp;&nbsp;Search</asp:LinkButton>
                </div>
            </div>
            <div class="card-body">
                <div class="row">
                    <div class="col-3">
                        Order Number
                        <asp:TextBox ID="txtOrdNum" runat="server" class="form-control" />
                    </div>
                    <div class="col-3">
                        Name
                        <asp:TextBox ID="txtSrch" runat="server" class="form-control" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-12">
        <div class="row">
            <div class="col-6">
                <div class="text-left">
                    <br />
                    <asp:Label ID = "lblTtlProfit" runat="server" Text=""/>
                </div>
            </div>
            <div class="col-6">
                <div class="text-right">
                    <br />
                    <asp:Label ID="lblGridCount" runat="server" Text="" />
                </div>
                <br />
                <br />
            </div>
        </div>
    </div>
    <div class="col-12">
        <div class="table-responsive">
            <asp:GridView ID="gvOrder" runat="server" CssClass="table table-sm table-hover" AutoGenerateColumns="False" EnableModelValidation="True" DataKeyNames="ID" onrowcommand="gvOrder_RowCommand" AllowSorting="true" OnSorting="gvOrder_Sorting">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" ItemStyle-CssClass="hidden-print" HeaderStyle-CssClass="hidden-print">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnSelect" class="btn btn-sm tableButton" runat="server" CommandName="Select" CommandArgument='<%# Eval("ID") %>'><i class="fas fa-credit-card"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="false" />
                    <asp:BoundField DataField="OrderNum" HeaderText="Order Number" ReadOnly="True" SortExpression="OrderNum" />
                    <asp:BoundField DataField="Username" HeaderText="Username" ReadOnly="True" SortExpression="Username" />
                    <asp:BoundField DataField="Price" HeaderText="Price" ReadOnly="True" SortExpression="Price" />
                    <asp:BoundField DataField="Paid" HeaderText="Is Paid" ReadOnly="True" SortExpression="Paid" />
                    <asp:BoundField DataField="OrderDate" HeaderText="Date Ordered" ReadOnly="True" SortExpression="OrderDate" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJS" runat="server">
    <script>
        $(function () {
            $('[data-toggle="popover"]').popover()
        })
    </script>
</asp:Content>