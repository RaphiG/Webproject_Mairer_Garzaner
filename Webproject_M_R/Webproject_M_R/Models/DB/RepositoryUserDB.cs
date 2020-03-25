using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Common;


namespace Webproject_M_R.Models.DB
{
    public class RepositoryUserDB : IRepositoryUser
    {
        private string _connectionString = "Server=localhost;Database=db_webproject;Uid=root;Pwd=Platin12;";

        private MySqlConnection _connection = null;

        public void Open()
        {
            if (this._connection == null)
            {
                this._connection = new MySqlConnection(this._connectionString);
            }

            if (this._connection.State != ConnectionState.Open)
            {
                this._connection.Open();
            }
        }

        public void Close()
        {
            if ((this._connection != null) && (this._connection.State != ConnectionState.Closed))
            {
                this._connection.Close();
            }
        }

        public bool Delete(int id)
        {
            DbCommand cmdDel = this._connection.CreateCommand();
            cmdDel.CommandText = "DELETE FROM users WHERE id=@userid";

            DbParameter paramId = cmdDel.CreateParameter();
            paramId.ParameterName = "userid";
            paramId.Value = id;
            paramId.DbType = DbType.Int32;

            cmdDel.Parameters.Add(paramId);

            return cmdDel.ExecuteNonQuery() == 1;

        }

        public bool Insert(User user)
        {
            if (user==null)
            {
                return false;
            }

            DbCommand cmdInsert = this._connection.CreateCommand();

            cmdInsert.CommandText = "INSERT INTO users VALUES(null, @firstname, @lastname, @gender, @birthdate, @username, sha2(@password, 512))";

            DbParameter paramFN = cmdInsert.CreateParameter();
            paramFN.ParameterName = "firstname";
            paramFN.Value = user.Firstname;
            paramFN.DbType = DbType.String;

            DbParameter paramLN = cmdInsert.CreateParameter();
            paramLN.ParameterName = "lastname";
            paramLN.Value = user.Lastname;
            paramLN.DbType = DbType.String;

            DbParameter paramGender = cmdInsert.CreateParameter();
            paramGender.ParameterName = "gender";
            paramGender.Value = user.Gender;
            paramGender.DbType = DbType.Int32;

            DbParameter paramBirthdate = cmdInsert.CreateParameter();
            paramBirthdate.ParameterName = "birthdate";
            paramBirthdate.Value = user.Birthdate;
            paramBirthdate.DbType = DbType.Date;

            DbParameter paramUsername = cmdInsert.CreateParameter();
            paramUsername.ParameterName = "username";
            paramUsername.Value = user.Username;
            paramUsername.DbType = DbType.String;

            DbParameter paramPassword = cmdInsert.CreateParameter();
            paramPassword.ParameterName = "password";
            paramPassword.Value = user.Password;
            paramPassword.DbType = DbType.String;

            //Parameter mit dem command verbinden
            cmdInsert.Parameters.Add(paramFN);
            cmdInsert.Parameters.Add(paramLN);
            cmdInsert.Parameters.Add(paramGender);
            cmdInsert.Parameters.Add(paramBirthdate);
            cmdInsert.Parameters.Add(paramUsername);
            cmdInsert.Parameters.Add(paramPassword);

            return cmdInsert.ExecuteNonQuery() == 1;

        }

