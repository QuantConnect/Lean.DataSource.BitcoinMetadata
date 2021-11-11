/*
 * QUANTCONNECT.COM - Democratizing Finance, Empowering Individuals.
 * Lean Algorithmic Trading Engine v2.0. Copyright 2014 QuantConnect Corporation.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/

using QuantConnect.DataSource;

namespace QuantConnect.DataLibrary.Tests
{
    /// <summary>
    /// Example algorithm using Blockchain Bitcoin data as a source of alpha
    /// In this algorithm, we're trading the supply-demand of the Bitcoin blockchain services will affect its price
    /// </summary>
    public class BlockchainBitcoinDataAlgorithm : QCAlgorithm
    {
        private Symbol _bitcoinDataSymbol;
        private Symbol _btcSymbol;
        private decimal? _lastDemandSupply = null;

        /// <summary>
        /// Initialise the data and resolution required, as well as the cash and start-end dates for your algorithm. All algorithms must initialized.
        /// </summary>
        public override void Initialize()
        {
            SetStartDate(2020, 10, 07);  //Set Start Date
            SetEndDate(2020, 10, 11);    //Set End Date
            SetCash(100000);

            _btcSymbol = AddCrypto("BTCUSD", Resolution.Minute).Symbol; 
            // Requesting data
            _bitcoinDataSymbol = AddData<BlockchainBitcoinData>(_btcSymbol).Symbol;

            // Historical data
            var history = History<BlockchainBitcoinData>(_bitcoinDataSymbol, 60, Resolution.Daily);
            Debug($"We got {history.Count()} items from our history request for {_btcSymbol} Blockchain Bitcoin data");
        }

        /// <summary>
        /// OnData event is the primary entry point for your algorithm. Each new data point will be pumped in here.
        /// </summary>
        /// <param name="slice">Slice object keyed by symbol containing the stock data</param>
        public override void OnData(Slice slice)
        {
            // Get data
            var data = slice.Get<BlockchainBitcoinData>();
            if (!data.IsNullOrEmpty())
            {
                var currentDemandSupply = data[_bitcoinDataSymbol].NumberofTransactions / data[_bitcoinDataSymbol].HashRate;

                // comparing the average transaction-to-hash-rate ratio changes, we will buy bitcoin or hold cash
                if (_lastDemandSupply != null && currentDemandSupply > _lastDemandSupply)
                {
                    SetHoldings(_btcSymbol, 1);
                }
                else
                {
                    SetHoldings(_btcSymbol, 0);
                }

                _lastDemandSupply = currentDemandSupply;
            }
        }
    }
}
