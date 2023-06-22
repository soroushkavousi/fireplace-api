using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Application.Models;
using FireplaceApi.Application.Tools;
using FireplaceApi.Application.ValueObjects;

namespace FireplaceApi.Presentation.Converters;

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
