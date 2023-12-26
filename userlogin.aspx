<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="userlogin.aspx.cs" Inherits="ELibraryManagement.userlogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container">
        <div class="col-md-6 mx-auto">
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col">
                            <center>
                                <img width="150px" src="Images/imgs/generaluser.png" />
                            </center>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <center>
                                <h3>Member Login</h3>
                            </center>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <center>
                                <hr />
                            </center>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <label>Member ID</label>
                            <div class="form-group">
                                <asp:TextBox ID="TextBox1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <label>Password</label>
                            <div class="form-group">
                                <asp:TextBox ID="TextBox2" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                            </div>
                                <div class="form-group">
                                    <br />
                                    <asp:Button ID="Button1" CssClass="btn btn-success" runat="server" Text="Login" OnClick="Button1_Click" />
                                </div>
                                <br />
                                <div class="form-group">
                                    <a href="signup.aspx">
                                        <button type="button" class="btn btn-primary">Sign Up</button></a>
                                </div>
                            </div>
                    </div>
                </div>
            </div>
            <br />
            <a href="Homepage.aspx"><< Back to home</a>
            <br />
            <br />
        </div>
    </div>
</asp:Content>
