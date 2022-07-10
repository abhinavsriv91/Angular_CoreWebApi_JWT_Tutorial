using Microsoft.Extensions.Configuration;
using Tutorial.Data;
using Tutorial.Global.DTO;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;
using Tutorial.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace Tutorial.Data.Repository
{
    /// <summary>
    /// Sql Reposirty class for Spot
    /// </summary>
    public class SqlTutorialRepo : ITutorialRepo
    {
        private readonly IConfiguration _configuration;

        private readonly ISqlManager _sqlManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="sqlManager"></param>
        public SqlTutorialRepo(IConfiguration configuration, ISqlManager sqlManager)
        {
            _configuration = configuration;
            _sqlManager = sqlManager;
        }

        /// <summary>
        /// Retrieves roles of a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<string>> SelectRoles(string userName)
        {
            try
            {
                IList<string> userRoles = new List<string>();
                string message = string.Empty;
                SqlParameter[] parameters = new SqlParameter[]
                {
                    await _sqlManager.GetParameter("username", userName),
                };
                DataSet dataSet = await _sqlManager.ExecuteDataset("usp_selectroles", parameters);

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        userRoles.Add(Convert.ToString(row[0]));
                    }
                }
                else
                    throw new TutorialApplicationException("Data not available.");
                return userRoles;
            }
            catch (DataLayerException dlEx)
            {
                string message = "Unable to fetch role" + dlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, dlEx.StackTrace);
                throw new BusinessLayerException(message, (int)HttpStatusCode.InternalServerError, dlEx);
            }
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AuthenticatedUserDTO> AuthenticateUser(string userName, string password)
        {
            try
            {
                AuthenticatedUserDTO authenticatedUser = new AuthenticatedUserDTO();
                SqlParameter[] parameters = new SqlParameter[]
                {
                    await _sqlManager.GetParameter("username", userName),
                    await _sqlManager.GetParameter("password", password),
                };
                DataSet dataSet = await _sqlManager.ExecuteDataset("usp_AuthenticateUser", parameters);

                if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        authenticatedUser.UserName = Convert.ToString(dataSet.Tables[0].Rows[0][0]);
                        authenticatedUser.FullName = Convert.ToString(dataSet.Tables[0].Rows[0][1]);
                        authenticatedUser.EmailAddress = Convert.ToString(dataSet.Tables[0].Rows[0][2]);
                    }
                    if (dataSet.Tables[1].Rows.Count > 0)
                    {
                        authenticatedUser.Roles = new List<string>();
                        foreach (DataRow row in dataSet.Tables[1].Rows)
                        {
                            authenticatedUser.Roles.Add(Convert.ToString(row[1]));
                        }
                    }
                }
                return authenticatedUser;
            }
            catch (DataLayerException dlEx)
            {
                string message = "Unable to authenticate user" + dlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, dlEx.StackTrace);
                throw new BusinessLayerException(message, (int)HttpStatusCode.InternalServerError, dlEx);
            }
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<int> AddUser(AuthenticatedUserDTO userDTO)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    await _sqlManager.GetParameter("username", userDTO.UserName),
                    await _sqlManager.GetParameter("password", userDTO.Password),
                    await _sqlManager.GetParameter("FullName", userDTO.FullName),
                    await _sqlManager.GetParameter("EmailAddress", userDTO.EmailAddress),
                };
                int retrunCode = (int)await _sqlManager.ExecuteNonQuery("usp_AddUser", parameters);

                return retrunCode;
            }
            catch (DataLayerException dlEx)
            {
                string message = "Unable to Add user" + dlEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, dlEx.StackTrace);
                throw new BusinessLayerException(message, (int)HttpStatusCode.InternalServerError, dlEx);
            }
        }
    }
}
