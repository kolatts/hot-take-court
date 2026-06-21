using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using Verdict.Functions.Domain;

namespace Verdict.Functions.Functions;

public class AddReaction(GameService game)
{
    private record Request(string PlayerGuid, int Round, string ArgAuthorGuid, string Emoji);

    [Function("AddReaction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "rooms/{code}/reactions")] HttpRequest req,
        string code)
    {
        Request? body;
        try { body = await JsonSerializer.DeserializeAsync<Request>(req.Body, JsonOptions.Web); }
        catch { return new BadRequestObjectResult(new { error = "Invalid request body." }); }

        if (body is null || string.IsNullOrWhiteSpace(body.PlayerGuid))
            return new BadRequestObjectResult(new { error = "playerGuid is required." });

        try
        {
            await game.AddReactionAsync(
                code.ToUpperInvariant(), body.PlayerGuid, body.Round, body.ArgAuthorGuid, body.Emoji);
            return new OkObjectResult(new { ok = true });
        }
        catch (GameException ex)
        {
            return new ObjectResult(new { error = ex.Message }) { StatusCode = ex.StatusCode };
        }
    }
}
