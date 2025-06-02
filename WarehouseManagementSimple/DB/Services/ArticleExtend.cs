using Core;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB.Services
{
    public class ArticleExtend
    {
        private readonly int articleId;
        private readonly SqliteConnection connection;

        public ArticleExtend(int articleId, SqliteConnection connection)
        {
            this.articleId = articleId;
            this.connection = connection;
        }

        /// <summary>
        /// Check if an article exists.
        /// </summary>
        /// <returns>True when the article exists. False otherwise.</returns>
        public bool IsExisting()
        {
            Article article = new Article();
            article.ArticleID = this.articleId;
            DBArticle dBArticle = new DBArticle(this.connection);
            return dBArticle.GetByArticleID(ref article);
        }
    }
}
