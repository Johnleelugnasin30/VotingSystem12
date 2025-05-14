using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Voting_System_Final
{
    public partial class AdminForm: Form
    {
        private string connectionString = "server=localhost;database=votingdb;username=root;password=;";
        public AdminForm()
        {
            InitializeComponent();
        }

        private void AdminForm_Click(object sender, EventArgs e)
        {

        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = Username.Text;
            string password = Password.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Assuming there is a separate `admin` table with plain text passwords
                    string query = "SELECT FullName FROM admin WHERE Username = @username AND Password = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password); // No hashing

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string fullName = reader["FullName"].ToString();
                                MessageBox.Show("Admin login successful!\nWelcome, " + fullName);

                                AdminDashboard adminDashboard = new AdminDashboard();
                                adminDashboard.Show();

                                this.Hide();
                                // Open Admin Dashboard here if needed
                            }
                            else
                            {
                                MessageBox.Show("Invalid admin username or password.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }

        private void Password_TextChanged(object sender, EventArgs e)
        {
            Password.UseSystemPasswordChar = true; // Hide password on form load
        }


        private void Showpass_CheckedChanged(object sender, EventArgs e)
        {
            // Show password only if checkbox is checked
            if (Showpass.Checked)
            {
                Password.UseSystemPasswordChar = false; // Show password
            }
            else
            {
                Password.UseSystemPasswordChar = true; // Hide password
            }
        }

        private void Password_Click(object sender, EventArgs e)
        {
            Password.UseSystemPasswordChar = true; // Force hide again on click
            Showpass.Checked = false;              // Optional: uncheck the box
        }

    }
}
