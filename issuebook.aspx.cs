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
    public partial class issuebook : System.Web.UI.Page
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Fetch and populate details
            string memberId, bookId;
            memberId = TextBox2.Text.Trim();
            bookId = TextBox1.Text.Trim();

            if (!inputValidationPopulator(memberId, bookId))
            {
                Response.Write("<script>alert('Invalid values for member ID or Book ID')</script>");
            }
            else
            {
                var book = this.queryRunner.getBookById(bookId);
                var member = this.queryRunner.getMemberById(memberId);

                TextBox3.Text = member.FullName;
                TextBox4.Text = book.BookName;

                var books = this.queryRunner.GetAllBooksBorrowedByMemberId(memberId);

                foreach (var bookiterator in books)
                {
                    if (bookiterator.BookId.Equals(book.BookId))
                    {
                        // Parse and set IssueDate
                        if (DateTime.TryParse(bookiterator.IssueDate, out DateTime issueDate))
                        {
                            TextBox5.Text = issueDate.ToString("yyyy-MM-dd");
                            TextBox5.ReadOnly = true;
                        }
                        else
                        {
                            // Handle invalid date format for IssueDate
                            TextBox5.Text = "Invalid Date Format";
                        }

                        // Parse and set DueDate
                        if (DateTime.TryParse(bookiterator.DueDate, out DateTime dueDate))
                        {
                            if( dueDate < DateTime.Now.ToLocalTime())
                            {
                                TextBox6.ForeColor = System.Drawing.Color.Red;
                                TextBox6.BorderColor = System.Drawing.Color.Red;
                                Button4.CssClass = "btn btn-block btn-danger";
                                Response.Write("<script>alert('Defaulter alert')</script>");
                            }
                            TextBox6.Text = dueDate.ToString("yyyy-MM-dd");
                            TextBox6.ReadOnly = true;
                        }
                        else
                        {
                            // Handle invalid date format for DueDate
                            TextBox6.Text = "Invalid Date Format";
                        }

                        Button4.Enabled = true;
                        break;
                    }
                }

                if (!Button4.Enabled)
                {
                    Button2.Enabled = true;
                    TextBox5.ReadOnly = false;
                }
            }
        }


        private bool inputValidationPopulator(string memberId, string bookId)
        {
            if (memberId == null || bookId == null )
            {
                return false;
            }
            if (memberId == "" || bookId == "")
            {
                return false;
            }
            if( this.queryRunner.getBookById(bookId) == null || this.queryRunner.getMemberById(memberId) == null)
            {
                return false;
            }
            return true;
        }

        //Issue Button
        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                var issueModel = new BookIssueDTO(TextBox1.Text, TextBox2.Text, TextBox4.Text, TextBox3.Text, TextBox5.Text, TextBox6.Text);
                this.queryRunner.IssueBook(issueModel);
                Response.Write("<script>alert('Book Issued')</script>");
                this.CleanForm();
            }
            catch
            {
                Response.Write("<script>alert('Internal server error')</script>");
            }
        }

        //Return Button
        protected void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.queryRunner.ReturnBook(TextBox1.Text, TextBox2.Text);
                Response.Write("<script>alert('Book returned')</script>");
                this.CleanForm();
            }
            catch
            {
                Response.Write("<script>alert('Internal server error')</script>");
            }
        }

        protected void CleanForm()
        {
            Button2.Enabled = false;
            Button4.Enabled = false;
            TextBox6.ForeColor = TextBox5.ForeColor;
            TextBox6.BorderColor = TextBox5.BorderColor;
            TextBox5.ReadOnly = true;
            Button4.CssClass = "btn btn-block btn-success";
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox5.Text = "";
            TextBox6.Text = "";
        }

        protected void TextBox5_TextChanged(object sender, EventArgs e)
        {
            TextBox6.Attributes["min"] = TextBox5.Text;
            TextBox6.ReadOnly = false;
        }
    }
}