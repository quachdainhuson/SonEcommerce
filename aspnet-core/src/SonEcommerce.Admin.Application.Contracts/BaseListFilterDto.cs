﻿using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Admin
{
    public class BaseListFilterDto : PagedResultRequestDto
    {
        public string ?Keyword { get; set; }
    }
}
