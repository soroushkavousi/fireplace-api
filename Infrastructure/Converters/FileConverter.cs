using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using System;

namespace FireplaceApi.Infrastructure.Converters;

public static class FileConverter
{
    private static readonly Uri _baseUri = new(Configs.Current.File.BaseUrlPath);
    private static readonly string _basePhysicalPath = Configs.Current.File.BasePhysicalPath;

    public static FileEntity ToEntity(this File file)
    {
        if (file == null)
            return null;

        var relativeUri = file.Uri.ToRelativeUri().ToString();
        var relativePhysicalPath = file.PhysicalPath.ToRelativePhysicalPath();

        var fileEntity = new FileEntity(file.Id, file.Name, file.RealName,
            relativeUri, relativePhysicalPath, file.CreationDate,
            file.ModifiedDate);

        return fileEntity;
    }

    public static File ToModel(this FileEntity fileEntity)
    {
        if (fileEntity == null)
            return null;

        var uri = new Uri(fileEntity.RelativeUri, UriKind.Relative).ToAbsoluteUri();
        var physicalPath = fileEntity.RelativePhysicalPath.ToAbsolutePhysicalPath();

        var file = new File(fileEntity.Id, fileEntity.Name, fileEntity.RealName,
            uri, physicalPath, fileEntity.CreationDate, fileEntity.ModifiedDate);

        return file;
    }

    public static Uri ToRelativeUri(this Uri absoluteUri)
    {
        var relativeUri = _baseUri.MakeRelativeUri(absoluteUri);
        return relativeUri;
    }

    public static Uri ToAbsoluteUri(this Uri relativeUri)
    {
        var absoluteUri = new Uri(_baseUri, relativeUri);
        return absoluteUri;
    }

    public static string ToRelativePhysicalPath(this string absolutePhysicalPath)
    {
        var relativePhysicalPath = System.IO.Path.GetRelativePath(_basePhysicalPath, absolutePhysicalPath);
        return relativePhysicalPath;
    }

    public static string ToAbsolutePhysicalPath(this string relativePhysicalPath)
    {
        var absolutePhysicalPath = System.IO.Path.GetFullPath(relativePhysicalPath, _basePhysicalPath);
        return absolutePhysicalPath;
    }
}
