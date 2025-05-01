using Microsoft.EntityFrameworkCore;
using TestProject.Business;
using TestProject.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TaskContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        }));
builder.Services.AddControllers();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();