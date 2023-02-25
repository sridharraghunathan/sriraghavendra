using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Configuration;

namespace API.Data
{
    public class GenericRepository : IGenericRepository
    {

        // SpreadSheet id coming from 
        //https://docs.google.com/spreadsheets/d/1kKCSKUXXPkiJiIE7kM2Pt8zODd9PabYLh6ft3sSG8Zo/edit#gid=0


        string SpreadsheetId, GeneralSpreadSheetId, AradhanaSpreadSheetId;
        int gid;
        private readonly IConfiguration _config;
        public GenericRepository(IConfiguration config, IPhotoService photoService)
        {

            _config = config;
            GeneralSpreadSheetId = _config["GeneralSpreadSheetId"];
            AradhanaSpreadSheetId = _config["AradhanaSpreadSheetId"];
            gid = Convert.ToInt32(_config["gid"]);
        }

        public SheetsService GoogleServiceIntialise()
        {
            SheetsService service;
            // Code is for the intializing the sheet service of the Google API .
            string[] Scopes = { SheetsService.Scope.Spreadsheets };
            // we can give any name for the APPLICATION
            string ApplicationName = "SriRaghavendra App";

            GoogleCredential credential;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Use connection string from file.
            using (var stream = new FileStream("sriraghavendra-credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }
            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;

        }

        public async Task<bool> DeleteDataByIdAsync(string sheetName, string id, SheetsService service)
        {
            // After Getting index add one for header .
            var indexValue = await GetIndexByIdAsync(sheetName, id, service);
            if (indexValue == 0) return false;
            var requestBody = new ClearValuesRequest();
            Request RequestBody = new Request()
            {
                DeleteDimension = new DeleteDimensionRequest()
                {
                    Range = new DimensionRange()
                    {
                        //https://docs.google.com/spreadsheets/d/1kKCSKUXXPkiJiIE7kM2Pt8zODd9PabYLh6ft3sSG8Zo/edit#gid=0
                        //gid is sheet id
                        SheetId = gid,
                        Dimension = "ROWS",
                        StartIndex = indexValue, // Before Row Just Pointer
                        EndIndex = indexValue + 1 // Row to be deleted
                    }
                }
            };

            List<Request> RequestContainer = new List<Request>();
            RequestContainer.Add(RequestBody);

            BatchUpdateSpreadsheetRequest deleteRequest = new BatchUpdateSpreadsheetRequest();
            deleteRequest.Requests = RequestContainer;

            SpreadsheetsResource.BatchUpdateRequest Deletion = new SpreadsheetsResource.BatchUpdateRequest
            (service, deleteRequest, SpreadsheetId);

            await Deletion.ExecuteAsync();

            return true;
        }

        /// <summary>
        /// Create an Record Excel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheetName">Takes Input as Sheet Name where the data goes</param>
        /// <param name="data">Take an Object as input and object definition should be same as data definition</param>
        /// <param name="service">Google service to access the Google sheet</param>
        /// <returns></returns>
        public async Task<bool> CreateDataAsync<T>(string sheetName, T data, SheetsService service) where T : class
        {
            var status = false;
            try
            {
                var range = GetRangeBySheetName(sheetName);
                var valueRange = new ValueRange();

                /* #region   */
                // var oblist = new List<object>() {
                //  Id,
                //  contact.FullName,
                //   contact.Email,
                //    contact.MobileNumber,
                //    contact.Address ,
                //    contact.City,
                //    contact.State,
                //    contact.Pincode,
                //    contact.Country
                //    };
                /* #endregion */
                var collection = new List<object>();
                var properties = typeof(T).GetProperties().ToList();
                properties.ForEach(x => collection.Add(x.GetValue(data)));
                // Google API is expecting the 2d Array Like List of List
                // First we are converting the Object to List<Object> which is collection variable and then
                // placing the List Object in another List
                valueRange.Values = new List<IList<object>> { collection };
                var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
                appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
                var appendReponse = await appendRequest.ExecuteAsync();
                return true;
            }

            catch (Exception)
            {
                return status;
            }

        }

        /* #region  Old Code */
        // public async Task<bool> CreateDataAsync(string sheetName, Contact contact, SheetsService service)
        // {
        //     var status = false;
        //     try
        //     {
        //         var range = GetRangeBySheetName(sheetName);
        //         var valueRange = new ValueRange();
        //         var Id = Guid.NewGuid().ToString();
        //         Id = Id.Replace("/", "aa");
        //         Id = Id.Replace("-", "bb");
        //         contact.Id= Id;

        //         // var oblist = new List<object>() {
        //         //  Id,
        //         //  contact.FullName,
        //         //   contact.Email,
        //         //    contact.MobileNumber,
        //         //    contact.Address ,
        //         //    contact.City,
        //         //    contact.State,
        //         //    contact.Pincode,
        //         //    contact.Country
        //         //    };

        //          var collection = new List<object>();
        //          var properties = typeof(Contact).GetProperties().ToList() ;
        //          properties.ForEach(x => collection.Add(x.GetValue(contact)));
        //         valueRange.Values = new List<IList<object>> { collection };
        //         var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
        //         appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
        //         var appendReponse = await appendRequest.ExecuteAsync();
        //         return true;
        //     }

        //     catch (Exception)
        //     {

        //         return status;
        //     }

        // }

        /* #endregion */
        // we have used generics implmentation where 
        // contacts and Aradhana does the same function only the object is different but both are for getting 
        // data from the function
        public async Task<List<T>> GetListAsync<T>(string sheetName, SheetsService service, string searchValue = "-1", string category = "all") where T : new()
            // we need to mention where condition new() saying we are instantiating the Empty constructor
        {
            var range = GetRangeBySheetName(sheetName);
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);
            // Output response will be of List of List
            var response = await request.ExecuteAsync();
            var values = response.Values;
            var header = response.Values[0];
            searchValue = searchValue.ToLower();

            // REMOVE HEADER
            values.RemoveAt(0);
            var cnt = values.Count();

            List<T> dataList = new List<T>();

            if (values != null && values.Count > 0)
            {
                // First For loop for converting the Multi List to single List
                foreach (var row in values)
                {
                    if (searchValue == "-1" && category == "all")  // no search criteria given generate all records
                    {
                        CreateDataList<T>(dataList, row, header);
                    }
                    else if (searchValue == Convert.ToString(row[0]).ToLower()) // search by Id
                    {
                        CreateDataList<T>(dataList, row, header);
                        break;
                    }

                    else if (category == "name" && Convert.ToString(row[1]).ToLower().Contains(searchValue)) // search by Name
                    {
                        CreateDataList<T>(dataList, row, header);
                    }


                    else if (category == "email" && Convert.ToString(row[2]).ToLower().Contains(searchValue)) // search by Email
                    {
                        CreateDataList<T>(dataList, row, header);
                    }

                    else if (category == "phone" && searchValue == Convert.ToString(row[3]).ToLower()) // search by phone
                    {
                        CreateDataList<T>(dataList, row, header);
                    }

                    else if (category == "city" && searchValue == Convert.ToString(row[5]).ToLower()) // search by city
                    {
                        CreateDataList<T>(dataList, row, header);
                    }

                    else if (category == "year" && searchValue == Convert.ToString(row[12]).ToLower()) // search by city
                    {
                        CreateDataList<T>(dataList, row, header);
                    }
                }
            }
            else
            {
                return dataList = new List<T>();
            }
            return dataList;
        }

        private void CreateDataList<T>(List<T> dataList, IList<object> row, IList<object> headers) where T : new()
        {
            // getting the properties of the class instance
            var cols = typeof(T).GetProperties();

            /* #region   */
            // var info = new Contact
            // {
            //     Id = Convert.ToString(row[0]),
            //     FullName = Convert.ToString(row[1]),
            //     Email = Convert.ToString(row[2]),
            //     MobileNumber = Convert.ToString(row[3]),
            //     Address = Convert.ToString(row[4]),
            //     City = Convert.ToString(row[5]),
            //     State = Convert.ToString(row[6]),
            //     Pincode = Convert.ToString(row[7]),
            //     Country = Convert.ToString(row[8])
            // };
            /* #endregion */
            /*
             Using Generic we are Checking the Header Column Name and Property Name are Same

            */
            // when we instance the class using the generics we need to mention saying that 
            // we are going to instance the class new () in where condition
            var info1 = new T();
            for (var i = 0; i < headers.Count; i++)
            {
                foreach (var col in cols)
                {
                    if (col.Name == Convert.ToString(headers[i]))
                    {
                        //Converting the row data to Property type of target
                        //setting the value to the property
                        //Below is reflection concept 
                        col.SetValue(info1, Convert.ChangeType(row[i], col.PropertyType));
                    }
                }
            }
            dataList.Add(info1);
        }

        public async Task<int> GetIndexByIdAsync(string sheetName, string rowId, SheetsService service)
        {
            var range = GetRangeBySheetName(sheetName);
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = await request.ExecuteAsync();
            var values = response.Values;

            // REMOVE HEADER
            values.RemoveAt(0);
            var cnt = values.Count();
            int i = 0;

            /*
                since we cannot get the data by id in google sheet only option can be used is
                we need to find the row where the record exist to find the row 
                we can go thru each every record and find the value matches the given id 
                so we will the row number 
            */
            if (values != null && values.Count > 0)
            {
                foreach (var RowData in values)
                {
                    i += 1;

                    if (RowData[0].ToString() == rowId)
                    {
                        return i;
                    }
                }
            }
            else
            {
                //
            }
            return 0;
        }

        // whenever any new sheet is created need to update the below methos and 
        // corresponding the Column length has to be updated the Common class as well with sheet name and range
        private string GetRangeBySheetName(string sheetName)
        {
            var range = "";
            if (sheetName == CommonItem.ContactSheet)
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.Contactrange;
            }

            else if (sheetName == CommonItem.LatestUpdateSheet)
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.LatestUpdaterange;
            }

            else if (sheetName == CommonItem.SevaUpdateSheet)
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.SevaUpdaterange;
            }

            else if (sheetName.ToLower().Contains(CommonItem.UserClarificationSheet))
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.UserClarificationrange;
            }

            else if (sheetName.ToLower().Contains(CommonItem.AboutSheet))
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.Aboutrange;
            }

            else if (sheetName.ToLower().Contains(CommonItem.UploadPhotoSheet))
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.UploadPhotorange;
            }

            else if (sheetName.ToLower().Contains(CommonItem.TempleInfoSheet))
            {
                SpreadsheetId = GeneralSpreadSheetId;
                range = CommonItem.TempleInforange;
            }

            else if (sheetName.ToLower().Contains(CommonItem.AradhanaSheet))
            {
                SpreadsheetId = AradhanaSpreadSheetId;
                range = CommonItem.Aradhanarange;
            }

            return range;
        }
    }
}