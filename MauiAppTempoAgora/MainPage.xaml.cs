using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try 
            {
                if (!string.IsNullOrEmpty(txt_cidade.Text))
                {
                    Tempo? t = await DataService.GetPrevisao(txt_cidade.Text);

                    if(t !=null)
                    {
                        string dados_previsao = "";

                        dados_previsao = $"Latitude: {t.lat} \n" +
                                         $"Longitude: {t.lon} \n" +
                                         $"Descrição: {t.description} \n" +
                                         $"Velocidade do vento: {t.speed} m/s \n" +
                                         $"Visibilidade: {t.visibility} metros \n" +
                                         $"Nascer do Sol: {t.sunrise} \n" +
                                         $"Pôr do Sol: {t.sunset} \n" +
                                         $"Temp. Máx: {t.temp_max} °C \n" +
                                         $"Temp. Min: {t.temp_min} °C \n";

                        lbl_res.Text = dados_previsao;
                    }else
                    {
                        lbl_res.Text = "Não foi possível obter a previsão.";
                    }
                }else
                {
                    lbl_res.Text = "Prencha a cidade.";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }

        }
    }
}
