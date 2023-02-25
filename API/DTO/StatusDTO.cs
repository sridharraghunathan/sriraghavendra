using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace API.DTO
{
    public class StatusDTO
    {
        public StatusDTO(Status status, string message)
        {
            this.StatusCode = status;
            this.MessageInfo = message;

        }
        public Status StatusCode { get; set; }
        public string MessageInfo { get; set; }

        public enum Status
        {
            Success,
            Failure
        }

        public enum MessageData
        {
            [Description("Request has been submitted")]
            Created,
            [Description("Request has been updated")]
            Updated,
            [Description("Email has been sent")]
            Sent,
            [Description("Unable to process the request")]
            NotProcessed,

            [Description("Internet Connection is Lost, Please check your Connection")]
            ServiceUnavailable,

            [Description("Your Query / Suggestions has been submitted , we will respond soon")]
            RequestSubmitted


        }

        public static string GetDescription(Enum value)
        {
            var enumMember = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            var descriptionAttribute =
                enumMember == null
                    ? default(DescriptionAttribute)
                    : enumMember.GetCustomAttribute(typeof(DescriptionAttribute)) as DescriptionAttribute;
            return
                descriptionAttribute == null
                    ? value.ToString()
                    : descriptionAttribute.Description;
        }
    }
}