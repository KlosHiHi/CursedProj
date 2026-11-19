using System;
using System.Diagnostics;
using System.IO;

namespace AndesiteAvaloniaApp
{
    public sealed class TokenStorage
    {
        private TokenStorage() { }

        private static readonly Lazy<TokenStorage> _instance = new(() => new TokenStorage());

        public static TokenStorage Instance => _instance.Value;

        private string _token = "";
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                Save();
            }
        }

        private static readonly string _jwtTokenPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Andesite", "settings.txt");

        public void Load()
        {
            try
            {
                if (File.Exists(_jwtTokenPath))
                    _token = File.ReadAllText(_jwtTokenPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _token = "";
            }
        }

        public void Save()
        {
            var dir = Path.GetDirectoryName(_jwtTokenPath)!;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            try
            {
                File.WriteAllText(_jwtTokenPath, _token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public void Clear()
        {
            Token = "";
            if (File.Exists(_jwtTokenPath))
                File.Delete(_jwtTokenPath);

        }
    }
}
