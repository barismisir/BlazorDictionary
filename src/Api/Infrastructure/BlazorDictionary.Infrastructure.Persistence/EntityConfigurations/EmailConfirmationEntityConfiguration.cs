﻿using BlazorDictionary.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Infrastructure.Persistence.Context.EntityConfigurations
{
    public class EmailConfirmationEntityConfiguration : BaseEntityConfiguration <EmailConfirmation>
    {
        public override void Configure(EntityTypeBuilder<EmailConfirmation> builder)
        {
            base.Configure(builder);

            builder.ToTable("emailconfirmation", BlazorDictionaryContext.DEFAULT_SCHEMA);
        }
    }
}
