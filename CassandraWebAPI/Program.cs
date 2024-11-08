using CassandraWebAPI.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register CassandraHelper
var cassandraSettings = builder.Configuration.GetSection("CassandraSettings");
builder.Services.AddSingleton(_ => new CassandraHelper(
    cassandraSettings["ContactPoints"],
    int.Parse(cassandraSettings["Port"]),
    cassandraSettings["Keyspace"]
));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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