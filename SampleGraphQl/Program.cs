using Microsoft.EntityFrameworkCore;
using SampleGraphQl.Data;
using SampleGraphQl.GraphQL;
using SampleGraphQl.GraphQL.Users;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ApplicationDb"));

builder.Services.AddGraphQLServer()
               .AddQueryType<SampleGraphQl.GraphQL.Query>()
               .AddMutationType<Mutation>()
               //.AddType<ArticleType>()
               .AddType<UserType>()
               .AddProjections()
               .AddFiltering()
               .AddSorting();

var app = builder.Build();
//Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    SeedData.Initialize(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGraphQL();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGraphQL();
//});

app.UseGraphQLVoyager("/graphql-voyager");

app.Run();
