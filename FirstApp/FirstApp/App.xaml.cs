using Xamarin.Forms;

namespace FirstApp
{
    public partial class App : Application
    {
        public static string DateBaseLocation = string.Empty;
        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new SelectPage());
        }

        public App(string dataBaseLocation)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new SelectPage());
            DateBaseLocation = dataBaseLocation;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
