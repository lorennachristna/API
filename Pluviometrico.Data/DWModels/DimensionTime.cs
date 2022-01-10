using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.DWModels
{
    [Table("dimensao_tempo")]
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

        [Column("semestre")]
        public int Semester { get; set; }

        [Column("semana")]
        public int Week { get; set; }
    }
}
