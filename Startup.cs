using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using AutoMapper;
//using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Hackathon;

namespace Hackathon
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        //METODO CONSTRUTOR DA CLASSE STARTUP
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // ADICIONA OS CONTROLLERS E VIEWS.
            services.AddControllersWithViews()
                //SUPORTE AO RAZORRUNTIMECOMPILATION
                .AddRazorRuntimeCompilation();
            //ADICIONA RAZOR PAGES
            services.AddRazorPages()
                //SUPORTE AO RAZORRUNTIMECOMPILATION
                .AddRazorRuntimeCompilation();

            //services.AddAutoMapper(typeof(EntitiesToDTOMappingProfile));

            //Adicionado
            //services.AddHttpClient();

            //services.AddInfraestructure(Configuration);

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});

            //services.AddAuthentication().AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
            //    googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
            //});

            services.AddMvc();
        }

        public void Configure(WebApplication app, IWebHostEnvironment enviroment)
        {
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Privacy");
                app.UseHsts();
            }

            //app.Use(async (context, next) =>
            //{
            //    string? token = context.Request.Cookies["NomedoCookie"];
            //    string encryptionKey = "SuaChaveDeCriptografiattttttttttttttttttttttttt";

            //    if (!string.IsNullOrEmpty(token) && !context.Request.Headers.ContainsKey("Authorization"))
            //    {
            //        //GerarKeyService gerarchave = context.RequestServices.GetRequiredService<GerarKeyService>();
                    
            //        byte[] salt = Encoding.UTF8.GetBytes("valor_de_sal_fixo");

            //        //Chave de descriptografia em byte
            //        byte[] aesKeyBytes = gerarchave.GenerateAESKey(encryptionKey, salt, 128);
            //        //Chave de descriptografia em string
            //        string aesKey = BitConverter.ToString(aesKeyBytes).Replace("-", "");

            //        DescriptografarService descriptografarService = context.RequestServices.GetRequiredService<DescriptografarService>();
            //        string tokenDescriptografado = descriptografarService.Decrypt(token, aesKey);

            //        context.Request.Headers.Add("Authorization", "Bearer " + tokenDescriptografado);
            //    }

            //    await next();
            //});

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}