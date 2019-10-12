using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Global variables
        ConverterRate converterRate;
        Dictionary<string, double> currencyRates;
        DateTime updateStatus;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            updateStatus = new DateTime();
            converterRate = new ConverterRate();
            currencyRates = new Dictionary<string, double>();
            loadCurrencyList();
        }
        #endregion
        #region Properties
        double inputAmount;
        public double InputAmount
        {
            get
            {
                return inputAmount;
            }
            set
            {
                if (inputAmount != value)
                {
                    inputAmount = value;
                    OutputAmount = calculate(inputAmount);
                    OnPropertyChanged(nameof(InputAmount));
                }
            }
        }
        double outputAmount;
        public double OutputAmount
        {
            get
            {
                return outputAmount;
            }
            set
            {
                if (outputAmount != value)
                {
                    outputAmount = value;
                    OnPropertyChanged(nameof(OutputAmount));
                }
            }
        }
        public List<string> CurrencyList
        {
            get;
            set;
        }
        string connectionStatus = string.Empty;
        public string ConnectionStatus
        {
            get
            {
                return connectionStatus;
            }
            set
            {
                if (connectionStatus != value)
                {
                    connectionStatus = value;
                    OnPropertyChanged("ConnectionStatus");
                }
            }
        }
        string localDataStatus = string.Empty;
        public string LocalDataStatus
        {
            get
            {
                return localDataStatus;
            }
            set
            {
                if (localDataStatus != value)
                {
                    localDataStatus = value;
                    OnPropertyChanged("LocalDataStatus");
                }
            }
        }
        
        public DateTime UpdateStatus
        {
            get
            {
                return updateStatus;
            }
            set
            {
                if (updateStatus != value)
                {
                    updateStatus = value;
                    OnPropertyChanged("UpdateStatus");
                }
            }
        }
        string inputCurrencyRate=string.Empty;
        public string InputCurrencyRate
        {
            get
            {
                return inputCurrencyRate;
            }
            set
            {
                if (inputCurrencyRate != value)
                {
                    inputCurrencyRate = value;
                    OnPropertyChanged(nameof(InputCurrencyRate));
                }
                
            }
        }
        string outputCurrencyRate=string.Empty;
        public string OutputCurrencyRate
        {
            get
            {
                return outputCurrencyRate;
            }
            set
            {
                if (outputCurrencyRate != value)
                {
                    outputCurrencyRate = value;
                    OnPropertyChanged(nameof(OutputCurrencyRate));
                }
            }

        }
        string url = string.Empty;
        public string Url
        {
            get
            {
                return url;
            }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }
        #endregion

        #region Methods
        private void loadCurrencyList()
        {
            var currencyRatedata = converterRate.GetCurrencyRates();
            if (currencyRatedata != null)
            {              
                ConnectionStatus = "Online";
            }
            else
            {
                ConnectionStatus = "Offline";
                currencyRatedata = converterRate.readSavedCurrencyRates();
                if (currencyRatedata == null)
                {
                    LocalDataStatus = "No Local Data/error";
                    ConverterGrid.IsEnabled = false;
                    return;
                }
            }
            ConverterGrid.IsEnabled = true;
            LocalDataStatus = "Local Data Present";
            currencyRates = currencyRatedata.Rates;
            CurrencyList = currencyRates.Keys.ToList();
            UpdateStatus = currencyRatedata.Date;
            InputCurrencyRate = CurrencyList[0];
            OutputCurrencyRate = CurrencyList[1];
            InputAmount = 1; 
        }
        private double calculate(double amount)
        {
            var source = currencyRates[InputCurrencyRate];
            var target = currencyRates[OutputCurrencyRate];
            
            return (target/source) * Convert.ToDouble(amount);
        }
        #endregion

        #region Events
        private void TargetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OutputAmount = calculate(inputAmount);
        }
        private void SourceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OutputAmount = calculate(inputAmount);
        }
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            loadCurrencyList();
        }
        private void ConfigureURL_Click(object sender, RoutedEventArgs e)
        {
            if(configURLGrid.Visibility == Visibility.Collapsed)
            {
                configURLGrid.Visibility = Visibility.Visible;
            }
            else
            {
                configURLGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void SetURLButton_Click(object sender, RoutedEventArgs e)
        {
            converterRate.updateURL(url);
            configURLGrid.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region INotifyChanedEvent
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        
    }
}
