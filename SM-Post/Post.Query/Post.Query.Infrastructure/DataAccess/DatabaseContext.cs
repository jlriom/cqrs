using System;
using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions options): base(options) 
    {

    }

    public DbSet<PostEntity> Post {get; set;}
    public DbSet<CommentEntity> Comments {get; set;}
}
