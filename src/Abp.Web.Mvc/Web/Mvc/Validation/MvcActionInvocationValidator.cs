﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Runtime.Validation.Interception;

namespace Abp.Web.Mvc.Validation
{
    public class MvcActionInvocationValidator : MethodInvocationValidator
    {
        protected ActionExecutingContext ActionContext { get; private set; }
        
        public void Initialize(ActionExecutingContext actionContext, MethodInfo methodInfo)
        {
            base.Initialize(
                methodInfo,
                GetParameterValues(actionContext, methodInfo)
            );

            ActionContext = actionContext;
        }

        protected override void SetDataAnnotationAttributeErrors(object validatingObject)
        {
            var modelState = ActionContext.Controller.As<Controller>().ModelState;
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

        protected virtual object[] GetParameterValues(ActionExecutingContext actionContext, MethodInfo methodInfo)
        {
            var parameters = methodInfo.GetParameters();
            var parameterValues = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                parameterValues[i] = actionContext.ActionParameters.GetOrDefault(parameters[i].Name);
            }

            return parameterValues;
        }
    }
}