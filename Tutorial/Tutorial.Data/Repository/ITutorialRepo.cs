using Tutorial.Global.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Tutorial.Data.Repository
{
    /// <summary>
    /// Reposirty for the application
    /// </summary>
    public interface ITutorialRepo
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
        /// <param name="userDTO"></param>
        /// <returns></returns>
        Task<int> AddUser(AuthenticatedUserDTO userDTO);
    }
}
