using ELibraryManagement.enums;
using ELibraryManagement.Models;
using ELibraryManagement.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELibraryManagement
{
    public partial class bookinventory : System.Web.UI.Page
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null || Session["role"].Equals(UserTypes.USER.ToString()) )
            {
                Response.Redirect("adminlogin.aspx");
            }
            GridView1.DataBind();
            DropDownList2.DataBind();
            DropDownList3.DataBind();
        }

        protected void ClearForm()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox11.Text = "";
            TextBox4.Text = "";
            TextBox6.Text = "";

            DropDownList1.SelectedIndex = -1;
            DropDownList2.SelectedIndex = -1;
            DropDownList3.SelectedIndex = -1;

            foreach (ListItem item in ListBox1.Items)
            {
                item.Selected = false;
            }

            Button1.Enabled = true;
            Button2.Enabled = false;
            Button3.Enabled = false;

            fileLink.NavigateUrl = "#";
            fileLink.Visible = false;
        }

        protected void populateForm(string book_id)
        {
            var book = queryRunner.getBookById(book_id);

            fileLink.Visible = true;
            fileLink.NavigateUrl = book.ImagePath.Trim();

            TextBox1.Text = book.BookId.Trim();
            TextBox2.Text = book.BookName.Trim();
            TextBox3.Text = book.PublisherDate.Trim();
            TextBox4.Text = book.ActualStock.Trim();
            TextBox5.Text = book.CostPerUnit.Trim();
            TextBox6.Text = book.BookDescription.Trim();
            TextBox7.Text = book.IssuedBooks.Trim();
            TextBox9.Text = book.Edition.Trim();
            TextBox10.Text = book.CostPerUnit.Trim();
            TextBox11.Text = book.Pages.Trim();

            DropDownList1.SelectedValue = book.Language;
            foreach(ListItem li in DropDownList2.Items)
            {
                if(li.Text .Equals(book.PublisherName))
                {
                    li.Selected = true;
                    break;
                }
            }
            foreach (ListItem li in DropDownList3.Items)
            {
                if (li.Text.Equals(book.AuthorName))
                {
                    li.Selected = true;
                    break;
                }
            }

            string[] listBoxSelections = book.Genre.Split(',');
            foreach( string listBoxSelection in listBoxSelections)
            {
                if (ListBox1.Items.FindByText(listBoxSelection) != null)
                {
                    ListBox1.Items.FindByText(listBoxSelection).Selected = true;
                }
            }
            Button1.Enabled = false;
        }

        //Add button click
        protected void Button1_Click(object sender, EventArgs e)
        {

            try
            {
                string genres = "";
                foreach (int i in ListBox1.GetSelectedIndices())
                {
                    genres = genres + ListBox1.Items[i] + ",";
                }
                genres = genres.Remove(genres.Length - 1);

                string filepath = "~/book_inventory/books1.png";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                filepath = "~/book_inventory/" + filename;

                /* public BookDTO(string imagePath, string bookId, string bookName, 
                 * string language, string publisherName ,string authorName, string genre, 
                 * string publisherDate, string edition, string costPerUnit, 
                 * string pages, string actualStock, string bookDescription)
                 * */

                var book = new BookDTO(
                    filepath,
                    TextBox1.Text.Trim(),
                    TextBox2.Text.Trim(),
                    DropDownList1.SelectedValue.Trim(),
                    DropDownList2.SelectedItem.Text.Trim(),
                    DropDownList3.SelectedItem.Text.Trim(),
                    genres,
                    TextBox3.Text.Trim(),
                    TextBox9.Text.Trim(),
                    TextBox10.Text.Trim(),
                    TextBox11.Text.Trim(),
                    TextBox4.Text.Trim(),
                    TextBox6.Text.Trim()
                    );
                if (queryRunner.CheckIfBookExists(book))
                {
                    Response.Write("<script>alert('Book already exists')</script>");
                    ClearForm();
                }
                else
                {
                    queryRunner.SaveBook(book);
                    Response.Write("<script>alert('Book saved')</script>");
                    ClearForm();
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Internal Server Error')</script>");
                ClearForm();
            }
            GridView1.DataBind();
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            string book_id = TextBox1.Text.Trim();
            if( !queryRunner.CheckIfBookExists(book_id))
            {
                Response.Write("<script>alert('ID does not exist')</script>");
                ClearForm();
            }
            else
            {
                populateForm(book_id);
                Button1.Enabled = false;
                Button2.Enabled = true;
                Button3.Enabled = true;
            }
        }

        //Update Button Click
        protected void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                string genres = "";
                foreach (int i in ListBox1.GetSelectedIndices())
                {
                    genres = genres + ListBox1.Items[i] + ",";
                }
                genres = genres.Remove(genres.Length - 1);

                string filepath = "~/book_inventory/books1.png";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (filename == "")
                {
                    filepath = fileLink.NavigateUrl.Trim();
                }
                else
                {
                    FileUpload1.SaveAs(Server.MapPath("book_inventory/" + filename));
                    filepath = "~/book_inventory/" + filename;
                    string filePathExisting = Server.MapPath(fileLink.NavigateUrl);
                    // Check if the file exists before attempting to delete
                    if (File.Exists(filePathExisting))
                    {
                        File.Delete(filePathExisting);
                    }
                }

                /* public BookDTO(string imagePath, string bookId, string bookName, 
                 * string language, string publisherName ,string authorName, string genre, 
                 * string publisherDate, string edition, string costPerUnit, 
                 * string pages, string actualStock, string bookDescription)
                 * */

                var book = new BookDTO(
                    filepath,
                    TextBox1.Text.Trim(),
                    TextBox2.Text.Trim(),
                    DropDownList1.SelectedValue.Trim(),
                    DropDownList2.SelectedItem.Text.Trim(),
                    DropDownList3.SelectedItem.Text.Trim(),
                    genres,
                    TextBox3.Text.Trim(),
                    TextBox9.Text.Trim(),
                    TextBox10.Text.Trim(),
                    TextBox11.Text.Trim(),
                    TextBox4.Text.Trim(),
                    TextBox6.Text.Trim()
                    );
                queryRunner.UpdateBook(book);
                Response.Write("<script>alert('Book updated')</script>");
                ClearForm();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('Internal Server Error')</script>");
                ClearForm();
            }

            GridView1.DataBind();
         }

        //Delete Button Click
        protected void Button2_Click(object sender, EventArgs e)
        {
            string book_id = TextBox1.Text.Trim();
            try
            {
                queryRunner.DeleteBook(book_id);
                Response.Write("<script>alert('Book deleted')</script>");

                string filePath = Server.MapPath(fileLink.NavigateUrl);
                // Check if the file exists before attempting to delete
                if (File.Exists(filePath))
                {

                    File.Delete(filePath);
                }
                ClearForm();
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Internal Server Error')</script>");
                ClearForm();
            }
            GridView1.DataBind();
        }
    }
}