using BataviaReseveringsSysteem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BataviaReseveringsSysteem.Controllers
{
    public class NewsMessageController
    {
        private string notification;

        public string Notification()
        {
            return notification;
        }

        public Boolean WhiteCheck(string title, string message)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
            {
                notification = "U heeft niet alle velden ingevuld";

                return false;

            }
            else
            {
                notification = "";
                return true;
            }
        }

        public void Add_NewsMessage(string title, string message)
        {
            using (DataBase context = new DataBase())
            {
                var NewsMessage = new Models.News_Message
                {
                    Title = title,
                    Message = message,
                    CreatedAt = DateTime.Now,
                };
                context.News_Messages.Add(NewsMessage);
                context.SaveChanges();
            }
        }

        public void Update_NewsMessage(int newsMessageID, string title, string message)
        {
            using (DataBase context = new DataBase())
            {
                Models.News_Message newsMessage = context.News_Messages.Where(d => d.NewsMessageID == newsMessageID).First();
                if (newsMessage != null)
                {
                    newsMessage.Title = title;
                    newsMessage.Message = message;
                    newsMessage.UpdatedAt = DateTime.Now;
                    newsMessage.DeletedAt = null;
                    context.SaveChanges();
                }
            }
        }

        public void Delete_NewsMessage(int newsMessageID)
        {
            using (DataBase context = new DataBase())
            {
                Models.News_Message newsMessage = context.News_Messages.Where(d => d.NewsMessageID == newsMessageID).First();
                if (newsMessage != null)
                {
                    newsMessage.DeletedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }
    }
}
