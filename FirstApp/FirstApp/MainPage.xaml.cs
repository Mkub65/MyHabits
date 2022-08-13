using FirstApp.Model;
using FirstApp.ViewModels;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly MainPageVM vm;
        public MainPage()
        {
            InitializeComponent();
            vm = Resources["vm"] as MainPageVM;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.GetHabitsFromTable();
        }
    }
}