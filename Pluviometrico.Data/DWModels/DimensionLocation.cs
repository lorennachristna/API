using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dim_localidade_cemaden")]
    public class DimensionLocation
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("bairro")]
        public string NeighborHood { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("long")]
        public double Longitude { get; set; }

        [Column("estado")]
        public string State { get; set; }

        [Column("uf")]
        public string UF { get; set; }

        [Column("municipio")]
        public string City { get; set; }
    }
}
