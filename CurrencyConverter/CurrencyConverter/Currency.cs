using System;
using System.Collections.Generic;

namespace CurrencyConverter
{
    class Currency
    {
        public string CurrencyName { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
}
