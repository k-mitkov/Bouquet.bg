using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace Bouquet.Api.Extensions
{
    public static class ApplicationExtension
    {
        #region

        /// <summary>
        /// Configuration of hangfire
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void ConfigureHangFire(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseHangfireDashboard("/jobs",
                                 new DashboardOptions
                                 {
                                     //AppPath = configuration.GetSection("AppConfiguration:ApplicationUrl").Value,
                                     DashboardTitle = "Electric Stations Jobs",
                                     Authorization = new[]
                                     {
                                         new HangfireCustomBasicAuthenticationFilter
                                         {
                                             User = configuration.GetSection("SecuritySettings:UserName")
                                                 .Value,
                                             Pass = configuration.GetSection("SecuritySettings:Password")
                                                 .Value,
                                         },
                                     },
                                 });

        }

        #endregion

    }
}
