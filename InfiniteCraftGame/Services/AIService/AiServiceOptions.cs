using System.ComponentModel.DataAnnotations;

namespace InfiniteCraftGame.Services.AIService;

public class AiServiceOptions
{
    [Required]
    public string ApiKey { get; set; } = string.Empty;
}
