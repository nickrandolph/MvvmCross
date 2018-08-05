﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Localization;
using MvvmCross.ViewModels;
using Playground.Core.Services;
using Playground.Core.ViewModels;

namespace Playground.Core
{
    public class App : MvxApplication
    {
        private int ClickCount { get; set; }
        /// <summary>
        /// Breaking change in v6: This method is called on a background thread. Use
        /// Startup for any UI bound actions
        /// </summary>
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.IoCProvider.RegisterSingleton<IMvxTextProvider>(new TextProviderBuilder().TextProvider);

            RegisterAppStart<RootViewModel>();

            // Note: The goal is to have the registration of navigation to be more fluent e.g:
            // this.When<RootViewModel>().IsCompleted().NavigateTo<ChildViewModel>();
            // this.When<RootViewModel>().IsCompleted().And(vm=> ++ClickCount==2).NavigateTo<ChildViewModel>();
            // this.When<ChildViewModel>().IsCompleted().CloseViewModel();
            // this.When<ChildViewModel>().Requests(NextViewModel.Next).NavigateTo<SecondChildViewModel>().WithParameter<ParameterType>();
            // this.When<ChildViewModel>().Requests(NextViewModel.Completed).CloseViewModel();
            // this.When<ChildViewModel>().RequestsInput().From<InputViewModel>();


            RegisterNavigation<RootViewModel, ChildViewModel>(vm=> ++ClickCount==2);
            RegisterCompletion<ChildViewModel>();
            RegisterNext<ChildViewModel, SecondChildViewModel, NextViewModel>(NextViewModel.Next);
            RegisterNextCompletion<ChildViewModel, NextViewModel>(NextViewModel.Completed);
        }

        /// <summary>
        /// Do any UI bound startup actions here
        /// </summary>
        public override Task Startup()
        {
            return base.Startup();
        }

        /// <summary>
        /// If the application is restarted (eg primary activity on Android 
        /// can be restarted) this method will be called before Startup
        /// is called again
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }
    }
}
