//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication;
//using System.Collections.Concurrent;
//using System.Security.Claims;

//public class AuthMiddleware
//{
//    private readonly RequestDelegate next;
//    public static IDictionary<Guid, ClaimsPrincipal> Logins
//    { get; private set; } = new ConcurrentDictionary<Guid, ClaimsPrincipal>();

//    public AuthMiddleware(RequestDelegate next)
//    {
//        this.next = next ?? throw new ArgumentNullException(nameof(next));
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        Console.WriteLine($"AuthMiddleware processing path: {context.Request.Path}");

//        // Special handling for login path with key parameter
//        // Note: removed orders path from this condition
//        if (context.Request.Path == "/login" && context.Request.Query.ContainsKey("key"))
//        {
//            try
//            {
//                string keyString = context.Request.Query["key"];
//                Console.WriteLine($"Received key: {keyString}");

//                if (Guid.TryParse(keyString, out Guid key))
//                {
//                    if (Logins.TryGetValue(key, out ClaimsPrincipal claim))
//                    {
//                        // Sign in the user with the claims
//                        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claim);
//                        Console.WriteLine($"User signed in with claims: {string.Join(", ", claim.Claims.Select(c => $"{c.Type}={c.Value}"))}");

//                        // Remove the key after it's been used
//                        Logins.Remove(key);

//                        // Check for redirect parameter
//                        if (context.Request.Query.ContainsKey("redirect"))
//                        {
//                            string redirectPath = context.Request.Query["redirect"];
//                            Console.WriteLine($"Redirecting to: {redirectPath}");
//                            context.Response.Redirect(redirectPath);
//                        }
//                        else
//                        {
//                            // Default redirect if no redirect parameter is provided
//                            context.Response.Redirect("/productlist");
//                        }
//                        return;
//                    }
//                    else
//                    {
//                        Console.WriteLine($"Key {key} not found in Logins dictionary");
//                    }
//                }
//                else
//                {
//                    Console.WriteLine($"Failed to parse key: {keyString}");
//                }

//                // If we get here, something went wrong
//                context.Response.Redirect("/login?error=invalid_key");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error in AuthMiddleware: {ex.Message}");
//                context.Response.Redirect("/login?error=exception");
//            }
//        }
//        else
//        {
//            // Simply proceed to the next middleware without any orders page checks
//            await next(context);
//        }
//    }
//}



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

        // Special handling for login path with key parameter
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
                            // Role-based navigation
                            string userRole = GetUserRole(claim);
                            Console.WriteLine($"User role: {userRole}");

                            if (userRole == "Admin" || userRole == "Supplier")
                            {
                                // Admin and Supplier go to products page
                                context.Response.Redirect("/products");
                            }
                            else if (userRole == "User")
                            {
                                // Regular users go to product list
                                context.Response.Redirect("/productlist");
                            }
                            else
                            {
                                // Default redirect for any other roles
                                context.Response.Redirect("/productlist");
                            }
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
            // Simply proceed to the next middleware without any orders page checks
            await next(context);
        }
    }

    // Helper method to get the user role from claims
    private string GetUserRole(ClaimsPrincipal principal)
    {
        var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        return roleClaim?.Value ?? "User"; // Default to "User" if no role claim is found
    }
}