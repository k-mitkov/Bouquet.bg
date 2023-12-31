﻿using System;
using System.Reflection;
using System.Windows.Input;
//using WarehouseMobile.Enums;
//using WarehouseMobile.Helpers;
//using WarehouseMobile.Interfaces;
//using WarehouseMobile.Resx;
//using WarehouseMobile.Settings;
using Xamarin.Forms;

namespace WarehouseMobile.Commands
{
    public sealed class ExtendedCommand<T> : ExtendedCommand
    {
        public ExtendedCommand(Action<T> execute)
            : base(o =>
            {
                if (IsValidParameter(o))
                {
                    execute((T)o);
                }
            })
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }
        }

        public ExtendedCommand(Action<T> execute, Func<T, bool> canExecute)
            : base(o =>
            {
                if (IsValidParameter(o))
                {
                    execute((T)o);
                }
            }, o => IsValidParameter(o) && canExecute((T)o))
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        static bool IsValidParameter(object o)
        {
            if (o != null)
            {
                // The parameter isn't null, so we don't have to worry whether null is a valid option
                return o is T;
            }

            var t = typeof(T);

            // The parameter is null. Is T Nullable?
            if (Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }

            // Not a Nullable, if it's a value type then null is not valid
            return !t.GetTypeInfo().IsValueType;
        }
    }

    public class ExtendedCommand : ICommand
    {
        readonly Func<object, bool> _canExecute;
        readonly Action<object> _execute;
        readonly WeakEventManager _weakEventManager = new WeakEventManager();
        //private INetworkHelper networkHelper;

        public ExtendedCommand(Action<object> execute)
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));

            _execute = execute;

            //networkHelper = new NetworkHelper(new DatabaseSettingsManager());
        }

        public ExtendedCommand(Action execute) : this(o => execute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
        }

        public ExtendedCommand(Action<object> execute, Func<object, bool> canExecute) : this(execute)
        {
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));

            _canExecute = canExecute;
        }

        public ExtendedCommand(Action execute, Func<bool> canExecute) : this(o => execute(), o => canExecute())
        {
            if (execute == null)
                throw new ArgumentNullException(nameof(execute));
            if (canExecute == null)
                throw new ArgumentNullException(nameof(canExecute));
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute != null)
                return _canExecute(parameter);

            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { _weakEventManager.AddEventHandler(value); }
            remove { _weakEventManager.RemoveEventHandler(value); }
        }

        public void Execute(object parameter)
        {
            //if(SettingsManager.OperationMode == OperationModeEnum.OffLine || networkHelper.IsServerConnected())
            if(true)
            {
                try
                {
                    _execute(parameter);
                }
                catch(Exception)
                {
                    //DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoInternetConnection);
                }
            }
            else
            {
                //DependencyService.Resolve<IMessage>().LongAlert(AppResources.strNoInternetConnection);
            }
        }

        public void ChangeCanExecute()
        {
            _weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));
        }
    }
}