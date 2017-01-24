using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using MySql.Data.MySqlClient;
using user.Models;
using Microsoft.Extensions.Options;
using dashboard;
using Microsoft.AspNetCore.Identity;

namespace user.Factory
{
    public class UserFactory : IFactory<User>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;

        public UserFactory(IOptions<MySqlOptions> conf)
        {
            mysqlConfig = conf;
        }

        internal IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(mysqlConfig.Value.ConnectionString);
            }
        }
        public void Add(User item)
        {
            using (IDbConnection dbConnection = Connection)
            {
                // dbConnection.Open();
                bool makeadmin = FirstUser();
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                item.password = Hasher.HashPassword(item, item.password);
                if (makeadmin == false)
                {
                    string query = "INSERT INTO user (first_name, last_name, email, password, admin, created_at, updated_at) VALUES (@first_name, @last_name, @email, @password, 1, NOW(), NOW())";
                    dbConnection.Execute(query, item);
                }
                else
                {
                    string query = "INSERT INTO user (first_name, last_name, email, password, created_at, updated_at) VALUES (@first_name, @last_name, @email, @password, NOW(), NOW())";
                    dbConnection.Execute(query, item);
                }
            }
        }

        public bool FirstUser()
        {
            using (IDbConnection dbConnection = Connection)
            {
                // dbConnection.Open(); //do i need to open this connection again if i only use it inside of another connection??
                object result = dbConnection.Query<User>("SELECT * FROM user", new { admin = 1 }).FirstOrDefault(); ;
                if (result == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

        }
        public IEnumerable<User> AllUsers()
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM user");
            }
        }
        public User Login(string email)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                return dbConnection.Query<User>("SELECT * FROM user WHERE email = @email", new { email = email }).FirstOrDefault();
            }
        }
        public User FindUser(int id)
        {
            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
            return dbConnection.Query<User>("SELECT * FROM user WHERE id = @id", new { id = id }).FirstOrDefault();
            }
        }
        public void RemoveUser(int id) {
            using (IDbConnection dbConnection = Connection) {
                dbConnection.Open();
                string query = $"DELETE FROM user WHERE id = {id}";
                dbConnection.Execute(query);
            }
        }
        public void UpdateUser(User item, int myid) {
            using (IDbConnection dbconnection = Connection) {
                dbconnection.Open();
                string query = $"UPDATE user SET first_name=@first_name, last_name=@last_name, email=@email, description=@description WHERE id={myid};";
                dbconnection.Execute(query, item);
            }
        }
        public void EditUser(User item, int myid) {
            using (IDbConnection dbconnection = Connection) {
                dbconnection.Open();
                string query = $"UPDATE user SET first_name=@first_name, last_name=@last_name, email=@email, admin=@admin WHERE id={myid};";
                dbconnection.Execute(query, item);
            }
        }
    }

}