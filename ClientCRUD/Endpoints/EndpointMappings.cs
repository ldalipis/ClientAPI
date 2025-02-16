using ClientCRUD.Abstracts;
using ClientCRUD.Models;
using Microsoft.AspNetCore.Diagnostics;

namespace ClientCRUD.Endpoints;

public static class EndpointMapping
{
    public static void MapEndpoints(this WebApplication app)
    {
        // Endpoint to fetch all records
        app.MapGet("/items", async (IResourceLoader loader) =>
            {
                var items = await loader.GetAllAsync();
                return Results.Ok(items);
            })
            .WithName("GetAllItems")
            .WithTags("Items")
            .WithOpenApi(op =>
            {
                op.Description = "Fetch all items.";
                return op;
            });

        // Endpoint to fetch a single record by id
        app.MapGet("/items/{id}", async (string id, IResourceLoader loader) =>
            {
                var item = await loader.GetByIdAsync(id);
                return Results.Ok(item);
            })
            .WithName("GetItemById")
            .WithTags("Items")
            .WithOpenApi(op =>
            {
                op.Description = "Fetch all item by id.";
                return op;
            });

        // Endpoint to create a new record
        app.MapPost("/items", async (UnifiedRequestModel newItem, IResourceLoader loader) =>
            {
                await loader.AddAsync(newItem);
                return Results.Created($"/items/{newItem.Id}", newItem);
            })
            .WithName("CreateItem")
            .WithTags("Items")
            .WithOpenApi(op =>
            {
                op.Description = "Create a new record.";
                return op;
            });

        // Endpoint to update an existing record
        app.MapPut("/items", async (UnifiedRequestModel updatedItem, IResourceLoader loader) =>
            {
                await loader.UpdateAsync(updatedItem);
                return Results.NoContent();
            })
            .WithName("UpdateItem")
            .WithTags("Items")
            .WithOpenApi(op =>
            {
                op.Description = "Update an existing record.";
                return op;
            });

        // Endpoint to delete a record by id
        app.MapDelete("/items/{id}", async (string id, IResourceLoader loader) =>
            {
                await loader.DeleteAsync(id);
                return Results.NoContent();
            })
            .WithName("DeleteItem")
            .WithTags("Items")
            .WithOpenApi(op =>
            {
                op.Description = "Delete an existing record.";
                return op;
            });

        // Endpoint for unknown errors
        app.Map("/error", (HttpContext context) =>
        {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            return exception is not null
                ? Results.Problem(exception.Message)
                : Results.Problem("An unknown error occurred.");
        });
    }
}
