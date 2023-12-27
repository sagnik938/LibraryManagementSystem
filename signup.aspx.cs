using ELibraryManagement.enums;
using ELibraryManagement.Models;
using ELibraryManagement.SQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELibraryManagement
{
    public partial class signup : System.Web.UI.Page
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //sign up click
        protected void Button1_Click(object sender, EventArgs e)
        {
            MemberDTO member = new MemberDTO
            {
                FullName = TextBox1.Text,
                DOB = TextBox2.Text,
                ContactNo = TextBox3.Text,
                EmailId = TextBox4.Text,
                State = DropDownList1.SelectedValue,
                City = TextBox6.Text,
                Pincode = TextBox7.Text,
                FullAddress = TextBox5.Text,
                MemberId = TextBox8.Text,
                Password = TextBox9.Text,
                AccountStatus = AccountTypes.PENDING.ToString()
                // You may set default values for other properties if needed
            };

            try
            {
                if( queryRunner.CheckUsernameAvailability(member.MemberId) )
                {
                    queryRunner.SignUpQueryRunner(member);
                    Response.Write("<script>alert('Account created successfully')</script>");
                }
                else
                {
                    Response.Write("<script>alert('Username already exists')</script>");
                }
            }
            catch( Exception ex)
            {
                Response.Write($"<script>alert('{ex.Message}')</script>");
            }

        }
    }
}