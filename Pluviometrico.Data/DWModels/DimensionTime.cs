using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dim_tempo")]
    public class DimensionTime
    {
        [Column("id_tempo")]
        public int Id { get; set; }

        [Column("ano")]
        public int Year { get; set; }

        [Column("mes")]
        public int Month { get; set; }

        [Column("dia")]
        public int Day { get; set; }

        [Column("hora")]
        public int Hour { get; set; }
    }
}
