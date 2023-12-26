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
    public partial class membermanagement : System.Web.UI.Page
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"] == UserTypes.USER.ToString())
            {
                Response.Redirect("adminlogin.aspx");
            }

            GridView1.DataBind();
        }

        protected void GoBtnClick(Object sender, EventArgs e)
        {
            string member_id = TextBox1.Text.Trim();
            MemberDTO memberDto = queryRunner.getMemberById(member_id);
            if (memberDto == null)
            {
                Response.Write("<script>alert('Invalid ID')</script>");
                ClearForm();
                LinkButton1.Visible = false;
                LinkButton2.Visible = false;
                LinkButton3.Visible = false;
                Button2.Visible = false;
            }
            else
            {
                PopulateForm(memberDto);
                LinkButton1.Visible = true;
                LinkButton2.Visible = true;
                LinkButton3.Visible = true;
                Button2.Visible = true;
            }

        }
        protected void ApproveBtnClick(Object sender , EventArgs e)
        {
            string member_id = TextBox1.Text.Trim();
            var response = queryRunner.ChangeAccountStatus(member_id , AccountTypes.APPROVED.ToString());
            Response.Write($"<script>alert('{response["remarks"]}')</script>");
            GridView1.DataBind();
            ClearForm();
        }
        protected void PendingBtnClick(Object sender, EventArgs e)
        {
            string member_id = TextBox1.Text.Trim();
            var response = queryRunner.ChangeAccountStatus(member_id , AccountTypes.PENDING.ToString());
            Response.Write($"<script>alert('{response["remarks"]}')</script>");
            GridView1.DataBind();
            ClearForm();
        }
        protected void RejectBtnClick(Object sender, EventArgs e)
        {
            string member_id = TextBox1.Text.Trim();
            var response = queryRunner.ChangeAccountStatus(member_id, AccountTypes.REJECTED.ToString());
            Response.Write($"<script>alert('{response["remarks"]}')</script>");
            GridView1.DataBind();
            ClearForm();
        }

        protected void PopulateForm(MemberDTO memberDTO)
        {
            TextBox1.Text = memberDTO.MemberId;
            TextBox2.Text = memberDTO.FullName;
            TextBox3.Text = memberDTO.ContactNo;
            TextBox4.Text = memberDTO.EmailId;
            TextBox6.Text = memberDTO.FullAddress;
            TextBox7.Text = memberDTO.AccountStatus;
            TextBox8.Text = memberDTO.DOB;
            TextBox9.Text = memberDTO.State;
            TextBox10.Text = memberDTO.City;
            TextBox11.Text = memberDTO.Pincode;
        }
        protected void ClearForm()
        {
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox6.Text = "";
            TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox11.Text = "";
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            ClearForm();
            LinkButton1.Visible = false;
            LinkButton2.Visible = false;
            LinkButton3.Visible = false;
            Button2.Visible = false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string member_id = TextBox1.Text.Trim();
            var response = queryRunner.DeleteMemberById(member_id);
            Response.Write($"<script>alert('{response["remarks"]}')</script>");
            GridView1.DataBind();
            ClearForm();
        }
    }
}