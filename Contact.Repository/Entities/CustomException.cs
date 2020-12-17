namespace Contact.Repository.Entities
{
    /// <summary>
    /// Custom Exception object to return for errors.
    /// </summary>
    public class CustomException
    {
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        /// <value>
        /// HTTP status code.
        /// </value>
        public int StatusCode { get; set; }
        /// <summary>
        /// Gets or sets the custom error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the exception trace.
        /// </summary>
        /// <value>
        /// The exception trace.
        /// </value>
        public string ExceptionTrace { get; set; }
    }
}
