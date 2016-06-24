﻿using Abp.AspNetCore.Mvc.Auditing;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetCore.Mvc.ExceptionHandling;
using Abp.AspNetCore.Mvc.Results;
using Abp.AspNetCore.Mvc.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Abp.AspNetCore.Mvc
{
    public static class AbpMvcOptionsExtensions
    {
        public static void AddAbp(this MvcOptions options)
        {
            options.Filters.AddService(typeof(AbpAuthorizationFilter));
            options.Filters.AddService(typeof(AbpAuditActionFilter));
            options.Filters.AddService(typeof(AbpValidationActionFilter));
            options.Filters.AddService(typeof(AbpExceptionFilter));
            options.Filters.AddService(typeof(AbpResultFilter));

            options.OutputFormatters.Add(new JsonOutputFormatter(
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }));
        }
    }
}