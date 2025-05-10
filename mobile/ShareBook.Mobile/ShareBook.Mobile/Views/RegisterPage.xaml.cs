using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareBook.Mobile.ViewModels;

namespace ShareBook.Mobile.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegistrationViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}