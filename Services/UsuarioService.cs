using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgBlazor.Models; //RpgBlazor é o nome do projeto caso vc fez certo.
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;

namespace RpgBlazor.Services
{
    public class UsuarioService
    {
        private readonly HttpClient _http;

        public UsuarioService(HttpClient http)
        {
            _http = http;
        }

        public async Task<UsuarioViewModel> AutenticarAsync(UsuarioViewModel usuario)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(usuario),
                Encoding.UTF8,
                "application/json");

            var response = await _http.PostAsync("usuarios/autenticar", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var usuarioRetornado = JsonSerializer.Deserialize<UsuarioViewModel>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return usuarioRetornado ?? new UsuarioViewModel();
            }
            else
            {
                // Garante que a mensagem de erro sempre tenha algo legível
                throw new Exception(!string.IsNullOrEmpty(responseContent)
                    ? responseContent
                    : $"Erro {response.StatusCode} ao autenticar usuário.");
            }
        }


       public async Task<UsuarioViewModel> RegistrarAsync(UsuarioViewModel usuario)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(usuario),
                Encoding.UTF8,
                "application/json");

            var response = await _http.PostAsync("usuarios/registrar", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var usuarioCriado = JsonSerializer.Deserialize<UsuarioViewModel>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (usuarioCriado != null)
                        return usuarioCriado;

                    // fallback caso a API retorne apenas o ID
                    usuario.Id = Convert.ToInt32(responseContent.Trim('"'));
                    return usuario;
                }
                catch
                {
                    usuario.Id = Convert.ToInt32(responseContent.Trim('"'));
                    return usuario;
                }
            }
            else
            {
                throw new Exception(!string.IsNullOrEmpty(responseContent)
                    ? responseContent
                    : $"Erro {response.StatusCode} ao registrar usuário.");
            }
        }
    }
}