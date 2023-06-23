﻿using FireplaceApi.Presentation.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class PostFileInputFormDto : IValidator
{
    [Required, FromForm(Name = "file")]
    public IFormFile FormFile { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {

    }
}
