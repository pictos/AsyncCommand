using AsC.Helpers;
using AsC.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AsC.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Propriedades

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (SetProperty(ref isBusy, value))
                {
                    NavigationCommand.ChangeCanExecute();
                    ServiceCommand.ChangeCanExecute();
                }
            }
        }


        private bool visible;

        public bool Visible
        {
            get { return visible; }
            set { SetProperty(ref visible, value); }
        }

        public string Retorno { get; private set; }
        public Command NavigationCommand { get; }

        //Testando com ICommand
        public ICommand TaskCommand                { get; }
        public Command ServiceCommand              { get; }

        #endregion

        public MainViewModel()
        {
            Visible           = false;
            ServiceCommand    = new AsyncCommand(ExecuteServiceCommand, !IsBusy);
            TaskCommand       = new AsyncCommand(ExecuteTaskCommand, (!IsBusy || Visible));
            NavigationCommand = new AsyncCommand(ExecuteNavigationCommand, Teste);
        }

        private bool Teste() => !IsBusy;

        async Task ExecuteNavigationCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await PushAsync<ListaViewModel>();

                }
                catch (Exception ex)
                {

                    await DisplayAlert("Erro", $"Erro:{ex.Message}", "Ok");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            return; 
        }

        async Task ExecuteTaskCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await Task.Delay(2000);
                    await DisplayAlert("Aviso", "Esse foi o Task Command");
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Erro", $"Erro:{ex.Message}", "Ok");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            return;
        }

        public async Task ExecuteServiceCommand()
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    Retorno = await ApiService.Current.ObterCepAsync();
                    await DisplayAlert("Retorno", Retorno, "Ok");
                }
                catch (Exception ex)
                {

                    await DisplayAlert("Erro", $"Erro:{ex.Message}", "Ok");
                }
                finally
                {
                    IsBusy = false;
                }
            }
            return;
        }
    }
}
