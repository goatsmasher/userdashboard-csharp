using System.Data;
using MySql.Data.MySqlClient;
using comment.Models;
using Microsoft.Extensions.Options;
using dashboard;
using Dapper;
using System.Collections.Generic;

namespace comment.Factory
{
    public class CommentFactory : IFactory<Comment>
    {
        private readonly IOptions<MySqlOptions> mysqlConfig;

        public CommentFactory(IOptions<MySqlOptions> conf)
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
        public void Add(Comment item)
        {
            using (IDbConnection dbConnection = Connection)
            {
            dbConnection.Open();
            string query = "INSERT INTO comments (comment, created_at, updated_at, author_id, messages_id) VALUES (@comment, NOW(), NOW(), @author_id, @messages_id);";  
            dbConnection.Execute(@query, item);
            }
        }
        public IEnumerable<Comment> AllComments(){
            using (IDbConnection dbConnection = Connection) {
                dbConnection.Open();
                string query = "SELECT * FROM comments;";
                return dbConnection.Query<Comment>(query);
            }
        }
    }
}