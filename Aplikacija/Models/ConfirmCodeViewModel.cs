// Models/ConfirmCodeViewModel.cs
using System.ComponentModel.DataAnnotations;

public class ConfirmCodeViewModel
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(6, MinimumLength = 6)]
    public string Code { get; set; } = string.Empty;
}
