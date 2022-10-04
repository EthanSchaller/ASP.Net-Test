<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="TestApp.UserEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="~/Dashboard.aspx" id="lnkDashboard" runat="server">Dashboard</a></li>
            <li class="breadcrumb-item"><a href="~/User/UserList.aspx" id="lnkUserList" runat="server">User List</a></li>
            <li class="breadcrumb-item active" aria-current="page">User Edit</li>
        </ol>
    </nav>
    <div class="row">
        <div class="col-12">
            <h2>
                <span>User Details</span>
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
            First Name
            <asp:TextBox ID="txtFName" runat="server" class="form-control" />
            <div class="invalid-feedback">
               First name field is required.
            </div>
        </div>
        <div class="col">
            Middle Name(s)
            <asp:TextBox ID="txtMName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                Middle name field is required
            </div>
        </div>
        <div class="col">
            Last Name
            <asp:TextBox ID="txtLName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                Last name field is required.
            </div>
        </div>
    </div>
    <div class="form-row"> 
        <div class="col">
            Username
            <asp:TextBox ID="txtUsrName" runat="server" class="form-control" />
            <div class="invalid-feedback">
                Username field is required.
            </div>
        </div>           
        <div class="col">
            Role
            <asp:Textbox id="txtRole" class="form-control" runat="server" Visible="False" ReadOnly="true"/>
            <asp:DropDownList data-placeholder="" runat="server" ID="ddlRole" class="chosen-select form-control" />            
            <div class="invalid-feedback">
                Please select a role for the user.
            </div>
        </div>
        <div class="col">
            <br />
            Active
            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
        </div>
    </div>
    <div class="form-row">
        <div class="col">
            Email
            <asp:TextBox ID="txtEmail" runat="server" class="form-control" />
            <div class="invalid-feedback">
                <asp:label ID="lblEmailError" Text="" runat="server" />
            </div>
        </div>   
    </div>
    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Delete User?</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    Are you sure you wish to delete this User?
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-warning" data-dismiss="modal"><i class="fas fa-times"></i>&nbsp;&nbsp;Cancel</button>
                    <asp:LinkButton ID="btnDeleteConfirm" runat="server" CssClass="btn btn-danger" onclick="btnDeleteConfirm_Click"><i class="fas fa-times"></i>&nbsp;&nbsp;Delete</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <input id="hidUserID" type="hidden" value="" runat="server" /> 

    <script type="text/javascript">
        function openDeleteModal() {
            $('#deleteModal').modal('show');
            return false;
        }
    </script>
</asp:Content>
