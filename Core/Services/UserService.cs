using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Models.UserInformations;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Operators;
using System.Net;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly UserValidator _userValidator;
        private readonly UserOperator _userOperator;
        
        public UserService(ILogger<UserService> logger, UserValidator userValidator, UserOperator userOperator)
        {
            _logger = logger;
            _userValidator = userValidator;
            _userOperator = userOperator;
        }

        public async Task<User> LogInWithGoogleAsync(IPAddress ipAddress, string state, 
            string code, string scope, string authUser, string prompt, string error)
        {
            await _userValidator.ValidateLogInWithGoogleInputParametersAsync(ipAddress, state, code, scope, authUser, prompt, error);
            return await _userOperator.LogInWithGoogleAsync(ipAddress, state, code, scope, authUser, prompt);
        }

        public async Task<User> SignUpWithEmailAsync(IPAddress ipAddress, string firstName, 
            string lastName, string username, Password password, string emailAddress)
        {
            await _userValidator.ValidateSignUpWithEmailInputParametersAsync(ipAddress, firstName, lastName,
                username, password, emailAddress);
            return await _userOperator.SignUpWithEmailAsync(ipAddress, firstName, lastName,
                username, password, emailAddress);
            
        }

        public async Task<string> GetGoogleAuthUrlAsync(IPAddress ipAddress)
        {
            return await _userOperator.GetGoogleAuthUrlAsync(ipAddress);
        }

        public async Task<User> LogInWithEmailAsync(IPAddress ipAddress, string emailAddress, Password password)
        {
            await _userValidator.ValidateLogInWithEmailInputParametersAsync(ipAddress, emailAddress, password);
            var user = await _userOperator.LogInWithEmailAsync(ipAddress, emailAddress, password);
            return user;
        }

        public async Task<User> LogInWithUsernameAsync(IPAddress ipAddress, string username, Password password)
        {
            await _userValidator.ValidateLogInWithUsernameInputParametersAsync(ipAddress, username, password);
            var user = await _userOperator.LogInWithUsernameAsync(ipAddress, username, password);
            return user;
        }

        public async Task<List<User>> ListUsersAsync(User requesterUser, bool? includeEmail,
            bool? includeSessions)
        {
            await _userValidator.ValidateListUsersInputParametersAsync(requesterUser, includeEmail, includeSessions);
            var user = await _userOperator.ListUsersAsync(includeEmail.Value, false, false, includeSessions.Value);
            return user;
        }

        public async Task<User> GetUserByIdAsync(User requesterUser, long? id, bool? includeEmail, bool? includeSessions)
        {
            await _userValidator.ValidateGetUserByIdInputParametersAsync(requesterUser, id, includeEmail, includeSessions);
            var user = await _userOperator.GetUserByIdAsync(id.Value, includeEmail.Value, false, false, includeSessions.Value);
            return user;
        }

        public async Task<User> PatchUserByIdAsync(User requesterUser, long? id, string firstName,
            string lastName, string username, Password oldPassword, Password password, string emailAddress)
        {
            await _userValidator.ValidatePatchUserInputParametersAsync(requesterUser, id, firstName,
                lastName, username, oldPassword, password, emailAddress);
            var user = await _userOperator.PatchUserByIdAsync(id.Value, firstName: firstName, lastName: lastName,
                username: username, password: password, emailAddress: emailAddress);
            return user;
        }

        public async Task DeleteUserAsync(User requesterUser, long? id)
        {
            await _userValidator.ValidateDeleteProcutInputParametersAsync(requesterUser, id);
            await _userOperator.DeleteUserAsync(id.Value);
        }
    }
}
