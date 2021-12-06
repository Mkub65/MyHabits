using MyHabit.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MyHabit.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}