using ELibraryManagement.enums;
using ELibraryManagement.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace ELibraryManagement.SQL
{
    public class QueryRunner
    {
        private string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        public void SignUpQueryRunner(MemberDTO member)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.strcon);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("insert into dbo.member_master_tbl" +
                    "( full_name , dob,contact_no,email,state,city,pincode,full_address,member_id,password,account_status )" +
                    "values( @full_name , @dob , @contact_no , @email , @state , @city , @pincode , @full_address , @member_id , @password , @account_status )", conn);

                cmd.Parameters.Add("@full_name", SqlDbType.NVarChar).Value = member.FullName;
                cmd.Parameters.Add("@dob", SqlDbType.NVarChar).Value = member.DOB;
                cmd.Parameters.Add("@contact_no", SqlDbType.NVarChar).Value = member.ContactNo;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = member.EmailId;
                cmd.Parameters.Add("@state", SqlDbType.NVarChar).Value = member.State;
                cmd.Parameters.Add("@city", SqlDbType.NVarChar).Value = member.City;
                cmd.Parameters.Add("@pincode", SqlDbType.NVarChar).Value = member.Pincode;
                cmd.Parameters.Add("@full_address", SqlDbType.NVarChar).Value = member.FullAddress;
                cmd.Parameters.Add("@member_id", SqlDbType.NVarChar).Value = member.MemberId;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = member.Password;
                cmd.Parameters.Add("@account_status", SqlDbType.NVarChar).Value = member.AccountStatus;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Insertion failed due to an Exception: {ex.Message}");
            }
        }

        public bool CheckUsernameAvailability(string username)
        {

            SqlConnection conn = new SqlConnection(this.strcon);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from dbo.member_master_tbl where member_id = @member_id", conn);
            cmd.Parameters.Add("@member_id", SqlDbType.NVarChar).Value = username;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // If there are any rows, it means the member_id is not unique
                return !reader.HasRows;
            }
        }

        public Dictionary<string, string> DoLogin(LoginDto loginDto, HttpSessionState Session, bool isAdmin = false)
        {

            SqlConnection conn = new SqlConnection(strcon);
            SqlCommand cmd;
            int index = new int();
            UserTypes usertype;
            Dictionary<string, string> loginDetails = new Dictionary<string, string>();

            if (!isAdmin)
            {
                cmd = new SqlCommand("select * from dbo.member_master_tbl where member_id = @member_id and password = @password", conn);
                index = 8;
                usertype = UserTypes.USER;
            }
            else
            {
                cmd = new SqlCommand("select * from dbo.admin_login_tbl where username = @member_id and password = @password", conn);
                index = 0;
                usertype = UserTypes.ADMIN;
            }
            cmd.Parameters.Add("@member_id", SqlDbType.NVarChar).Value = loginDto.Username;
            cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = loginDto.Password;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Session["username"] = reader.GetValue(index).ToString();
                            Session["password"] = reader.GetValue(index + 1).ToString();
                            Session["role"] = usertype.ToString();
                            if (usertype == UserTypes.USER)
                            {
                                Session["status"] = reader.GetValue(index + 2).ToString();
                            }
                        }
                        if (usertype == UserTypes.ADMIN)
                        {
                            loginDetails["loginstatus"] = "true";
                            loginDetails["remarks"] = "Login successfull";
                        }
                        if (usertype == UserTypes.USER && Session["status"].Equals(AccountTypes.APPROVED.ToString()))
                        {
                            loginDetails["loginstatus"] = "true";
                            loginDetails["remarks"] = "Login successfull";
                        }
                        else if (usertype == UserTypes.USER && !Session["status"].Equals(AccountTypes.APPROVED.ToString()))
                        {
                            loginDetails["loginstatus"] = "false";
                            loginDetails["remarks"] = $"Account approval status: {Session["status"]}";
                            Session.Clear();
                        }
                    }
                    else
                    {
                        loginDetails["loginstatus"] = "false";
                        loginDetails["remarks"] = "Invalid Credentials";
                    }
                }
                return loginDetails;
            }
            catch (Exception ex)
            {
                loginDetails["loginstatus"] = "false";
                loginDetails["remarks"] = $"Internal Server Error: {ex.Message}";
                return loginDetails;
            }
        }

        public void DoLogout(HttpSessionState Session)
        {
            Session.Clear();
        }

        public bool CheckAuthorIdAvailability(string authorId)
        {
            SqlConnection conn = new SqlConnection(this.strcon);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from dbo.author_master_tbl where author_id = @author_id", conn);
            cmd.Parameters.Add("@author_id", SqlDbType.NVarChar).Value = authorId;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // If there are any rows, it means the member_id is not unique
                return !reader.HasRows;
            }
        }

        public bool CheckPublisherIdAvailability(string publisherId)
        {
            SqlConnection conn = new SqlConnection(this.strcon);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from dbo.publisher_master_tbl where publisher_id = @publisher_id", conn);
            cmd.Parameters.Add("@publisher_id", SqlDbType.NVarChar).Value = publisherId;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                // If there are any rows, it means the member_id is not unique
                return !reader.HasRows;
            }
        }

        public Dictionary<string, string> AddAuthor(Author author)
        {
            Dictionary<string, string> authorDetails = new Dictionary<string, string>();
            if (!this.CheckAuthorIdAvailability(author.AuthorId))
            {
                authorDetails["status"] = "failed";
                authorDetails["remarks"] = "ID not unique already exists";
                return authorDetails;
            }

            try
            {
                SqlConnection conn = new SqlConnection(this.strcon);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("insert into dbo.author_master_tbl" +
                    "( author_id , author_name )" +
                    "values( @author_id , @author_name )", conn);

                cmd.Parameters.Add("@author_id", SqlDbType.NVarChar).Value = author.AuthorId;
                cmd.Parameters.Add("@author_name", SqlDbType.NVarChar).Value = author.AuthorName;

                cmd.ExecuteNonQuery();
                conn.Close();
                authorDetails["status"] = "success";
                authorDetails["remarks"] = $"Author inserted successfully";
                return authorDetails;
            }
            catch (Exception ex)
            {
                authorDetails["status"] = "failed";
                authorDetails["remarks"] = $"Internal Server Error: ${ex.Message}";
                return authorDetails;
            }

        }

        public Dictionary<string, string> UpdateAuthor(Author author)
        {
            Dictionary<string, string> authorDetails = new Dictionary<string, string>();
            if (!this.CheckAuthorIdAvailability(author.AuthorId))
            {
                try
                {
                    SqlConnection conn = new SqlConnection(this.strcon);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand("update dbo.author_master_tbl" +
                        " set author_name=@author_name " +
                        "where author_id=@author_id", conn);

                    cmd.Parameters.Add("@author_id", SqlDbType.NVarChar).Value = author.AuthorId;
                    cmd.Parameters.Add("@author_name", SqlDbType.NVarChar).Value = author.AuthorName;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    authorDetails["status"] = "success";
                    authorDetails["remarks"] = $"Author updated successfully";
                    return authorDetails;
                }
                catch (Exception ex)
                {
                    authorDetails["status"] = "failed";
                    authorDetails["remarks"] = $"Internal Server Error: ${ex.Message}";
                    return authorDetails;
                }

            }
            else
            {
                authorDetails["status"] = "failed";
                authorDetails["remarks"] = "Author with given ID not found";
                return authorDetails;
            }
        }

        public Dictionary<string, string> DeleteAuthor(Author author)
        {
            Dictionary<string, string> authorDetails = new Dictionary<string, string>();
            if (!this.CheckAuthorIdAvailability(author.AuthorId))
            {

                try
                {
                    SqlConnection conn = new SqlConnection(this.strcon);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand("delete from dbo.author_master_tbl " +
                        "where author_id = @author_id", conn);

                    cmd.Parameters.Add("@author_id", SqlDbType.NVarChar).Value = author.AuthorId;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    authorDetails["status"] = "success";
                    authorDetails["remarks"] = $"Author deleted successfully";
                    return authorDetails;
                }
                catch (Exception ex)
                {
                    authorDetails["status"] = "failed";
                    authorDetails["remarks"] = $"Internal Server Error: ${ex.Message}";
                    return authorDetails;
                }

            }
            else
            {
                authorDetails["status"] = "failed";
                authorDetails["remarks"] = "Author with given ID not found";
                return authorDetails;
            }
        }

        public bool getAuthor(string authorId, out string authorName)
        {
            SqlConnection conn = new SqlConnection(this.strcon);
            authorName = "";
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from dbo.author_master_tbl where author_id = @author_id", conn);
            cmd.Parameters.Add("@author_id", SqlDbType.NVarChar).Value = authorId;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return false;
                }
                while (reader.Read())
                {
                    authorName = reader.GetValue(1).ToString();
                }
            }
            return true;
        }

        public Dictionary<string, string> AddPublisher(Publisher publisher)
        {
            Dictionary<string, string> publisherDetails = new Dictionary<string, string>();
            if (!this.CheckPublisherIdAvailability(publisher.PublisherId))
            {
                publisherDetails["status"] = "failed";
                publisherDetails["remarks"] = "ID not unique already exists";
                return publisherDetails;
            }

            try
            {
                SqlConnection conn = new SqlConnection(this.strcon);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("insert into dbo.publisher_master_tbl" +
                    "( publisher_id , publisher_name )" +
                    "values( @publisher_id , @publisher_name )", conn);

                cmd.Parameters.Add("@publisher_id", SqlDbType.NVarChar).Value = publisher.PublisherId;
                cmd.Parameters.Add("@publisher_name", SqlDbType.NVarChar).Value = publisher.PublisherName;

                cmd.ExecuteNonQuery();
                conn.Close();
                publisherDetails["status"] = "success";
                publisherDetails["remarks"] = $"Publisher inserted successfully";
                return publisherDetails;
            }
            catch (Exception ex)
            {
                publisherDetails["status"] = "failed";
                publisherDetails["remarks"] = $"Internal Server Error: ${ex.Message}";
                return publisherDetails;
            }

        }

        public Dictionary<string, string> UpdatePublisher(Publisher publisher)
        {
            Dictionary<string, string> publisherDetails = new Dictionary<string, string>();
            if (!this.CheckPublisherIdAvailability(publisher.PublisherId))
            {
                try
                {
                    SqlConnection conn = new SqlConnection(this.strcon);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand("update dbo.publisher_master_tbl" +
                        " set publisher_name=@publisher_name " +
                        "where publisher_id=@publisher_id", conn);

                    cmd.Parameters.Add("@publisher_id", SqlDbType.NVarChar).Value = publisher.PublisherId;
                    cmd.Parameters.Add("@publisher_name", SqlDbType.NVarChar).Value = publisher.PublisherName;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    publisherDetails["status"] = "success";
                    publisherDetails["remarks"] = $"Publisher updated successfully";
                    return publisherDetails;
                }
                catch (Exception ex)
                {
                    publisherDetails["status"] = "failed";
                    publisherDetails["remarks"] = $"Internal Server Error: ${ex.Message}";
                    return publisherDetails;
                }

            }
            else
            {
                publisherDetails["status"] = "failed";
                publisherDetails["remarks"] = "Publisher with given ID not found";
                return publisherDetails;
            }
        }

        public Dictionary<string, string> DeletePublisher(Publisher publisher)
        {
            Dictionary<string, string> publisherDetails = new Dictionary<string, string>();
            if (!this.CheckPublisherIdAvailability(publisher.PublisherId))
            {

                try
                {
                    SqlConnection conn = new SqlConnection(this.strcon);
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();
                    }

                    SqlCommand cmd = new SqlCommand("delete from dbo.publisher_master_tbl " +
                        "where publisher_id = @publisher_id", conn);

                    cmd.Parameters.Add("@publisher_id", SqlDbType.NVarChar).Value = publisher.PublisherId;

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    publisherDetails["status"] = "success";
                    publisherDetails["remarks"] = $"Publisher deleted successfully";
                    return publisherDetails;
                }
                catch (Exception ex)
                {
                    publisherDetails["status"] = "failed";
                    publisherDetails["remarks"] = $"Internal Server Error: ${ex.Message}";
                    return publisherDetails;
                }

            }
            else
            {
                publisherDetails["status"] = "failed";
                publisherDetails["remarks"] = "Publisher with given ID not found";
                return publisherDetails;
            }
        }

        public bool getPublisher(string publisherId, out string publisherName)
        {
            SqlConnection conn = new SqlConnection(this.strcon);
            publisherName = "";
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand("select * from dbo.publisher_master_tbl where publisher_id = @publisher_id", conn);
            cmd.Parameters.Add("@publisher_id", SqlDbType.NVarChar).Value = publisherId;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return false;
                }
                while (reader.Read())
                {
                    publisherName = reader.GetValue(1).ToString();
                }
            }
            return true;
        }

        public MemberDTO getMemberById(string memberId)
        {
            SqlConnection conn = new SqlConnection(this.strcon);
            MemberDTO member = new MemberDTO();
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand(" SELECT [full_name],[dob],[contact_no],[email],[state],[city],[pincode],[full_address],[member_id],[account_status],[password] " +
                "FROM[elibraryDB].[dbo].[member_master_tbl] " +
                "WHERE [member_id] = @member_id", conn);
            cmd.Parameters.Add("@member_id", SqlDbType.NVarChar).Value = memberId;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null;
                }
                while (reader.Read())
                {
                    // public MemberDTO(string memberId, string fullName, string accountStatus, string dob, string contactNo, string email, string state, string city, string pincode, string fullAddress)

                    member = new MemberDTO(
                        reader.GetValue(reader.GetOrdinal("member_id"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("full_name"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("account_status"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("dob"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("contact_no"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("email"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("state"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("city"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("pincode"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("full_address"))?.ToString(),
                        reader.GetValue(reader.GetOrdinal("password"))?.ToString()
                    );
                }
            }
            return member;
        }

        public Dictionary<string, string> ChangeAccountStatus(string member_id, string status)
        {
            Dictionary<string, string> memberDetails = new Dictionary<string, string>();
            try
            {
                SqlConnection conn = new SqlConnection(this.strcon);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("update dbo.member_master_tbl" +
                    " set account_status=@status " +
                    "where member_id=@member_id", conn);

                cmd.Parameters.Add("@status", SqlDbType.NVarChar).Value = status;
                cmd.Parameters.Add("@member_id", SqlDbType.NVarChar).Value = member_id;

                cmd.ExecuteNonQuery();
                conn.Close();
                memberDetails["status"] = "success";
                memberDetails["remarks"] = $"Member updated successfully";
                return memberDetails;
            }
            catch (Exception ex)
            {
                memberDetails["status"] = "failed";
                memberDetails["remarks"] = $"Internal Server Error: {ex.Message}";
                return memberDetails;
            }
        }

        public Dictionary<string, string> DeleteMemberById(string member_id)
        {
            var memberDetails = new Dictionary<string, string>();
            try
            {
                SqlConnection conn = new SqlConnection(this.strcon);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("delete from dbo.member_master_tbl " +
                    "where member_id = @member_id", conn);

                cmd.Parameters.Add("@member_id", SqlDbType.NVarChar).Value = member_id;

                cmd.ExecuteNonQuery();
                conn.Close();
                memberDetails["status"] = "success";
                memberDetails["remarks"] = $"Member deleted successfully";
                return memberDetails;
            }
            catch (Exception ex)
            {
                memberDetails["status"] = "failed";
                memberDetails["remarks"] = $"Internal Server Error: {ex.Message}";
                return memberDetails;
            }
        }

        public void updateMember(MemberDTO member)
        {
            this.DeleteMemberById(member.MemberId);
            this.SignUpQueryRunner(member);
        }

        public bool CheckIfBookExists(BookDTO bookDTO)
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from book_master_tbl where book_id=@book_id OR book_name=@book_name;", con);
                cmd.Parameters.Add("@book_id", SqlDbType.NVarChar).Value = bookDTO.BookId;
                cmd.Parameters.Add("@book_name", SqlDbType.NVarChar).Value = bookDTO.BookName;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public bool CheckIfBookExists(string book_id)
        {
            try
            {
                SqlConnection con = new SqlConnection(strcon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * from book_master_tbl where book_id=@book_id;", con);
                cmd.Parameters.Add("@book_id", SqlDbType.NVarChar).Value = book_id;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void SaveBook(BookDTO book)
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO book_master_tbl(book_id,book_name,genre,author_name," +
                "publisher_name,publish_date,language,edition,book_cost,no_of_pages,book_description,actual_stock,current_stock,book_img_link) " +
                "values(@book_id,@book_name,@genre,@author_name,@publisher_name,@publish_date,@language,@edition,@book_cost," +
                "@no_of_pages,@book_description,@actual_stock,@current_stock,@book_img_link)",
                con);

            cmd.Parameters.AddWithValue("@book_id", book.BookId);
            cmd.Parameters.AddWithValue("@book_name", book.BookName);
            cmd.Parameters.AddWithValue("@genre", book.Genre);
            cmd.Parameters.AddWithValue("@author_name", book.AuthorName);
            cmd.Parameters.AddWithValue("@publisher_name", book.PublisherName);
            cmd.Parameters.AddWithValue("@publish_date", book.PublisherDate);
            cmd.Parameters.AddWithValue("@language", book.Language);
            cmd.Parameters.AddWithValue("@edition", book.Edition);
            cmd.Parameters.AddWithValue("@book_cost", book.CostPerUnit);
            cmd.Parameters.AddWithValue("@no_of_pages", book.Pages);
            cmd.Parameters.AddWithValue("@book_description", book.BookDescription);
            cmd.Parameters.AddWithValue("@actual_stock", book.ActualStock);
            cmd.Parameters.AddWithValue("@current_stock", book.ActualStock);
            cmd.Parameters.AddWithValue("@book_img_link", book.ImagePath);

            cmd.ExecuteNonQuery();
            con.Close();
        }

        public BookDTO getBookById(string bookId)
        {
            SqlConnection conn = new SqlConnection(this.strcon);
            BookDTO book = new BookDTO();

            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand(
                "SELECT [book_id], [book_name], [genre], [author_name], [publisher_name], [publish_date], " +
                "[language], [edition], [book_cost], [no_of_pages], [book_description], [actual_stock], " +
                "[current_stock], [book_img_link],[issued_books] " +
                "FROM [elibraryDB].[dbo].[book_master_tbl] " +
                "WHERE [book_id] = @book_id", conn);

            cmd.Parameters.Add("@book_id", SqlDbType.NVarChar).Value = bookId; // Assuming bookId is a parameter

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                while (reader.Read())
                {

                    /*public BookDTO(string imagePath, string bookId, string bookName, string language, string publisherName ,
                     * string authorName, string genre, string publisherDate, 
                     * string edition, string costPerUnit, string pages, string actualStock, string currentStock, string issuedBooks, 
                     * string bookDescription)
                     */

                    book = new BookDTO(
                                        reader.GetValue(reader.GetOrdinal("book_img_link"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("book_id"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("book_name"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("language"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("publisher_name"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("author_name"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("genre"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("publish_date"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("edition"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("book_cost"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("no_of_pages"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("actual_stock"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("current_stock"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("issued_books"))?.ToString(),
                                        reader.GetValue(reader.GetOrdinal("book_description"))?.ToString()
                                      );
                }
            }
            return book;
        }

        public void DeleteBook(string bookId)
        {
            try
            {
                SqlConnection conn = new SqlConnection(this.strcon);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                SqlCommand cmd = new SqlCommand("delete from dbo.book_master_tbl " +
                    "where book_id = @book_id", conn);

                cmd.Parameters.Add("@book_id", SqlDbType.NVarChar).Value = bookId;

                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBook(BookDTO book)
        {
            this.DeleteBook(book.BookId);
            this.SaveBook(book);
        }

        public List<BookDTO> GetAllBooksBorrowedByMemberId(string memberId)
        {
            List<BookDTO> books = new List<BookDTO>();

            using (SqlConnection connection = new SqlConnection(this.strcon))
            {
                connection.Open();

                string query = @"
                SELECT [book_id]
                      ,[book_name]
                      ,[genre]
                      ,[author_name]
                      ,[publisher_name]
                      ,[publish_date]
                      ,[language]
                      ,[edition]
                      ,[book_cost]
                      ,[no_of_pages]
                      ,[book_description]
                      ,[book_img_link]
                FROM [elibraryDB].[dbo].[book_master_tbl] 
                WHERE [book_id] IN (SELECT book_id FROM book_issue_tbl WHERE member_id = @MemberId);
            ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberId", memberId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            BookDTO book = new BookDTO
                            {
                                BookId = reader["book_id"].ToString(),
                                BookName = reader["book_name"].ToString(),
                                Genre = reader["genre"].ToString(),
                                AuthorName = reader["author_name"].ToString(),
                                PublisherName = reader["publisher_name"].ToString(),
                                PublisherDate = reader["publish_date"].ToString(),
                                Language = reader["language"].ToString(),
                                Edition = reader["edition"].ToString(),
                                CostPerUnit = reader["book_cost"].ToString(),
                                Pages = reader["no_of_pages"].ToString(),
                                BookDescription = reader["book_description"].ToString(),
                                ImagePath = reader["book_img_link"].ToString()
                            };

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }

        public bool BookIsIssued(string bookId)
        {
            bool isIssued = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(this.strcon))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM [elibraryDB].[dbo].[book_issue_tbl] WHERE [book_id] = @BookId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@BookId", bookId);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // If count is greater than 0, it means the book is issued
                        isIssued = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Internal Server Error");
            }

            return isIssued;
        }
    }
}