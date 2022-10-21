using BlazorDictionary.Common.Events.User;
using BlazorDictionary.Common.Infrastructure.Exceptions;
using BlazorDictionary.Common.Infrastructure.Results;
using BlazorDictionary.Common.ViewModels.Queries;
using BlazorDictionary.WebApp.Infrastructure.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace BlazorDictionary.WebApp.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(HttpClient client)
        {
            _client = client;
        }

        public async Task<UserDetailViewModel> GetUserDetail(Guid? id)
        {
            var userDetail = await _client.GetFromJsonAsync<UserDetailViewModel>($"/api/user/{id}");

            return userDetail;
        }

        public async Task<UserDetailViewModel> GetUserDetail(string userName)
        {
            var userDetail = await _client.GetFromJsonAsync<UserDetailViewModel>($"/api/user/username/{userName}");

            return userDetail;
        }

        public async Task<bool> UpdateUser(UserDetailViewModel user)
        {
            var res = await _client.PostAsJsonAsync($"/api/user/update", user);

            return res.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeUserPassword(string oldPassword, string newPassword)
        {
            var command = new ChangeUserPasswordCommand(null, oldPassword, newPassword);

            var httpResponse = await _client.PostAsJsonAsync($"/api/user/changepassword", command);

            if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var responseStr = await httpResponse.Content.ReadAsStringAsync();
                    var validation = JsonSerializer.Deserialize<ValidationResponseModel>(responseStr);
                    responseStr = validation.FlattenErrors;
                    throw new DatabaseValidationException(responseStr);
                }

                return false;
            }

            return httpResponse.IsSuccessStatusCode;
        }
    }
}
