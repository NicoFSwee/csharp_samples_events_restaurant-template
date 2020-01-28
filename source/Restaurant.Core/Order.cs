using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Restaurant.Core
{
    public class Order
    {
        private OrderType _type;
        public OrderType OrderType 
        { 
            get
            {
                if(FastClock.Instance.Time >= TimeReady)
                {
                    return OrderType.Ready;
                }
                else
                {
                    return _type;
                }
            }
            set
            {
                _type = value;
            }
        }

        public string Name { get; set; }

        private int _delay;
        private DateTime _timeOfCreation;
        public DateTime TimeReady 
        { 
            get
            {
                if(Article != null)
                {
                    return TimeForAction.AddMinutes(Article.TimeToBuild);
                }
                else
                {
                    return DateTime.MaxValue;
                }   
            }
        }
        public Article Article { get; set; }

        public Order(int delay, string name, OrderType type, Article article)
        {
            _delay = delay;
            Name = name;
            OrderType = type;
            Article = article;
            _timeOfCreation = FastClock.Instance.Time;
        }

        public DateTime TimeForAction 
        { 
            get
            {
                return _timeOfCreation.AddMinutes(_delay);
            }
        }
    }
}
