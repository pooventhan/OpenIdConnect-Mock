namespace OpenIdConnectMock.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public sealed class OpenIdConfiguration
    {
        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; }

        [JsonProperty("token_endpoint")]
        public string TokenEndpoint { get; set; }

        [JsonProperty("end_session_endpoint")]
        public string EndSessionEndpoint { get; set; }

        [JsonProperty("jwks_uri")]
        public string JwksUri { get; set; }

        [JsonProperty("response_modes_supported")]
        public IList<string> ResponseModeSupported { get; set; }

        [JsonProperty("response_types_supported")]
        public IList<string> ResponseTypeSupported { get; set; }

        [JsonProperty("scopes_supported")]
        public IList<string> ScopesSupported { get; set; }

        [JsonProperty("subject_types_supported")]
        public IList<string> SubjectTypeSupported { get; set; }

        [JsonProperty("id_token_signing_alg_values_supported")]
        public IList<string> IdTokenSigningAlgValuesSupported { get; set; }

        [JsonProperty("token_endpoint_auth_methods_supported")]
        public IList<string> TokenEndpointAuthMethodsSuported { get; set; }

        [JsonProperty("claims_supported")]
        public IList<string> ClaimsSupported { get; set; }
    }
}