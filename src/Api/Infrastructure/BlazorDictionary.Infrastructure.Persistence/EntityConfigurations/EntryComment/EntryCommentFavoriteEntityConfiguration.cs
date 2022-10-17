using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorDictionary.Infrastructure.Persistence.Context.EntityConfigurations.EntryComment
{
    public class EntryCommentFavoriteEntityConfiguration : BaseEntityConfiguration<Api.Domain.Models.EntryCommentFavorite>
    {
        public override void Configure(EntityTypeBuilder<Api.Domain.Models.EntryCommentFavorite> builder)
        {
            base.Configure(builder);

            builder.ToTable("entrycommentfavorite", BlazorDictionaryContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.CreatedUser)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.EntryComment)
                .WithMany(i => i.EntryCommentFavorites)
                .HasForeignKey(i => i.EntryCommentId);
        }
    }
}
