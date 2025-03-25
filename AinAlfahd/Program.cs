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
builder.Services.AddSession();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "A sample ASP.NET Core API"
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
    options.Password.RequireDigit = false; // يجب أن تحتوي على رقم
    options.Password.RequiredLength = 3;  // الحد الأدنى للطول
    options.Password.RequireNonAlphanumeric = false; // يجب أن تحتوي على رمز خاص
    options.Password.RequireUppercase = false; // يجب أن تحتوي على حرف كبير
    options.Password.RequireLowercase = false; // يجب أن تحتوي على حرف صغير

    options.Lockout.MaxFailedAccessAttempts = 5; // عدد المحاولات الفاشلة قبل القفل
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15); // مدة القفل
    options.Lockout.AllowedForNewUsers = true; // تفعيل القفل للمستخدمين الجدد

    options.User.RequireUniqueEmail = true; // منع تكرار البريد الإلكتروني

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
