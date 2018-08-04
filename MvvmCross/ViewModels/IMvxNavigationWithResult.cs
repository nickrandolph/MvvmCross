// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;

namespace MvvmCross.ViewModels
{
    public interface IMvxNavigationWithResult<TResult>
    {
        Func<Task<TResult>> OnCompleted { get; set; }
    }

    public interface IMvxNavigationWithResult<TParameter, TResult>
    {
        Func<TParameter, Task<TResult>> OnCompletedWithParameter { get; set; }
    }
}
