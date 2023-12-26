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
    public partial class userlogin : System.Web.UI.Page
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var loginData = new LoginDto(TextBox1.Text, TextBox2.Text);
            try
            {
                var loginstatus = queryRunner.DoLogin(loginData, Session);
                Response.Write($"<script>alert('{loginstatus["remarks"]}')</script>");
                if ( bool.Parse( loginstatus["loginstatus"] ) )
                {
                    Response.Redirect("Homepage.aspx");
                }
            }
            catch(Exception ex)
            {
                Response.Write($"<script>alert(Internal Error occurred: {ex.Message})</script>");
            }
        }
    }
}