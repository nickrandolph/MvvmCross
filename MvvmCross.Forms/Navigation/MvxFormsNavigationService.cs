// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace MvvmCross.Navigation
{
    public class MvxFormsNavigationService : MvxNavigationService
    {
        public MvxFormsNavigationService(IMvxNavigationCache navigationCache, IMvxViewModelLoader viewModelLoader) : base(navigationCache, viewModelLoader)
        {
        }

        public override Task<TResult> Navigate<TViewModel, TParameter, TResult>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Application.Current?.MainPage is Xamarin.Forms.Shell shell)
            {
                shell.GoToAsync(typeof(TViewModel).Name, true);
                return Task.FromResult(default(TResult));
            }
            return base.Navigate<TViewModel, TParameter, TResult>(param, presentationBundle, cancellationToken);
        }

        public override Task<bool> Navigate<TViewModel>(IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Application.Current?.MainPage is Xamarin.Forms.Shell shell)
            {
                shell.GoToAsync(typeof(TViewModel).Name, true);
                return Task.FromResult(true);
            }
            return base.Navigate<TViewModel>(presentationBundle, cancellationToken);
        }

        public override Task<bool> Navigate<TViewModel, TParameter>(TParameter param, IMvxBundle presentationBundle = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Application.Current?.MainPage is Xamarin.Forms.Shell shell)
            {
                shell.GoToAsync(typeof(TViewModel).Name, true);
                return Task.FromResult(true);
            }
            return base.Navigate<TViewModel, TParameter>(param, presentationBundle, cancellationToken);
        }
    }
}
