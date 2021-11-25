using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class GlobalOperator
    {
        private readonly ILogger<GlobalOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IGlobalRepository _globalRepository;
        private readonly IGoogleGateway _googleGateway;
        public static GlobalValues GlobalValues { get; set; }

        public GlobalOperator(ILogger<GlobalOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, IGlobalRepository globalRepository,
            IGoogleGateway googleGateway)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _globalRepository = globalRepository;
            _googleGateway = googleGateway;
        }

        public async Task<List<Global>> ListGlobalsAsync()
        {
            var globals = await _globalRepository.ListGlobalsAsync();
            return globals;
        }

        public async Task<Global> GetGlobalByIdAsync(ulong id)
        {
            var global = await _globalRepository.GetGlobalByIdAsync(id);

            if (global == null)
                return global;

            return global;
        }

        public async Task<Global> CreateGlobalAsync(ulong id, EnvironmentName environmentName,
            GlobalValues globalValues)
        {
            var global = await _globalRepository.CreateGlobalAsync(id, environmentName, globalValues);
            return global;
        }

        public async Task DeleteGlobalAsync(ulong id)
        {
            await _globalRepository.DeleteGlobalAsync(id);
        }

        public async Task<bool> DoesGlobalIdExistAsync(ulong id)
        {
            var globalIdExists = await _globalRepository.DoesGlobalIdExistAsync(id);
            return globalIdExists;
        }
    }
}
