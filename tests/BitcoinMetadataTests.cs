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
using ProtoBuf;
using System.IO;
using System.Linq;
using ProtoBuf.Meta;
using Newtonsoft.Json;
using NUnit.Framework;
using QuantConnect.Data;
using QuantConnect.DataSource;

namespace QuantConnect.DataLibrary.Tests
{
    [TestFixture]
    public class BitcoinMetadataTests
    {
        [Test]
        public void JsonRoundTrip()
        {
            var expected = CreateNewInstance();
            var type = expected.GetType();
            var serialized = JsonConvert.SerializeObject(expected);
            var result = JsonConvert.DeserializeObject(serialized, type);

            AssertAreEqual(expected, result);
        }

        [Test]
        public void ProtobufRoundTrip()
        {
            var expected = CreateNewInstance();
            var type = expected.GetType();

            RuntimeTypeModel.Default[typeof(BaseData)].AddSubType(2000, type);

            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, expected);

                stream.Position = 0;

                var result = Serializer.Deserialize(type, stream);

                AssertAreEqual(expected, result, filterByCustomAttributes: true);
            }
        }

        [Test]
        public void Clone()
        {
            var expected = CreateNewInstance();
            var result = expected.Clone();

            AssertAreEqual(expected, result);
        }

        [Test]
        public void ToStringTest()
        {
            var expected = CreateNewInstance();
            var text = $@"{expected.Symbol} - Difficulty 100,
                My Wallet Number of Users 10000,
                Average Block Size 100,
                Blockchain Size 300000,
                Median Transaction Confirmation Time 0.01,
                Miners Revenue 10,
                Hash Rate 20,
                Cost Per Transaction 0.01,
                Cost Percent of Transaction Volume 0.005,
                Estimated Transaction Volume USD 100000000,
                Estimated Transaction Volume 20000,
                Total Output Volume 2000,
                Number of Transactionper Block 100,
                Number of UniqueBitcoin Addresses Used 50000,
                Number of Transactions Excluding Popular Addresses 10000,
                Total Number of Transactions 10000000,
                Number of Transactions 10000,
                Total Transaction Fees USD 10000,
                Total Transaction Fees 100,
                Market Capitalization 3000000000,
                Total Bitcoins 21000000,
                MyWalletNumberofTransactionPerDay 1000,
                MyWalletTransactionVolume 1000";
            var result = expected.ToString();

            AssertAreEqual(text, result);
        }

        private void AssertAreEqual(object expected, object result, bool filterByCustomAttributes = false)
        {
            foreach (var propertyInfo in expected.GetType().GetProperties())
            {
                // we skip Symbol which isn't protobuffed
                if (filterByCustomAttributes && propertyInfo.CustomAttributes.Count() != 0)
                {
                    Assert.AreEqual(propertyInfo.GetValue(expected), propertyInfo.GetValue(result));
                }
            }
            foreach (var fieldInfo in expected.GetType().GetFields())
            {
                Assert.AreEqual(fieldInfo.GetValue(expected), fieldInfo.GetValue(result));
            }
        }

        private BaseData CreateNewInstance()
        {
            return new BitcoinMetadata
            {
                Symbol = Symbol.Empty,
                Time = DateTime.Today,
                DataType = MarketDataType.Base,
                Difficulty = 100m,
                MyWalletNumberofUsers = 10000m,
                AverageBlockSize = 100m,
                BlockchainSize = 300000m,
                MedianTransactionConfirmationTime = 0.01m,
                MinersRevenue = 10m,
                HashRate = 20m,
                CostPerTransaction = 0.01m,
                CostPercentofTransactionVolume = 0.005m,
                EstimatedTransactionVolumeUSD = 100000000m,
                EstimatedTransactionVolume = 20000m,
                TotalOutputVolume = 2000m,
                NumberofTransactionperBlock = 100m,
                NumberofUniqueBitcoinAddressesUsed = 50000m,
                NumberofTransactionsExcludingPopularAddresses = 10000m,
                TotalNumberofTransactions = 10000000m,
                NumberofTransactions = 10000m,
                TotalTransactionFeesUSD = 10000m,
                TotalTransactionFees = 100m,
                MarketCapitalization = 3000000000m,
                TotalBitcoins = 21000000m,
                MyWalletNumberofTransactionPerDay = 1000m,
                MyWalletTransactionVolume = 1000m
            };
        }
    }
}
