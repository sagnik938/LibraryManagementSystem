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
    public partial class authormanagement : System.Web.UI.Page
    {
        QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {
            if( Session["role"] == null || Session["role"] == UserTypes.USER.ToString() )
            {
                Response.Redirect("adminlogin.aspx");
            }
            GridView1.DataBind();
        }

        //add button click
        protected void Button2_Click(object sender, EventArgs e)
        {
            Author author = new Author(TextBox1.Text.Trim(), TextBox2.Text.Trim());
            Dictionary<string , string> response = queryRunner.AddAuthor(author);
            Response.Write($"<script>alert( '{ response["remarks"]}' )</script>");
            clearForm();
            GridView1.DataBind();
        }

        //update button click
        protected void Button3_Click(object sender, EventArgs e)
        {
            Author author = new Author(TextBox1.Text.Trim(), TextBox2.Text.Trim());
            Dictionary<string, string> response = queryRunner.UpdateAuthor(author);
            Response.Write($"<script>alert( '{ response["remarks"]}' )</script>");
            clearForm();
            GridView1.DataBind();
        }

        //delete button click
        protected void Button4_Click(object sender, EventArgs e)
        {
            Author author = new Author(TextBox1.Text.Trim(), TextBox2.Text.Trim());
            Dictionary<string, string> response = queryRunner.DeleteAuthor(author);
            Response.Write($"<script>alert( '{ response["remarks"]}' )</script>");
            clearForm();
            GridView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string authorName = "";
            if( this.queryRunner.getAuthor(TextBox1.Text , out authorName) )
            {
                TextBox2.Text = authorName;
            }
            else
            {
                Response.Write("<script>alert('Author not found')</script>");
                clearForm();
            }
        }
        protected void clearForm()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
        }
    }
}