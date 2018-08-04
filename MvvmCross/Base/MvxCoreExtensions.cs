﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using MvvmCross.IoC;

namespace MvvmCross.Base
{
    public static class MvxCoreExtensions
    {
        // core implementation of ConvertToBoolean
        public static bool ConvertToBooleanCore(this object result)
        {
            if (result == null)
                return false;

            var s = result as string;
            if (s != null)
                return !string.IsNullOrEmpty(s);

            if (result is bool)
                return (bool)result;

            var resultType = result.GetType();
            if (resultType.GetTypeInfo().IsValueType)
            {
                var underlyingType = Nullable.GetUnderlyingType(resultType) ?? resultType;
                return !result.Equals(underlyingType.CreateDefault());
            }

            return true;
        }

        // core implementation of MakeSafeValue
        public static object MakeSafeValueCore(this Type propertyType, object value)
        {
            if (value == null)
            {
                return propertyType.CreateDefault();
            }

            var safeValue = value;
            if (!propertyType.IsInstanceOfType(value))
            {
                if (propertyType == typeof(string))
                {
                    safeValue = value.ToString();
                }
                else if (propertyType.GetTypeInfo().IsEnum)
                {
                    var s = value as string;
                    safeValue = s != null ? Enum.Parse(propertyType, s, true) : Enum.ToObject(propertyType, value);
                }
                else if (propertyType.GetTypeInfo().IsValueType)
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                    safeValue = underlyingType == typeof(bool) ? value.ConvertToBooleanCore() : ErrorMaskedConvert(value, underlyingType, CultureInfo.CurrentUICulture);
                }
                else
                {
                    safeValue = ErrorMaskedConvert(value, propertyType, CultureInfo.CurrentUICulture);
                }
            }
            return safeValue;
        }

        private static object ErrorMaskedConvert(object value, Type type, CultureInfo cultureInfo)
        {
            try
            {
                return Convert.ChangeType(value, type, cultureInfo);
            }
            catch (Exception)
            {
                // pokemon - mask the error
                return value;
            }
        }

        public static Task AsSafeAwaitable(this Task taskToAwait)
        {
            if (taskToAwait == null)
                return Task.CompletedTask;
            return taskToAwait;
        }

        public static Task SafeInvoke(this Func<Task> function)
        {
            if (function == null)
                return Task.CompletedTask;

            return function();
        }

        public static Task SafeInvoke<TParam>(this Func<TParam, Task> function, TParam param)
        {
            if (function == null)
                return Task.CompletedTask;

            return function(param);
        }
    }
}
