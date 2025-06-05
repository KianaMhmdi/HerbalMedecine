using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;
using System.Data;




namespace WpfMedecine.Data
{
    internal class HerbalDB
    {

        //آدرس دیتابیس
        private string connectionString = "Data Source=.;Initial Catalog=HerbalProjectDB;Integrated Security=true";

        ///اضافه کردن فرد به دیتابیس
        public bool AddPerson(Person person)
        {
            string hashedPassword = PasswordHash.HashPassword(person.PasswordHash);
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "INSERT INTO Person_DB_Table(NationalCode, FirstName, LastName,PhoneNumber, Gender, PasswordHash, Address,BirthDate, Role, Email,FavoriteNumber) " +
               "VALUES (@nationalCode, @firstName, @lastName, @phoneNumber, @gender, @passwordHash, @address, @birthDate, @role, @email,@favoriteNumber)";


                SqlCommand command = new SqlCommand(query, connection);

                //مقدار دهی پارامتر ها

                command.Parameters.AddWithValue("@nationalCode", person.Id);
                command.Parameters.AddWithValue("@firstName", person.FirstName);
                command.Parameters.AddWithValue("@lastName", person.LastName);
                command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                command.Parameters.AddWithValue("@gender", person.Gender);
                command.Parameters.AddWithValue("@passwordHash", hashedPassword);

                command.Parameters.AddWithValue("@address", person.Address);
                command.Parameters.AddWithValue("@birthDate", person.BirthDate);
                command.Parameters.AddWithValue("@role", person.Role);
                command.Parameters.AddWithValue("@email", person.Email);
                command.Parameters.AddWithValue("@favoriteNumber", person.FavoriteNumber);

                command.ExecuteNonQuery();
                return true;
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

        public bool EditPassword(Person person)
        {
            string hashedNewPassword = PasswordHash.HashPassword(person.PasswordHash);
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "update Person_DB_Table set PasswordHash=@NewPassword where NationalCode=@Id and PhoneNumber=@phoneNumber and FavoriteNumber=@favoriteNumber";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@NewPassword", hashedNewPassword);
                command.Parameters.AddWithValue("@Id", person.Id);
                command.Parameters.AddWithValue("@phoneNumber", person.PhoneNumber);
                command.Parameters.AddWithValue("@favoriteNumber", person.FavoriteNumber);


                command.ExecuteNonQuery();
                return true;

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

        ///select owner,seles,customer
        public DataTable SelectPerson(string RoleType)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $"select NationalCode,FirstName, LastName,PhoneNumber,Address from Person_DB_Table where Role='{RoleType}'";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable data = new DataTable();
            adapter.Fill(data);
            connection.Close();
            return data;

        }

        //filter tables search
        public DataTable Search(string filter, string nameTable, string RoleType)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            string query = $" select NationalCode,FirstName,LastName,PhoneNumber from {nameTable} where Role='{RoleType}' and ( FirstName like '{filter}%' or LastName like '{filter}%') ";
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable data = new DataTable();
            adapter.Fill(data);
            return data;

        }
        //deletePerson
        public bool DeletePeson(string NationalCode, string NameTable)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                connection.Open();
                string query = $"delete {NameTable} where NationalCode='{NationalCode}'";
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
                return true;

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
        //updatePerson
        public bool UpdatePerson(Person PersonEdite)
        {
            string hashedPassword = PasswordHash.HashPassword(PersonEdite.PasswordHash);
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                string query = "update Person_DB_Table set " +
                    "NationalCode=@nationalCode, FirstName=@firstName" +
                    ", LastName=@lastName,PhoneNumber=@phoneNumber, Gender=@gender, PasswordHash=@passwordHash" +
                    ", Address=@address,BirthDate=@birthDate, Role=@role, Email=@email,FavoriteNumber=@favoriteNumber " +
                    "where NationalCode=@nationalCode";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("nationalCode", PersonEdite.Id);
                command.Parameters.AddWithValue("@firstName", PersonEdite.FirstName);
                command.Parameters.AddWithValue("@lastName", PersonEdite.LastName);
                command.Parameters.AddWithValue("@phoneNumber", PersonEdite.PhoneNumber);
                command.Parameters.AddWithValue("@gender", PersonEdite.Gender);
                command.Parameters.AddWithValue("@passwordHash", hashedPassword);
                command.Parameters.AddWithValue("@address", PersonEdite.Address);
                command.Parameters.AddWithValue("@birthDate", PersonEdite.BirthDate);
                command.Parameters.AddWithValue("@role", PersonEdite.Role);
                command.Parameters.AddWithValue("@email", PersonEdite.Email);
                command.Parameters.AddWithValue("@favoriteNumber", PersonEdite.FavoriteNumber);
                command.ExecuteNonQuery();
                return true;
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