using MySqlConnector;
using System.Data;
using HelpMeOut.Repository.Repositories;
using HelpMeOut.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IDbConnection>(sp => new MySqlConnection("Server=localhost;Port=3306;Database=HelpMeOut;Uid=mafathi;Pwd=mafathi;"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IAccountRepository, AccountRepository>();
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<ISkillRepository, SkillRepository>();
builder.Services.AddTransient<IHelperSkillRepository, HelperSkillRepository>();
builder.Services.AddTransient<IHelpRequestRepository, HelpRequestRepository>();

builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IAddressesService, AddressesService>();
builder.Services.AddTransient<IAccountsService, AccountsService>();
builder.Services.AddTransient<ISkillsService, SkillsService>();
builder.Services.AddTransient<IHelpRequestsService, HelpRequestsService>();

builder.Services.AddControllers();

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