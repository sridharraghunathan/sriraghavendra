using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Helper
{
    // Attribute are used to decorate the class and here we
    // are using the IAuth filter so we are override the implmentation of Authorize from dotnet core
    public class TokenAuthorization : Attribute,  IAsyncAuthorizationFilter
    {

        // This is the Key value
        private const string _idsToken = "Authorization";
        private static string clienttoken = Startup.StaticConfig["ClientId"];

        // This is very important method will be called for every controller or method where its 
        // decorated with and this is used for the admin validation is user entitled to perform certain action.
        // OnAuthorization is implementation of interface IAuth
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Skip the Authorization validation check if we enabler Allow Anonymous in the method
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()) return;
            var headerToken = context.HttpContext.Request.Headers.SingleOrDefault(x => x.Key == _idsToken);
            // try
            // {
            //     if (headerToken.Key != null)
            //     {
            //         var idToken = headerToken.Value.SingleOrDefault();
            //         if (!string.IsNullOrEmpty(idToken))
            //         {
            //             UserDetails userDetails = await ValidateToken(idToken);
            //             //Validating is the Logged person Admin or not
            //             userDetails.IsAdmin = AdminValidation.IsAdmin(userDetails.Email);
            //             bool status = (userDetails?.IsAdmin != null && userDetails.IsAdmin) ? true : false;
            //             if (userDetails.Email != null && status)
            //             {
            //                 context.HttpContext.Items.Add("userInfo", userDetails);
            //             }
            //             else
            //             {
            //                 // context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            //                 // return;
            //                 StatusCode(context, StatusCodes.Status403Forbidden);
            //             }
            //         }
            //         else
            //         {
            //             StatusCode(context, StatusCodes.Status400BadRequest);
            //         }
            //     }
            //     else
            //     {
            //         StatusCode(context, StatusCodes.Status400BadRequest);
            //         //context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);

            //     }

            //     return;
            // }


            try
            {
                if (headerToken.Key == null) { StatusCode(context, StatusCodes.Status400BadRequest); return; }

                var idToken = headerToken.Value.SingleOrDefault();
                if (string.IsNullOrEmpty(idToken))
                {
                    StatusCode(context, StatusCodes.Status400BadRequest); return;
                }
                UserDetails userDetails = await ValidateToken(idToken);

                //Validating is the Logged person Admin or not
                userDetails.IsAdmin = AdminValidation.IsAdmin(userDetails.Email);
                bool status = (userDetails?.IsAdmin != null && userDetails.IsAdmin);
                if (userDetails.Email == null || !status) { StatusCode(context, StatusCodes.Status403Forbidden); return; }
                context.HttpContext.Items.Add("userInfo", userDetails);

            }
            catch (Exception)
            {
                StatusCode(context, StatusCodes.Status400BadRequest);
                // context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                // return;
            }

        }

        private static void StatusCode(AuthorizationFilterContext context, [ActionResultStatusCode] int status)
        {
            context.Result = new StatusCodeResult(status);
            return;
        }

        /// <summary>
        ///  This method is used for validating the token for first time
        ///  and subsequent time will be called from authorization method from above
        /// </summary>
        /// <param name="idToken"></param>
        /// <returns></returns>
        public async static Task<UserDetails> ValidateToken(string idToken)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings();

            settings.Audience = new List<string>() { clienttoken };
            // Validating the Token Signature for Authorization Purpose.
            GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            var result = new UserDetails
            {
                idToken = idToken,
                FullName = payload.Name,
                Email = payload.Email,
                Picture = payload.Picture,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName
            };
            return result;
        }

      
    }
}