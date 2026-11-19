using AndesiteAvaloniaApp.Options;
using Avalonia.Controls;
using OxideClassLibrary.DTOs;
using System;
using System.Diagnostics;
using System.Net;
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
        Task<bool> CheckLogin(string login);
    }

    public class AuthService() : IAuthService
    {
        public Window MainWindow { get; set; } = null!;
        public Action<string> LoginAction { get; set; } = null!;

        public async Task<string?> AuthUserAsync(string login, string password)
        {
            string url = $"Login/Authorize";

            var json = JsonContent.Create(new
            {
                Login = login,
                Password = password
            });

            try
            {
                using var response = await HttpClientOption.Client.PostAsync(url, json);
                response.EnsureSuccessStatusCode();
                string jwtToken = await response.Content.ReadAsStringAsync();

                return String.IsNullOrEmpty(jwtToken)
                    ? null
                    : jwtToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{ex.GetType()}] - {ex.Message}, {ex.Data}");
                return null;
            }
        }

        public async Task<UserData?> RegisterUserAsync(string login, string password, string? email)
        {
            string url = $"Login/Register";

            var json = JsonContent.Create(new
            {
                Login = login,
                Password = password,
                Email = email ?? null,
            });
            try
            {
                using var response = await HttpClientOption.Client.PostAsync(url, json);
                response.EnsureSuccessStatusCode();
                var createdUser = await response.Content.ReadFromJsonAsync<UserData>();

                return (createdUser is null)
                    ? null
                    : createdUser;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{ex.GetType()}] - {ex.Message}, {ex.Data}");
                return null;
            }
        }

        public void CompleteWork(Window callWindow, string workType)
        {
            LoginAction?.Invoke(workType);
            callWindow!.Close();
        }

        public async Task<bool> CheckLogin(string login)
        {
            try
            {
                string url = $"Login/IsLoginAvailable/{login}";

                using var responce = await HttpClientOption.Client.GetAsync(url);

                return responce.StatusCode switch
                {
                    HttpStatusCode.BadRequest => false,
                    HttpStatusCode.OK => true,
                    _ => false
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{ex.GetType()}] - {ex.Message}, {ex.Data}");
                return false;
            }
        }
    }
}
