using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace FixALeak.TestApi
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new Client();
            c.Authorize("Andrzej", "dupaZbita!");
            c.GetCategories();
            //c.Register("Andrzej", "dupaZbita!");
            //c.Register("Stefan", "eloelo");
            
           
        }
    }
}
