﻿using System;
using System.Net;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Logging;
using Abp.Reflection;
using Abp.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Abp.AspNetCore.Mvc.ExceptionHandling
{
    public class AbpExceptionFilter : IExceptionFilter, ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IErrorInfoBuilder _errorInfoBuilder;

        public AbpExceptionFilter(IErrorInfoBuilder errorInfoBuilder)
        {
            _errorInfoBuilder = errorInfoBuilder;
            Logger = NullLogger.Instance;
        }

        public void OnException(ExceptionContext context)
        {
            var wrapResultAttribute =
                ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<WrapResultAttribute>(
                    context.ActionDescriptor.GetMethodInfo(),
                    WrapResultAttribute.Default
                );

            if (wrapResultAttribute.LogError)
            {
                LogHelper.LogException(Logger, context.Exception);
            }

            if (wrapResultAttribute.WrapOnError)
            {
                HandleAndWrapException(context);
            }
        }

        private void HandleAndWrapException(ExceptionContext context)
        {
            if (!IsObjectResult(context.ActionDescriptor.GetMethodInfo().ReturnType))
            {
                return;
            }
            
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(
                new AjaxResponse(
                    _errorInfoBuilder.BuildForException(context.Exception),
                    context.Exception is AbpAuthorizationException
                )
            );

            context.Exception = null; //Handled!
        }

        private static bool IsObjectResult(Type returnType)
        {
            if (typeof(IActionResult).IsAssignableFrom(returnType))
            {
                if (typeof(JsonResult).IsAssignableFrom(returnType) ||
                    typeof(ObjectResult).IsAssignableFrom(returnType))
                {
                    return true;
                }

                return false;
            }

            return true;
        }
    }
}