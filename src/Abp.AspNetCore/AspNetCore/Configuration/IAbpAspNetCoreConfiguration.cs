﻿using System.Reflection;
using Abp.Domain.Uow;
using Abp.Web.Models;

namespace Abp.AspNetCore.Configuration
{
    public interface IAbpAspNetCoreConfiguration
    {
        WrapResultAttribute DefaultWrapResultAttribute { get; }

        UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        void CreateControllersForAppServices(Assembly assembly, string moduleName = AbpServiceControllerSetting.DefaultServiceModuleName, bool useConventionalHttpVerbs = true);
    }
}
