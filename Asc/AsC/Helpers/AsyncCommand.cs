using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AsC.Helpers
{
    //Pra subir
    public class AsyncCommand : Command
    {
        public AsyncCommand(Func<Task> execute, bool CanExecute) : base(() => execute(), () => CanExecute)
        {

        }
        public AsyncCommand(Func<Task> execute) : base(() => execute())
        {
        }

        public AsyncCommand(Func<Task> execute, Func<bool> canExecute) : base(() => execute(), () => canExecute())
        {
        }

        public AsyncCommand(Func<object, Task> execute, Func<object, bool> canExecute)
            : base((o) => execute(o), (o) => canExecute(o))
        {
        }

        public AsyncCommand(Func<object, Task> execute)
            : base(o => execute(o))
        {
        }

        //public async void Execute(object args)
        //{
        //    var t = Task.Run(()=> base.Execute(args));
        //    await t;
        //}
    }

    public sealed class AsyncCommand<T> : Command
    {
        public AsyncCommand(Func<T, Task> execute, Func<object, bool> canExecute)
            : base(async a => await execute((T)a), (a) => canExecute(a))
        {
        }

        public AsyncCommand(Func<T, Task> execute, bool canExecute)
            : base(async o => await execute((T)o), (o) => canExecute)
        {
        }

        //public Task MyExecute(object args)
        //{
        //    Task.Run(() => base.Execute(args));
        //    return Task.FromResult(true);
        //  //  await t;
        //}
    }
}
