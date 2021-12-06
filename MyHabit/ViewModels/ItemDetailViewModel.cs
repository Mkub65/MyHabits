using MyHabit.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyHabit.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        private DateTime startDate;
        private DateTime endDate;
        private bool archieved;

        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public  DateTime StartDate
        {
            get => startDate;
            set => SetProperty(ref startDate, value);
        }

        public DateTime EndDate
        {
            get => endDate;
            set => SetProperty(ref endDate, value);
        }

        public bool Archieved
        {
            get => archieved;
            set => SetProperty(ref archieved, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
                StartDate = item.StartDate;
                EndDate = item.EndDate;
                Archieved = item.Archieved;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
