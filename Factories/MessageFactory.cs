using System.Data;
using MySql.Data.MySqlClient;
using message.Models;
using Microsoft.Extensions.Options;
using dashboard;
using System.Collections.Generic;
using Dapper;

namespace message.Factory
{
    public class MessageFactory : IFactory<Messages>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;

        public MessageFactory(IOptions<MySqlOptions> conf)
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
        public void Add(Messages item)
        {
            using (IDbConnection dbConnection = Connection)
            {
            dbConnection.Open();
            string query = "INSERT INTO messages (message, created_at, updated_at, user_id, author_id) VALUES (@message, NOW(), NOW(), @user_id, @author_id);";  
            dbConnection.Execute(@query, item);
            }
        }
        public IEnumerable<Messages> Messages(int id){
            using (IDbConnection dbConnection = Connection) {
                dbConnection.Open();
                string query = "SELECT * FROM messages WHERE user_id = @id ORDER BY id DESC";
                return dbConnection.Query<Messages>(query, new {id = id});
            }
        }
    }
}
