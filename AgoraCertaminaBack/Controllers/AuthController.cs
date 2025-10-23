using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AgoraCertaminaBack.Authorization.Settings;
using System.Text;
using static AgoraCertaminaBack.Authorization.AuthenticationEntities;
using Microsoft.AspNetCore.Http;

namespace AgoraCertaminaBack.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly ICognitoSettings _cognitoSettings;
        private readonly string _BasicAuth;

        public AuthController(HttpClient httpClient, ICognitoSettings cognitoSettings)
        {
            _httpClient = httpClient;
            _cognitoSettings = cognitoSettings;
            _BasicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_cognitoSettings.ClientId}:{_cognitoSettings.ClientSecret}"));
        }

        [AllowAnonymous]
        [HttpPost("exchange/{code}")]
        public async Task<ActionResult<TokenDTO>> Exchange(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest(new { message = "Code session value is missing" });

            return await ExchangeCode(code);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<TokenDTO>> RefreshToken()
        {
            var refreshTokenFromCookie = HttpContext.Request.Cookies["refresh_token"];

            if (string.IsNullOrEmpty(refreshTokenFromCookie))
                return BadRequest(new { message = "RefreshToken value is missing" });

            return await ExchangeNewTokens(refreshTokenFromCookie);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public ActionResult Logout()
        {
            DeleteHttpOnlyCookie("refresh_token");
            return Ok();
        }

        private async Task<ActionResult> ExchangeCode(string code)
        {
            var url = $"{_cognitoSettings.Domain}/oauth2/token";
            var data = new StringContent(
                $"grant_type=authorization_code&code={code}&client_id={_cognitoSettings.ClientId}&redirect_uri={_cognitoSettings.RedirectUri}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers = { { "Authorization", $"Basic {_BasicAuth}" } },
                Content = data
            };

            try
            {
                var httpResponse = await _httpClient.SendAsync(requestMessage);
                var result = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)httpResponse.StatusCode, new { message = "An error occurred while retrieving the tokens", error = result });
                }

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);

                if (tokenResponse == null)
                    return StatusCode(500, new { message = "An error occurred during token deserialization" });

                SetHttpOnlyCookie("refresh_token", tokenResponse.RefreshToken, tokenResponse.ExpiresIn);

                // ✅ CORRECCIÓN: Devolver el ID Token en lugar del Access Token
                return Ok(new TokenDTO(tokenResponse.IDToken, tokenResponse.ExpiresIn));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        private async Task<ActionResult> ExchangeNewTokens(string refreshToken)
        {
            var url = $"{_cognitoSettings.Domain}/oauth2/token";
            var data = new StringContent(
                $"grant_type=refresh_token&client_id={_cognitoSettings.ClientId}&refresh_token={refreshToken}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Headers = { { "Authorization", $"Basic {_BasicAuth}" } },
                Content = data
            };

            try
            {
                var httpResponse = await _httpClient.SendAsync(requestMessage);
                var result = await httpResponse.Content.ReadAsStringAsync();

                if (!httpResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)httpResponse.StatusCode, new { message = "An error occurred while refreshing tokens", error = result });
                }

                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(result);

                if (tokenResponse == null)
                    return StatusCode(500, new { message = "An error occurred during token deserialization" });

                // ✅ CORRECCIÓN: Devolver el ID Token en lugar del Access Token
                return Ok(new TokenDTO(tokenResponse.IDToken, tokenResponse.ExpiresIn));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        private void SetHttpOnlyCookie(string Key, string Value, int expiresInSeconds)
        {
            var isLocalhost = HttpContext.Request.Host.Host.Contains("localhost");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddSeconds(_cognitoSettings.RefreshTokenExpiresIn),
                Path = "/",
                SameSite = isLocalhost ? SameSiteMode.None : SameSiteMode.Lax,
                Secure = true
            };

            HttpContext!.Response.Cookies.Append(Key, Value, cookieOptions);
        }

        private void DeleteHttpOnlyCookie(string Key)
        {
            var isLocalhost = HttpContext.Request.Host.Host.Contains("localhost");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/",
                SameSite = isLocalhost ? SameSiteMode.None : SameSiteMode.Lax,
                Secure = true
            };

            HttpContext!.Response.Cookies.Append(Key, string.Empty, cookieOptions);
        }
    }
}