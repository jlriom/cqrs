using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories;

public class PostRepository : IPostRepository
{
    private  readonly DatabaseContextFactory _contextFactory;
    public PostRepository(DatabaseContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
    }
    public async Task CreateAsync(PostEntity post)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Post.Add(post);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        using var context = _contextFactory.CreateDbContext();
        var post  = await GetByIdAsync(id);

        if(post == null) return;

        context.Post.Remove(post);
        await context.SaveChangesAsync();
    }

    public async Task<List<PostEntity>> GetAllAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(p => p.Comments)
            .ToListAsync();
    }

    public async Task<PostEntity?> GetByIdAsync(Guid postId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(x => x.PostId == postId);

    }

    public async Task<List<PostEntity>> ListByAuthorAsync(string author)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(p => p.Comments).AsNoTracking()
            .Where(p => p.Author == author)
            .Where (x => x.Comments != null && x.Comments.Any())
            .ToListAsync();
    }

    public async Task<List<PostEntity>> ListWithCommentsAsync()
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(p => p.Comments).AsNoTracking()
            .Where (x => x.Comments != null && x.Comments.Any())
            .ToListAsync();    
    }

    public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.Post.AsNoTracking()
            .Include(p => p.Comments).AsNoTracking()
            .Where (x => x.Likes >= numberOfLikes)
            .ToListAsync();    
    }

    public async Task UpdateAsync(PostEntity post)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Post.Update(post);
        await context.SaveChangesAsync();
    }
}
