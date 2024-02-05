using ChatRoom.API.DAL;
using ChatRoom.API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ChatRoomDb>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyMethod();
    policy.AllowAnyHeader();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//User CRUD
app.MapGet("/user", async (ChatRoomDb db) =>
{
    return await db.Users.ToListAsync();
});

app.MapGet("/user/{username}", async (string username, ChatRoomDb db) =>
    await db.Users.FirstOrDefaultAsync(x => x.Username == username)
        is User user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/user", async (User user, ChatRoomDb db) =>
{
    db.Users.Add(user);
    await db.SaveChangesAsync();

    return Results.Created($"/user/{user.Id}", user);
});

app.MapDelete("/user/{id}", async (int id, ChatRoomDb db) =>
{
    if (await db.Users.FindAsync(id) is User user)
    {
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

//Chat CRUD

app.MapGet("/chat/{participantId}", async (int participantId, ChatRoomDb db) =>
{
    var participant = await db.Users.FirstOrDefaultAsync(p => p.Id == participantId);
    if (participant != null)
    {
        var chatIds = await db.UsersChats
            .Where(c => c.UserId == participantId)
            .Select(c => c.ChatId)
            .ToListAsync();

        var chats = await db.Chats
            .Include(c => c.Participants)
            .Include(c => c.Messages)
            .Where(c => chatIds.Contains(c.Id))
            .ToListAsync();

        if (chats.Any())
        {
            return Results.Ok(chats);
        }
        return Results.NotFound();
    }

    return Results.NotFound();
});


app.MapPost("/chat/{userIds}", async (string userIds, ChatRoomDb db) =>
{
    var participantIds = userIds.Split(',').Select(int.Parse).ToList();

    var participants = await db.Users
        .Where(u => participantIds.Contains(u.Id))
        .ToListAsync();

    if (participants.Count != participantIds.Count)
    {
        return Results.NotFound("One or more participants not found");
    }

    var chat = new Chat
    {
        Participants = participants.Select(u => new UserChat { UserId = u.Id, User = u }).ToList(),
        Messages = new List<Message>()
    };

    db.Chats.Add(chat);
    await db.SaveChangesAsync();

    return Results.Created($"/chat/{chat.Id}", chat);
});


app.MapDelete("/chat/{id}", async (int id, ChatRoomDb db) =>
{
    if (await db.Chats.FindAsync(id) is Chat chat)
    {
        db.Chats.Remove(chat); 
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

//Message CRUD
app.MapGet("/message/{chatId}", async (int chatId, ChatRoomDb db) =>
{
    return await db.Messages.Where(m => m.Id == chatId).ToListAsync();
});

app.MapPost("/message", async (Message message, ChatRoomDb db) =>
{
    db.Messages.Add(message);
    await db.SaveChangesAsync();

    return Results.Created($"/message/{message.Id}", message);
});

app.MapDelete("/message/{id}", async (int id, ChatRoomDb db) =>
{
    if (await db.Messages.FindAsync(id) is Message message)
    {
        db.Messages.Remove(message);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
