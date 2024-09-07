namespace HashiCorpVault.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class VaultController : ControllerBase
{
	private readonly IConfiguration configuration;
	private readonly ILogger<VaultController> logger;
	private readonly VaultClient vaultClient;

	public VaultController(IConfiguration configuration, ILogger<VaultController> logger, VaultClient vaultClient)
	{
		this.configuration = configuration;
		this.logger = logger;
		this.vaultClient = vaultClient;
	}

	[HttpGet("{secretKey}")]
	public IActionResult ReadSecret(string secretKey)
	{
		try
		{
			var mountPath = configuration.GetValue<string>("Vault:MountPath");
			var response = vaultClient.Secrets.KvV2Read(secretKey, mountPath);

			var result = JsonSerializer.Deserialize<object>(response.Data.ToJson())!.ToString();


			return string.IsNullOrEmpty(result) ? NotFound() : Ok(JsonSerializer.Deserialize<object>(result));
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPost]
	public IActionResult WriteSecret([FromBody] VaultDTO request)
	{
		try
		{
			var mountPath = configuration.GetValue<string>("Vault:MountPath");
			var requestData = new KvV2WriteRequest(request.Data);
			vaultClient.Secrets.KvV2Write(request.Name, requestData, mountPath);

			return Ok();
		}
		catch (Exception ex)
		{
			return BadRequest(ex.Message);
		}
	}
}