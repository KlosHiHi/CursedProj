using AndesiteAvaloniaApp.Options;
using Avalonia.Controls;
using OxideClassLibrary.DTOs;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AndesiteAvaloniaApp.Services
{
    public interface IAuthService
    {
        Action<string> LoginAction { get; set; }
        Window MainWindow { get; set; }
        Task<string?> AuthUserAsync(string login, string password);

        Task<UserData?> RegisterUserAsync(string login, string password, string? email);

        void CompleteWork(Window callWindow, string workType);
    }

    public class AuthService() : IAuthService
    {
        public Window MainWindow { get; set; } = null!;
        public Action<string> LoginAction { get; set; } = null!;

        public async Task<string?> AuthUserAsync(string login, string password)
        {
            string url = $"{HttpClientOption.BaseUrl}Login/Authorize";

            var json = JsonContent.Create(new
            {
                Login = login,
                Password = password
            });

            using var response = await HttpClientOption.Client.PostAsync(url, json);
            response.EnsureSuccessStatusCode();
            string jwtToken = await response.Content.ReadAsStringAsync();

            return String.IsNullOrEmpty(jwtToken)
                ? null
                : jwtToken;
        }

        public async Task<UserData?> RegisterUserAsync(string login, string password, string? email)
        {
            string url = $"{HttpClientOption.BaseUrl}Login/Register";

            var json = JsonContent.Create(new
            {
                Login = login,
                Password = password,
                Email = email ?? null,
            });

            using var response = await HttpClientOption.Client.PostAsync(url, json);
            response.EnsureSuccessStatusCode();
            var createdUser = await response.Content.ReadFromJsonAsync<UserData>();

            return (createdUser is null)
                ? null
                : createdUser;
        }

        public void CompleteWork(Window callWindow, string workType)
        {
            LoginAction?.Invoke(workType);
            callWindow!.Close();
        }
    }
}
