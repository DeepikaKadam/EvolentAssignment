using Contact.Api.Repository.Helper;
using Contact.Repository.Entities;
using Contact.Repository.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Repository
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Contact.Repository.IContactRepository" />
    public class ContactRepository : IContactRepository
    {
        /// <summary>
        /// The database context
        /// </summary>
        private readonly DatabaseContext databaseContext;

        /// <summary>
        /// The entities
        /// </summary>
        private readonly DbSet<Contact.Repository.Entities.Contact> contacts;
        /// <summary>
        /// Initializes a new instance of the <see cref="ContactRepository"/> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        public ContactRepository(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;

            contacts = databaseContext.Set<Contact.Repository.Entities.Contact>();
        }
        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<object> AddContact(Entities.Contact entity)
        {
            object returnValue = false;
            try
            {
                var result = await contacts.AddAsync(entity);
                await databaseContext.SaveChangesAsync();

                if (result != null)
                    return result.Entity;
            }
            catch (Exception ex)
            {
                returnValue = CustomExceptionHandler.GetCustomException(ex);
            }
            return returnValue;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public async Task<object> DeleteContact(int id, int userId)
        {
            object returnValue = false;
            try
            {
                var contact = contacts.Where(contactEntity => contactEntity.Id.Equals(id))?.FirstOrDefault();
                if (contact != null)
                {
                    contact.IsActive = false;
                    contact.Timestamp = DateTime.UtcNow;
                    contact.UserId = userId;

                    databaseContext.Attach(contact);
                    var entry = databaseContext.Entry(contact);
                    entry.State = EntityState.Modified;
                    await databaseContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                returnValue = CustomExceptionHandler.GetCustomException(ex);
            }
            return returnValue;
        }

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetContacts()
        {
            object returnValue;
            try
            {
                var contactsList= await contacts.ToListAsync();
                return contactsList.Where(contactEntity => contactEntity.IsActive.Equals(true));
            }
            catch (Exception ex)
            {
                returnValue = CustomExceptionHandler.GetCustomException(ex);
            }
            return returnValue;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<object> UpdateContact(Entities.Contact entity)
        {
            object returnValue = false;
            try
            {
                var contact = contacts.Where(contactEntity => contactEntity.Id.Equals(entity.Id))?.FirstOrDefault();
                if (contact!=null)
                {
                    if(!string.IsNullOrWhiteSpace(entity.FirstName))
                        contact.FirstName = entity.FirstName;
                    if (!string.IsNullOrWhiteSpace(entity.LastName))
                        contact.LastName = entity.LastName;
                    if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
                        contact.PhoneNumber = entity.PhoneNumber;
                    if (!string.IsNullOrWhiteSpace(entity.EmailId))
                        contact.EmailId = entity.EmailId;
                    contact.Timestamp = entity.Timestamp;
                    contact.UserId = entity.UserId;

                    databaseContext.Attach(contact);
                    var entry = databaseContext.Entry(contact);
                    entry.State = EntityState.Modified;
                    await databaseContext.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex)
            {
                returnValue = CustomExceptionHandler.GetCustomException(ex);
            }
            return returnValue;
        }
    }
}
