using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Orders.Frontend.AuthenticationProviders
{
	public class AuthenticationProviderTest : AuthenticationStateProvider
	{
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			await Task.Delay(3000);
			var user = new ClaimsIdentity(authenticationType: "test"); // Create User Auth
			var anonimous = new ClaimsIdentity(); // Create User Anonimo
			// Create User Admin
			var admin = new ClaimsIdentity(new List<Claim>
			{
				new Claim("FirstName", "Javier"),
				new Claim("LastName", "Pineda"),
				new Claim(ClaimTypes.Name, "epineda@yopmail.com"),
				new Claim(ClaimTypes.Role, "Admin")
			},
			authenticationType: "test");

			return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
		}
	}
}