namespace HashiCorpVault.BusinessLayer.Service;

public class VaultService : IVaultService
{
    private readonly ILogger<VaultService> logger;
    private readonly VaultClient vaultClient;

    public VaultService(ILogger<VaultService> logger, VaultClient vaultClient)
    {
        this.logger = logger;
        this.vaultClient = vaultClient;
    }

    public Results<Ok<object>, NotFound, BadRequest<string>> ReadVaultService(IConfiguration configuration, string secretKey)
    {
        try
        {
            var readValue = vaultClient.Secrets.KvV2Read(secretKey, configuration.GetValue<string>("Vault:MountPath"));

            return string.IsNullOrEmpty(JsonSerializer.Deserialize<object>(readValue.Data.ToJson())!.ToString()) ?
                TypedResults.NotFound() :
                TypedResults.Ok(JsonSerializer.Deserialize<object>(JsonSerializer.Deserialize<object>(readValue.Data.ToJson())!.ToString()!));
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest<string>(ex.Message);
        }
    }

    public Results<Ok, BadRequest<string>> WriteVaultService(IConfiguration configuration, VaultDTO request)
    {
        try
        {
            vaultClient.Secrets.KvV2Write(request.Name, new KvV2WriteRequest(request.Data),
                configuration.GetValue<string>("Vault:MountPath"));

            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest<string>(ex.Message);
        }
    }
}