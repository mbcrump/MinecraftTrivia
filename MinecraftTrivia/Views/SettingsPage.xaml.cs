using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using System;

namespace MinecraftTrivia.Views
{
    public sealed partial class SettingsPage : Page
    {
        Template10.Services.SerializationService.ISerializationService _SerializationService;

        public SettingsPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            _SerializationService = Template10.Services.SerializationService.SerializationService.Json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
            MyPivot.SelectedIndex = index;
        }

        private async void btnResetHighScore_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog showDialog = new MessageDialog("Are you REALLY sure that you want to delete your HighScore?");
            showDialog.Commands.Add(new UICommand("Yes")
            {
                Id = 0
            });
            showDialog.Commands.Add(new UICommand("No")
            {
                Id = 1
            });
            showDialog.DefaultCommandIndex = 0;
            showDialog.CancelCommandIndex = 1;
            var result = await showDialog.ShowAsync();
            if ((int)result.Id == 0)
            {
                ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
                localSettings.Values["lastScore"] = 0;
            }
            else
            {
                //skip your task  
            }
        }

    }
}
