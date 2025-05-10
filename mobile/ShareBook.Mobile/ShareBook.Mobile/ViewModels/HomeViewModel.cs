using System.Collections.ObjectModel;
using Namespace.ShareBook.Mobile.Models;
using Namespace.ShareBook.Mobile.Services;
using ShareBook.Mobile.ViewModels;

namespace Namespace.ShareBook.Mobile.ViewModels;

public class HomeViewModel : ViewModel
{
    private readonly HttpHelperService httpHelperService;
    private readonly Task initTask;

    public HomeViewModel(HttpHelperService httpHelperService)
    {
        this.httpHelperService = httpHelperService;
        this.initTask = GetLibrariesAsync();
    }

    private ObservableCollection<Library> libraries = new ObservableCollection<Library>();
    public ObservableCollection<Library> Libraries
    {
        get { return libraries; }
        set
        {
            libraries = value;
            OnPropertyChanged(nameof(Libraries));
        }
    }

    public async Task GetLibrariesAsync()
    {
        var libraries = await httpHelperService.GetAsync<List<Library>>("libraries");
        if (libraries == null)
        {
            // TODO: Handle the case when libraries are null
            libraries = new List<Library>();
        }
        Libraries = new ObservableCollection<Library>(libraries);
    }
}
