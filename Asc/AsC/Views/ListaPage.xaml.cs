using AsC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AsC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListaPage : ContentPage
	{
		public ListaPage ()
		{
			InitializeComponent ();
		}
        //Subir

        private void Lista_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            
            if (BindingContext is ListaViewModel vm)
                vm.PalavraCommand.Execute((string)e.Item);
        }
    }
}