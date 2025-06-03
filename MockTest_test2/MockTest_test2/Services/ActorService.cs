using MockTest_test2.Controllers;
using MockTest_test2.Database;

namespace MockTest_test2.Services;

public class ActorService(AppDbContext dbContext) : IActorService
{
    public async Task<IEnumerable<GetAllActorsResponse>> GetAllActors(string? name, string? surname)
    {
        // This method should return all actors from the database.
        // Implementation will depend on the specific requirements and structure of the AppDbContext.
        var actors = await dbContext.Actors.ToListAsync();
        return actors;
    }
}
