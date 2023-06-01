using FireplaceApi.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Application.Controllers;

public class PostFileInputFormParameters : IValidator
{
    [Required, FromForm(Name = "file")]
    public IFormFile FormFile { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {

    }
}
