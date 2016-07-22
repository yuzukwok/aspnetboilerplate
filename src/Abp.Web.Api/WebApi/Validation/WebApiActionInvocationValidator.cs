﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http.Controllers;
using Abp.Collections.Extensions;
using Abp.Runtime.Validation.Interception;

namespace Abp.WebApi.Validation
{
    public class WebApiActionInvocationValidator : MethodInvocationValidator
    {
        protected HttpActionContext ActionContext { get; private set; }
        
        public void Initialize(HttpActionContext actionContext, MethodInfo methodInfo)
        {
            base.Initialize(
                methodInfo,
                GetParameterValues(actionContext, methodInfo)
            );

            ActionContext = actionContext;
        }

        protected override void SetDataAnnotationAttributeErrors(object validatingObject)
        {
            var modelState = ActionContext.ModelState;
            if (modelState.IsValid)
            {
                return;
            }

            foreach (var state in modelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    ValidationErrors.Add(new ValidationResult(error.ErrorMessage, new[] { state.Key }));
                }
            }
        }

        protected virtual object[] GetParameterValues(HttpActionContext actionContext, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = actionContext.ActionArguments.GetOrDefault(parameters[i].Name);
            }

            return parameterValues;
        }
    }
}