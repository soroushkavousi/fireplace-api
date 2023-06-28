﻿using FireplaceApi.Domain.Common;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FireplaceApi.Application.RequestTraces;

public interface IRequestTraceCacheService
{
    public Task AddIpRequestTimeAsync(IPAddress ip, TimeSpan lifeSpan);
    public Task<int> CoutIpRequestTimesAsync(IPAddress ip);
    public Task<ExpirableCollection<ExpirableData>> ListIpRequestTimesAsync(IPAddress ip);
}