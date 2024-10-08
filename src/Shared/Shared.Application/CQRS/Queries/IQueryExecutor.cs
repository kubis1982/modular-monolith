﻿namespace ModularMonolith.Shared.CQRS.Queries
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IQueryExecutor
    {
        Task<TResult> Execute<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}
