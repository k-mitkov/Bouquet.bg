﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Bouquet.Mobile.Converters
{
    public class EventToCommandBehavior : BaseBehavior<VisualElement>
    {
        #region Declarations

        Delegate eventHandler;
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));

        #endregion

        #region Properties

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        #endregion

        #region Methods

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        protected static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;

            if (behavior.AssociatedObject == null) return;

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }

        protected void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", EventName));

            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        protected void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || eventHandler == null)
                return;

            EventInfo eventInfo = AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", EventName));

            eventInfo.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        protected void OnEvent(object sender, object eventArgs)
        {
            if (Command == null) return;

            object resolvedParameter;

            resolvedParameter = CommandParameter;

            if (eventArgs != null && eventArgs != EventArgs.Empty)
            {
                resolvedParameter = eventArgs;
            }

            if (Command.CanExecute(resolvedParameter))
                Command.Execute(resolvedParameter);
        }

        #endregion
    }
}
