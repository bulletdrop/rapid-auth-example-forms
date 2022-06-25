using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace rapid_auth_example_forms
{
    public static class new_license_class
    {
        public static string status { get; set; }
        public static string message { get; set; }
    }
    public partial class products : Form
    {
        public products()
        {
            InitializeComponent();
        }

        private void products_Load(object sender, EventArgs e)
        {
            load_products();

        }

        private void load_products()
        {
            string products_response = rapid_auth_dll.rapid_auth.get_active_keys(user.username, user.password);
            Console.WriteLine(products_response);
            JObject response_json_obj = JObject.Parse(products_response);

            string status = (string)response_json_obj.SelectToken("['status']");

            
            if (status != "error")
            {
                IEnumerable<JToken> active_keys = response_json_obj.SelectTokens("$..products[*]");

                foreach (JToken item in active_keys)
                {
                    Console.WriteLine(item);
                    keys_list.Items.Add($"Product: {item.SelectToken("product_name")} - Days left: {item.SelectToken("days_left")}");
                }             
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            keys_list.Items.Clear();

            string key = new_license.Text;
            string response = rapid_auth_dll.rapid_auth.redeem_license_key(user.username, user.password, key);

            JObject response_json = JObject.Parse(response);

            new_license_class.status = (string)response_json.SelectToken("['status']");
            new_license_class.message = (string)response_json.SelectToken("['message']");
            if (new_license_class.status == "success")
            {
                MessageBox.Show(new_license_class.message);
                new_license.Text = "";
                keys_list.Items.Clear();

            }
            else
            {
                MessageBox.Show(new_license_class.message);
            }
            load_products();
        }
    }
}
