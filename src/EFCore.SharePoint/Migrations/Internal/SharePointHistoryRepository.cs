// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace Microsoft.EntityFrameworkCore.SharePoint.Migrations.Internal;

/// <summary>
/// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
/// the same compatibility standards as public APIs. It may be changed or removed without notice in
/// any release. You should only use it directly in your code with extreme caution and knowing that
/// doing so can result in application failures when updating to a new Entity Framework Core release.
/// </summary>
public class SharePointHistoryRepository : HistoryRepository
{
    /// <summary>
    /// This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    /// the same compatibility standards as public APIs. It may be changed or removed without notice in
    /// any release. You should only use it directly in your code with extreme caution and knowing that
    /// doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public SharePointHistoryRepository(HistoryRepositoryDependencies dependencies)
        : base(dependencies)
    {
    }

    /// <summary>
    /// Gets the name of the migrations history table.
    /// </summary>
    protected override string TableName => "__EFMigrationsHistory";

    /// <summary>
    /// Gets the schema of the migrations history table, or null if not applicable.
    /// </summary>
    protected override string? TableSchema => null;

    /// <summary>
    /// Gets a value indicating the behavior to use when releasing the database lock.
    /// </summary>
    public override LockReleaseBehavior LockReleaseBehavior => LockReleaseBehavior.Explicit;

    /// <summary>
    /// Gets the SQL script to check if the history table exists.
    /// </summary>
    protected override string ExistsSql =>
        $"SELECT OBJECT_ID(N'{SqlGenerationHelper.DelimitIdentifier(TableName)}')";

    /// <summary>
    /// Interprets the result of the exists query.
    /// </summary>
    protected override bool InterpretExistsResult(object? value) => value != DBNull.Value;

    /// <summary>
    /// Gets the script to create the history table if it doesn't exist.
    /// </summary>
    public override string GetCreateIfNotExistsScript() =>
        GetCreateScript();

    /// <summary>
    /// Gets the SQL to start a block that executes if the history table exists.
    /// </summary>
    public override string GetBeginIfExistsScript(string migrationId) =>
        $"IF OBJECT_ID(N'{SqlGenerationHelper.DelimitIdentifier(TableName)}') IS NOT NULL";

    /// <summary>
    /// Gets the SQL to start a block that executes if the history table does not exist.
    /// </summary>
    public override string GetBeginIfNotExistsScript(string migrationId) =>
        $"IF OBJECT_ID(N'{SqlGenerationHelper.DelimitIdentifier(TableName)}') IS NULL";

    /// <summary>
    /// Gets the SQL to end a conditional block.
    /// </summary>
    public override string GetEndIfScript() => "";

    /// <summary>
    /// Acquires a lock on the database.
    /// </summary>
    public override IMigrationsDatabaseLock AcquireDatabaseLock()
    {
        // SharePoint doesn't support database locks, so return a no-op lock
        return new NoOpMigrationsDatabaseLock(this);
    }

    /// <summary>
    /// Asynchronously acquires a lock on the database.
    /// </summary>
    public override Task<IMigrationsDatabaseLock> AcquireDatabaseLockAsync(CancellationToken cancellationToken = default)
    {
        // SharePoint doesn't support database locks, so return a no-op lock
        return Task.FromResult<IMigrationsDatabaseLock>(new NoOpMigrationsDatabaseLock(this));
    }

    /// <summary>
    /// A no-op implementation of IMigrationsDatabaseLock for SharePoint.
    /// </summary>
    private class NoOpMigrationsDatabaseLock : IMigrationsDatabaseLock
    {
        public NoOpMigrationsDatabaseLock(IHistoryRepository historyRepository)
        {
            HistoryRepository = historyRepository;
        }

        public IHistoryRepository HistoryRepository { get; }

        public void Dispose()
        {
            // No-op
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
