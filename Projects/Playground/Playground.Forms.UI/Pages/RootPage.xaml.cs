// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Playground.Core.ViewModels;
using Xamarin.Forms;

namespace Playground.Forms.UI.Pages
{
    [MvxContentPagePresentation(WrapInNavigationPage = true)]

    //[MvxModalPresentation]
    public partial class RootPage : MvxContentPage<RootViewModel>
    {
        public RootPage()
        {
            InitializeComponent();
        }

        private void ShowChildClicked(object sender, System.EventArgs e)
        {
            (Application.Current.MainPage as Shell).GoToAsync(nameof(ChildViewModel));
        }
    }
}
