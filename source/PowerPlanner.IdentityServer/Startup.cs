using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityServer.Identity;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://account.powerplanner.net/";
#if DEBUG
                    options.RequireHttpsMetadata = false;
#endif
                    options.Audience = "identityServer";
                });

            var identityBuilder = services.AddIdentityServer()
                .AddInMemoryClients(Clients.GetClients())
                .AddPersistedGrantStore<PersistedGrantStore>()
                .AddProfileService<UserProfileService>()
                .AddInMemoryApiResources(Identity.Resources.GetApiResources())
                .AddInMemoryIdentityResources(Identity.Resources.GetIdentityResources())
                .AddResourceOwnerValidator<Identity.ResourceOwnerPasswordValidator>();

            var certResult = IdentityServerCertificate.GetAsync();
            certResult.Wait();

            if (certResult.Exception != null)
            {
                throw certResult.Exception;
            }

            identityBuilder.AddSigningCredential(certResult.Result);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Needed to enable the identity server
            app.UseIdentityServer();

            // IMPORTANT: Order matters. Authentication must be before MVC.
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
