using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Covid_Api.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime Date { get; set; }


    }
}