using ELibraryManagement.enums;
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
            Member member = new Member(
                                        TextBox1.Text,
                                        TextBox2.Text,
                                        TextBox3.Text,
                                        TextBox4.Text,
                                        DropDownList1.SelectedItem.Value,
                                        TextBox6.Text,
                                        TextBox7.Text,
                                        TextBox5.Text,
                                        TextBox8.Text,
                                        TextBox9.Text,
                                        AccountTypes.PENDING.ToString()
                                      );
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