using FireplaceApi.Infrastructure.Entities;
using IdGen;
using System;

namespace FireplaceApi.Infrastructure.Tools;

public partial class ApiIdGenerator : IIdGenerator
{
    public static readonly byte EpochLength = 43;
    public static readonly byte ServerIdLength = 7; //only 127 server instances are allowed
    public static readonly byte SequenceLength = 13; //only 16,383 ids per millisecond
    private readonly IdGenerator _idGenerator;

    public ApiIdGenerator()
    {
        var options = CreateOptions();
        var serverId = Convert.ToInt32(ServerEntity.Current.Id);
        _idGenerator = new IdGenerator(serverId, options);
    }

    private IdGeneratorOptions CreateOptions()
    {
        var structure = new IdStructure(EpochLength, ServerIdLength, SequenceLength);
        var options = new IdGeneratorOptions(structure);
        return options;
    }

    public ulong GenerateNewId()
    {
        ulong id; bool idIsValid;
        do
        {
            id = (ulong)_idGenerator.CreateId();
            idIsValid = IsIdValid(id);
        }
        while (!idIsValid);
        return id;
    }

    private static bool IsIdValid(ulong id)
    {
        // To filter 10-length encoded IDs
        if (id % 256 < 6)
            return false;

        try
        {
            var encodedId = new EncodedId(id);
        }
        catch { return false; }

        return true;
    }
}
