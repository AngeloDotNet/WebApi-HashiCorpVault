namespace HashiCorpVault.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        // Lowercase URLs - First solution
        //builder.Services.AddRouting(options => options.LowercaseUrls = true);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<IVaultService, VaultService>();
        builder.Services.AddSingleton((serviceProvider) =>
        {
            var vaultAddress = builder.Configuration["Vault:Address"];
            var config = new VaultConfiguration(vaultAddress);

            var vaultClient = new VaultClient(config);
            vaultClient.SetToken(builder.Configuration["Vault:Token"]);

            return vaultClient;
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(options =>
            {
                // Lowercase URLs - Second solution
                options.PreSerializeFilters.Add((document, request) =>
                {
                    var paths = document.Paths.ToDictionary(item => item.Key.ToLowerInvariant(), item => item.Value);
                    document.Paths.Clear();

                    foreach (var pathItem in paths)
                    {
                        document.Paths.Add(pathItem.Key, pathItem.Value);
                    }
                });
            });
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.Run();
    }
}