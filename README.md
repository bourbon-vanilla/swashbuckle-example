# swashbuckle-example

This is ASP NET Core 3.1 application.

The address to get the swagger specification:<br>
https://localhost:44396/swagger/open-api-spec/swagger.json

The address to get the ui (is set as default):<br>
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

### Description of possible responses

Add attributes and comments to extend the generated open API specification by possible responses:

```csharp
        /// <summary>
        /// Get a book by id for a specific author.
        /// </summary>
        /// <param name="authorId">The id of the book author.</param>
        /// <param name="bookId">The id of the book.</param>
        /// <returns>An ActionResult of type Book.</returns>
        <b>/// <response code="200">Returns the requested book.</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]</b>
        [HttpGet("{bookId}")]
        public async Task<ActionResult<Book>> GetBook(
    ...
```

If you know, that every method in your controller produces a specific response, you can add the information at controller level:

```csharp
    [Route("api/authors/{authorId}/books")]
    [ApiController]
    <b>[ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]</b>
    public class BooksController : ControllerBase
    {
        ...
```

If the response types apply even for all methods in all controllers you can define it at project level in `ConfigureServices`-method:

```csharp
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                <b>setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));</b>
            });
        ...
```