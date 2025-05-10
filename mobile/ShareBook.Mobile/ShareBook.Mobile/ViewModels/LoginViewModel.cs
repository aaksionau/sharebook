using System.Windows.Input;
using Namespace.ShareBook.Mobile.Services;
using ShareBook.Mobile.Models;

namespace ShareBook.Mobile.ViewModels;

public class LoginViewModel : ViewModel
{
    public LoginViewModel(HttpHelperService httpHelperService)
    {
        LoginModel = new LoginModel();
        AuthenticateCommand = new Command(AuthenticateAsync);
        this.httpHelperService = httpHelperService;
    }

    private LoginModel? loginModel;
    private readonly HttpHelperService httpHelperService;

    public LoginModel? LoginModel
    {
        get => loginModel;
        set
        {
            loginModel = value;
            OnPropertyChanged(nameof(LoginModel));
        }
    }

    public ICommand AuthenticateCommand { get; private set; }

    public async void AuthenticateAsync()
    {
        if (LoginModel == null)
        {
            // TODO: Handle the case when LoginModel is null
            return;
        }

        var result = await this.httpHelperService.PostAsync<TokenModel, LoginModel>(
            "login",
            this.LoginModel!
        );

        if (string.IsNullOrEmpty(result?.Token))
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
