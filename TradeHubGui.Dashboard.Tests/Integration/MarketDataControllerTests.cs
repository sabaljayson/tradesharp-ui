﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Spring.Context.Support;
using TradeHub.Common.Core.Constants;
using TradeHub.Common.Core.DomainModels;
using TradeHub.StrategyEngine.MarketData;
using TradeHubGui.Common;
using TradeHubGui.Common.Constants;
using TradeHubGui.Common.Models;
using TradeHubGui.Common.ValueObjects;
using TradeHubGui.Dashboard.Services;
using MarketDataProvider = TradeHubGui.Common.Models.MarketDataProvider;
using TradeHubConstants = TradeHub.Common.Core.Constants;

namespace TradeHubGui.Dashboard.Tests.Integration
{
    [TestFixture]
    public class MarketDataControllerTests
    {
        private MarketDataController _marketDataController;

        [SetUp]
        public void StartUp()
        {
             //var service = ContextRegistry.GetContext()["MarketDataService"] as MarketDataService;
            _marketDataController = ContextRegistry.GetContext()["MarketDataController"] as MarketDataController;

            //_marketDataController = new MarketDataController(service);
        }

        [TearDown]
        public void TearDown()
        {
            _marketDataController.Stop();
        }

        [Test]
        [Category("Integration")]
        public void RequestNewConnection_SendRequestToServer_ReceiveLogon()
        {
            Thread.Sleep(5000);
            MarketDataProvider provider = new MarketDataProvider();
            provider.ProviderType= ProviderType.MarketData;
            provider.ConnectionStatus= ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<MarketDataProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));
        }

        [Test]
        [Category("Console")]
        public void RequestMarketData_SendRequestToServer_ReceiveMarketData()
        {
            Thread.Sleep(5000);
            MarketDataProvider provider = new MarketDataProvider();
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<MarketDataProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            // Create Security to use the Symbol information
            Security security = new Security(){Symbol = "ERX"};
            
            // Create a new subscription request for requesting market data
            var subscriptionRequest = new SubscriptionRequest(security, provider, MarketDataType.Tick,
                SubscriptionType.Subscribe);
            
            // Create Tick details to hold market data information
            MarketDataDetail tickDetails = new MarketDataDetail(security);

            // Add TickDetails object to 
            provider.AddMarketDetail(tickDetails);
            
            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);

            Thread.Sleep(5000);

            Assert.IsTrue(tickDetails.AskPrice.Equals(1.23M));
        }

        [Test]
        [Category("Console")]
        public void RequestMarketData_SendRequestToServer_ReceiveMultipleMarketDataMessages()
        {
            Thread.Sleep(5000);
            MarketDataProvider provider = new MarketDataProvider();
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<MarketDataProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            // Create Security to use the Symbol information
            Security security = new Security() { Symbol = "ERX" };

            // Create a new subscription request for requesting market data
            var subscriptionRequest = new SubscriptionRequest(security, provider, MarketDataType.Tick, SubscriptionType.Subscribe);

            // Create Tick details to hold market data information
            MarketDataDetail tickDetails = new MarketDataDetail(security);

            // Add TickDetails object to 
            provider.AddMarketDetail(tickDetails);

            EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);

            Thread.Sleep(60000);

            Assert.IsTrue(tickDetails.AskPrice.Equals(1.23M), "Best Ask Price");
            Assert.IsTrue(tickDetails.BidRecordsCollection.Count.Equals(6), "LOB Collection count");
        }

        [Test]
        [Category("Integration")]
        public void UnsubscribeMarketData_SendRequestToServer()
        {
            Thread.Sleep(5000);
            MarketDataProvider provider = new MarketDataProvider();
            provider.ConnectionStatus = ConnectionStatus.Disconnected;
            provider.ProviderName = TradeHubConstants.MarketDataProvider.Simulated;

            // Rasie event to request connection
            EventSystem.Publish<MarketDataProvider>(provider);

            Thread.Sleep(5000);

            Assert.IsTrue(provider.ConnectionStatus.Equals(ConnectionStatus.Connected));

            // Create Security to use the Symbol information
            Security security = new Security() { Symbol = "AAPL" };

            // Create Tick details to hold market data information
            MarketDataDetail tickDetails = new MarketDataDetail(security);

            // Add TickDetails object to 
            provider.AddMarketDetail(tickDetails);

            {
                // Create a new subscription request for requesting market data
                var subscriptionRequest = new SubscriptionRequest(security, provider, MarketDataType.Tick, SubscriptionType.Subscribe);

                EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);
            }

            Thread.Sleep(2000);

            {
                // Create a new subscription request for requesting market data
                var subscriptionRequest = new SubscriptionRequest(security, provider, MarketDataType.Tick, SubscriptionType.Unsubscribe);

                EventSystem.Publish<SubscriptionRequest>(subscriptionRequest);
            }
        }
    }
}