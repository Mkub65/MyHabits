using System;

namespace MyHabit.Models
{
    public class Item
    {
        public Item()
        {
            StartDate = DateTime.Now;
        }

        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Archieved { get; set; }
    }
}