using AsC.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AsC.ViewModels
{
    public class ListaViewModel : BaseViewModel
    {
        #region Propriedades

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (SetProperty(ref isBusy, value))
                    PalavraCommand.ChangeCanExecute();
            }
        }

        public ObservableCollection<string> Palavras { get; }

        public Command PalavraCommand                { get; }

        #endregion

        public ListaViewModel()
        {
            Palavras = new ObservableCollection<string>
            {
                "oi",
                "olá",
                "aupa",
                "opa",
                "e ae",
                "hi"
            };

            PalavraCommand = new AsyncCommand<string>(ExecutePalavraCommand, !IsBusy);
        }

        async Task ExecutePalavraCommand(string arg)
        {
            if (!IsBusy)
            {
                try
                {
                    IsBusy = true;
                    await DisplayAlert("Aviso", $"O texto é {arg}");

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
