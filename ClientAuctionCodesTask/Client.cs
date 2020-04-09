using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAuctionCodesTask
{
    class Client
    {
        public string CardNumber { get; set; }
        public string AuctionCode { get; set; }
        public List<string> ProdcutCodes { get; set; }
    }
}
