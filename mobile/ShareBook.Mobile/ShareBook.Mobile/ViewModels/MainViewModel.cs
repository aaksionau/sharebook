using Namespace.ShareBook.Mobile.Services;

namespace ShareBook.Mobile.ViewModels;

public class MainViewModel : ViewModel
{
    private readonly HttpHelperService httpHelperService;

    public MainViewModel(HttpHelperService httpHelperService)
    {
        this.httpHelperService = httpHelperService;
    }

    public bool IsAuthenticated => httpHelperService.IsAuthenticated();
}
