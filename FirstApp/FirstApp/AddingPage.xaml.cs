using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingPage : ContentPage
    {
        public AddingPage()
        {
            InitializeComponent();
            BindingContext = new Habit();
        }
    }
}