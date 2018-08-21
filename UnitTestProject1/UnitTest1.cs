using System;
using System.Threading.Tasks;
using AsC.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        readonly MainViewModel _mainVM = new MainViewModel();

        [TestMethod]
        public void TestMainViewModel()
        {
            //_mainVM.NavigationCommand.Execute(null);
            //_mainVM.TaskCommand.Execute(null);
            //Task.Run(() => _mainVM.ServiceCommand.Execute(null))
            //    .ContinueWith((t) =>
            //    {
            //        var result = @"{'status':'1','code':'33400-000','state':'RJ','city':'Lagoa Santa','district':'','address':''}";
            //        var equals = (result == _mainVM.Retorno);
            //        Assert.IsTrue(equals);
            //    });

            var t = Task.Run(()=> _mainVM.ExecuteServiceCommand());

            Task.WaitAny(t);


            var result = @"{'status':'1','code':'33400-000','state':'MG','city':'Lagoa Santa','district':'','address':''}";
            //   var equals = ();
            Assert.IsTrue(result == _mainVM.Retorno);


        }
    }
}
