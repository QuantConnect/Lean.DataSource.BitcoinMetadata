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

using System;
using NodaTime;
using ProtoBuf;
using System.IO;
using QuantConnect.Data;
using System.Collections.Generic;

namespace QuantConnect.DataSource
{
    /// <summary>
    /// Blockchain Bitcoin Metadata dataset
    /// </summary>
    [ProtoContract(SkipConstructor = true)]
    public class BitcoinMetadata : BaseData
    {
        /// <summary>
        /// A relative measure of how difficult it is to find a new block. The difficulty is adjusted periodically as a function of how much hashing power has been deployed by the network of miners.
        /// </summary>
        [ProtoMember(10)]
        public decimal Difficulty { get; set; }

        /// <summary>
        /// Number of wallets hosts using our My Wallet Service.
        /// </summary>
        [ProtoMember(11)]
        public decimal MyWalletNumberofUsers  { get; set; }

        /// <summary>
        /// The average block size in MB.
        /// </summary>
        [ProtoMember(12)]
        public decimal AverageBlockSize { get; set; }

        /// <summary>
        /// The total size of all block headers and transactions. Not including database indexes.
        /// </summary>
        [ProtoMember(13)]
        public decimal BlockchainSize { get; set; }

        /// <summary>
        /// The median time for a transaction to be accepted into a mined block and added to the public ledger (note: only includes transactions with miner fees).
        /// </summary>
        [ProtoMember(14)]
        public decimal MedianTransactionConfirmationTime { get; set; }

        /// <summary>
        /// Total value of coinbase block rewards and transaction fees paid to miners.
        /// </summary>
        [ProtoMember(15)]
        public decimal MinersRevenue { get; set; }

        /// <summary>
        /// The estimated number of tera hashes per second (trillions of hashes per second) the Bitcoin network is performing
        /// </summary>
        [ProtoMember(16)]
        public decimal HashRate { get; set; }

        /// <summary>
        /// The miners revenue divided by the number of transactions.
        /// </summary>
        [ProtoMember(17)]
        public decimal CostPerTransaction { get; set; }

        /// <summary>
        /// The miners revenue as percentage of the transaction volume.
        /// </summary>
        [ProtoMember(18)]
        public decimal CostPercentofTransactionVolume { get; set; }

        /// <summary>
        /// The Estimated Transaction Value in USD value.
        /// </summary>
        [ProtoMember(19)]
        public decimal EstimatedTransactionVolumeUSD  { get; set; }

        /// <summary>
        /// The total estimated value of transactions on the Bitcoin blockchain (does not include coins returned to sender as change).
        /// </summary>
        [ProtoMember(20)]
        public decimal EstimatedTransactionVolume  { get; set; }

        /// <summary>
        /// The total value of all transaction outputs per day (includes coins returned to the sender as change).
        /// </summary>
        [ProtoMember(21)]
        public decimal TotalOutputVolume  { get; set; }

        /// <summary>
        /// The average number of transactions per block.
        /// </summary>
        [ProtoMember(22)]
        public decimal NumberofTransactionperBlock  { get; set; }

        /// <summary>
        /// The total number of unique addresses used on the Bitcoin blockchain.
        /// </summary>
        [ProtoMember(23)]
        public decimal NumberofUniqueBitcoinAddressesUsed  { get; set; }

        /// <summary>
        /// The total number of Bitcoin transactions, excluding those involving any of the network's 100 most popular addresses.
        /// </summary>
        [ProtoMember(24)]
        public decimal NumberofTransactionsExcludingPopularAddresses  { get; set; }

        /// <summary>
        /// The Total Number of transactions.
        /// </summary>
        [ProtoMember(25)]
        public decimal TotalNumberofTransactions  { get; set; }

        /// <summary>
        /// The number of daily confirmed Bitcoin transactions.
        /// </summary>
        [ProtoMember(26)]
        public decimal NumberofTransactions  { get; set; }

        /// <summary>
        /// The total value of all transaction fees in USD paid to miners (not including the coinbase value of block rewards).
        /// </summary>
        [ProtoMember(27)]
        public decimal TotalTransactionFeesUSD  { get; set; }

        /// <summary>
        /// The total value of all transaction fees in Bitcoin paid to miners (not including the coinbase value of block rewards).
        /// </summary>
        [ProtoMember(28)]
        public decimal TotalTransactionFees  { get; set; }

        /// <summary>
        /// The total USD value of bitcoin supply in circulation, as calculated by the daily average market price across major exchanges.
        /// </summary>
        [ProtoMember(29)]
        public decimal MarketCapitalization  { get; set; }

