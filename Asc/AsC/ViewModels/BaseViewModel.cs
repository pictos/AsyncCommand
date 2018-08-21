using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AsC.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public static CultureInfo culture = new CultureInfo("pt-BR");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        public async Task DisplayAlert(string title, string message, string cancel = "Ok") =>
          await Application.Current.MainPage.DisplayAlert(title, message, cancel);

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel) =>
            await Application.Current.MainPage.DisplayAlert(title, message, accept, cancel);

        #region Navegação
        public async Task PushAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var page = Locator<TViewModel>(args);

            await Application.Current.MainPage.Navigation.PushAsync(page);
            await (page.BindingContext as BaseViewModel).InitializeAsync(args);
        }
        static Page Locator<TViewModel>(object[] args) where TViewModel : BaseViewModel
        {
            var viewModelType = typeof(TViewModel);
            var viewModelTypeName = viewModelType.Name;
            var viewType = VerificarPage(viewModelType);
            Page page;
            if (viewType == null)
            {
                var name = typeof(BaseViewModel).AssemblyQualifiedName.Split('.')[0];

                var viewTypeName = $"{name}.Views.{viewModelTypeName.Substring(0, viewModelTypeName.Length - 9)}Page";
                viewType = Type.GetType(viewTypeName);
                page = Activator.CreateInstance(viewType) as Page;
                CriarMapeamento(page.GetType(), viewModelType);

            }
            else
            {
                page = Activator.CreateInstance(viewType) as Page;
            }

            //Não precisa mais passar argumentos ao construtor
            var viewModel = Activator.CreateInstance(viewModelType/*, args*/);
            if (page != null)
                page.BindingContext = viewModel;

            return page;
        }

        public async Task PopAsync() =>
            await Application.Current.MainPage.Navigation.PopAsync();

        public async Task PopToRootAsync() =>
            await Application.Current.MainPage.Navigation.PopToRootAsync();

        public async Task PushModalAsync<TViewModel>(params object[] args) where TViewModel : BaseViewModel
        {
            var page = Locator<TViewModel>(args);

            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            await (page.BindingContext as BaseViewModel).InitializeAsync(args);
        }

        public async Task PopModalAsync()
            => await Application.Current.MainPage.Navigation.PopModalAsync();

        public virtual Task InitializeAsync(object[] args) =>
            Task.FromResult(false);

        public Task RemovePage(Type page)
        {
            var listaPagina = new List<Page>();
            var pagina = Application.Current.MainPage.Navigation.NavigationStack;
            foreach (var item in pagina)
            {
                if (item.GetType() == page)
                    listaPagina.Add(item);
            }
            foreach (var item in listaPagina)
                Application.Current.MainPage.Navigation.RemovePage(item);

            return Task.FromResult(true);
        }

        public static void SetarMainPage<TViewModel>(params object[] args ) where TViewModel : BaseViewModel
        {
            var page = Locator<TViewModel>(args);
            if (App.Current.MainPage == null)
            {
                App.Current.MainPage = new NavigationPage(page);

                (page.BindingContext as BaseViewModel).InitializeAsync(args);
            }
        }

        protected static readonly Dictionary<Type, Type> mapeamento = new Dictionary<Type, Type>();

        static void CriarMapeamento(Type page, Type vm)
        {
            //if (VerificarPage(page.GetType()) == null)
            //{
            //var vm = (page.BindingContext).GetType();
            mapeamento.Add(vm, page);
            //}
        }

        static Type VerificarPage(Type vm)
        {
            if (!mapeamento.ContainsKey(vm))
                return null;

            return mapeamento[vm];
        }

        #endregion
    }
}