using System;

namespace CargaMoedasRabbitMQ.Models
{
    public class Cotacao
    {
        public string Sigla { get; set; }
        public double? Valor { get; set; }        
    }
}