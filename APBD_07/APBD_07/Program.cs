using DotNetEnv;
using APBD_07.Services;    
using APBD_07.Services;    

Env.Load();                
var builder = WebApplication.CreateBuilder(args);

// Register your application services
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITripService,   TripService>();

// Add framework services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();