using System;

namespace DTO
{
    public class FactsDto
    {
        public string Fact {get;set;} = string.Empty;
        public List<string> Values {get;set;} = new List<string>();
    }
}