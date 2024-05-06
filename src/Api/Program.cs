
using Api.Configurations;

var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCorsALLOWALL();

app.UseAuthentication();

app.UseAuthorization();

app.AddStripeSecretKey();

app.MapControllers();

app.MapChatHub();

app.UseGlobalExceptionHandling();

app.Run();
