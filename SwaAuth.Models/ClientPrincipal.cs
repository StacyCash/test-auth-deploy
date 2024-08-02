using System.Collections.Generic;

namespace SwaAuth.Models;

public record ClientPrincipal(
	string IdentityProvider, 
	string UserId, 
	string UserDetails, 
	IEnumerable<string> UserRoles, 
	IEnumerable<SwaClaims> Claims, 
	string AccessToken);
