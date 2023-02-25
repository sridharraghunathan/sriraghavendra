using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;

namespace API.Interfaces
{
    public interface IGenericRepository
    {

        //Task<List<Contact>> GetListAsync(string sheetName, SheetsService service, string index = "-1");
        Task<List<T>> GetListAsync<T>(string sheetName, SheetsService service, string searchValue = "-1", string category = "all") where T : new();

        Task<int> GetIndexByIdAsync(string sheetName, string rowId, SheetsService service);

        // Task<bool> CreateDataAsync(string sheetName, Contact contact, SheetsService service);
        Task<bool> CreateDataAsync<T>(string sheetName, T contact, SheetsService service) where T : class;

        Task<bool> DeleteDataByIdAsync(string sheetName, string id, SheetsService service);

        SheetsService GoogleServiceIntialise();


    }
}