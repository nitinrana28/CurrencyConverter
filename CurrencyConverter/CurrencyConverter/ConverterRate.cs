using System;
using System.IO;
using System.Net.Http;
namespace CurrencyConverter
{
    class ConverterRate
    {
        #region Global variables
        private Currency allCurrencies;
        string filePath = Environment.CurrentDirectory.ToString() + "\\CurrencyRate.json";
        string urlfile = Environment.CurrentDirectory.ToString() + "\\URL.txt";
        #endregion

        #region Constructor
        public ConverterRate()
        {
            allCurrencies = new Currency();
        }
        #endregion

        #region Methods
        public void saveCurrencyRates(string data)
        {            
            File.WriteAllText(filePath, data);
        }
        public Currency readSavedCurrencyRates()
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            string data=File.ReadAllText(filePath);
            allCurrencies = Newtonsoft.Json.JsonConvert.DeserializeObject<Currency>(data);
            return allCurrencies;
        }
        private string readURL()
        {
            return File.ReadAllText(urlfile);
        }
        public void updateURL(string url)
        {
            File.WriteAllText(urlfile,url);
        }
        public Currency GetCurrencyRates()
        {
            try
            {
                string url = readURL();
                HttpResponseMessage httpresponse = new HttpClient().GetAsync(url).Result;
                string content = httpresponse.Content.ReadAsStringAsync().Result;
                allCurrencies = Newtonsoft.Json.JsonConvert.DeserializeObject<Currency>(content);
                saveCurrencyRates(content);
                return allCurrencies;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
