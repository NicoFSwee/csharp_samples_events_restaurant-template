using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Core
{
    public class Article
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int TimeToBuild { get; set; }

        public Article(string name, double price, int time)
        {
            Name = name;
            Price = price;
            TimeToBuild = time;
        }
    }
}
