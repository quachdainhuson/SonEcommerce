﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SonEcommerce.Public.Web.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync() 
        { 
           return View();
        }
    }
}
