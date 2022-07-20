using FirstApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FirstApp.ViewModels
{
    public class MainPageVM
    {
        public NewAddingPageCommand NewAddingPageCommand { get; set; }
        public ObservableCollection<Habit> Habits { get; set; }
        private Habit selectedHabit;
        public Habit SelectedHabit
        {
            get { return selectedHabit; }
            set
            {
                selectedHabit = value;
                if (selectedHabit != null)
                    App.Current.MainPage.Navigation.PushAsync(new HabitPlanPage(selectedHabit));
            }
        }
        public MainPageVM()
        {
            NewAddingPageCommand = new NewAddingPageCommand(this);
            Habits = new ObservableCollection<Habit>();
            GetHabitsFromTable();
        }
        public void NewAddingPageNavigation()
        {
            App.Current.MainPage.Navigation.PushAsync(new AddingPage());
        }
        public void GetHabitsFromTable()
        {
            Habits.Clear();
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<Habit>();
                var habitstable = conn.Table<Habit>().ToList();
                foreach(var habit in habitstable)
                {
                    Habits.Add(habit);
                }
            }
        }
    }
    public class NewAddingPageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly MainPageVM vM;

        public NewAddingPageCommand(MainPageVM vm)
        {
            vM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            vM.NewAddingPageNavigation();
        }
    }
}
