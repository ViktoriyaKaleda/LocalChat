using DevExpress.Mvvm;
using LocalChat.Wpf.Validators;
using System.Globalization;
using System.Windows.Input;

namespace LocalChat.Wpf.ViewModels
{
    public class LoginViewModel : BindableBase
    {
        public string Username
        {
            get { return GetProperty(() => Username); }
            set { SetProperty(() => Username, value); }               
        }
    }
}