        public bool UpdateUserData(int id, User newUserData)
        {
            DbCommand cmdUpdate = this._connection.CreateCommand();
            cmdUpdate.CommandText = "UPDATE users SET firstname=@firstname, lastname=@lastname, gender=@gender," +
                "birthdate=@birthdate, username=@username, password=sha2(@password, 512) WHERE id=@uid";

            DbParameter paramFN = cmdUpdate.CreateParameter();
            paramFN.ParameterName = "firstname";
            paramFN.Value = newUserData.Firstname;
            paramFN.DbType = DbType.String;

            DbParameter paramLN = cmdUpdate.CreateParameter();
            paramLN.ParameterName = "lastname";
            paramLN.Value = newUserData.Lastname;
            paramLN.DbType = DbType.String;

            DbParameter paramGender = cmdUpdate.CreateParameter();
            paramGender.ParameterName = "gender";
            paramGender.Value = newUserData.Gender;
            paramGender.DbType = DbType.Int32;

            DbParameter paramBirthdate = cmdUpdate.CreateParameter();
            paramBirthdate.ParameterName = "birthdate";
            paramBirthdate.Value = newUserData.Birthdate;
            paramBirthdate.DbType = DbType.Date;

            DbParameter paramUsername = cmdUpdate.CreateParameter();
            paramUsername.ParameterName = "username";
            paramUsername.Value = newUserData.Username;
            paramUsername.DbType = DbType.String;

            DbParameter paramPassword = cmdUpdate.CreateParameter();
            paramPassword.ParameterName = "password";
            paramPassword.Value = newUserData.Password;
            paramPassword.DbType = DbType.String;

            cmdUpdate.Parameters.Add(paramFN);
            cmdUpdate.Parameters.Add(paramLN);
            cmdUpdate.Parameters.Add(paramGender);
            cmdUpdate.Parameters.Add(paramBirthdate);
            cmdUpdate.Parameters.Add(paramUsername);
            cmdUpdate.Parameters.Add(paramPassword);

            return cmdUpdate.ExecuteNonQuery() == 1;


        }

        public List<User> GetAllUser()
        {
            List<User> users = new List<User>();

            DbCommand cmdSelect = this._connection.CreateCommand();
            cmdSelect.CommandText = "SELECT * FROM users";

            using (DbDataReader reader = cmdSelect.ExecuteReader())
            {
                while (reader.Read())
                {
                    users.Add(
                        new User
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Firstname = Convert.ToString(reader["firstname"]),
                            Lastname = Convert.ToString(reader["lastname"]),
                            Gender = (Gender)Convert.ToInt32(reader["gender"]),
                            Birthdate = Convert.ToDateTime(reader["birthdate"]),
                            Username = Convert.ToString(reader["username"]),
                            Password = ""
                        });
                }
            }

            return users;

        }

        public User GetUser(int id)
        {
            DbCommand cmdGetUser = this._connection.CreateCommand();
            cmdGetUser.CommandText = "SELECT * FROM users WHERE id=@uid";

            DbParameter paramId = cmdGetUser.CreateParameter();
            paramId.ParameterName = "uid";
            paramId.Value = id;
            paramId.DbType = DbType.Int32;

            cmdGetUser.Parameters.Add(paramId);

            using (DbDataReader reader = cmdGetUser.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();

                return new User
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Firstname = Convert.ToString(reader["firstname"]),
                    Lastname = Convert.ToString(reader["lastname"]),
                    Gender = (Gender)Convert.ToInt32(reader["gender"]),
                    Birthdate = Convert.ToDateTime(reader["birthdate"]),
                    Username = Convert.ToString(reader["username"]),
                    Password = ""
                };
            }


        }

        public User Login(UserLogin user)
        {
            DbCommand cmdLogin = this._connection.CreateCommand();
            cmdLogin.CommandText = "SELECT * FROM users WHERE username=@username AND password=sha2(@password, 512)";

            DbParameter paramUsername = cmdLogin.CreateParameter();
            paramUsername.ParameterName = "username";
            paramUsername.Value = user.Username;
            paramUsername.DbType = DbType.String;

            DbParameter paramPWD = cmdLogin.CreateParameter();
            paramPWD.ParameterName = "password";
            paramPWD.Value = user.Password;
            paramPWD.DbType = DbType.String;

            cmdLogin.Parameters.Add(paramUsername);
            cmdLogin.Parameters.Add(paramPWD);

            using (DbDataReader reader = cmdLogin.ExecuteReader())
            {
                if (!reader.HasRows)
                {
                    return null;
                }
                //dieser Aufruf ist notwendig, damit der erste Datensatz gelesen wird
                reader.Read();

                return new User
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Firstname = Convert.ToString(reader["firstname"]),
                    Lastname = Convert.ToString(reader["lastname"]),
                    Gender = (Gender)Convert.ToInt32(reader["gender"]),
                    Birthdate = Convert.ToDateTime(reader["birthdate"]),
                    Username = Convert.ToString(reader["username"]),
                    Password = ""
                };
            }
        }


    }
}