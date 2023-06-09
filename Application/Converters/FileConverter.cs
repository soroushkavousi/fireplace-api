using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Domain.ValueObjects;

namespace FireplaceApi.Application.Converters;

public static class FileConverter
{
    public static FileDto ToDto(this File file)
    {
        if (file == null)
            return null;

        var fileDto = new FileDto(file.Id.IdEncode(), file.Uri.AbsoluteUri, file.CreationDate);

        return fileDto;
    }

    public static QueryResultDto<FileDto> ToDto(this QueryResult<File> queryResult)
        => queryResult.ToDto(ToDto);
}
