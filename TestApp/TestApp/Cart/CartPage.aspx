<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CartPage.aspx.cs" Inherits="TestApp.CartPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="col-12">
        <div class="row">
            <div class="col-9">
                <h2>
                    <span>Cart</span>
                </h2>
            </div>
            <div class="col-3 text-right">
                <h2>
                    <asp:LinkButton ID="btnConfirmCheckout" runat="server" CssClass="btn btn-success" onclick="btnConfirmCheckout_Click"><i class="fas fa-cart-plus"></i>&nbsp;&nbsp;Check-Out</asp:LinkButton>
                </h2>
            </div>
        </div>
        <hr />
    </div>
    <div class="col-12">
        <div class="table-responsive">
            <asp:GridView ID="gvProds" runat="server" CssClass="table table-sm table-hover" AutoGenerateColumns="False" EnableModelValidation="True" DataKeyNames="ID, IsDeleted" onrowcommand="gvProd_RowCommand" AllowSorting="true" OnSorting="gvProd_Sorting" OnRowDataBound="gvProd_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" ItemStyle-CssClass="hidden-print" HeaderStyle-CssClass="hidden-print">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnSelect" class="btn btn-sm tableButton" runat="server" CommandName="Select" CommandArgument='<%# Eval("ID") %>'><i class="fas fa-trash"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="false" />
                    <asp:BoundField DataField="Prod" HeaderText="Product" ReadOnly="True" SortExpression="Prod" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" ReadOnly="True" SortExpression="Quantity" />
                    <asp:BoundField DataField="Price" HeaderText="Price" ReadOnly="True" SortExpression="Price" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="modal fade" id="confirmDelFromCart" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Check Out?</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure you want to delete this item(s) from your cart?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal"><i class="fas fa-times"></i>&nbsp;&nbsp;Cancel</button>
                    <asp:LinkButton ID="btnConfirmDelToCart" runat="server" CssClass="btn btn-danger" onclick="btnConfirmDelToCart_Click"><i class="fas fa-minus"></i>&nbsp;&nbsp;Delete From Cart</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <input id="hiCartID" type="hidden" value="" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJS" runat="server">
    <script type="text/javascript">
        function delFromCart() {
            $('#confirmDelFromCart').modal('show');
            return false;
        }
    </script>
</asp:Content>