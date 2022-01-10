using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("fato_chuva_cemaden")]
    public class FactRain
    {   
        [Column("id_fonte")]
        [ForeignKey(nameof(DimensionSource))]
        public int SourceId { get; set; }

        [Column("id_tempo")]
        [ForeignKey(nameof(DimensionTime))]
        public int TimeId { get; set; }

        [Column("id_localidade")]
        [ForeignKey(nameof(DimensionLocation))]
        public int LocationId { get; set; }

        [Column("id_estacao")]
        [ForeignKey(nameof(DimensionStation))]
        public int StationId { get; set; }

        [Column("indice_pluviometrico")]
        public double RainfallIndex { get; set; }

        //Properties that facilitates querying, but are not on the database (relative to foreign keys)

        public DimensionLocation Location { get; set; }
        public DimensionSource Source { get; set; }
        public DimensionStation Station { get; set; }
        public DimensionTime Time { get; set; }
    }
}
