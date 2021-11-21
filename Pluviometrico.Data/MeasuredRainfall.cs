using Nest;

namespace Pluviometrico.Data
{
    public class MeasuredRainfall
    {

        [PropertyName("id")]
        public int Id { get; set; }

        [PropertyName("municipio")]
        public string Municipio { get; set; }

        [PropertyName("cod_estacao_original")]
        public string CodEstacaoOriginal { get; set; }

        [PropertyName("uf")]
        public string UF { get; set; }

        [PropertyName("nome_estacao_original")]
        public string NomeEstacaoOriginal { get; set; }

        [PropertyName("latitude")]
        public double Latitude { get; set; }

        [PropertyName("longitude")]
        public double Longitude { get; set; }

        [PropertyName("datahora")]
        public string DataHora { get; set; }

        [PropertyName("valormedida")]
        public double ValorMedida { get; set; }

        [PropertyName("hora")]
        public int Hora { get; set; }

        [PropertyName("dia")]
        public int Dia { get; set; }

        [PropertyName("minuto")]
        public int Minuto { get; set; }

        [PropertyName("mes")]
        public int Mes { get; set; }

        [PropertyName("ano")]
        public int Ano { get; set; }

        [PropertyName("datahora_ajustada")]
        public string DataHoraAjustada { get; set; }

        [PropertyName("estado")]
        public string Estado { get; set; }

        [PropertyName("bairro")]
        public string Bairro { get; set; }

        [PropertyName("cidade")]
        public string Cidade { get; set; }
    }
}
