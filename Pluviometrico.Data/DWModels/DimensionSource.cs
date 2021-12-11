using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dimensao_fonte")]
    public class DimensionSource
    {
        [Column("id_dimensao_fonte")]
        public int Id { get; set; }

        [Column("fonte")]
        public string Source { get; set; }

        [Column("url")]
        public string Url { get; set; }
    }
}
