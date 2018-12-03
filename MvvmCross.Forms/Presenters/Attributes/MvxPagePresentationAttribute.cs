// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;

namespace MvvmCross.Forms.Presenters.Attributes
{
    public interface IMvxPagePresentationAttribute: IMvxPresentationAttribute
    {
        /// <summary>
        /// ViewModel Type to show as host before showing the actual view. Optional when not using switching between Forms views and native views.
        /// </summary>
        Type HostViewModelType { get; set; }

        /// <summary>
        /// Wraps the Page in a MvxNavigationPage if set to true. If the current stack already is a MvxNavigationPage it will push the Page onto that.
        /// </summary>
        /// <value><c>true</c> if wrap in navigation page; otherwise, <c>false</c>.</value>
        bool WrapInNavigationPage { get; set; } 

        /// <summary>
        /// Clears the backstack of the current NavigationPage when set to true
        /// </summary>
        /// <value><c>true</c> if no history; otherwise, <c>false</c>.</value>
        bool NoHistory { get; set; } 

        bool Animated { get; set; } 

        /// <summary>
        /// Ensures any open modals are closed before navigating to the new page
        /// </summary>
        /// <value><c>true</c> if open modals are to be closed; otherwise, <c>false</c>.</value>
        bool CloseAnyOpenModals { get; set; }

        string Title { get; set; }

        string Icon { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public abstract class MvxPagePresentationAttribute : MvxBasePresentationAttribute, IMvxPagePresentationAttribute
    {
        public MvxPagePresentationAttribute()
        {
        }

        /// <summary>
        /// ViewModel Type to show as host before showing the actual view. Optional when not using switching between Forms views and native views.
        /// </summary>
        public virtual Type HostViewModelType { get; set; }

        /// <summary>
        /// Wraps the Page in a MvxNavigationPage if set to true. If the current stack already is a MvxNavigationPage it will push the Page onto that.
        /// </summary>
        /// <value><c>true</c> if wrap in navigation page; otherwise, <c>false</c>.</value>
        public virtual bool WrapInNavigationPage { get; set; } = true;

        /// <summary>
        /// Clears the backstack of the current NavigationPage when set to true
        /// </summary>
        /// <value><c>true</c> if no history; otherwise, <c>false</c>.</value>
        public virtual bool NoHistory { get; set; } = false;

        public virtual bool Animated { get; set; } = true;

        /// <summary>
        /// Ensures any open modals are closed before navigating to the new page
        /// </summary>
        /// <value><c>true</c> if open modals are to be closed; otherwise, <c>false</c>.</value>
        public virtual bool CloseAnyOpenModals { get; set; } = true;

        public virtual string Title { get; set; }

        public virtual string Icon { get; set; }
    }
}
