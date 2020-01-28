using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Restaurant.Core
{
    public class Waiter
    {
        private List<Order> _orders;
        private List<Order> _beingPrepared;
        private List<Article> _articles;
        private FastClock _clock;

        public Waiter()
        {
            _articles = GetArticles();
            _orders = FillOrders();
            GuestBills = new Dictionary<string, double>();
            _beingPrepared = new List<Order>();
            _clock = FastClock.Instance;
            _clock.OneMinuteIsOver += Instance_OneMinuteIsOver;
        }

        public Dictionary<string, double> GuestBills;
        public event EventHandler<Order> OrderReady;

        private List<Order> FillOrders()
        {
            string path = MyFile.GetFullNameInApplicationTree("Tasks.csv");
            string[] lines = File.ReadAllLines(path, Encoding.Default);
            List<Order> result = new List<Order>();

            for(int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(';');
                int delay = Convert.ToInt32(fields[0]);
                string name = fields[1];
                OrderType type = new OrderType();
                type = (OrderType)Enum.Parse(type.GetType(), fields[2], true);
                Article article = null;

                for (int j = 0; j < _articles.Count; j++)
                {
                    if(_articles[j].Name == fields[3])
                    {
                        article = _articles[j];
                    }
                }

                result.Add(new Order(delay, name, type, article));
            }

            return result;
        }

        private List<Article> GetArticles()
        {
            string path = MyFile.GetFullNameInApplicationTree("Articles.csv");
            string[] lines = File.ReadAllLines(path, Encoding.Default);
            List<Article> result = new List<Article>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(';');
                string name = fields[0];
                double price = Convert.ToDouble(fields[1]);
                int timeToBuild = Convert.ToInt32(fields[2]);
                result.Add(new Article(name, price, timeToBuild));
            }

            return result;
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime clockTime)
        {
            CheckIfSomethingToDo(clockTime);
        }

        private void CheckIfSomethingToDo(DateTime clockTime)
        {
            for (int j = 0; j < _beingPrepared.Count; j++)
            {
                if(clockTime >= _beingPrepared[j].TimeReady)
                {
                    _beingPrepared[j].OrderType = OrderType.Ready;
                    OrderReady?.Invoke(this, _beingPrepared[j]);
                    _beingPrepared.RemoveAt(j);
                    j -= 1;
                }
            }
            for (int i = 0; i < _orders.Count; i++)
            {
                if (clockTime >= _orders[i].TimeForAction && _orders[i].OrderType == OrderType.Order)
                {
                    OrderReady?.Invoke(this, _orders[i]);

                    if(!GuestBills.ContainsKey(_orders[i].Name))
                    {
                        GuestBills.Add(_orders[i].Name, _orders[i].Article.Price);
                    }
                    else
                    {
                        GuestBills[_orders[i].Name] += _orders[i].Article.Price;
                    }

                    _beingPrepared.Add(_orders[i]);
                    _orders.RemoveAt(i);
                    i -= 1;
                }
                else if(clockTime >= _orders[i].TimeForAction && _orders[i].OrderType == OrderType.ToPay)
                {
                    OrderReady?.Invoke(this, _orders[i]);
                    _orders.RemoveAt(i);
                }
            }

        }
    }

}
