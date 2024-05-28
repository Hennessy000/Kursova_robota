using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KURSUVA
{
    public partial class Form2 : Form
    {
        DB database = new DB();

        public Form2()
        {
            InitializeComponent();

            UserNameField.Text = "Ім\'я";
            UserNameField.ForeColor = Color.Gray;

            UserSurnameField.Text = "Прізвище";
            UserSurnameField.ForeColor = Color.Gray;

            Middlename.Text = "По-батькові";
            Middlename.ForeColor = Color.Gray;

            passwordField.Text = "Password";
            passwordField.ForeColor = Color.Gray;

            PasswordRetryField.Text = "retryPassword";
            PasswordRetryField.ForeColor = Color.Gray;

            NumberPhone.Text = "Номер телефону";
            NumberPhone.ForeColor = Color.Gray;

            Mail.Text = "Електронна почта";
            Mail.ForeColor = Color.Gray;
        }

        private void UserNameField_Enter(object sender, EventArgs e)
        {
            if (UserNameField.Text == "Ім\'я")
            {
                UserNameField.Text = "";
                UserNameField.ForeColor = Color.Black;
            }
        }
        private void UserNameField_Leave(object sender, EventArgs e)
        {
            if (UserNameField.Text == "")
            {
                UserNameField.Text = "І\'мя";
                UserNameField.ForeColor = Color.Gray;
            }
        }

        private void UserSurnameField_Enter(object sender, EventArgs e)
        {
            if (UserSurnameField.Text == "Прізвище")
            {
                UserSurnameField.Text = "";
                UserSurnameField.ForeColor = Color.Black;
            }
        }

        private void UserSurnameField_Leave(object sender, EventArgs e)
        {
            if (UserSurnameField.Text == "")
            {
                UserSurnameField.Text = "Прізвище";
                UserSurnameField.ForeColor = Color.Gray;
            }
        }
        private void Middlename_Enter(object sender, EventArgs e)
        {
            if (Middlename.Text == "По-батькові")
            {
                Middlename.Text = "";
                Middlename.ForeColor = Color.Black;
            }
        }

        private void Middlename_Leave(object sender, EventArgs e)
        {
            if (Middlename.Text == "")
            {
                Middlename.Text = "По-батькові";
                Middlename.ForeColor = Color.Gray;
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

        private void passwordField_Leave(object sender, EventArgs e)
        {
            if (passwordField.Text == "")
            {
                passwordField.Text = "Password";
                passwordField.ForeColor = Color.Gray;
            }
        }

        private void PasswordRetryField_Enter(object sender, EventArgs e)
        {
            if (PasswordRetryField.Text == "retryPassword")
            {
                PasswordRetryField.Text = "";
                PasswordRetryField.ForeColor = Color.Black;
            }
        }

        private void PasswordRetryField_Leave(object sender, EventArgs e)
        {
            if (PasswordRetryField.Text == "")
            {
                PasswordRetryField.Text = "retryPassword";
                PasswordRetryField.ForeColor = Color.Gray;
            }
        }

        private void NumberPhone_Enter(object sender, EventArgs e)
        {
            if (NumberPhone.Text == "Номер телефону")
            {
                NumberPhone.Text = "";
                NumberPhone.ForeColor = Color.Black;
            }
        }

        private void NumberPhone_Leave(object sender, EventArgs e)
        {
            if (NumberPhone.Text == "")
            {
                NumberPhone.Text = "Номер телефону";
                NumberPhone.ForeColor = Color.Gray;
            }
        }

        private void Mail_Enter(object sender, EventArgs e)
        {
            if (Mail.Text == "Електронна почта")
            {
                Mail.Text = "";
                Mail.ForeColor = Color.Black;
            }
        }

        private void Mail_Leave(object sender, EventArgs e)
        {
            if (Mail.Text == "")
            {
                Mail.Text = "Електронна почта";
                Mail.ForeColor = Color.Gray;
            }
        }

        private void ButtomRegister_Click(object sender, EventArgs e)
        {
            if (UserNameField.Text == "Ім\'я")
            {
                MessageBox.Show("Введіть Ім\'я");
                return;
            }

            if (UserSurnameField.Text == "Прізвище")
            {
                MessageBox.Show("Введіть прізвище");
                return;
            }

            if (Middlename.Text == "По-батькові")
            {
                MessageBox.Show("Введіть По-батькові");
                return;
            }

            if (passwordField.Text == "Password")
            {
                MessageBox.Show("Введіть Password");
                return;
            }

            if (PasswordRetryField.Text == "retryPassword")
            {
                MessageBox.Show("Введіть пароль повторно");
                return;
            }

            if (string.IsNullOrWhiteSpace(NumberPhone.Text))
            {
                MessageBox.Show("Введіть номер телефону");
                return;
            }

            using (SqlConnection connection = new SqlConnection(@"Server=DESKTOP-ONBL7EE;Database=BDB;Integrated Security=True"))
            {
                try
                {


                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM users WHERE numberPhone = @numberPhone";
                    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@numberPhone", NumberPhone.Text);

                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Номер вже зареєстрований");
                        return;
                    }

                    string query = "INSERT INTO users (name, surname, midllename, password, numberPhone, mail) " +
                    "VALUES (@name, @surname, @midllename, @password, @numberPhone, @mail)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", UserNameField.Text);
                    command.Parameters.AddWithValue("@surname", UserSurnameField.Text);
                    command.Parameters.AddWithValue("@midllename", Middlename.Text);
                    command.Parameters.AddWithValue("@password", passwordField.Text);
                    command.Parameters.AddWithValue("@numberPhone", NumberPhone.Text);
                    command.Parameters.AddWithValue("@mail", Mail.Text);

                    int rowsAffected = command.ExecuteNonQuery();

             
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Аккаунт зареєстрований");
                    }
                    else
                    {
                        MessageBox.Show("Аккаунт не Зареєстровано");
                    }
                }
                catch (Exception ex)
                {
    
                    MessageBox.Show("Помилка при реєстрації аккаунта: " + ex.Message);
                }
            }
        }

        private void ButtomExit_click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoginLabel(object sender, EventArgs e)
        {
            this.Hide();
            Form3 loginform = new Form3();
            loginform.Show();
        }
    }
}
