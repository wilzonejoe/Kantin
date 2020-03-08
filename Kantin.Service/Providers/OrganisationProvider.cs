﻿using Core.Providers;
using Kantin.Data;
using Kantin.Data.Models;
using Kantin.Service.Models;
using System;

namespace Kantin.Service.Providers
{
    public class OrganisationProvider : GenericProvider<Organisation, KantinEntities>
    {
        public OrganisationProvider(KantinEntities context) : base(context) { }
    }
}