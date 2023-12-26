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
    public partial class publishermanagement : System.Web.UI.Page
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            Publisher publisher = new Publisher(TextBox1.Text.Trim(), TextBox2.Text.Trim());
            Dictionary<string, string> response = queryRunner.AddPublisher(publisher);
            Response.Write($"<script>alert( '{ response["remarks"]}' )</script>");
            clearForm();
            GridView1.DataBind();
        }

        protected void clearForm()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Publisher publisher = new Publisher(TextBox1.Text.Trim(), TextBox2.Text.Trim());
            Dictionary<string, string> response = queryRunner.UpdatePublisher(publisher);
            Response.Write($"<script>alert( '{ response["remarks"]}' )</script>");
            clearForm();
            GridView1.DataBind();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            Publisher publisher = new Publisher(TextBox1.Text.Trim(), TextBox2.Text.Trim());
            Dictionary<string, string> response = queryRunner.DeletePublisher(publisher);
            Response.Write($"<script>alert( '{ response["remarks"]}' )</script>");
            clearForm();
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string publisherName = "";
            if (this.queryRunner.getAuthor(TextBox1.Text, out publisherName))
            {
                TextBox2.Text = publisherName;
            }
            else
            {
                Response.Write("<script>alert('Publisher not found')</script>");
                clearForm();
            }
        }
    }
}