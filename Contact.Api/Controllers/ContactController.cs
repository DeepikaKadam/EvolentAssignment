using Contact.Api.Constants;
using Contact.Repository;
using Contact.Repository.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ContactManagementApp.Controllers
{
    /// <summary>
    /// Provides all Contact APIs
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    //[Authorize]
    [ApiController]
    [Route(RouteConstants.ROUTE_CONTACT)]
    public class ContactController : ControllerBase
    {
        #region Private Variables

        /// <summary>
        /// The contact repository
        /// </summary>
        private readonly IContactRepository contactRepository;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactController"/> class.
        /// </summary>
        /// <param name="contactRepository">The contact repository.</param>
        /// <param name="logger">The logger.</param>
        public ContactController(IContactRepository contactRepository)
        {
            this.contactRepository = contactRepository;
        }

        #endregion

        #region Public APIs

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await contactRepository.GetContacts();

            if (result == null)
            {
                return new ObjectResult(MessageConstants.ERROR_MESSAGE_SOMETHING_WENT_WRONG) { StatusCode = StatusCodes.Status500InternalServerError, Value = "Something went wrong" };
            }
            else if (result is CustomException databaseException)
            {
                return new ObjectResult(databaseException.ErrorMessage) { StatusCode = databaseException.StatusCode, Value = databaseException.ExceptionTrace };
            }

            return new OkObjectResult(result);
        }

        /// <summary>
        /// Adds the specified contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Contact.Api.Models.Contact contact, [FromHeader] int userId)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(Environment.NewLine, ModelState.Values.SelectMany(errors => errors.Errors)
                                                            .Select(error => error.ErrorMessage));
                return BadRequest(errorMessage);
            }
            else if (userId == 0)
            {
                return BadRequest(MessageConstants.VALIDATION_ERROR_MESSAGE_FIELD_USER_ID);
            }

            Contact.Repository.Entities.Contact newContact = new Contact.Repository.Entities.Contact()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                EmailId = contact.EmailId,
                IsActive = true,
                PhoneNumber = contact.PhoneNumber,
                Timestamp = DateTime.UtcNow,
                UserId = userId
            };

            var result = await contactRepository.AddContact(newContact);

            if (result == null)
            {
                return new ObjectResult(MessageConstants.ERROR_MESSAGE_SOMETHING_WENT_WRONG) { StatusCode = StatusCodes.Status500InternalServerError, Value = "Something went wrong" };
            }
            else if (result is CustomException databaseException)
            {
                return new ObjectResult(databaseException.ErrorMessage) { StatusCode = databaseException.StatusCode, Value = databaseException.ExceptionTrace };
            }

            return new OkObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }


        /// <summary>
        /// Updates the specified contact.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [Route(RouteConstants.ROUTE_CONTACT_BY_ID)]
        public async Task<IActionResult> Update([FromBody] Contact.Api.Models.Contact contact
            , int contactId, [FromHeader]int userId)
        {
            if (!ModelState.IsValid)
            {
                var errorMessage = string.Join(Environment.NewLine, ModelState.Values.SelectMany(errors => errors.Errors)
                                                            .Select(error => error.ErrorMessage));
                return BadRequest(errorMessage);
            }
            else if (contactId <= 0)
            {
                return BadRequest(MessageConstants.VALIDATION_ERROR_MESSAGE_INVALD_CONTACT_ID);
            }
            else if (userId == 0)
            {
                return BadRequest(MessageConstants.VALIDATION_ERROR_MESSAGE_FIELD_USER_ID);
            }

            Contact.Repository.Entities.Contact updatedContact = new Contact.Repository.Entities.Contact()
            {
                Id = contactId,
                FirstName = contact.FirstName ?? null,
                LastName = contact.LastName ?? null,
                EmailId = contact.EmailId ?? null,
                PhoneNumber = contact.PhoneNumber ?? null,
                Timestamp = DateTime.UtcNow,
                UserId = userId > 0 ? userId : 0
            };

            var result = await contactRepository.UpdateContact(updatedContact);

            if (result == null || (result is bool && (bool)result == false))
            {
                return new ObjectResult(MessageConstants.ERROR_MESSAGE_SOMETHING_WENT_WRONG) { StatusCode = StatusCodes.Status500InternalServerError, Value = "Something went wrong" };
            }
            else if (result is CustomException databaseException)
            {
                return new ObjectResult(databaseException.ErrorMessage) { StatusCode = databaseException.StatusCode, Value = databaseException.ExceptionTrace };
            }

            return new OkObjectResult(MessageConstants.SUCCESS_MESSAGE) { StatusCode = StatusCodes.Status200OK };
        }

        /// <summary>
        /// Deteles the specified contact identifier.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Route(RouteConstants.ROUTE_CONTACT_BY_ID)]
        public async Task<IActionResult> Detele(int contactId, [FromHeader]int userId)
        {
            if (contactId <= 0)
            {
                return BadRequest(MessageConstants.VALIDATION_ERROR_MESSAGE_INVALD_CONTACT_ID);
            }
            else if (userId == 0)
            {
                return BadRequest(MessageConstants.VALIDATION_ERROR_MESSAGE_FIELD_USER_ID);
            }

            var result = await contactRepository.DeleteContact(contactId, userId);

            if (result == null || (result is bool && (bool)result == false))
            {
                return new ObjectResult(MessageConstants.ERROR_MESSAGE_SOMETHING_WENT_WRONG) { StatusCode = StatusCodes.Status500InternalServerError, Value = "Something went wrong" };
            }
            else if (result is CustomException databaseException)
            {
                return new ObjectResult(databaseException.ErrorMessage) { StatusCode = databaseException.StatusCode, Value = databaseException.ExceptionTrace };
            }

            return new OkObjectResult(MessageConstants.SUCCESS_MESSAGE) { StatusCode = StatusCodes.Status200OK };
        }

        #endregion
    }
}