        /// <summary>
        /// The total number of bitcoins that have already been mined; in other words, the current supply of bitcoins on the network.
        /// </summary>
        [ProtoMember(30)]
        public decimal TotalBitcoins { get; set; }

        /// <summary>
        /// Number of transactions made by My Wallet Users per day.
        /// </summary>
        [ProtoMember(31)]
        public decimal MyWalletNumberofTransactionPerDay { get; set; }

        /// <summary>
        /// 24hr Transaction Volume of our web wallet service.
        /// </summary>
        [ProtoMember(32)]
        public decimal MyWalletTransactionVolume { get; set; }

        /// <summary>
        /// Return the URL string source of the file. This will be converted to a stream
        /// </summary>
        /// <param name="config">Configuration object</param>
        /// <param name="date">Date of this source file</param>
        /// <param name="isLiveMode">true if we're in live mode, false for backtesting mode</param>
        /// <returns>String URL of source file.</returns>
        public override SubscriptionDataSource GetSource(SubscriptionDataConfig config, DateTime date, bool isLiveMode)
        {
            return new SubscriptionDataSource(
                Path.Combine(
                    Globals.DataFolder,
                    "alternative",
                    "blockchain",
                    $"{config.Symbol.Value.ToLowerInvariant()}.csv"
                ),
                SubscriptionTransportMedium.LocalFile
            );
        }

        /// <summary>
        /// Parses the data from the line provided and loads it into LEAN
        /// </summary>
        /// <param name="config">Subscription configuration</param>
        /// <param name="line">Line of data</param>
        /// <param name="date">Date</param>
        /// <param name="isLiveMode">Is live mode</param>
        /// <returns>New instance</returns>
        public override BaseData Reader(SubscriptionDataConfig config, string line, DateTime date, bool isLiveMode)
        {
            var csv = line.Split(',');

            var parsedDate = Parse.DateTimeExact(csv[0], "yyyy-MM-dd");
            return new BitcoinMetadata()
            {
                Symbol = config.Symbol,
                Difficulty = decimal.Parse(csv[1], System.Globalization.NumberStyles.Float),
                MyWalletNumberofUsers = decimal.Parse(csv[2], System.Globalization.NumberStyles.Float),
                AverageBlockSize = decimal.Parse(csv[3], System.Globalization.NumberStyles.Float),
                BlockchainSize = decimal.Parse(csv[4], System.Globalization.NumberStyles.Float),
                MedianTransactionConfirmationTime = decimal.Parse(csv[5], System.Globalization.NumberStyles.Float),
                MinersRevenue = decimal.Parse(csv[6], System.Globalization.NumberStyles.Float),
                HashRate = decimal.Parse(csv[7], System.Globalization.NumberStyles.Float),
                CostPerTransaction = decimal.Parse(csv[8], System.Globalization.NumberStyles.Float),
                CostPercentofTransactionVolume = decimal.Parse(csv[9], System.Globalization.NumberStyles.Float),
                EstimatedTransactionVolumeUSD = decimal.Parse(csv[10], System.Globalization.NumberStyles.Float),
                EstimatedTransactionVolume = decimal.Parse(csv[11], System.Globalization.NumberStyles.Float),
                TotalOutputVolume = decimal.Parse(csv[12], System.Globalization.NumberStyles.Float),
                NumberofTransactionperBlock = decimal.Parse(csv[13], System.Globalization.NumberStyles.Float),
                NumberofUniqueBitcoinAddressesUsed = decimal.Parse(csv[14], System.Globalization.NumberStyles.Float),
                NumberofTransactionsExcludingPopularAddresses = decimal.Parse(csv[15], System.Globalization.NumberStyles.Float),
                TotalNumberofTransactions = decimal.Parse(csv[16], System.Globalization.NumberStyles.Float),
                NumberofTransactions = decimal.Parse(csv[17], System.Globalization.NumberStyles.Float),
                TotalTransactionFeesUSD = decimal.Parse(csv[18], System.Globalization.NumberStyles.Float),
                TotalTransactionFees = decimal.Parse(csv[19], System.Globalization.NumberStyles.Float),
                MarketCapitalization = decimal.Parse(csv[20], System.Globalization.NumberStyles.Float),
                TotalBitcoins = decimal.Parse(csv[21], System.Globalization.NumberStyles.Float),
                MyWalletNumberofTransactionPerDay = decimal.Parse(csv[22], System.Globalization.NumberStyles.Float),
                MyWalletTransactionVolume = decimal.Parse(csv[23], System.Globalization.NumberStyles.Float),
                Time = parsedDate,
                EndTime = parsedDate + TimeSpan.FromDays(1)     // Shift to next day 00:00 for that day's data
            };
        }

