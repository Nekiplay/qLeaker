using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudLibrary
{
    public class Store
    {
        public string name;
        public string url;

        public Store(string name, string url)
        {
            this.name = name;
            this.url = url;
        }
    }
}
