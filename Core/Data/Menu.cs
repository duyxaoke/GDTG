using System;
namespace Core.Data
{
    public class Menu
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }
        public int Order { get; set; }
    }
}
