namespace HashiCorpVault.API.Models;

public class VaultDTO
{
	public string Name { get; set; } = null!;
	public Dictionary<string, string> Data { get; set; } = null!;
}