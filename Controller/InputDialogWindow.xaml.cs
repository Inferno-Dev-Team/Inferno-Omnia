using System;
using System.Windows;

namespace Inferno_Mod_Manager.Controller
{
    public partial class InputDialogSample : Window
    {
        public InputDialogSample(string question, string question2, string question3, string question4, string question5, string question6, string defaultAnswer = "")
        {
            InitializeComponent();
            lblQuestion.Content = question;
            lblQuestion2.Content = question2;
            lblQuestion3.Content = question3;
            lblQuestion4.Content = question4;
            lblQuestion5.Content = question5;
            lblQuestion6.Content = question6;
            txtAnswer.Text = defaultAnswer;
            txtAnswer2.Text = defaultAnswer;
            txtAnswer3.Text = defaultAnswer;
            txtAnswer4.Text = defaultAnswer;
            txtAnswer5.Text = defaultAnswer;
            txtAnswer6.Text = defaultAnswer;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }
        public string Answer2
        {
            get { return txtAnswer2.Text; }
        }
        public string Answer3
        {
            get { return txtAnswer3.Text; }
        }
        public string Answer4
        {
            get { return txtAnswer4.Text; }
        }
        public string Answer5
        {
            get { return txtAnswer5.Text; }
        }
        public string Answer6
        {
            get { return txtAnswer6.Text; }
        }
    }
}
