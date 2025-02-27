using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Concurrent;
using System.Security.Claims;

public class AuthMiddleware
{
    private readonly RequestDelegate next;

    public static IDictionary<Guid, ClaimsPrincipal> Logins
    { get; private set; } = new ConcurrentDictionary<Guid, ClaimsPrincipal>();

    public AuthMiddleware(RequestDelegate next)
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        Console.WriteLine($"AuthMiddleware processing path: {context.Request.Path}");

        // Special handling for login and orders paths with key parameter
        if ((context.Request.Path == "/login" || context.Request.Path == "/orders") && context.Request.Query.ContainsKey("key"))
        {
            try
            {
                string keyString = context.Request.Query["key"];
                Console.WriteLine($"Received key: {keyString}");

                if (Guid.TryParse(keyString, out Guid key))
                {
                    if (Logins.TryGetValue(key, out ClaimsPrincipal claim))
                    {
                        // Sign in the user with the claims
                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claim);
                        Console.WriteLine($"User signed in with claims: {string.Join(", ", claim.Claims.Select(c => $"{c.Type}={c.Value}"))}");

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
            // Check if this is the orders page without a key - this means it's a regular access
            if (context.Request.Path == "/orders")
            {
                Console.WriteLine("Processing regular access to Orders page");

                // Make sure the user is authenticated before proceeding
                if (!context.User.Identity.IsAuthenticated)
                {
                    Console.WriteLine("User is not authenticated, redirecting to login");
                    context.Response.Redirect("/login");
                    return;
                }

                // Check if user is admin
                bool isAdmin = context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
                Console.WriteLine($"User is admin: {isAdmin}");
            }

            await next(context);
        }
    }
}