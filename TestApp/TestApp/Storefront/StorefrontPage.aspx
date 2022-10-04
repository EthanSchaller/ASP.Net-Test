<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StorefrontPage.aspx.cs" Inherits="TestApp.StorefrontPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="col-12">
        <h2>
            <span>Storefront</span>
        </h2>
        <hr />
    </div>
    <div class="col-12">
        <div class="table-responsive">
            <asp:GridView ID="gvProds" runat="server" CssClass="table table-sm table-hover" AutoGenerateColumns="False" EnableModelValidation="True" DataKeyNames="ID, TV_IsDeleted" onrowcommand="gvProd_RowCommand" AllowSorting="true" OnSorting="gvProd_Sorting" OnRowDataBound="gvProd_RowDataBound">
                <Columns>
                    <asp:TemplateField ItemStyle-Width="40px" ItemStyle-CssClass="hidden-print" HeaderStyle-CssClass="hidden-print">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnSelect" class="btn btn-sm tableButton" runat="server" CommandName="Select" CommandArgument='<%# Eval("ID") %>'><i class="fas fa-tag"></i></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" SortExpression="ID" Visible="false" />
                    <asp:TemplateField HeaderText="Image" ItemStyle-HorizontalAlign="Center"  ControlStyle-Height="30px">
                        <ItemTemplate>
                            <asp:Image ID="Picture" runat="server" ImageUrl='<%# "~/classes/DBImageHandler.ashx?img=" + Eval("Image")  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name" />
                    <asp:BoundField DataField="Price" HeaderText="Price" ReadOnly="True" SortExpression="Price" />
                    <asp:BoundField DataField="Desc" HeaderText="Description" ReadOnly="True" SortExpression="Desc" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div class="modal fade" id="confirmAddToCart" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Check Out?</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure you want to Add this to your cart?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal"><i class="fas fa-times"></i>&nbsp;&nbsp;Cancel</button>
                    <asp:LinkButton ID="btnConfirmAddToCart" runat="server" CssClass="btn btn-success" onclick="btnConfirmAddToCart_Click"><i class="fas fa-cart-plus"></i>&nbsp;&nbsp;Add To Cart</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <input id="hidProdID" type="hidden" value="" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJS" runat="server">
    <script type="text/javascript">
        function addToCart() {
            $('#confirmAddToCart').modal('show');
            return false;
        }
    </script>
</asp:Content>