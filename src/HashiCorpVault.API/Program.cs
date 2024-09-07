namespace HashiCorpVault.API;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

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
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.MapControllers();
		app.Run();
	}
}