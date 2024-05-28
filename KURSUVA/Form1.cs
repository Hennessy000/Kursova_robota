using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KURSUVA
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
            UpdateCurrentTime();
            Timer timer = new Timer();
            timer.Tick += Timer_tick1;
            timer.Start();

            UpdateBalanceAndCardNumber();
        }


        private void UpdateCurrentTime()
        {
            DateTime currentTime = DateTime.Now;
            TimerTick.Text = "Зараз часу: " + currentTime.ToString("HH:mm:ss");
        }

        private void Timer_tick1(object sender, EventArgs e)
        {
            UpdateCurrentTime();
        }

        private void RegisterNewBankCards_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 RegisterCardform = new Form4();

            RegisterCardform.Show();
        }

        private void ExitForm_click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void UpdateBalanceAndCardNumber()
        {
            int userId = Form3.CurrentUser.id_users;

            DB db = new DB();

            decimal balance = db.GetBalanceFromDatabase(userId);
            string cardNumber = db.GetCardNumberFromDatabase(userId);

            InfoBalance.Text = $"{balance:0.##} грн";
            DetailInformationCard.Text = $"{balance:0.##} UAH";
            MoneyTransactionUp.Text = $"+{balance:0.##} UAH";
            DetailNumberCard.Text = $"{cardNumber}";
            LastIncome.Text = $"{balance:0.##} UAH";
            NumberCard.Text = $"{cardNumber}";
        }
        private void UserInfoBalance(object sender, PaintEventArgs e)
        {
            UpdateBalanceAndCardNumber();
        }
        private void UserNumberCard(object sender, PaintEventArgs e)
        {
            UpdateBalanceAndCardNumber();
        }

        private void DetailsInformation(object sender, PaintEventArgs e)
        {
            UpdateBalanceAndCardNumber();
        }

        private void DetailNumberCard_Paint(object sender, PaintEventArgs e)
        {
            UpdateBalanceAndCardNumber();
        }
        private void UpdateWelcomeLabel()
        {
            int userId = Form3.CurrentUser.id_users;

            DB db = new DB();

            string userName = db.GetUserNameFromDatabase(userId);

            LabelWelcome.Text = $"Привіт, {userName}!";
        }

        private void LableWelcomeUser(object sender, PaintEventArgs e)
        {
            UpdateWelcomeLabel();
        }

        private void Income(object sender, PaintEventArgs e)
        {
            UpdateBalanceAndCardNumber();
        }

        private void MoneyTransactionUp_Paint(object sender, PaintEventArgs e)
        {
            UpdateBalanceAndCardNumber();
        }

        private void TransferToCard_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 TransferToCard = new Form5();
            TransferToCard.Show();
        }
    }
}
