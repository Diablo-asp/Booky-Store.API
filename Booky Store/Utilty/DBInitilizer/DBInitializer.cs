
namespace Booky_Store.API.Utilty.DBInitilizer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DBInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            if (_roleManager.Roles is not null) //&& _userManager.Users is not null)
            {
                _roleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new(SD.Admin)).GetAwaiter().GetResult(); ;
                _roleManager.CreateAsync(new(SD.Employee)).GetAwaiter().GetResult(); ;
                _roleManager.CreateAsync(new(SD.Company)).GetAwaiter().GetResult(); ;
                _roleManager.CreateAsync(new(SD.Customer)).GetAwaiter().GetResult(); ;

                _userManager.CreateAsync(new()
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin",
                    Email = "Di0ab0lo0@Gmail.com",
                    EmailConfirmed = true,
                }, "2143Db@@").GetAwaiter().GetResult(); ;

                var user = _userManager.FindByNameAsync("SuperAdmin").GetAwaiter().GetResult();
                
                    _userManager.AddToRoleAsync(user, SD.SuperAdmin).GetAwaiter().GetResult();
                

            }
        }
    }
}
