using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgBlazor.Models;
using System.Text.Json;
using System.Net.Http.Headers;  

namespace RpgBlazor.Services
{
    public class PersonagemService
    {
        private readonly HttpClient _http;

        public PersonagemService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<PersonagemViewModel>> GetAllAsync(string token)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync("Personagens/GetAll");
            var responseContent = await response.Content.ReadAsStringAsync();
            List<PersonagemViewModel> lista = new List<PersonagemViewModel>();

            if (response.IsSuccessStatusCode)
            {
                lista = JsonSerializer
                    .Deserialize<List<PersonagemViewModel>>(responseContent, JsonSerializerOptions.Web);
                return lista;
            }
            else
            {
                throw new Exception(responseContent);
            }
        }

        public async Task<PersonagemViewModel> InsertAsync(string token, PersonagemViewModel personagem)
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonSerializer.Serialize(personagem));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _http.PostAsync("personagens", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                personagem.Id = Convert.ToInt32(responseContent);
                return personagem;
            }
            else
            {
                throw new Exception(responseContent);
            }
        }
    }
}

