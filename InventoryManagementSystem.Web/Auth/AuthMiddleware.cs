using InventoryManagement.Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Concurrent;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryManagementSystem.Web.Auth
{
	// Step 1: Create the CustomAuthStateProvider class
	public class CustomAuthStateProvider : AuthenticationStateProvider
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CustomAuthStateProvider(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			// Get the current HttpContext's user (which will have the claims from cookie auth)
			var user = _httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());
			return Task.FromResult(new AuthenticationState(user));
		}
	}

	// Step 2: Keep your existing AuthMiddleware
	public class AuthMiddleware
	{
		private readonly RequestDelegate next;
		public static IDictionary<Guid, ClaimsPrincipal> Logins
		{ get; private set; }
			= new ConcurrentDictionary<Guid, ClaimsPrincipal>();

		public AuthMiddleware(RequestDelegate next)
		{
			this.next = next ?? throw new ArgumentNullException(nameof(next));
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
			{
				try
				{
					string keyString = context.Request.Query["key"];
					Console.WriteLine($"Received key: {keyString}");
					if (Guid.TryParse(keyString, out Guid key))
					{
						if (Logins.TryGetValue(key, out ClaimsPrincipal claim))
						{
							await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claim);
							// Remove the key after it's been used
							Logins.Remove(key);
							// Check for redirect parameter
							if (context.Request.Query.ContainsKey("redirect"))
							{
								string redirectPath = context.Request.Query["redirect"];
								Console.WriteLine($"Redirecting to: {redirectPath}");
								context.Response.Redirect(redirectPath);
							}
							else
							{
								// Default redirect if no redirect parameter is provided
								context.Response.Redirect("/productlist");
							}
							return;
						}
						else
						{
							Console.WriteLine($"Key {key} not found in Logins dictionary");
						}
					}
					else
					{
						Console.WriteLine($"Failed to parse key: {keyString}");
					}
					// If we get here, something went wrong
					context.Response.Redirect("/login?error=invalid_key");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error in AuthMiddleware: {ex.Message}");
					context.Response.Redirect("/login?error=exception");
				}
			}
			else
			{
				await next(context);
			}
		}
	}

	// Step 3: Create a helper class to build claims with roles
	public static class ClaimsHelper
	{
		public static ClaimsPrincipal CreateUserPrincipal(string username, string email, string role)
		{
			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, username),
					new Claim(ClaimTypes.Email, email),
					new Claim(ClaimTypes.Role, role) // This is crucial for role-based navigation
				};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			return new ClaimsPrincipal(identity);
		}
	}
}