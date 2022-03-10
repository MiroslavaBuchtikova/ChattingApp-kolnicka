using System.ComponentModel.DataAnnotations;
using ChatAppBackend.Controllers;

namespace ChatAppBackend.Services;

public class RegisterModel : LoginModel
{
    [MaxLength(50)]
    [Required]
    public string Username { get; set; }
}