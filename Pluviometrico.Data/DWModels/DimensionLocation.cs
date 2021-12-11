using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dim_localidade_cemaden")]
    public class DimensionLocation
    {
        [Column("id_dimensao_localidade")]
        public int Id { get; set; }

        [Column("bairro")]
        public string NeighborHood { get; set; }

        [Column("latitude")]
        public double Latitude { get; set; }

        [Column("longitude")]
        public double Longitude { get; set; }

        [Column("cidade")]
        public string City { get; set; }

        [Column("estado")]
        public string State { get; set; }

        [Column("uf")]
        public string UF { get; set; }

        [Column("municipio")]
        public string Town { get; set; }
    }
}
