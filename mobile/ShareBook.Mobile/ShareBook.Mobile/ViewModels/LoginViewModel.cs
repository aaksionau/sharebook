namespace ShareBook.Mobile.ViewModels;

public class LoginViewModel : ViewModel
{
    private string? email;
    public string? Email
    {
        get => email;
        set  {
            if (email != value)
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
    }

    public async Task AuthenticateAsync()
    {
        
    }
}