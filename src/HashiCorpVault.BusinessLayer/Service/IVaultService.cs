namespace HashiCorpVault.BusinessLayer.Service;

public interface IVaultService
{
	Results<Ok<object>, NotFound, BadRequest<string>> ReadVaultService(IConfiguration configuration, string secretKey);
	Results<Ok, BadRequest<string>> WriteVaultService(IConfiguration configuration, VaultDTO request);
}
