#nullable disable

using Grupa5Tim3.Data;
using Grupa5Tim3.Models;
using Grupa5Tim3.servis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Grupa5Tim3.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SendGridEmailSender _emailSender;
        private readonly SignInManager<Korisnik> _signInManager;
        private readonly UserManager<Korisnik> _userManager;
        private readonly IUserStore<Korisnik> _userStore;
        private readonly IUserEmailStore<Korisnik> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        public RegisterModel(
            UserManager<Korisnik> userManager,
            IUserStore<Korisnik> userStore,
            SignInManager<Korisnik> signInManager,
            ILogger<RegisterModel> logger,
            SendGridEmailSender emailSender,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public string VerificationCode { get; set; }

        [BindProperty]
        public bool ShowVerificationModal { get; set; } = false;

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            [RegularExpression("^[a-zA-ZčćžšđČĆŽŠĐ\\s-]+$", ErrorMessage = "First name can only contain letters.")]
            public string ime { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [RegularExpression("^[a-zA-ZčćžšđČĆŽŠĐ\\s-]+$", ErrorMessage = "Last name can only contain letters.")]
            public string prezime { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date of Birth")]
            public DateTime datumRodjenja { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain at least 8 characters.")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
                ErrorMessage = "Password must contain an uppercase letter, a lowercase letter, a number and a special character.")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords don't match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            // Slanje verifikacionog koda
            var code = await _emailSender.SendVerificationCodeEmailAsync(Input.Email);

            HttpContext.Session.SetString("VerificationCode", code);
            HttpContext.Session.SetString("UserEmail", Input.Email);
            HttpContext.Session.SetString("ime", Input.ime);
            HttpContext.Session.SetString("prezime", Input.prezime);
            HttpContext.Session.SetString("datumRodjenja", Input.datumRodjenja.ToString("o")); // ISO format
            HttpContext.Session.SetString("Password", Input.Password);

            ShowVerificationModal = true;
            return Page(); // Prikazuje modal
        }

        public async Task<IActionResult> OnPostVerify()
        {
            var expectedCode = HttpContext.Session.GetString("VerificationCode");
            var expectedEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(expectedCode) || string.IsNullOrEmpty(expectedEmail))
            {
                ModelState.AddModelError(string.Empty, "Verification timed out. Please try again.");
                return RedirectToPage("./Register");
            }

            if (VerificationCode != expectedCode)
            {
                ModelState.AddModelError(string.Empty, "Code not correct.");
                ShowVerificationModal = true;
                return Page();
            }

            // Preuzmi podatke iz sesije
            var ime = HttpContext.Session.GetString("ime");
            var prezime = HttpContext.Session.GetString("prezime");
            var datumRodjenjaStr = HttpContext.Session.GetString("datumRodjenja");
            var password = HttpContext.Session.GetString("Password");

            if (!DateTime.TryParse(datumRodjenjaStr, null, System.Globalization.DateTimeStyles.RoundtripKind, out var parsedDate))
            {
                ModelState.AddModelError(string.Empty, "Invalid date format.");
                return Page();
            }

            var user = CreateUser();
            user.UserName = expectedEmail;
            user.Email = expectedEmail;
            user.ime = ime;
            user.prezime = prezime;
            user.datumRodjenja = parsedDate;

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error while creating a new user after verification.");
                return Page();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

            if (!confirmResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Error while confirming email.");
                return Page();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            // Očisti sesiju
            HttpContext.Session.Clear();

            return LocalRedirect("~/");
        }

        private Korisnik CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Korisnik>();
            }
            catch
            {
                throw new InvalidOperationException($"Error while creating a new user.");
            }
        }

        private IUserEmailStore<Korisnik> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
                throw new NotSupportedException("User store doesn't support Email.");
            return (IUserEmailStore<Korisnik>)_userStore;
        }
    }
}
