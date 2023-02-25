using Microsoft.Extensions.Configuration;
namespace API.Helper
{

    /// <summary>
    ///  Method created to validate the user is Admin or not.
    /// </summary>
    public class AdminValidation
    {
        public static bool IsAdmin(string email)
        {
            // If we use the App settting as the Array list then use the first line.
            //var EmailIdAdminLst = Startup.StaticConfig.GetSection("EmailIdAdmins").Get<string[]>();
            var EmailIdAdminLst = Startup.StaticConfig["EmailIdAdmins"].Split(",");
            foreach (var emailAdmin in EmailIdAdminLst)
            {
                if (emailAdmin == email)
                {
                    return true;
                }
            }

            return false;
        }
    }
}