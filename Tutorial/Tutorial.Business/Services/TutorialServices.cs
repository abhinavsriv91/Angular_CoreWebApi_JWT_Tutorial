using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Tutorial.Business.Interfaces;
using Tutorial.Business.Models;
using Tutorial.Data.Repository;
using Tutorial.Global.DTO;
using Tutorial.Global.Exceptions;
using Tutorial.Logger;
using Tutorial.Security;

namespace Tutorial.Business.Services
{
    /// <summary>
    /// Business logic of Spot
    /// </summary>
    public class TutorialServices : ITutorialServices
    {
        private readonly ITutorialRepo _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialize repository, mapper and configuration
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        /// <param name="configuration"></param>
        public TutorialServices(ITutorialRepo repository, IMapper mapper, IConfiguration configuration)
        {
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
        }

        /// <summary>
        /// Retrieves roles of a user
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<IList<string>> SelectRoles(string userName)
        {
            try
            {
                return await _repository.SelectRoles(userName);
            }
            catch (BusinessLayerException blEx)
            {
                string message = blEx.Message;
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), message, blEx.StackTrace);
                throw new APILayerException(message, (int)HttpStatusCode.InternalServerError, blEx);
            }
            catch (TutorialApplicationException ex)
            {
                TutorialLogger.LogError(CustomPrincipal.GetCurrentUserName(), ex.Message, ex.StackTrace);
                throw;
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
                return await _repository.AuthenticateUser(userName, password);
            }
            catch (BusinessLayerException blEx)
            {
                string message = blEx.Message;
                TutorialLogger.LogError(userName, message, blEx.StackTrace);
                throw new APILayerException(message, (int)HttpStatusCode.InternalServerError, blEx);
            }
            catch (Exception ex)
            {
                TutorialLogger.LogError(userName, ex.Message, ex.StackTrace);
                throw new APILayerException(ex.Message, (int)HttpStatusCode.InternalServerError, ex);
            }
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="requestUser"></param>
        /// <returns></returns>
        public async Task<int> AddUser(User requestUser)
        {
            AuthenticatedUserDTO userDTO = new AuthenticatedUserDTO();
            try
            {
                _mapper.Map(requestUser, userDTO);
                return await _repository.AddUser(userDTO);
            }
            catch (BusinessLayerException blEx)
            {
                string message = blEx.Message;
                TutorialLogger.LogError(requestUser.UserName, message, blEx.StackTrace);
                throw new APILayerException(message, (int)HttpStatusCode.InternalServerError, blEx);
            }
            catch (Exception ex)
            {
                TutorialLogger.LogError(requestUser.UserName, ex.Message, ex.StackTrace);
                throw new APILayerException(ex.Message, (int)HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
