using Core;
using DB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLI.Menu.SetMenus
{
    internal class SetArticleMenu : DatabaseMenuComponent
    {
        protected DBArticle articleTable;
        private IList<Article> articles;

        public SetArticleMenu(string menuName, SqliteConnection connection, params MenuComponent[] menuItems) : base(menuName, connection, menuItems)
        {
            articleTable = new DBArticle(_connection);
        }

        public override void ShowOptions()
        {
            base.ShowOptions();

            articles = articleTable.GetAll();

            if (articles.Count <= 0)
            {
                Console.WriteLine("No options available");
            }

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < articles.Count; i++)
            {
                sb.AppendLine($"{i} - {articles[i].ToString()}");
            }

            Console.WriteLine(sb.ToString());
        }

        protected override MenuComponent UseInput(string input)
        {
            try
            {
                if (int.TryParse(input, out int id))
                {
                    IRequireArticle setArticleParent = Parent as IRequireArticle;
                    setArticleParent.SetArticle(articles[id].ArticleID);
                    return Parent;
                }
                else
                {
                    throw new ArgumentException("Input invalid");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return this;
            }

            return this;
        }
    }
}
