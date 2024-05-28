using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KURSUVA
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();

            Card_number.Text = "Номер картки";
            Card_number.ForeColor = Color.Gray;

            TimeAction.Text = "Термін дії";
            TimeAction.ForeColor = Color.Gray;

            CodeSecurity.Text = "Код безпеки";
            CodeSecurity.ForeColor = Color.Gray;
        }

        private void ButtomRegisterCard_Click(object sender, EventArgs e)
        {
            if (Card_number.Text == "Номер картки" || TimeAction.Text == "Термін дії" || CodeSecurity.Text == "Код безпеки")
            {
                MessageBox.Show("Заповніть всі поля");
                return;
            }

            string phoneNumber = Form3.CurrentUser.numberPhone;


            DB db = new DB();

            int userId = db.GetUserIdByPhoneNumber(phoneNumber);

            using (SqlConnection connection = new SqlConnection(@"Server=DESKTOP-ONBL7EE;Database=BDB;Integrated Security=True"))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO bank_cards (id_users, CardNumber, ExpirationDate, SecurityCode) VALUES (@id_users, @CardNumber, @ExpirationDate, @SecurityCode)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_users", userId);
                    command.Parameters.AddWithValue("@CardNumber", Card_number.Text.Replace(" ", ""));
                    command.Parameters.AddWithValue("@ExpirationDate", TimeAction.Text.Replace("/", ""));
                    command.Parameters.AddWithValue("@SecurityCode", CodeSecurity.Text);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Картка додана");
                    }
                    else
                    {
                        MessageBox.Show("Помилка додавання карти");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка додавання карти: " + ex.Message);
                }
            }
            this.Hide();
            Form1 mainForm = new Form1();
            mainForm.Show();
        }

        private void Card_number_Enter(object sender, EventArgs e)
        {
            if (Card_number.Text == "Номер картки")
            {
                Card_number.Text = "";
                Card_number.ForeColor = Color.Black;
            }
        }

        private void Card_number_Leave(object sender, EventArgs e)
        {
            if (Card_number.Text == "")
            {
                Card_number.Text = "Номер картки";
                Card_number.ForeColor = Color.Gray;
            }
            else
            {
                string cardNumber = Card_number.Text.Replace(" ", "");
                if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                {
                    MessageBox.Show("Номер картки повинен містити 16 цифр.");
                    Card_number.Text = "Номер картки";
                    Card_number.ForeColor = Color.Gray;
                }
                else
                {
                    Card_number.Text = string.Join(" ", Enumerable.Range(0, 16 / 4).Select(i => cardNumber.Substring(i * 4, 4)));
                }
            }
        }

        private void TimeAction_Enter(object sender, EventArgs e)
        {
            if (TimeAction.Text == "Термін дії")
            {
                TimeAction.Text = "";
                TimeAction.ForeColor = Color.Black;
            }
        }

        private void TimeAction_Leave(object sender, EventArgs e)
        {
            if (TimeAction.Text == "")
            {
                TimeAction.Text = "Термін дії";
                TimeAction.ForeColor = Color.Gray;
            }
            else
            {
                string expiryDate = TimeAction.Text.Replace("/", "");
                if (expiryDate.Length != 4 || !expiryDate.All(char.IsDigit))
                {
                    MessageBox.Show("Термін дії повинен бути у форматі MM/РР (наприклад, 01/25).");
                    TimeAction.Text = "Термін дії";
                    TimeAction.ForeColor = Color.Gray;
                }
                else
                {
                    TimeAction.Text = expiryDate.Insert(2, "/");
                }
            }
        }

        private void CodeSecurity_Enter(object sender, EventArgs e)
        {
            if (CodeSecurity.Text == "Код безпеки")
            {
                CodeSecurity.Text = "";
                CodeSecurity.ForeColor = Color.Black;
            }
        }

        private void CodeSecurity_Leave(object sender, EventArgs e)
        {
            if (CodeSecurity.Text == "")
            {
                CodeSecurity.Text = "Код безпеки";
                CodeSecurity.ForeColor = Color.Gray;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(CodeSecurity.Text, @"^\d{3}$"))
                {
                    MessageBox.Show("Код безпеки повинен містити 3 цифри.");
                    CodeSecurity.Text = "Код безпеки";
                    CodeSecurity.ForeColor = Color.Gray;
                }
            }
        }

        private void ButtomExit_click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
