using Productos.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 👇 Agregar HttpClient para consumir la API
builder.Services.AddHttpClient("Api", client =>
{
    //client.BaseAddress = new Uri("https://localhost:7076/");
    //client.BaseAddress = new Uri("https://localhost:7076/");
    client.BaseAddress = new Uri("http://localhost:5116/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();