using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Covid_Api.Models
{
    public class DailyData
    {
        public int Id { get; set; }
        public string CountryName { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }
        public int TotalConfirmed { get; set; }
        public int TotalRecovered { get; set; }

        public int TotalDeaths { get; set; }
        public int ActiveCases { get; set; }
        public int Serious { get; set; }

        public int CasesPer1MPopulation { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}