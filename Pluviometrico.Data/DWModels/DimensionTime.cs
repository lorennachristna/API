using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dimensao_tempo")]
    public class DimensionTime
    {
        [Column("id_dimensao_tempo")]
        public int Id { get; set; }

        [Column("ano")]
        public string Year { get; set; }

        [Column("mes")]
        public string Month { get; set; }

        [Column("dia")]
        public string Day { get; set; }

        [Column("hora")]
        public string Hour { get; set; }

        [Column("minuto")]
        public string Minute { get; set; }

    }
}
