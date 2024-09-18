using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ODataBookStore.EDM;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
	var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
	options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

// Add CORS
builder.Services.AddCors(p =>
	p.AddPolicy(
		"corspolicy",
		build =>
		{
			build
				.AllowAnyOrigin()
				.AllowAnyMethod()
				.AllowAnyHeader();
		}
	)
);

// Config Kernel handles listening on IP addresses
builder.WebHost.ConfigureKestrel(serverOptions =>
{
	//builder.WebHost.ConfigureKestrel(serverOptions =>
	//{
	//	serverOptions.Listen(IPAddress.Any, 7110);
	//});

	serverOptions.Listen(IPAddress.Any, 7110);
});

// Configure OData
builder.Services.AddControllers().AddOData(opt =>
{
	opt.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100)
		.AddRouteComponents("odata", BuildEdmModel());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ODataBookStore.API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ODataBookStore.API v1"));
}
app.UseCors("corspolicy");

app.UseHttpsRedirection();

app.UseODataBatching();


// Test middleware 
app.Use(next => context =>
{
	var endpoint = context.GetEndpoint();
	if (endpoint == null)
	{
		return next(context);
	}

	IODataRoutingMetadata metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
	if (metadata != null)
	{
		IEnumerable<string> templates = metadata.Template.GetTemplates();
	}

	return next(context);
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

// EDM model builder method
IEdmModel BuildEdmModel()
{
	ODataConventionModelBuilder modelBuilder = new ODataConventionModelBuilder();
	modelBuilder.EntitySet<Book>("Books");
	modelBuilder.EntitySet<Press>("Presses");
	return modelBuilder.GetEdmModel();
}