namespace MessMate.Api.Extensions
{
    public static class CorsExtensions
    {
        public  static IServiceCollection AddCorsPolicy(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy.WithOrigins(
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "http://localhost:5757",
                            "https://localhost:5757"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            return services;
        }
    }
}
