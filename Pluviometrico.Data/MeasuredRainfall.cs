namespace Pluviometrico.Data
{
    public class MeasuredRainfall
    {
        public int Id { get; set; }
        public string Municipio { get; set; }
        public string CodEstacaoOriginal { get; set; }
        public string UF { get; set; }
        public string NomeEstacaoOriginal { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DataHora { get; set; }
        public double ValorMedida { get; set; }
        public int Hora { get; set; }
        public int Dia { get; set; }
        public int Minuto { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public string DataHoraAjustada { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
    }
}
