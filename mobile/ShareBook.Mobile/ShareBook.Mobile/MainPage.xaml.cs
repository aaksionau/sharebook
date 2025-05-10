using ShareBook.Mobile.ViewModels;
using ZXing.Net.Maui;

namespace ShareBook.Mobile;

public partial class MainPage : ContentPage
{
    private MainViewModel _viewModel;

    public MainPage(MainViewModel mainViewModel)
    {
        InitializeComponent();

        _viewModel = mainViewModel;
        BindingContext = mainViewModel;

        barcodeReader.Options = new BarcodeReaderOptions()
        {
            Formats = BarcodeFormat.Ean13,
            AutoRotate = true,
            Multiple = true,
        };
    }

    private void CameraBarcodeReaderView_OnBarcodesDetected(
        object? sender,
        BarcodeDetectionEventArgs e
    )
    {
        var result = e.Results?.FirstOrDefault();
        if (result == null)
            return;

        Dispatcher.DispatchAsync(async () =>
            await DisplayAlert("Barcode Detected", result.Value, "OK")
        );
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!_viewModel.IsAuthenticated)
        {
            await Shell.Current.GoToAsync("/LoginPage");
        }
        else
        {
            await Shell.Current.GoToAsync("/HomePage");
        }
    }
}
