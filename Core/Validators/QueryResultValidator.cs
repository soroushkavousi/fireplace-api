using FireplaceApi.Core.Operators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Core.Validators
{
    public class QueryResultValidator : ApiValidator
    {
        private readonly ILogger<QueryResultValidator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly QueryResultOperator _queryResultOperator;

        public QueryResultValidator(ILogger<QueryResultValidator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, QueryResultOperator queryResultOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _queryResultOperator = queryResultOperator;
        }
    }
}
