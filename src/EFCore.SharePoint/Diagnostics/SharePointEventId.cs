// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.Diagnostics;

/// <summary>
///     Event IDs for SharePoint events that correspond to messages logged to an <see cref="ILogger" />
///     and events sent to a <see cref="DiagnosticSource" />.
/// </summary>
/// <remarks>
///     <para>
///         These IDs are also used with <see cref="WarningsConfigurationBuilder" /> to configure the
///         behavior of warnings.
///     </para>
///     <para>
///         See <see href="https://aka.ms/efcore-docs-diagnostics">Logging, events, and diagnostics</see> for more information and examples.
///     </para>
/// </remarks>
public static class SharePointEventId
{
    // Warning: These values must not change between releases.
    // Only add new values to the end of sections, never in the middle.
    // Try to use <Noun><Verb> naming and be consistent with existing names.
    private enum Id
    {
        // Model validation events
        ListConfiguredWarning = CoreEventId.ProviderBaseId,
        CompositeKeyWithValueGeneration,

        // Infrastructure events
        UnexpectedConnectionTypeWarning = CoreEventId.ProviderBaseId + 100,

        // Migrations events
        ListRebuildPendingWarning = CoreEventId.ProviderBaseId + 200,

        // Scaffolding events
        FieldFound = CoreEventId.ProviderDesignBaseId,
        LookupFieldFound,
        LookupFieldPrincipalColumnMissingWarning,
        LookupFieldReferencesMissingListWarning,
        IndexFound,
        MissingListWarning,
        PrimaryKeyFound,
        ContentTypesNotSupportedWarning,
        ListFound,
        UniqueConstraintFound,
        InferringTypes,
        OutOfRangeWarning,
        FormatWarning
    }

    private static readonly string ValidationPrefix = DbLoggerCategory.Model.Validation.Name + ".";

    private static EventId MakeValidationId(Id id)
        => new((int)id, ValidationPrefix + id);

    /// <summary>
    ///     A list was configured for an entity type, but SharePoint may not support all list features.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This event is in the <see cref="DbLoggerCategory.Model.Validation" /> category.
    ///     </para>
    /// </remarks>
    public static readonly EventId ListConfiguredWarning = MakeValidationId(Id.ListConfiguredWarning);

    /// <summary>
    ///     An entity type has composite key which is configured to use generated values. SharePoint does not support generated values
    ///     on composite keys.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This event is in the <see cref="DbLoggerCategory.Model.Validation" /> category.
    ///     </para>
    /// </remarks>
    public static readonly EventId CompositeKeyWithValueGeneration = MakeValidationId(Id.CompositeKeyWithValueGeneration);

    private static readonly string InfraPrefix = DbLoggerCategory.Infrastructure.Name + ".";

    private static EventId MakeInfraId(Id id)
        => new((int)id, InfraPrefix + id);

    /// <summary>
    ///     A connection of an unexpected type is being used.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This event is in the <see cref="DbLoggerCategory.Infrastructure" /> category.
    ///     </para>
    /// </remarks>
    public static readonly EventId UnexpectedConnectionTypeWarning = MakeInfraId(Id.UnexpectedConnectionTypeWarning);

    private static readonly string MigrationsPrefix = DbLoggerCategory.Migrations.Name + ".";

    private static EventId MakeMigrationsId(Id id)
        => new((int)id, MigrationsPrefix + id);

    /// <summary>
    ///     An operation may fail due to a pending rebuild of the list.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Migrations" /> category.
    /// </remarks>
    public static readonly EventId ListRebuildPendingWarning = MakeMigrationsId(Id.ListRebuildPendingWarning);

    private static readonly string ScaffoldingPrefix = DbLoggerCategory.Scaffolding.Name + ".";

    private static EventId MakeScaffoldingId(Id id)
        => new((int)id, ScaffoldingPrefix + id);

    /// <summary>
    ///     A field was found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId FieldFound = MakeScaffoldingId(Id.FieldFound);

    /// <summary>
    ///     SharePoint does not support content types in this context.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId ContentTypesNotSupportedWarning = MakeScaffoldingId(Id.ContentTypesNotSupportedWarning);

    /// <summary>
    ///     A lookup field references a missing list.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId LookupFieldReferencesMissingListWarning =
        MakeScaffoldingId(Id.LookupFieldReferencesMissingListWarning);

    /// <summary>
    ///     A list was found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId ListFound = MakeScaffoldingId(Id.ListFound);

    /// <summary>
    ///     The database is missing a list.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId MissingListWarning = MakeScaffoldingId(Id.MissingListWarning);

    /// <summary>
    ///     A field referenced by a lookup field was not found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId LookupFieldPrincipalColumnMissingWarning =
        MakeScaffoldingId(Id.LookupFieldPrincipalColumnMissingWarning);

    /// <summary>
    ///     An index was found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId IndexFound = MakeScaffoldingId(Id.IndexFound);

    /// <summary>
    ///     A lookup field was found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId LookupFieldFound = MakeScaffoldingId(Id.LookupFieldFound);

    /// <summary>
    ///     A primary key was found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId PrimaryKeyFound = MakeScaffoldingId(Id.PrimaryKeyFound);

    /// <summary>
    ///     A unique constraint was found.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId UniqueConstraintFound = MakeScaffoldingId(Id.UniqueConstraintFound);

    /// <summary>
    ///     Inferring CLR types.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId InferringTypes = MakeScaffoldingId(Id.InferringTypes);

    /// <summary>
    ///     Values are out of range for the type.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId OutOfRangeWarning = MakeScaffoldingId(Id.OutOfRangeWarning);

    /// <summary>
    ///     Values are in an invalid format for the type.
    /// </summary>
    /// <remarks>
    ///     This event is in the <see cref="DbLoggerCategory.Scaffolding" /> category.
    /// </remarks>
    public static readonly EventId FormatWarning = MakeScaffoldingId(Id.FormatWarning);
}
