using Restaurant.Core;
using System;
using System.Text;

namespace Restaurant.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Waiter _waiter;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void _waiter_OrderReady(object sender, Order order)
        {
            StringBuilder sb = new StringBuilder(TextBlockLog.Text);
            sb.Append(FastClock.Instance.Time.ToShortTimeString() + "\t");

            if(order.OrderType == OrderType.Order)
            {
                sb.Append($"{order.Article.Name} für {order.Name} ist bestellt \n");
            }
            else if(order.OrderType == OrderType.Ready)
            {
                sb.Append($"{order.Article.Name} für {order.Name} wird serviert \n");
            }
            else if (order.OrderType == OrderType.ToPay)
            {
                sb.Append($"{order.Name} bezahlt {_waiter.GuestBills[order.Name]:f2}€\n");
            }

            TextBlockLog.Text = sb.ToString();
        }

        private void MetroWindow_Initialized(object sender, EventArgs e)
        {
            Title = "Restaurantsimulator, Uhrzeit: " + FastClock.Instance.Time.ToShortTimeString();
            FastClock.Instance.IsRunning = true;
            FastClock.Instance.OneMinuteIsOver += Instance_OneMinuteIsOver;
            _waiter = new Waiter();
            _waiter.OrderReady += _waiter_OrderReady;
        }

        private void Instance_OneMinuteIsOver(object sender, DateTime e)
        {
            Title = "Restaurantsimulator, Uhrzeit: " + FastClock.Instance.Time.ToShortTimeString();
        }
    }
}
