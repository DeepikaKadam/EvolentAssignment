using Contact.Api.Constants;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Contact.Api.Models
{
    public class Contact
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessage = MessageConstants.VALIDATION_ERROR_MESSAGE_INVALD_FIRST_NAME),
            MinLength(1, ErrorMessage = MessageConstants.VALIDATION_ERROR_MESSAGE_INVALD_FIRST_NAME),
             DefaultValue("")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [DefaultValue("")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email identifier.
        /// </summary>
        /// <value>
        /// The email identifier.
        /// </value>
        [DefaultValue("")]
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Required(ErrorMessage = MessageConstants.VALIDATION_ERROR_MESSAGE_INVALD_PHONE_NUMBER),
            MinLength(1, ErrorMessage = MessageConstants.VALIDATION_ERROR_MESSAGE_INVALD_PHONE_NUMBER)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
