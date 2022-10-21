using BlazorDictionary.Common.Infrastructure.Exceptions;
using BlazorDictionary.Common.Infrastructure.Results;
using BlazorDictionary.Common.ViewModels.Queries;
using BlazorDictionary.Common.ViewModels.RequestModels;
using BlazorDictionary.WebApp.Infrastructure.Auth;
using BlazorDictionary.WebApp.Infrastructure.Extensions;
using BlazorDictionary.WebApp.Infrastructure.Services.Interfaces;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorDictionary.WebApp.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _client;
        private readonly ISyncLocalStorageService _syncLocalStorageService;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public IdentityService(HttpClient client, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _syncLocalStorageService = syncLocalStorageService;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(GetUserToken());

        public string GetUserToken()
        {
            return _syncLocalStorageService.GetToken();
        }

        public string GetUserName()
        {
            return _syncLocalStorageService.GetUserName();
        }

        public Guid GetUserId()
        {
            return _syncLocalStorageService.GetUserId();
        }

        public async Task<bool> Login(LoginUserCommand command)
        {
            string responseStr;

            var httpResponse = await _client.PostAsJsonAsync("/api/user/login", command);

            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr);
                    responseStr = validation.FlattenErrors;
                    throw new DatabaseValidationException(responseStr);
                }
            }

            responseStr = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<LoginUserViewModel>(responseStr);

            if (!string.IsNullOrEmpty(response.Token))
            {
                _syncLocalStorageService.SetToken(response.Token);
                _syncLocalStorageService.SetUserName(response.UserName);
                _syncLocalStorageService.SetUserId(response.Id);

                ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogin(response.UserName, response.Id);

                _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", response.UserName);

                return true;
            }
            return false;
        }

        public void Logout()
        {
            _syncLocalStorageService.RemoveItem(LocalStorageExtensions.TokenName);
            _syncLocalStorageService.RemoveItem(LocalStorageExtensions.UserName);
            _syncLocalStorageService.RemoveItem(LocalStorageExtensions.UserId);

            ((AuthStateProvider)_authenticationStateProvider).NotifyUserLogout();

            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
