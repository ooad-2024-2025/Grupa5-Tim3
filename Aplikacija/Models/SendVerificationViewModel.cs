using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Models
{
    public class SendVerificationViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
