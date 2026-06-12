using SelfServicePortal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using SelfServicePortal;
using SelfServicePortal.Components.Pages.Auth;
using SelfServicePortal.Data;
using SelfServicePortal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Razor & Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add DbContext
builder.Services.AddDbContextFactory<AppDbContext>(options =>


    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,            // Retry up to 5 times
            maxRetryDelay: TimeSpan.FromSeconds(30), // Wait up to 30 seconds between retries
            errorNumbersToAdd: null      // Look for standard SQL transient error codes
        )
    ));


// Add MudBlazor
builder.Services.AddMudServices();

// Add Auth
builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddScoped<AuthenticationStateProvider,
    CustomAuthStateProvider>();
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options => {
        options.LoginPath = "/auth/login";
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<INotificationService,
    NotificationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<SelfServicePortal.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();