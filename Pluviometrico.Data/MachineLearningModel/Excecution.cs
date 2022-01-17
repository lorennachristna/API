using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pluviometrico.Data.MachineLearningModel
{
    [Table("execucoes")]
    public class Excecution
    {
        [Column("id")]
        public int Id { get; set; }


        [Column("sgbd")]
        public string DBMS { get; set; }

        [Column("querynumber")]
        public int QueryNumber { get; set; }


        [Column("maquinaaws")]
        public string AWSMachine { get; set; }


        [Column("qtdlinhas")]
        public int NumberOfLine { get; set; }


        [Column("datainicio")]
        DateTime StartDate { get; set; }


        [Column("datafim")]
        DateTime EndDate { get; set; }


        [Column("tempoexecucao")]
        public int ExecutionTime { get; set; }
    }
}
