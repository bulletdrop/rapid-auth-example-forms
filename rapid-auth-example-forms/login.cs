using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using System.Net;
using rapid_auth_example_forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections.Specialized;

namespace rapid_auth_example_forms
{
    public static class user
    {
        public static string username { get; set; }
        public static string password { get; set; }
    }
    public static class response_class
    {
        public static string Status { get; set; }
        public static string Message { get; set; }
    }
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            rapid_auth_dll.rapid_auth.API_KEY = "YOUR API KEY";
            rapid_auth_dll.rapid_auth.OPENSSL_KEY = "YOUR OPEN SSL KEY";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            string username = username_textbox.Text;
            string password = password_textbox.Text;

            string response = rapid_auth_dll.rapid_auth.sign_in(username, password);
            JObject response_json = JObject.Parse(response);

            response_class.Status = (string)response_json.SelectToken("['status']");
            response_class.Message = (string)response_json.SelectToken("['message']");

            if (response_class.Status == "error")
            {
                MessageBox.Show("Error: " + response_class.Message);
                return;
            }

            if (response_class.Status == "success")
            {
                user.username = username;
                user.password = password;
                products Products = new products();
                Products.Show();
                this.Hide();
            }
            
        }
    }
}
