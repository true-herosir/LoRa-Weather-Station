using WeatherThingy.Sources.ViewModels;

namespace WeatherThingy.Sources.Views
{
    public partial class DetailPage : ContentPage
    {
        public DetailPage()
        {
            InitializeComponent();
            BindingContext = new DetailViewModel();
        }

        // Example: Trigger ShowData when the page is loaded or when a button is clicked
        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Trigger data fetching when the page appears
            if (BindingContext is DetailViewModel viewModel)
            {
                viewModel.ShowData();
            }
        }

        // Alternatively, you can bind it to a button click
        private void OnLoadDataClicked(object sender, EventArgs e)
        {
            if (BindingContext is DetailViewModel viewModel)
            {
                viewModel.ShowData();
            }
        }
    }
}
