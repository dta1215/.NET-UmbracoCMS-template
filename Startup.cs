using System.Net.NetworkInformation;
using Umbraco.Cms.Web.Common.PublishedModels;
using UmbracoBoutique.Middlewares;
using UmbracoBoutique.Services;
using UmbracoBoutique.Services.Interfaces;
using UmbracoBoutique.Services.ProductOrderStrategy;

namespace UmbracoBoutique
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="webHostEnvironment">The web hosting environment.</param>
        /// <param name="config">The configuration.</param>
        /// <remarks>
        /// Only a few services are possible to be injected here https://github.com/dotnet/aspnetcore/issues/9337.
        /// </remarks>
        public Startup(IWebHostEnvironment webHostEnvironment, IConfiguration config)
        {
            _env = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <remarks>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940.
        /// </remarks>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddUmbraco(_env, _config)
                .AddBackOffice()
                .AddWebsite()
                .AddComposers()
                .Build();

            services.AddDistributedMemoryCache();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromHours(24 * 7);
                option.Cookie.HttpOnly = true;
                option.Cookie.IsEssential = true;
            });

            services.AddTransient<ICartService, CartService>();

            services.AddTransient<IProductOrderStrategy<SanPham_base>, OrderByNameAscending>();
            services.AddTransient<IProductOrderStrategy<SanPham_base>, OrderByNameDescending>();
            services.AddTransient<IProductOrderStrategy<SanPham_base>, OrderByPriceAscending>();
            services.AddTransient<IProductOrderStrategy<SanPham_base>, OrderByPriceDescending>();
            services.AddTransient<IProductOrderStrategy<SanPham_base>, OrderByDateAscending>();
            services.AddTransient<IProductOrderStrategy<SanPham_base>, OrderByDateDescending>();

            services.AddTransient(typeof(IProductOrderService<>), typeof(ProductOrderService<>));

            services.AddTransient<ICommonService, CommonService>();
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Compress image middleware
            app.UseMiddleware<CompressedImageMiddleware>();

            app.UseStaticFiles();

            app.UseUmbraco()
                .WithMiddleware(u =>
                {
                    u.UseBackOffice();
                    u.UseWebsite();
                })
                .WithEndpoints(u =>
                {
                    u.UseInstallerEndpoints();
                    u.UseBackOfficeEndpoints();
                    u.UseWebsiteEndpoints();
                });

        }
    }
}