        /// <summary>
        /// Clones the data
        /// </summary>
        /// <returns>A clone of the object</returns>
        public override BaseData Clone()
        {
            return new BitcoinMetadata()
            {
                Symbol = Symbol,
                Time = Time,
                EndTime = EndTime,
                Difficulty = Difficulty,
                MyWalletNumberofUsers = MyWalletNumberofUsers,
                AverageBlockSize = AverageBlockSize,
                BlockchainSize = BlockchainSize,
                MedianTransactionConfirmationTime = MedianTransactionConfirmationTime,
                MinersRevenue = MinersRevenue,
                HashRate = HashRate,
                CostPerTransaction = CostPerTransaction,
                CostPercentofTransactionVolume = CostPercentofTransactionVolume,
                EstimatedTransactionVolumeUSD = EstimatedTransactionVolumeUSD,
                EstimatedTransactionVolume = EstimatedTransactionVolume,
                TotalOutputVolume = TotalOutputVolume,
                NumberofTransactionperBlock = NumberofTransactionperBlock,
                NumberofUniqueBitcoinAddressesUsed = NumberofUniqueBitcoinAddressesUsed,
                NumberofTransactionsExcludingPopularAddresses = NumberofTransactionsExcludingPopularAddresses,
                TotalNumberofTransactions = TotalNumberofTransactions,
                NumberofTransactions = NumberofTransactions,
                TotalTransactionFeesUSD = TotalTransactionFeesUSD,
                TotalTransactionFees = TotalTransactionFees,
                MarketCapitalization = MarketCapitalization,
                TotalBitcoins = TotalBitcoins,
                MyWalletNumberofTransactionPerDay = MyWalletNumberofTransactionPerDay,
                MyWalletTransactionVolume = MyWalletTransactionVolume
            };
        }

        /// <summary>
        /// Indicates whether the data source is tied to an underlying symbol and requires that corporate events be applied to it as well, such as renames and delistings
        /// </summary>
        /// <returns>false</returns>
        public override bool RequiresMapping()
        {
            return false;
        }

        /// <summary>
        /// Indicates whether the data is sparse.
        /// If true, we disable logging for missing files
        /// </summary>
        /// <returns>true</returns>
        public override bool IsSparseData()
        {
            return true;
        }

        /// <summary>
        /// Converts the instance to string
        /// </summary>
        public override string ToString()
        {
            return @"{Symbol} - Difficulty {Difficulty},
                My Wallet Number of Users {MyWalletNumberofUsers},
                Average Block Size {AverageBlockSize},
                Blockchain Size {BlockchainSize},
                Median Transaction Confirmation Time {MedianTransactionConfirmationTime},
                Miners Revenue {MinersRevenue},
                Hash Rate {HashRate},
                Cost Per Transaction {CostPerTransaction},
                Cost Percent of Transaction Volume {CostPercentofTransactionVolume},
                Estimated Transaction Volume USD {EstimatedTransactionVolumeUSD},
                Estimated Transaction Volume {EstimatedTransactionVolume},
                Total Output Volume {TotalOutputVolume},
                Number of Transactionper Block {NumberofTransactionperBlock},
                Number of UniqueBitcoin Addresses Used {NumberofUniqueBitcoinAddressesUsed},
                Number of Transactions Excluding Popular Addresses {NumberofTransactionsExcludingPopularAddresses},
                Total Number of Transactions {TotalNumberofTransactions},
                Number of Transactions {NumberofTransactions},
                Total Transaction Fees USD {TotalTransactionFeesUSD},
                Total Transaction Fees {TotalTransactionFees},
                Market Capitalization {MarketCapitalization},
                Total Bitcoins {TotalBitcoins},
                MyWalletNumberofTransactionPerDay {MyWalletNumberofTransactionPerDay},
                MyWalletTransactionVolume {MyWalletTransactionVolume}";
        }

        /// <summary>
        /// Gets the default resolution for this data and security type
        /// </summary>
        public override Resolution DefaultResolution()
        {
            return Resolution.Daily;
        }

        /// <summary>
        /// Gets the supported resolution for this data and security type
        /// </summary>
        public override List<Resolution> SupportedResolutions()
        {
            return DailyResolution;
        }

        /// <summary>
        /// Specifies the data time zone for this data type. This is useful for custom data types
        /// </summary>
        /// <returns>The <see cref="T:NodaTime.DateTimeZone" /> of this data type</returns>
        public override DateTimeZone DataTimeZone()
        {
            return DateTimeZone.Utc;
        }
    }
}
