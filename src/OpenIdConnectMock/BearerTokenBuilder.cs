namespace OpenIdConnectMock
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IdentityModel.Tokens;
    using System.Security.Claims;

    public static class BearerTokenBuilder
    {
        public const string Realm = "http://foo";
        public const string Issuer = "https://localhost:44316/22e92fa8-837a-4f2d-8324-b548a5ac0d38/v2.0/";
        public const string Secret = "dGhpc2lzYXNlY3JldHRoYXRuZWVkdG9iZW1vcmV0aGFuMTI4Yml0c2xvbmc=";

        public static string CreateToken(string name, string email, string role, string policy, string nonce)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Version, "1.0")
            };

            claims.Add(new Claim("acr", policy));
            claims.Add(new Claim("nonce", nonce));
            claims.Add(new Claim("iat", DateTime.UtcNow.Ticks.ToString()));
            claims.Add(new Claim("auth_time", DateTime.UtcNow.Ticks.ToString()));
            claims.Add(new Claim("authenticationSource", "AzureADAuthentication"));
            claims.Add(new Claim("idp", "https://sts.windows.net/be67623c-1932-42a6-9d24-6c359fe5ea71/"));
            claims.Add(new Claim("alternativeSecurityId", "{\"type\":6,\"identityProvider\":\"https://sts.windows.net/be67623c-1932-42a6-9d24-6c359fe5ea71/\",\"key\":\"NDBmZTFhNGYtMTY1Ni00NjkzLTkzZmYtY2U5ZjFlN2U1Mjkx\"}"));

            claims.Add(new Claim("sub", Guid.NewGuid().ToString()));

            var claimsIdentity = new ClaimsIdentity(claims, "Custom");
            var symmetricKey = Convert.FromBase64String(Secret);

            var signingCredentials = new SigningCredentials(
                new InMemorySymmetricSecurityKey(symmetricKey),
                SecurityAlgorithms.HmacSha256Signature,
                SecurityAlgorithms.Sha256Digest);

            var lifetime = new Lifetime(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1));

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claimsIdentity,
                SigningCredentials = signingCredentials,
                TokenIssuerName = Issuer,
                AppliesToAddress = Realm, //Aud
                Lifetime = lifetime,
            };

            var plainToken = tokenHandler.CreateToken(tokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);
            return signedAndEncodedToken;
        }
    }
}