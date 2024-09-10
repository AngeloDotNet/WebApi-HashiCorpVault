namespace HashiCorpVault.API.Controllers;

public class VaultController : BaseController
{
    private readonly IConfiguration configuration;
    private readonly ILogger<VaultController> logger;
    private readonly IVaultService vaultService;

    public VaultController(IConfiguration configuration, ILogger<VaultController> logger, IVaultService vaultService)
    {
        this.configuration = configuration;
        this.logger = logger;
        this.vaultService = vaultService;
    }

    [HttpGet("readSecret/{secretKey}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IResult ReadSecret(string secretKey) => vaultService.ReadVaultService(configuration, secretKey);

    [HttpPost("writeSecret")]
    public IResult WriteSecret([FromBody] VaultDTO request) => vaultService.WriteVaultService(configuration, request);
}