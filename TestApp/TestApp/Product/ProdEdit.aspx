<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProdEdit.aspx.cs" Inherits="TestApp.ProdsEdit" ValidateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><a href="~/Dashboard.aspx" id="lnkDashboard" runat="server">Dashboard</a></li>
        <li class="breadcrumb-item"><a href="~/ProdList.aspx" id="lnkItemList" runat="server">Product List</a></li>
        <li class="breadcrumb-item active" aria-current="page">Product Edit</li>
      </ol>
    </nav>
    <div class="row">
        <div class="col-12">
            <h2>
                <span>Product Details</span>
                <div class="float-right">
                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-warning" onclick="btnCancel_Click"><i class="fas fa-times"></i>&nbsp;&nbsp;Cancel</asp:LinkButton>
                    <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-danger" onclick="btnDelete_Click"><i class="fas fa-times"></i>&nbsp;&nbsp;Delete</asp:LinkButton>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" onclick="btnSave_Click" onclientclick="return CheckIsRepeatPostback();"><i class="fas fa-save"></i>&nbsp;&nbsp;Save</asp:LinkButton>
                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-primary" onclick="btnEdit_Click"><i class="fas fa-pencil-alt"></i>&nbsp;&nbsp;Edit</asp:LinkButton>
                </div>
            </h2>
            <hr />
        </div>
    </div>
    <div class="form-row">
        <div class="col">
            <asp:Image ID="imgUpload" runat="server" /> <br />
            <asp:FileUpload ID="FileUploader" runat="server" onchange="this.form.submit();" />
        </div>
    </div>
    <div class="form-row">
        <div class="col">
            Name
            <asp:TextBox ID="txtName" runat="server" class="form-control" autocomplete="off" />            
            <div class="invalid-feedback">
                You must fill in the Name field.
            </div>
        </div>
        <div class="col">
            Price
            <asp:TextBox ID="txtPrice" runat="server" class="form-control" autocomplete="off" />            
            <div class="invalid-feedback">
                You must fill in the Price field.
            </div>
        </div>
    </div>
    <div class="form-row">
        <div class="col">
            Decription
            <asp:TextBox ID="txtDesc" runat="server" class="form-control" ReadOnly ="true" TextMode="MultiLine" Rows="4" />
        </div>
    </div>
    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Delete Product?</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure you wish to delete this Product?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal"><i class="fas fa-times"></i>&nbsp;&nbsp;Cancel</button>
                    <asp:LinkButton ID="btnDeleteConfirm" runat="server" CssClass="btn btn-danger" onclick="btnDeleteConfirm_Click"><i class="fas fa-times"></i>&nbsp;&nbsp;Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <input id="hidProdID" type="hidden" value="" runat="server" /> 
    <script type="text/javascript">
        function openDeleteModal() {
            $('#deleteModal').modal('show');
            return false;
        }

        function openEditAttachmentModal() {
            $('#editAttachmentModal').modal('show');
            return false;
        }

        function openDeleteAttachmentModal() {
            $('#deleteAttachmentModal').modal('show');
            return false;
        }

        function selectAttachmentsTab() {
            $('#myTab a[href="#attachments"]').tab('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footerJS" runat="server">
</asp:Content>
