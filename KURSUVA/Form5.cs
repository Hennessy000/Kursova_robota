using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using Mysqlx;
using System.Globalization;

namespace KURSUVA
{
    public partial class Form5 : Form
    {
        private readonly DB db = new DB();

        public Form5()
        {
            InitializeComponent();


            PressYourNumberCard.Text = "Номер картки";
            PressYourNumberCard.ForeColor = Color.Gray;

            YourDataTime1.Text = "Термін дії";
            YourDataTime1.ForeColor = Color.Gray;

            YourCVV.Text = "Код безпеки";
            YourCVV.ForeColor = Color.Gray;

            NumberCardPeople1.Text = "Номер картки отримувача";
            NumberCardPeople1.ForeColor = Color.Gray;

            YourSum1.Text = "Сума";
            YourSum1.ForeColor = Color.Gray;
        }

        private void ButtomExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void PressYourNumberCard_Enter(object sender, EventArgs e)
        {
            if (PressYourNumberCard.Text == "Номер картки")
            {
                PressYourNumberCard.Text = "";
                PressYourNumberCard.ForeColor = Color.Black;
            }
        }

        private void PressYourNumberCard_Leave(object sender, EventArgs e)
        {
            if (PressYourNumberCard.Text == "")
            {
                PressYourNumberCard.Text = "Номер картки";
                PressYourNumberCard.ForeColor = Color.Gray;
            }
            else
            {
                string cardNumber = PressYourNumberCard.Text.Replace(" ", "");
                if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                {
                    MessageBox.Show("Номер картки повинен містити 16 цифр.");
                    PressYourNumberCard.Text = "Номер картки";
                    PressYourNumberCard.ForeColor = Color.Gray;
                }
                else
                {
                    PressYourNumberCard.Text = string.Join(" ", Enumerable.Range(0, 16 / 4).Select(i => cardNumber.Substring(i * 4, 4)));
                }
            }
        }

        private void YourDataTime_Enter(object sender, EventArgs e)
        {
            if (YourDataTime1.Text == "Термін дії")
            {
                YourDataTime1.Text = "";
                YourDataTime1.ForeColor = Color.Black;
            }
        }

        private void YourDataTime_Leave(object sender, EventArgs e)
        {
            if (YourDataTime1.Text == "")
            {
                YourDataTime1.Text = "Термін дії";
                YourDataTime1.ForeColor = Color.Gray;
            }
            else
            {
                string expiryDate = YourDataTime1.Text.Replace("/", "");
                if (expiryDate.Length != 4 || !expiryDate.All(char.IsDigit))
                {
                    MessageBox.Show("Термін дії повинен бути у форматі MM/РР (наприклад, 01/25).");
                    YourDataTime1.Text = "Термін дії";
                    YourDataTime1.ForeColor = Color.Gray;
                }
                else
                {
                    YourDataTime1.Text = expiryDate.Insert(2, "/");
                }
            }
        }

        private void YourCVV_Enter(object sender, EventArgs e)
        {
            if (YourCVV.Text == "Код безпеки")
            {
                YourCVV.Text = "";
                YourCVV.ForeColor = Color.Black;
            }
        }

        private void YourCVV_Leave(object sender, EventArgs e)
        {
            if (YourCVV.Text == "")
            {
                YourCVV.Text = "Код безпеки";
                YourCVV.ForeColor = Color.Gray;
            }
            else
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(YourCVV.Text, @"^\d{3}$"))
                {
                    MessageBox.Show("Код безпеки повинен містити 3 цифри.");
                    YourCVV.Text = "Код безпеки";
                    YourCVV.ForeColor = Color.Gray;
                }
            }
        }

        private void NumberCardPeople_Enter(object sender, EventArgs e)
        {
            if (NumberCardPeople1.Text == "Номер картки отримувача")
            {
                NumberCardPeople1.Text = "";
                NumberCardPeople1.ForeColor = Color.Black;
            }
        }

        private void NumberCardPeople_Leave(object sender, EventArgs e)
        {
            if (NumberCardPeople1.Text == "")
            {
                NumberCardPeople1.Text = "Номер картки отримувача";
                NumberCardPeople1.ForeColor = Color.Gray;
            }
            else
            {
                string cardNumber = NumberCardPeople1.Text.Replace(" ", "");
                if (cardNumber.Length != 16 || !cardNumber.All(char.IsDigit))
                {
                    MessageBox.Show("Номер картки повинен містити 16 цифр.");
                    NumberCardPeople1.Text = "Номер картки отримувача";
                    NumberCardPeople1.ForeColor = Color.Gray;
                }
                else
                {
                    NumberCardPeople1.Text = string.Join(" ", Enumerable.Range(0, 16 / 4).Select(i => cardNumber.Substring(i * 4, 4)));
                }
            }
        }

        private void YourSum_Enter(object sender, EventArgs e)
        {
            if (YourSum1.Text == "Сума")
            {
                YourSum1.Text = "";
                YourSum1.ForeColor = Color.Black;
            }
        }

        private void YourSum_Leave(object sender, EventArgs e)
        {
            if (YourSum1.Text == "")
            {
                YourSum1.Text = "Сума";
                YourSum1.ForeColor = Color.Gray;
            }
        }

        private void Transaction_Click_1(object sender, EventArgs e)
        {
            try
            {
                string cardNumber = PressYourNumberCard.Text;
                string expirationDate = YourDataTime1.Text;
                string securityCode = YourCVV.Text;
                string destinationCard = NumberCardPeople1.Text;
                decimal transactionAmount = Convert.ToDecimal(YourSum1.Text);

                
                int senderUserId = db.GetUserIdByCardNumber(cardNumber);
                if (senderUserId == 0)
                {
                    MessageBox.Show("Карта відправника не знайдена", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int recipientUserId = db.GetUserIdByCardNumber(destinationCard);
                if (recipientUserId == 0)
                {
                    MessageBox.Show("Карта отримувача не знайдена", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (transactionAmount < 1.00m)
                {
                    MessageBox.Show("Мінімальна сума перевода - 1.00 UAH", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                decimal balanceCheck = db.GetBalanceFromDatabase(senderUserId);
                if (transactionAmount < balanceCheck)
                {
                    MessageBox.Show("Недостатньо коштів для проведення операції", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                bool transactionResult = db.Transaction(senderUserId, recipientUserId, transactionAmount);
                if (transactionResult)
                {
                    
                    bool recordResult = db.RecordTransaction(senderUserId, recipientUserId, transactionAmount);
                    if (recordResult)
                    {
                        MessageBox.Show("Операція успішно виконана", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Помилка при виконуванні транзакції", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Помилка при виконуванні транзакції", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при виконуванні транзакції: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
