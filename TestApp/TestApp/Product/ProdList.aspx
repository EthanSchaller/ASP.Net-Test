<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProdList.aspx.cs" Inherits="TestApp.ProdsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="~/Dashboard.aspx" id="lnkDashboard" runat="server">Dashboard</a></li>
            <li class="breadcrumb-item active" aria-current="page">Product List</li>
        </ol>
    </nav>
    <div class="col-12">
        <h2>
            <span>Product Search</span>
            <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-success float-right" onclick="btnAdd_Click"><i class="fa fa-plus"></i>&nbsp;&nbsp;Add New</asp:LinkButton>
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
                    Name
                    <asp:TextBox ID="txtSrch" runat="server" class="form-control" />
                </div>
            </div>
          </div>
        </div>
    </div>
    <div class="col-12">
        <div class="text-right">
            <br />
            <asp:Label ID="lblGridCount" runat="server" Text="" />
        </div>
        <br />
        <br />
    </div>
    <div class="col-12">
        <div class="table-responsive">
            <asp:GridView ID="gvProds" runat="server" CssClass="table table-sm table-hover" AutoGenerateColumns="False" EnableModelValidation="True" DataKeyNames="ID, TV_IsDeleted" onrowcommand="gvProd_RowCommand" AllowSorting="true" OnSorting="gvProd_Sorting" OnRowDataBound="gvProd_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" ItemStyle-CssClass="hidden-print" HeaderStyle-CssClass="hidden-print">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnSelect" class="btn btn-sm tableButton" runat="server" CommandName="Select" CommandArgument='<%# Eval("ID") %>'><i class="fa fa-eye"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="false" />
                    <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name" />
                    <asp:BoundField DataField="Desc" HeaderText="Description" ReadOnly="True" SortExpression="Desc" />
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