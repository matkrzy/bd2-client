using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BD_client.Windows
{
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>
    public partial class InputTextModalWindow : Window
    {
        public InputTextModalWindow(string title, string question, string answer = "")
        {
            InitializeComponent();
            Title = title;
            Question.Content = question;
            AnswerTextbox.Text = answer;
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            AnswerTextbox.SelectAll();
            AnswerTextbox.Focus();
        }

        private void OnAcceptClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string Answer
        {
            get { return AnswerTextbox.Text; }
        }
    }
}
