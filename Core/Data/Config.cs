using System;
namespace Core.Data
{
    public class Config
    {
        public int Id { get; set; }
        public bool SystemEnable { get; set; }
        public string Currency { get; set; }
        public decimal? ReferalBonus { get; set; }
        public int Percent { get; set; }
    }
}
