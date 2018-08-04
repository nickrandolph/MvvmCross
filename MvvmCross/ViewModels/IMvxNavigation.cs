// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.ViewModels
{
    public interface IMvxViewModelCompleted
    {
        Func<Task> OnCompleted { get; set; }
    }

    public interface IMvxViewModelCompleted<TParameter>
    {
        Func<TParameter, Task> OnCompletedWithParameter { get; set; }
    }

    public enum NextViewModel
    {
        Next,
        Completed
    }

    public interface IMvxNextViewModel<TNext> where TNext:struct
    {
        Func<TNext, Task> OnNext { get; set; }
    }

    public interface IMvxNextViewModel<TNext, TParameter> where TNext : struct
    {
        Func<TNext, TParameter, Task> OnNextParameter { get; set; }
    }
}
