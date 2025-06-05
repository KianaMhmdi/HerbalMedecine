using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfMedecine;

namespace WpfHerbalMedecine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public string ValidateRole(string Id)
        {

            string storedRole;
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=HerbalProjectDB;Integrated Security=true");
            try
            {
                connection.Open();
                var command = new SqlCommand("select Role from Person_DB_Table where NationalCode=@Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                storedRole = command.ExecuteScalar()?.ToString();
                return storedRole;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool login = PasswordHash.ValidatePassword(Password.Password.ToString(), UserName.Text);

            if (login && ValidateRole(UserName.Text) == "System.Windows.Controls.ComboBoxItem: Owner")
            {


                var OwnerPanel = new OwnerPanel();
                OwnerPanel.Show();
                this.Close();
            }
            if (login && ValidateRole(UserName.Text) == "System.Windows.Controls.ComboBoxItem: Seller")
            {
                MessageBox.Show("selers");

                //پنل فروشنده 
            }

            if (!login)
            {
                MessageBox.Show("Invalid user name or password.");
            }
        }

        private void ctnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            var forgotPassword = new ForgotPassword();
            forgotPassword.ShowDialog();
        }

        private void btnCreatAcount_Click(object sender, RoutedEventArgs e)
        {
            var registeration = new Registeration();
            registeration.ShowDialog();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Username : Your national ID number\nPassword : The password you set during registration", "Login Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }  
}
