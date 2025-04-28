using AinAlfahd.Authontocation;
using AinAlfahd.BL;
using AinAlfahd.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuestPDF.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache(); // لتخزين الجلسة في الذاكرة
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // مدة انتهاء الجلسة
    options.Cookie.HttpOnly = true; // الكوكي لا يمكن الوصول لها عبر JavaScript
    options.Cookie.IsEssential = true; // الكوكي ضرورية (للموافقة حسب GDPR)
    options.Cookie.Name = ".MyApp.Session"; // اسم الكوكي
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // ارسل فقط عبر HTTPS
    options.Cookie.SameSite = SameSiteMode.Strict; // سياسة SameSite
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "A sample ASP.NET Core API"
    });

    // هنا نضيف السيكيورتي
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "ضع التوكن هنا مباشرة مع كلمة Bearer مثال: Bearer eyJhbGciOiJIUzI1..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {} // لا تحتاج صلاحيات إضافية هنا
        }
    });
});


builder.Services.AddScoped<CreateJWT>();

builder.Services.AddDbContext<MasterDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FirstConnect")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<MasterDBContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false; 
    options.Password.RequiredLength = 3;  
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false; 

    options.Lockout.MaxFailedAccessAttempts = 5; 
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15); 
    options.Lockout.AllowedForNewUsers = true; 

    options.User.RequireUniqueEmail = true; 

});


builder.Services.AddHttpClient();

builder.Services.AddScoped<ICustomer, CutomerRepo> ();
builder.Services.AddScoped<ICustomerServices, CustomerServices> ();
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

QuestPDF.Settings.License = LicenseType.Community;

var configuration = builder.Configuration;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("AllowAll");
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "admin",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=LogIn}/{id?}");


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.Run();
