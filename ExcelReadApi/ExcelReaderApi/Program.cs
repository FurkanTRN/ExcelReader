using ExcelReadApi.Extension;
using ExcelReadApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddDbContext();
builder.Services.AddScoped();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddSwagger();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();