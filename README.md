<h1 align="center">:bar_chart: Transaction API</h1>
<p align="center">.NET API for the importing and exporting files about transactions</p>


## Notes

You need to setup MSSQL to use Transaction API locally

## Getting started

First you need to run the Web API project. Then you need to register, because only registered users have the access to the api.


### 1. Importing files

To import file you need to upload the .csv file to the import controller method. Then the data will be added to the database or the status of transaction will be updated if the transaction with the same id already exists.

### 2. Exporting files

To export the file you need to request to the one of the method which exports the file. You need to do this proceeding to the result that you need. For example you can export the file filtered by status or type, or you can export all data from the database in .csv file format.


# The API may be updated in the future
