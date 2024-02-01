import pathlib
import os
import requests

api = os.environ["QUANDL_API_KEY"]

base_link = "https://data.nasdaq.com/api/v3/datatables/QDL/BCHAIN?"

destination_folder = pathlib.Path('/temp-output-directory/alternative/blockchain/bitcoinmetadata')
# objectives:# download data from API -> temp folder or in memory. Output processed data to /temp-output-directory/alternative/blockchain/symbol.csv
destination_folder.mkdir(parents=True, exist_ok=True)

Dict = {"Difficulty": "DIFF",
        "My Wallet Number of Users": "MWNUS",
        "Average Block Size": "AVBLS",
        "Blockchain Size": "BLCHS",
        "Median Transaction Confirmation Time": "ATRCT",
        "Miners Revenue": "MIREV",
        "Hash Rate": "HRATE",
        "Cost Per Transaction": "CPTRA",
        "Cost Percent of Transaction Volume": "CPTRV",
        "Estimated Transaction Volume USD": "ETRVU",
        "Estimated Transaction Volume": "ETRAV",
        "Total Output Volume": "TOUTV",
        "Number of Transaction per Block": "NTRBL",
        "Number of Unique Bitcoin Addresses Used": "NADDU",
        "Number of Transactions Excluding Popular Addresses": "NTREP",
        "Total Number of Transactions": "NTRAT",
        "Number of Transactions": "NTRAN",
        "Total Transaction Fees USD": "TRFUS",
        "Total Transaction Fees": "TRFEE",
        "Market Capitalization": "MKTCP",
        "Total Bitcoins": "TOTBC",
        "My Wallet Number of Transaction Per Day": "MWNTD",
        "My Wallet Transaction Volume": "MWTRV"
    }

def download_blockchain_nasdaq():
    data = {}

    # Create CSV, save as bitcoin data
    with open(destination_folder / "btcusd.csv", "w") as csv_file:

        for data_name, data_code in Dict.items():

            download_link = f"{base_link}&code={data_code}&api_key={api}"

            i = 1
            
            while i <= 5:
                try:
                    # Fetch data
                    raw_data = requests.get(download_link, allow_redirects=True)
                    content = raw_data.json()['datatable']['data']

                    if not data:
                        
                        for row in content:
                            data[row[1]] = row[2]

                    else:
                        data_ = {}
        
                        for row in content:
                            data_[row[1]] = row[2]
                            
                        data = {k: str(data.get(k, '0')) + "," + str(data_.get(k, '0')) for k in set(data) | set(data_)}
                            
                    print(f"Downloaded '{data_name}' successfully")
                    break 
                
                except:
                    if i == 5:
                        print(f"Failed to download data from {download_link} (5 / 5) - Exiting")
                    else:
                        print(f"Failed to download file: '{data_name}' ({i} / 5)")
                
                i += 1
        
        # Sort by dates
        data = sorted(data.items(), key=lambda x: x[0])

        for date, value in data:
            csv_file.write(date + "," + str(value) + '\n')
                    
download_blockchain_nasdaq()
