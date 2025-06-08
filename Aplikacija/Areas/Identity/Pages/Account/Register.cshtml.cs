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
            public string ime { get; set; }

            [Required]
            public string prezime { get; set; }

            [Required]
            public string datumRodjenja { get; set; }

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

            // Generiši kod za verifikaciju
            var code = await _emailSender.SendVerificationCodeEmailAsync(Input.Email);

            HttpContext.Session.SetString("VerificationCode", code);
            HttpContext.Session.SetString("UserEmail", Input.Email);

            // Sačuvaj podatke korisnika u sesiju (ili koristi TempData ili skrivena polja)
            HttpContext.Session.SetString("ime", Input.ime);
            HttpContext.Session.SetString("prezime", Input.prezime);
            HttpContext.Session.SetString("datumRodjenja", Input.datumRodjenja);
            HttpContext.Session.SetString("Password", Input.Password);

            ShowVerificationModal = true;

            return Page(); // Prikaži modal
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
            var datumRodjenja = HttpContext.Session.GetString("datumRodjenja");
            var password = HttpContext.Session.GetString("Password");

            var user = CreateUser();
            user.UserName = expectedEmail;
            user.Email = expectedEmail;
            user.ime = ime;
            user.prezime = prezime;
            user.datumRodjenja = DateTime.Parse(datumRodjenja);

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
            HttpContext.Session.Remove("VerificationCode");
            HttpContext.Session.Remove("UserEmail");
            HttpContext.Session.Remove("ime");
            HttpContext.Session.Remove("prezime");
            HttpContext.Session.Remove("datumRodjenja");
            HttpContext.Session.Remove("Password");

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
