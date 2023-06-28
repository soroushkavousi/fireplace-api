using FireplaceApi.Domain.Errors;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Repositories;
using FireplaceApi.Infrastructure.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.HostedServices;

public class ServerInstanceHandlerHostedService : IHostedService
{
    private readonly ILogger<ServerInstanceHandlerHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private static readonly Random _random = new();

    public ServerInstanceHandlerHostedService(ILogger<ServerInstanceHandlerHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await CreateNewServerInstance(cancellationToken);
    }

    private async Task CreateNewServerInstance(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var serverRepository = scope.ServiceProvider
            .GetRequiredService<IServerRepository>();

        var serverMacAddress = GetMacAddress();

        var serverIds = await serverRepository.ListServerIdsAsync();
        ServerEntity server = null;
        server = await serverRepository.GetServerAsync(ServerIdentifier.OfMacAddress(serverMacAddress));
        if (server != null)
        {
            ServerEntity.Current = server;
            return;
        }
        while (true)
        {
            var newServerId = GenerateRandomServerIds(serverIds);
            try
            {
                server = await serverRepository.CreateServerAsync(newServerId, serverMacAddress);
            }
            catch (DbUpdateException ex)
            {
                if (!ex.Message.Contains("duplicate"))
                    throw new InternalServerException("Can not store server instance to database!", systemException: ex);
            }
            ServerEntity.Current = server;
            return;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        var serverRepository = scope.ServiceProvider
            .GetRequiredService<IServerRepository>();

        await serverRepository.DeleteServerAsync(ServerEntity.Current.Id);
    }

    private static ulong GenerateRandomServerIds(List<ulong> serverIds)
    {
        ulong randomServerId;
        while (true)
        {
            randomServerId = (ulong)_random.Next(0, (int)Math.Pow(2, ApiIdGenerator.ServerIdLength));
            if (!serverIds.Contains(randomServerId))
                return randomServerId;
        }
    }

    private static string GetMacAddress()
    {
        var firstMacAddress = NetworkInterface
            .GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .FirstOrDefault();
        return firstMacAddress;
    }
}
