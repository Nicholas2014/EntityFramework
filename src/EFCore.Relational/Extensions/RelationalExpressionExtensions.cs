// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore.Internal
{
    public static class RelationalExpressionExtensions
    {
        public static bool IsSimpleExpression([NotNull] this Expression expression)
        {
            var unwrappedExpression = expression.RemoveConvert();

            return unwrappedExpression is ConstantExpression
                   || unwrappedExpression is ColumnExpression
                   || unwrappedExpression is ParameterExpression
                   || unwrappedExpression is ColumnReferenceExpression
                   || unwrappedExpression is AliasExpression;
        }

        public static ColumnReferenceExpression LiftExpressionFromSubquery(this Expression expression, TableExpressionBase table)
            => expression is ColumnExpression columnExpression
                ? new ColumnReferenceExpression(columnExpression, table)
                : (expression is AliasExpression aliasExpression
                    ? new ColumnReferenceExpression(aliasExpression, table)
                    : (expression is ColumnReferenceExpression columnReferenceExpression
                        ? new ColumnReferenceExpression(columnReferenceExpression, table)
                        : null));
    }
}