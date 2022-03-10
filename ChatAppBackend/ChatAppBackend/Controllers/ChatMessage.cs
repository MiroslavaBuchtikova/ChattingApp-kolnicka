using System.ComponentModel.DataAnnotations;

namespace ChatAppBackend.Controllers;

public class ChatMessage
{
    [Required]
    public string User { get; set; }

    [Required]
    [MaxLength(250)]
    public string Message { get; set; }
}