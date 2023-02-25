using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTO;
using API.Entities;
using API.Helper;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static API.DTO.StatusDTO;

namespace API.Controllers
{
    [TokenAuthorization]
    public class ContactController : BaseApiController
    {
        string sheetName = CommonItem.ContactSheet;
        private readonly IGenericRepository _genericRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly IPhotoService _photoService;
        public ContactController(IGenericRepository genericRepository,
                IMapper mapper,
                IPhotoService photoService,
         IHttpContextAccessor contextAccessor
        , IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _contextAccessor = contextAccessor;
            _genericRepository = genericRepository;
            _mapper = mapper;
            _photoService = photoService;
        }

        // Getting the contacts of List of users contributing for Aradhana and other cause
        [HttpGet]
        public async Task<ActionResult<Contact>> GetContactsData()
        {
            var service = _genericRepository.GoogleServiceIntialise();
            var data = await _genericRepository.GetListAsync<Contact>(sheetName, service);
            return Ok(data);
        }

        // Getting the contacts of an users contributing for Aradhana and other cause
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Contact>>> GetContactDataById(string id)
        {
            var service = _genericRepository.GoogleServiceIntialise();
            var category = "id";
            var data = await _genericRepository.GetListAsync<Contact>(sheetName, service, id, category);
            return data;
        }


        // Getting the clarifcation List created by the users  
        [HttpGet("clarification")]
        public async Task<List<UserClarificationDTO>> GetUserClarificationsAsync()
        {
            string sheetName = CommonItem.UserClarificationSheet;
            var service = _genericRepository.GoogleServiceIntialise();
            var data = await _genericRepository.GetListAsync<UserClarification>(sheetName, service);
            data = data.OrderByDescending(l => l.CreatedDate).ToList();
            var clarification = _mapper.Map<List<UserClarification>, List<UserClarificationDTO>>(data);
            return clarification;
        }

        // Getting the List of Aradhana User who sent the amount by search criteria
        [HttpGet("category")]
        public async Task<ActionResult<List<Contact>>> GetContactDataByCategorySearchl(string searchValue, string category)
        {

            if ((!string.IsNullOrEmpty(searchValue)) && (!string.IsNullOrEmpty(category)))
            {
                var service = _genericRepository.GoogleServiceIntialise();
                var data = await _genericRepository.GetListAsync<Contact>(sheetName, service, searchValue, category);
                return data;
            }
            return BadRequest("Provide Proper Search Criteria");

        }

        // Upcoming events happening the temple
        [HttpPost("upload-photo")]
        public async Task<ActionResult> UploadPhoto([FromForm] PhotoDto latestEventsDTO)
        {
            string sheetName = CommonItem.UploadPhotoSheet;
            var service = _genericRepository.GoogleServiceIntialise();
            var latestEvents = _mapper.Map<PhotoDto, Photo>(latestEventsDTO);
            if (latestEventsDTO?.Image != null && latestEventsDTO.Image.Length > 0)
            {
                var uploadResult = await _photoService.AddPhotoAsync(latestEventsDTO.Image);
                latestEvents.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
                latestEvents.PublicId = uploadResult.PublicId;
                if (uploadResult.Error != null) return BadRequest(uploadResult.Error.Message);
            }

            // creating the record in the Googlesheets
            var status = await _genericRepository.CreateDataAsync<Photo>(sheetName, latestEvents, service);
            if (status) return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Created)));
            return Ok(new StatusDTO(Status.Failure, StatusDTO.GetDescription((MessageData)(short)MessageData.NotProcessed)));

        }

        // creating the contact of an User who sent the amount by search criteria
        [HttpPost]
        public async Task<ActionResult> CreateData(Contact contact)
        {

            var service = _genericRepository.GoogleServiceIntialise();
            contact.Id = CommonItem.GenerateUniqueId();
            var status = await _genericRepository.CreateDataAsync<Contact>(sheetName, contact, service);
            if (status) return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Created)));
            return Ok(new StatusDTO(Status.Failure, StatusDTO.GetDescription((MessageData)(short)MessageData.NotProcessed)));
        }

        // User posting the clarfication or suggesstion for the temple.
        [HttpPost("clarification")]
        public ActionResult SendEmail(ClarificationDTO clarificationDTO)
        {
            var clarification = new ClarificationMessage
            (clarificationDTO.FullName,
             clarificationDTO.To,
             clarificationDTO.Subject,
             clarificationDTO.Question,
             clarificationDTO.Reply,
             clarificationDTO.FromEmail,
             clarificationDTO.Password,
              clarificationDTO.Type
              );
            _emailSender.SendEmail<ClarificationMessage>(clarification, clarification.Password, clarification.FromEmail);
            return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Sent)));
        }

        // Updating the data by admin for the contacts
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateData(string id, Contact contact)
        {
            var status = false;
            //DELETE THE OLD RECORD AND Insert the new record.
            var service = _genericRepository.GoogleServiceIntialise();
            contact.Id = CommonItem.GenerateUniqueId();
            status = await _genericRepository.DeleteDataByIdAsync(sheetName, id, service);
            if (status)
            {
                status = await _genericRepository.CreateDataAsync<Contact>(sheetName, contact, service);
                if (status) return Ok(new StatusDTO(Status.Success, StatusDTO.GetDescription((MessageData)(short)MessageData.Updated)));
            }
            return Ok(new StatusDTO(Status.Failure, StatusDTO.GetDescription((MessageData)(short)MessageData.NotProcessed)));
        }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult> DeleteData(string id)
        // {
        //     var service = _genericRepository.GoogleServiceIntialise();
        //     var status = await _genericRepository.DeleteDataByIdAsync(sheetName, id, service);
        //     if (status) return Ok("Successfully deleted the record");
        //     return Ok("Unable to Delete the record");
        // }

    }
}