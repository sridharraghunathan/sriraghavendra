using System;

namespace API.Entities
{
    public static class CommonItem
    {
        //Contact sheet Information

        public static readonly string ContactSheet = "contact-info";
        public static readonly string Contactrange = $"{ContactSheet}!" + "A:I";
        public static readonly string LatestUpdateSheet = "latest-update";
        public static readonly string LatestUpdaterange = $"{LatestUpdateSheet}!" + "A:D";

        public static readonly string UploadPhotoSheet = "upload-photo";
        public static readonly string UploadPhotorange = $"{UploadPhotoSheet}!" + "A:E";
        public static readonly string SevaUpdateSheet = "seva-list";
        public static readonly string SevaUpdaterange = $"{SevaUpdateSheet}!" + "A:C";

        public static readonly string TempleInfoSheet = "temple-info";
        public static readonly string TempleInforange = $"{TempleInfoSheet}!" + "A:B";


        ///Aradhana Sheet Dynamic Information with different spreadsheet.
        public static readonly string AradhanaSheet = "aradhana-collection";
        public static readonly string Aradhanarange = $"{AradhanaSheet}!" + "A:N";
        public static readonly string UserClarificationSheet = "clarification-list";
        public static readonly string UserClarificationrange = $"{UserClarificationSheet}!" + "A:F";

        public static readonly string AboutSheet = "about";
        public static readonly string Aboutrange = $"{AboutSheet}!" + "A:C";

        public enum EmailFlow
        {
            AradhanaAmountReceived,
            Donation,
            Clarification
        }
        public static string GenerateUniqueId()
        {
            var Id = Guid.NewGuid().ToString();
            Id = Id.Replace("/", "aa");
            Id = Id.Replace("-", "bb");
            return Id;
        }
    }
}