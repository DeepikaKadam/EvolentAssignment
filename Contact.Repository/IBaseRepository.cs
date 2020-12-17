using System.Threading.Tasks;

namespace Contact.Repository
{
    /// <summary>
    /// IBaseRepository interface
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TId"></typeparam>
    public interface IBaseRepository<TEntity, TId>
    {
        #region Interface methods

        /// <summary>
        /// Get method
        /// </summary>
        /// <returns></returns>
        Task<object> GetContacts();

        /// <summary>
        /// Add method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<object> AddContact(TEntity entity);

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<object> UpdateContact(TEntity entity);

        /// <summary>
        /// Delete method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<object> DeleteContact(TId id, int userId);

        #endregion
    }
}
