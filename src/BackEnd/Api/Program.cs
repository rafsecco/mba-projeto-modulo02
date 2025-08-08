using Api.Configurations;
using Api.Models;
using Business;
using Data;
using Data.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.AddApplicationServices();
builder.AddDataServices();
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = false);
builder.Services.AddControllers();
builder.Services.AdicionaSwaggerConfiguracao(builder.Configuration);

var app = builder.Build();
app.UseDbMigrationHelper();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(x => x
	.AllowAnyHeader()
	.AllowAnyMethod()
	.AllowAnyOrigin()
	.WithExposedHeaders("content-disposition") // Para pegar o nome do arquivos no header
);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
