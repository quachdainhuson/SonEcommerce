using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace SonEcommerce.Public
{
    public class BaseListFilterDto : PagedResultRequestBase
    {
        public string ?Keyword { get; set; }
    }
}
