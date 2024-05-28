using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KURSUVA
{
    public partial class Form3 : Form
    {
        public static int UserId { get; private set; }

        public Form3()
        {
            InitializeComponent();

            numberPhone.Text = "Номер телефону";
            numberPhone.ForeColor = Color.Gray;

            passwordField.Text = "Password";
            passwordField.ForeColor = Color.Gray;
        }

        private void ButtomLogin_Click(object sender, EventArgs e)
        {
            string loginuser = numberPhone.Text;
            string passworduser = passwordField.Text;

            using (SqlConnection connection = new SqlConnection(@"Server=DESKTOP-ONBL7EE;Database=BDB;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT * FROM users WHERE numberPhone = @np AND password = @up";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@np", loginuser);
                    command.Parameters.AddWithValue("@up", passworduser);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {

                        reader.Read();


                        int userId = reader.GetInt32(reader.GetOrdinal("id_users"));

 
                        CurrentUser.id_users = userId;


                        this.Hide();
                        Form1 mainForm = new Form1();
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Невірний логін або пароль");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка входу: " + ex.Message);
                }
            }
        }

        public static class CurrentUser
        {
            public static string numberPhone;
            public static string password;
            public static int id_users;
        }

        private void numberPhone_Enter(object sender, EventArgs e)
        {
            if (numberPhone.Text == "Номер телефону")
            {
                numberPhone.Text = "";
                numberPhone.ForeColor = Color.Black;
            }
        }

        private void passwordField_Enter(object sender, EventArgs e)
        {
            if (passwordField.Text == "Password")
            {
                passwordField.Text = "";
                passwordField.ForeColor = Color.Black;
            }
        }

        private void numberPhone_Leave(object sender, EventArgs e)
        {
            if (numberPhone.Text == "")
            {
                numberPhone.Text = "Номер телефону";
                numberPhone.ForeColor = Color.Gray;
            }
        }

        private void passwordField_Leave(object sender, EventArgs e)
        {
            if (passwordField.Text == "")
            {
                passwordField.Text = "Password";
                passwordField.ForeColor = Color.Gray;
            }
        }

        private void registerLabel(object sender, EventArgs e)
        {
            this.Hide();
            Form2 registerform = new Form2();
            registerform.Show();
        }

        private void ButtomExit_click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
