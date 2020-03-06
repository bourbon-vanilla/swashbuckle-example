# swashbuckle-example

This is ASP NET Core 3.1 application.

The address to get the swagger specification:<br>
https://localhost:44396/swagger/open-api-spec/swagger.json

The address to get the ui:<br>
https://localhost:44396/index.html

### Documentation
In order to activate the documentation of the API you have to activate the xml-comments in the project.<br>
Next add the code to the `ConfigureServices`-method in Startup-class:

```csharp
    services.AddSwaggerGen(setupAction =>
    {
        setupAction.SwaggerDoc(OPEN_API_SPECIFICATION_NAME,
            new Microsoft.OpenApi.Models.OpenApiInfo()
            {
                Title = "Library API",
                Version = "1"
            });

        var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
        setupAction.IncludeXmlComments(xmlCommentsFullPath);
    });
```

In order to get return value comments you have to add xml-comments to the DTO-types, which will be returned.

You can add data annotations in order to extend the documentation:
```csharp
    [Required]
    [MaxLength(150)]
    public string Summary { get; set; }
```