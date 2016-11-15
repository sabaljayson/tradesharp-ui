﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeHub.Common.Core.Constants;
using TradeHubGui.Common.Constants;

namespace TradeHubGui.Common.Models
{
    /// <summary>
    /// Generic Provider class used for Market Data Provider or Order Execution Provider
    /// </summary>
    public class Provider : INotifyPropertyChanged
    {
        #region Fields

        private ProviderType _providerType;
        private string _providerName;
        private ConnectionStatus _connectionStatus;
        private List<ProviderCredential> _providerCredentials;

        #endregion

        #region Constructors

        public Provider()
        {
            // Initialize Maps
            _providerCredentials = new List<ProviderCredential>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Provider name
        /// </summary>
        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                if (_providerName != value)
                {
                    _providerName = value;
                    OnPropertyChanged("ProviderName");
                }
            }
        }

        /// <summary>
        /// Provider connection status
        /// </summary>
        public ConnectionStatus ConnectionStatus
        {
            get { return _connectionStatus; }
            set
            {
                if (_connectionStatus != value)
                {
                    _connectionStatus = value;
                    OnPropertyChanged("ConnectionStatus");
                }
            }
        }

        /// <summary>
        /// List of credentials for provider (i.e. Username, Password, IpAddress etc.)
        /// </summary>
        public List<ProviderCredential> ProviderCredentials
        {
            get { return _providerCredentials; }
            set
            {
                if (_providerCredentials != value)
                {
                    _providerCredentials = value;
                    OnPropertyChanged("Credentials");
                }
            }
        }

        /// <summary>
        /// Type of Provider e.g Market Data, Order Execution, etc.
        /// </summary>
        public ProviderType ProviderType
        {
            get { return _providerType; }
            set { _providerType = value; }
        }

        #endregion

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}