# Data optimizer
This application allows you to get the precise infromation about every country in the world(name, population, currency, region etc.). It uses [REST Countries API](https://restcountries.com/) that provides all these data.

Using filtering, sorting and pagination(in case you want to get information by chunks) functionality on our end, all results can be adjusted by your needs.

>NOTE: Currently result contains only main info which is the name of the country and its population, should support other fields automatically if they will be added to the Country model

## How to run locally
- Download this repository
- In `UC1-DataOptimizerFromAPI\DataOptimizer` directory run command - `dotnet restore`
- After it's completed run another command - `dotnet run --project DataOptimizer`
- Navigate to [https://localhost:7277](https://localhost:7277/swagger/index.html) or [http://localhost:5188](http://localhost:5188/swagger/index.html) swagger page and set filtering/sortings paremeters in the corresponding `/api/Countries` endpoint, then execute

## Examples
>NOTE: Pagination is always ON, by default it'll get by 15 records, you can change it in *numberOfItems* field
- All parameters set by default and get first 15 countries in the list (not sorted and pagination works)
- Type *countryName* and get all matches that provided (usually it should be one country)
- Type 'tu' in *countryName* and get countries that contains this part of a work in their name (Turkmenistan, Turkey etc.)
- Change *page* and *numberOfItems* to 2 and 10, it'll give you from 10th to 20th record from general list
- Set *sortOrder* to the previous case (e.g. "asc"), it'll first order the list by country name and then return you 2nd page from the beginning with 10 records
- Set *sortOrder* to the previous case (e.g. "desc"), it'll first order the list by country name and then return you 2nd page from the end with 10 records
- Set the *population* (in millions) and get all countries that under your value (e.g. you set - 9, it'll return all countries with population less than 9m people)
- You can combine any of these parameters to get various results
