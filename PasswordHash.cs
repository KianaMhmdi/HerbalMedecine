using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfMedecine
{
    internal class PasswordHash
    {

        public static string HashPassword(string password)
        {
            // تولید salt تصادفی
            byte[] salt = new byte[16];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            //تابع استخراج کلید(PBKDF2)
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256);

            byte[] hash = pbkdf2.GetBytes(32);//تولید هش
                                              // saltترکیب هش و  
            byte[] saltAndHash = new byte[salt.Length + hash.Length];
            Array.Copy(salt, 0, saltAndHash, 0, salt.Length);
            Array.Copy(hash, 0, saltAndHash, salt.Length, hash.Length);
            return Convert.ToBase64String(saltAndHash);// تبدیل باینری به رشته

        }

        public static bool ValidatePassword(string password, string Id)
        {

            string storedhash;
            SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=HerbalProjectDB;Integrated Security=true");
            try
            {
                //دریافت هش از دیتا بیس
                connection.Open();
                var command = new SqlCommand("select PasswordHash from Person_DB_Table where NationalCode=@Id", connection);
                command.Parameters.AddWithValue("@Id", Id);
                storedhash = command.ExecuteScalar()?.ToString();


                if (string.IsNullOrEmpty(storedhash))
                {
                    return false;
                }

                //Base64    دیکود کردن رشته    

                byte[] saltAndHash = Convert.FromBase64String(storedhash);
                //استخراج مقدار salt
                byte[] salt = new byte[16];
                Array.Copy(saltAndHash, 0, salt, 0, salt.Length);
                //استخراج مقدار Hash
                byte[] hash = new byte[saltAndHash.Length - salt.Length];
                Array.Copy(saltAndHash, salt.Length, hash, 0, hash.Length);

                //محاسبه دوباره هش با استفاده از رمز عبور و salt
                Rfc2898DeriveBytes pbkd2 = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256);

                byte[] computedHash = pbkd2.GetBytes(32);

                //مقایسه هش ها
                return StructuralComparisons.StructuralEqualityComparer.Equals(hash, computedHash);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
