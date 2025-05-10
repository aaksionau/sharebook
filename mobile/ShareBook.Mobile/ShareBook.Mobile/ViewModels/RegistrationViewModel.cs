using System.Windows.Input;
using Namespace.ShareBook.Mobile.Services;
using ShareBook.Mobile.Models;

namespace ShareBook.Mobile.ViewModels;

public class RegistrationViewModel : ViewModel
{
    private readonly HttpHelperService httpHelperService;

    public RegistrationViewModel(HttpHelperService httpHelperService)
    {
        RegisterModel = new RegisterModel();
        this.httpHelperService = httpHelperService;
    }

    private RegisterModel? _registerModel;

    public RegisterModel? RegisterModel
    {
        get => _registerModel;
        set
        {
            _registerModel = value;
            OnPropertyChanged(nameof(LoginModel));
        }
    }

    public ICommand RegisterCommand { get; set; }

    public async void RegisterAsync()
    {
        var registerResult = await this.httpHelperService.PostAsync<RegisterModel, RegisterModel>(
            "register",
            this.RegisterModel!
        );

        if (registerResult == null)
        {
            // TODO: show snackbar that something is not right
            return;
        }

        await Shell.Current.GoToAsync("/MainPage");
    }
}
