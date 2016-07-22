﻿using Abp.Domain.Uow;
using Abp.Web.Models;

namespace Abp.Web.Mvc.Configuration
{
    public interface IAbpMvcConfiguration
    {
        UnitOfWorkAttribute DefaultUnitOfWorkAttribute { get; }

        WrapResultAttribute DefaultWrapResultAttribute { get; }
    }
}
