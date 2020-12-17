using Contact.Repository.Entities;
using Microsoft.AspNetCore.Http;
using System;

namespace Contact.Repository.Helper
{
    /// <summary>
    /// Custom ExceptionHandler
    /// </summary>
    public class CustomExceptionHandler
    {
        /// <summary>
        /// Gets the custom exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns></returns>
        public static CustomException GetCustomException(Exception ex)
        {
            return new CustomException()
            {
                ErrorMessage = ex.Message,
                StatusCode = StatusCodes.Status500InternalServerError,
                ExceptionTrace = ex.StackTrace
            };
        }

    }
}
