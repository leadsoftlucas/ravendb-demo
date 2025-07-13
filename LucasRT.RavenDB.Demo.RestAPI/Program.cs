using LeadSoft.Common.Library.EnvUtils;
using LucasRT.RavenDB.Demo.Application.PostgreSQL_Services.Data;
using LucasRT.RavenDB.Demo.Domain;
using LucasRT.RavenDB.Demo.RestAPI.Configurations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;
using System.Text.Json.Serialization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string databaseScope = EnvUtil.Get(EnvConstant.DatabaseScope);

if (databaseScope.Equals("RavenDB"))
    builder.Services.AddRavenDB(builder.Configuration);

if (databaseScope.Equals("PostgreSQL"))
    builder.Services.AddPostgreSQL(builder.Configuration);

builder.Services.AddSingletonServices();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
    options.ReturnHttpNotAcceptable = true;
    options.EnableEndpointRouting = true;
    options.RequireHttpsPermanent = true;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    options.JsonSerializerOptions.IgnoreReadOnlyFields = true;
    options.JsonSerializerOptions.AllowTrailingCommas = true;
    options.JsonSerializerOptions.IncludeFields = true;
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
})
.AddNewtonsoftJson(newtonsoft =>
{
    newtonsoft.SerializerSettings.Converters.Add(new StringEnumConverter());
    newtonsoft.SerializerSettings.ContractResolver = new DefaultContractResolver()
    {
        IgnoreSerializableAttribute = true,
        SerializeCompilerGeneratedMembers = true,
        IgnoreIsSpecifiedMembers = false,
    };
    newtonsoft.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    newtonsoft.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    newtonsoft.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
});
builder.Services.AddResponseCompression();

builder.Services.AddOpenApi();
builder.Services.AddSwagger();

WebApplication app = builder.Build();

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"RavenDB Demo");
    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
    c.DocExpansion(DocExpansion.None);
    c.DefaultModelExpandDepth(2);
    c.DefaultModelRendering(ModelRendering.Example);
    c.DisplayOperationId();
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableTryItOutByDefault();
    c.EnableValidator();
    c.ShowCommonExtensions();
});

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

if (databaseScope.Equals("PostgreSQL"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PostgreSQL>();
    db.Database.Migrate();
}

app.MapControllers();

app.Run();
