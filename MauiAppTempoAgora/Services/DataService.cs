using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    internal class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "1bda3aa9ea581c33ebb66df6a3e44ba4";

            string url = $"https://api.openweathermap.org/data/2.5/weather?" +
                         $"q={cidade}&units=metric&appid={chave}&lang=pt_br";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        throw new Exception("Cidade não encontrada.");
                    }

                    if (!resp.IsSuccessStatusCode)
                    {
                        throw new Exception("Não foi possível consultar a previsão.");
                    }

                    string json = await resp.Content.ReadAsStringAsync();

                    var rascunho = JObject.Parse(json);

                    DateTime sunrise = DateTimeOffset
                        .FromUnixTimeSeconds((long)rascunho["sys"]["sunrise"])
                        .LocalDateTime;

                    DateTime sunset = DateTimeOffset
                        .FromUnixTimeSeconds((long)rascunho["sys"]["sunset"])
                        .LocalDateTime;

                    t = new Tempo
                    {
                        lat = (double)rascunho["coord"]["lat"],
                        lon = (double)rascunho["coord"]["lon"],

                        description = (string)rascunho["weather"][0]["description"],
                        main = (string)rascunho["weather"][0]["main"],

                        temp_min = (double)rascunho["main"]["temp_min"],
                        temp_max = (double)rascunho["main"]["temp_max"],

                        speed = (double)rascunho["wind"]["speed"],
                        visibility = (int)rascunho["visibility"],

                        sunrise = sunrise.ToString("HH:mm"),
                        sunset = sunset.ToString("HH:mm")
                    };
                }
            }
            catch (HttpRequestException)
            {
                throw new Exception("Sem conexão com a internet.");
            }

            return t;
        }
    }
}
