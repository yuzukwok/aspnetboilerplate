﻿using Abp.Domain.Entities;

namespace Abp.EntityFrameworkCore.Tests.Domain
{
    public class Blog : Entity
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}