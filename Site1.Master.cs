using ELibraryManagement.enums;
using ELibraryManagement.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ELibraryManagement
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        private QueryRunner queryRunner = new QueryRunner();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null )
            {
                LinkButton1.Visible = true;     //  user-login
                LinkButton2.Visible = true;     //  sign-up

                LinkButton3.Visible = false;    //  logout
                LinkButton7.Visible = false;    //  Hello User

                LinkButton6.Visible = true;     //  Admin Login

                LinkButton11.Visible = false;    //  Author mgmt
                LinkButton12.Visible = false;    //  Publisher mgmt
                LinkButton8.Visible = false;     //  Book Inventory
                LinkButton9.Visible = false;     //  Book Issuing     
                LinkButton10.Visible = false;    //  Member mgmt

            }

            else if (Session["role"].Equals(""))
            {
                LinkButton1.Visible = true;     //  user-login
                LinkButton2.Visible = true;     //  sign-up

                LinkButton3.Visible = false;    //  logout
                LinkButton7.Visible = false;    //  Hello User

                LinkButton6.Visible = true;     //  Admin Login

                LinkButton11.Visible = false;    //  Author mgmt
                LinkButton12.Visible = false;    //  Publisher mgmt
                LinkButton8.Visible = false;     //  Book Inventory
                LinkButton9.Visible = false;     //  Book Issuing     
                LinkButton10.Visible = false;    //  Member mgmt

            }

            else if( Session["role"].Equals(UserTypes.USER.ToString()))
            {
                    LinkButton1.Visible = false;     //  user-login
                    LinkButton2.Visible = false;     //  sign-up

                    LinkButton3.Visible = true;    //  logout
                    LinkButton7.Visible = true;    //  Hello User
                    LinkButton7.Text = $"Hello {Session["username"]}";    //  Set the text of User

                    LinkButton6.Visible = true;     //  Admin Login

                    LinkButton11.Visible = false;    //  Author mgmt
                    LinkButton12.Visible = false;    //  Publisher mgmt
                    LinkButton8.Visible = false;     //  Book Inventory
                    LinkButton9.Visible = false;     //  Book Issuing     
                    LinkButton10.Visible = false;    //  Member mgmt
            }

            else if (Session["role"].Equals(UserTypes.ADMIN.ToString()))
            {
                LinkButton1.Visible = false;     //  user-login
                LinkButton2.Visible = false;     //  sign-up

                LinkButton3.Visible = true;    //  logout
                LinkButton7.Visible = true;    //  Hello User
                LinkButton7.Text = $"Hello {Session["username"]}";    //  Set the text of User

                LinkButton6.Visible = false;     //  Admin Login

                LinkButton11.Visible = true;    //  Author mgmt
                LinkButton12.Visible = true;    //  Publisher mgmt
                LinkButton8.Visible = true;     //  Book Inventory
                LinkButton9.Visible = true;     //  Book Issuing     
                LinkButton10.Visible = true;    //  Member mgmt
            }
        }

        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Response.Redirect("adminlogin.aspx");
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            Response.Redirect("authormanagement.aspx");
        }

        protected void LinkButton12_Click(object sender, EventArgs e)
        {
            Response.Redirect("publishermanagement.aspx");
        }

        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            Response.Redirect("bookinventory.aspx");
        }

        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            Response.Redirect("issuebook.aspx");
        }

        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            Response.Redirect("membermanagement.aspx");
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Response.Redirect("viewbooks.aspx");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("userlogin.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("signup.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            queryRunner.DoLogout(Session);
            Response.Redirect("Homepage.aspx");
        }

        protected void LinkButton7_Click(object sender, EventArgs e)
        {
            Response.Redirect("userprofile.aspx");
        }
    }
}