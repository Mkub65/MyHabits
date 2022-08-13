using FirstApp.Model;
using FirstApp.ViewModels;
using Microcharts;
using SkiaSharp;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FirstApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailHabitPlanPage : ContentPage
    {
        readonly HabitPlan habitPlan;
        public Dictionary<string, int> dict;
        public DetailHabitPlanPage(HabitPlan habitPlan)
        {
            InitializeComponent();
            this.habitPlan = habitPlan;
            (Resources["vm"] as DetailsHabitPlanVM).HabitPlan = habitPlan;
            habitPlanName.Text = habitPlan.HabitPlanName;
            habitPlanStartDate.Text = habitPlan.StartDate.ToString("dd/MM/yyyy");
            habitPlanEndDate.Text = habitPlan.EndDate.ToString("dd/MM/yyyy");
            DrawChart();
        }
        void DrawChart()
        {
            dict = new Dictionary<string, int>();
            List<string> statusVariants = new List<string> { "None", "Done", "NotDone", "Ignore" };
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitTracker>();
                var statusTable = conn.Table<HabitTracker>().Where(i => i.HabitPlanId == habitPlan.Id);
                foreach (string variant in statusVariants)
                {
                    var statusList = statusTable.Where(n => n.Status == variant).Count();
                    dict.Add(variant, statusList);
                }
            }

            List<ChartEntry> DataList = new List<ChartEntry>();
            foreach (KeyValuePair<string, int> dic in dict)
            {
                DataList.Add(new ChartEntry(dic.Value)
                {
                    Label = dic.Key,
                    ValueLabel = dic.Value.ToString(),
                    Color = SKColor.Parse("#2c3e50")
                });
            }
            var chart = new BarChart { Entries = DataList, LabelTextSize = 45f };
            chartView.Chart = chart;
        }
    }
}