using System.Collections.Generic;
using System.Threading.Tasks;
using Tutorial.Business.Models;
using Tutorial.Global.DTO;

namespace Tutorial.Business.Interfaces
{
    /// <summary>
    /// Business logic Interface
    /// </summary>
    public interface ITutorialServices
    {

        /// <summary>
        /// Retrieves roles of a user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<IList<string>> SelectRoles(string userName);

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AuthenticatedUserDTO> AuthenticateUser(string userName, string password);

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="requestUser"></param>
        /// <returns></returns>
        Task<int> AddUser(User requestUser);
    }
}
