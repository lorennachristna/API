using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dim_fonte")]
    public class DimensionSource
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("fonte")]
        public string Source { get; set; }

        [Column("url")]
        public string Url { get; set; }
    }
}
