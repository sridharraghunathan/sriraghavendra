using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static API.DTO.StatusDTO;

namespace API.Controllers
{
    [TokenAuthorization ]
    public class AradhanaController : BaseApiController
    {
        string sheetName = CommonItem.AradhanaSheet;
        private readonly IGenericRepository _genericRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _contextAccessor;

        public AradhanaController(IGenericRepository genericRepository,
        IPhotoService photoService,
        IHttpContextAccessor contextAccessor,
        IMapper mapper, IEmailSender emailSender)
        {
            _contextAccessor = contextAccessor;
            _mapper = mapper;
            _emailSender = emailSender;
            _genericRepository = genericRepository;
            _photoService = photoService;
        }

        // Sending the Email to the User  for Acknowledgement of the amount
        [HttpPost("sendemail")]
        public ActionResult SendEmail(MessageDTO messagedto)
        {
            var message = new Message(messagedto.To, messagedto.Subject, messagedto.SevaName,
            messagedto.AmountReceived, messagedto.Address, messagedto.FullName, messagedto.FromEmail, messagedto.Password, messagedto.Type);
            _emailSender.SendEmail<Message>(message, message.Password, message.FromEmail);
            return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Sent)));
        }

        // Upcoming events happening the temple
        [HttpPost("latest-update")]
        public async Task<ActionResult> CreateLatestNews([FromForm] LatestEventsDTO latestEventsDTO)
        {
            string sheetName = CommonItem.LatestUpdateSheet;
            var service = _genericRepository.GoogleServiceIntialise();
            var latestEvents = _mapper.Map<LatestEventsDTO, LatestEvents>(latestEventsDTO);
            // creating the record in the Googlesheets
            var status = await _genericRepository.CreateDataAsync<LatestEvents>(sheetName, latestEvents, service);
            if (status) return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Created)));
            return Ok(new StatusDTO(Status.Failure, StatusDTO.GetDescription((MessageData)(short)MessageData.NotProcessed)));

        }

        // Getting the List of Aradhana User who sent the amount
        [HttpGet]
        public async Task<ActionResult<List<AradhanaDetails>>> GetData()
        {
            var service = _genericRepository.GoogleServiceIntialise();
            var data = await _genericRepository.GetListAsync<AradhanaDetails>(sheetName, service);
            return data;

        }

        // Getting the User of Aradhana  who sent the amount
        [HttpGet("{id}")]
        public async Task<ActionResult<List<AradhanaDetails>>> GetDataById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var service = _genericRepository.GoogleServiceIntialise();
                var category = "id";
                var data = await _genericRepository.GetListAsync<AradhanaDetails>(sheetName, service, id, category);
                return data;
            }
            else
            {
                return BadRequest("Provide Proper Search Criteria");
            }

        }
        // Getting the List of Aradhana User who sent the amount by search criteria
        // Used the Query PARAMS
        [HttpGet("category")]
        public async Task<ActionResult<List<AradhanaDetails>>> GetDataByCategorySearch(string searchValue, string category)
        {
            if (!string.IsNullOrEmpty(searchValue) && (!string.IsNullOrEmpty(category)))
            {
                var service = _genericRepository.GoogleServiceIntialise();
                var data = await _genericRepository.GetListAsync<AradhanaDetails>(sheetName, service, searchValue, category);
                var data1 = data.OrderByDescending(d => d.CreatedDate).ToList();
                return data1;
            }
            return BadRequest("Provide Proper Search Criteria");

        }

        // we have Allow anonymous attribute so any one access this method for submitting the Aradhana amount
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CreateData(AradhanaDetails aradhanaDetails)
        {
            var service = _genericRepository.GoogleServiceIntialise();
            aradhanaDetails.Id = CommonItem.GenerateUniqueId();
            //Aradhana Information Adding
            var status = await _genericRepository.CreateDataAsync(sheetName, aradhanaDetails, service);
            if (status)
            {
                sheetName = CommonItem.ContactSheet;
                string category = "email";
                //Contact information adding
                var data = await _genericRepository.GetListAsync<Contact>(sheetName, service, aradhanaDetails.Email, category);
                if (data.Count == 0)
                {
                    var contact = _mapper.Map<AradhanaDetails, Contact>(aradhanaDetails);
                    status = await _genericRepository.CreateDataAsync(sheetName, contact, service);

                    if (status) return Ok(new StatusDTO(Status.Success,
                    StatusDTO.GetDescription((MessageData)(short)MessageData.Created)));
                }
                return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Created)));
            }
            return Ok(new StatusDTO(Status.Failure, StatusDTO.GetDescription((MessageData)(short)MessageData.NotProcessed)));
        }
        #region 
        // private bool isAuthenticated()
        // {
        //     UserDetails user = new UserDetails();
        //     user = (UserDetails)_contextAccessor.HttpContext.Items["userInfo"];
        //     bool status = (user.isAdmin) ? true : false;
        //     return status;
        // }

        // [HttpPost("{id}")]
        // public async Task<ActionResult> UpdateData(string id, AradhanaDetails aradhanaDetails)
        // {
        //     var status = false;
        //     //DELETE THE OLD RECORD AND Insert the new record.
        //     var service = _genericRepository.GoogleServiceIntialise();
        //     aradhanaDetails.Id = CommonItem.GenerateUniqueId();
        //     status = await _genericRepository.DeleteDataByIdAsync(sheetName, id, service);
        //     if (status)
        //     {
        //         status = await _genericRepository.CreateDataAsync<AradhanaDetails>(sheetName, aradhanaDetails, service);
        //         if (status) return Ok("Record has been updated!!!");
        //     }
        //     return Ok("Unable to process the request");
        // }


        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeleteData(string id)
        // {
        //     var service = _genericRepository.GoogleServiceIntialise();
        //     var status = await _genericRepository.DeleteDataByIdAsync(sheetName, id, service);
        //     if (status) return Ok("Successfully deleted the record");
        //     return Ok("Unable to Delete the record");
        // }
        #endregion
    }
}