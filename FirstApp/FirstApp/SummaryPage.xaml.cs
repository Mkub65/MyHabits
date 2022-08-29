using FirstApp.Model;
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
    public partial class SummaryPage : ContentPage
    {
        private sealed class HabitPlanWithChartData
        {
            public string HabitPlanName { get; set; }
            public string DaysToEnd { get; set; }
            public Dictionary<string, int> KeyValuePairs { get; set; }
            public HabitPlanWithChartData(string Name, Dictionary<string, int> Dict, string daysToEnd)
            {
                HabitPlanName = Name;
                KeyValuePairs = Dict;
                DaysToEnd = daysToEnd;
            }
        }
        private sealed class MyChart
        {
            public string HabitPlanName { get; set; }
            public string DaysToEnd { get; set; }
            public Chart ChartData { get; set; }
            public MyChart(string habitPlanName, string daysToEnd)
            {
                HabitPlanName = habitPlanName;
                DaysToEnd = daysToEnd;
            }
        }
        List<MyChart> MyCharts;
        public SummaryPage()
        {
            InitializeComponent();
            MyCharts = new List<MyChart>();
            var dataForCharts = GetData();
            PopulateCharts(dataForCharts);
        }
        private void PopulateCharts(List<HabitPlanWithChartData> list)
        {
            List<string> chartColors = new List<string> { "#0000FF", "#008000", "#FF0000", "#808080" };
            foreach (var chart in list)
            {
                List<ChartEntry> DataList = new List<ChartEntry>();
                int n = 0;
                foreach (var dic in chart.KeyValuePairs)
                {
                    DataList.Add(new ChartEntry(dic.Value)
                    {
                        Label = dic.Key,
                        ValueLabel = dic.Value.ToString(),
                        Color = SKColor.Parse(chartColors[n]),
                        ValueLabelColor = SKColor.Parse(chartColors[n])
                    });
                    n++;
                }
                var newChart = new DonutChart { Entries = DataList, LabelTextSize = 45f , BackgroundColor = SKColors.Transparent};
                MyCharts.Add(new MyChart(chart.HabitPlanName, chart.DaysToEnd) { ChartData = newChart });
            }
            SummaryListView.ItemsSource = MyCharts;
        }
        private List<HabitPlanWithChartData> GetData()
        {
            List<HabitPlanWithChartData> plansWithData = new List<HabitPlanWithChartData>();
            using (SQLiteConnection conn = new SQLiteConnection(App.DateBaseLocation))
            {
                conn.CreateTable<HabitPlan>();
                var habitPlans = conn.Table<HabitPlan>().ToList();
                List<string> statusVariants = new List<string> { "None", "Done", "NotDone", "Ignore" };
                foreach (var habitPlan in habitPlans)
                {
                    var dictForHabit = new Dictionary<string, int>();
                    var daysToEnd = GetDaysToEnd(habitPlan);
                    var habitTrackers = conn.Table<HabitTracker>().Where(x => x.HabitPlanId == habitPlan.Id);
                    foreach (string variant in statusVariants)
                    {
                        var statusList = habitTrackers.Where(n => n.Status == variant).Count();
                        dictForHabit.Add(variant, statusList);
                    }
                    plansWithData.Add(new HabitPlanWithChartData(habitPlan.HabitPlanName, dictForHabit, daysToEnd));
                }
            }
            return plansWithData;
        }
        public string GetDaysToEnd(HabitPlan HabitPlan)
        {
            if (HabitPlan.StartDate > DateTime.Now)
            {
                TimeSpan daysToEnd = HabitPlan.EndDate - HabitPlan.StartDate;
                return daysToEnd.ToString();
            }
            else
            {
                TimeSpan daysToEnd = HabitPlan.EndDate - DateTime.Now;
                string format = @"dd";
                return daysToEnd.ToString(format);
            }
        }
    }
}