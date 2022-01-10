using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dim_estacao")]
    public class DimensionStation
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("cod_estacao_original")]
        public string StationCode { get; set; }

        [Column("nome_estacao_original")]
        public string StationName { get; set; }
    }
}
