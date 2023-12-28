using ELibraryManagement.enums;
using ELibraryManagement.Models;
using ELibraryManagement.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELibraryManagement
{
    public partial class userprofile : System.Web.UI.Page
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"] != UserTypes.USER.ToString())
            {
                Response.Redirect("userlogin.aspx");
            }
            if( !IsPostBack)
            {
                this.PopulateForm();
            }
            PopulateGridView();
        }

        protected void PopulateGridView()
        {
            var books = this.queryRunner.getAllBooksBorrowedByMemberId(Session["username"].ToString());
            GridView1.DataSource = books;
            GridView1.DataBind();
        }

        protected void PopulateForm()
        {
            string memberId = Session["username"].ToString();
            var member = queryRunner.getMemberById(memberId);

            TextBox1.Text = member.FullName;
            TextBox2.Text = member.DOB;
            TextBox3.Text = member.ContactNo;
            TextBox4.Text = member.EmailId;
            DropDownList1.SelectedValue = member.State;
            TextBox6.Text = member.City;
            TextBox7.Text = member.Pincode;
            TextBox5.Text = member.FullAddress;
            TextBox8.Text = member.MemberId;
            TextBox9.Text = member.Password;

            if( member.AccountStatus.Equals(AccountTypes.APPROVED.ToString()))
            {
                Label1.Text = member.AccountStatus;
            } 
            else if(member.AccountStatus.Equals(AccountTypes.REJECTED.ToString()))
            {
                Label1.Text = member.AccountStatus;
                Label1.CssClass = "badge rounded-pill text-bg-danger";
            }
            else if (member.AccountStatus.Equals(AccountTypes.PENDING.ToString()))
            {
                Label1.Text = member.AccountStatus;
                Label1.CssClass = "badge rounded-pill text-bg-warning";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var member = new MemberDTO(
                TextBox8.Text,
                TextBox1.Text,
                Label1.Text,
                TextBox2.Text,
                TextBox3.Text,
                TextBox4.Text,
                DropDownList1.SelectedValue,
                TextBox6.Text,
                TextBox7.Text,
                TextBox5.Text,
                (TextBox10.Text=="")?TextBox9.Text:TextBox10.Text
                );

            try
            {
                this.queryRunner.updateMember(member);
                this.PopulateForm();
                TextBox10.Text = "";
            }
            catch( Exception ex)
            {
                Response.Write($"<script>alert('{ex.Message}')</script>");
            }
        }
    }
}