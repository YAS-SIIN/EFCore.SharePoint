// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SharePoint.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SharePoint.Storage.Internal;

namespace Microsoft.EntityFrameworkCore;

public class SharePointOptionsExtensionTest
{
    [ConditionalFact]
    public void Can_create_options_extension_with_default_values()
    {
        var extension = new SharePointOptionsExtension();

        Assert.Null(extension.SiteUrl);
        Assert.Null(extension.ListName);
        Assert.False(extension.UseClientCredentials);
    }

    [ConditionalFact]
    public void Can_set_site_url()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test");

        Assert.Equal("https://contoso.sharepoint.com/sites/test", extension.SiteUrl);
    }

    [ConditionalFact]
    public void Can_set_list_name()
    {
        var extension = new SharePointOptionsExtension()
            .WithListName("CustomList");

        Assert.Equal("CustomList", extension.ListName);
    }

    [ConditionalFact]
    public void Can_set_use_client_credentials()
    {
        var extension = new SharePointOptionsExtension()
            .WithUseClientCredentials(true);

        Assert.True(extension.UseClientCredentials);
    }

    [ConditionalFact]
    public void Can_chain_configuration_methods()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test")
            .WithListName("Employees")
            .WithUseClientCredentials(true);

        Assert.Equal("https://contoso.sharepoint.com/sites/test", extension.SiteUrl);
        Assert.Equal("Employees", extension.ListName);
        Assert.True(extension.UseClientCredentials);
    }

    [ConditionalFact]
    public void Clone_preserves_all_options()
    {
        var original = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test")
            .WithListName("Employees")
            .WithUseClientCredentials(true);

        // Test cloning by calling a method that internally uses Clone()
        var clone = original.WithSiteUrl("https://contoso.sharepoint.com/sites/test");

        Assert.NotSame(original, clone);
        Assert.Equal(original.SiteUrl, clone.SiteUrl);
        Assert.Equal(original.ListName, clone.ListName);
        Assert.Equal(original.UseClientCredentials, clone.UseClientCredentials);
    }

   
    [ConditionalFact]
    public void ExtensionInfo_LogFragment_includes_configuration()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test")
            .WithListName("Employees")
            .WithUseClientCredentials(true);

        var logFragment = extension.Info.LogFragment;

        Assert.Contains("SiteUrl=https://contoso.sharepoint.com/sites/test", logFragment);
        Assert.Contains("ListName=Employees", logFragment);
        Assert.Contains("UseClientCredentials=True", logFragment);
    }

    [ConditionalFact]
    public void ExtensionInfo_PopulateDebugInfo_includes_all_options()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test")
            .WithListName("Employees")
            .WithUseClientCredentials(true);

        var debugInfo = new Dictionary<string, string>();
        extension.Info.PopulateDebugInfo(debugInfo);

        Assert.Equal("https://contoso.sharepoint.com/sites/test", debugInfo["SharePoint:SiteUrl"]);
        Assert.Equal("Employees", debugInfo["SharePoint:ListName"]);
        Assert.Equal("True", debugInfo["SharePoint:UseClientCredentials"]);
    }

    [ConditionalFact]
    public void ExtensionInfo_PopulateDebugInfo_handles_null_values()
    {
        var extension = new SharePointOptionsExtension();

        var debugInfo = new Dictionary<string, string>();
        extension.Info.PopulateDebugInfo(debugInfo);

        Assert.Equal("(null)", debugInfo["SharePoint:SiteUrl"]);
        Assert.Equal("(null)", debugInfo["SharePoint:ListName"]);
        Assert.Equal("False", debugInfo["SharePoint:UseClientCredentials"]);
    }

    [ConditionalFact]
    public void ExtensionInfo_ShouldUseSameServiceProvider_returns_true_for_identical_options()
    {
        var extension1 = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test")
            .WithListName("Employees")
            .WithUseClientCredentials(true);

        var extension2 = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test")
            .WithListName("Employees")
            .WithUseClientCredentials(true);

        Assert.True(extension1.Info.ShouldUseSameServiceProvider(extension2.Info));
    }

    [ConditionalFact]
    public void ExtensionInfo_ShouldUseSameServiceProvider_returns_false_for_different_site_url()
    {
        var extension1 = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test1");

        var extension2 = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test2");

        Assert.False(extension1.Info.ShouldUseSameServiceProvider(extension2.Info));
    }

    [ConditionalFact]
    public void ExtensionInfo_ShouldUseSameServiceProvider_returns_false_for_different_list_name()
    {
        var extension1 = new SharePointOptionsExtension()
            .WithListName("Employees");

        var extension2 = new SharePointOptionsExtension()
            .WithListName("Departments");

        Assert.False(extension1.Info.ShouldUseSameServiceProvider(extension2.Info));
    }

    [ConditionalFact]
    public void ExtensionInfo_ShouldUseSameServiceProvider_returns_false_for_different_client_credentials()
    {
        var extension1 = new SharePointOptionsExtension()
            .WithUseClientCredentials(true);

        var extension2 = new SharePointOptionsExtension()
            .WithUseClientCredentials(false);

        Assert.False(extension1.Info.ShouldUseSameServiceProvider(extension2.Info));
    }

    [ConditionalFact]
    public void ApplyServices_adds_correct_services()
    {
        var services = new ServiceCollection();
        var extension = new SharePointOptionsExtension();

        extension.ApplyServices(services);

        Assert.Contains(services, sd => sd.ServiceType == typeof(ISharePointConnection));
        Assert.Contains(services, sd => sd.ImplementationType == typeof(SharePointConnection));
    }

    [ConditionalFact]
    public void Validate_throws_for_missing_site_url()
    {
        var extension = new SharePointOptionsExtension();
        var options = new DbContextOptionsBuilder().Options;

        var exception = Assert.Throws<ArgumentException>(() => extension.Validate(options));
        Assert.Contains("SharePoint site URL is required", exception.Message);
    }

    [ConditionalFact]
    public void Validate_passes_with_valid_site_url()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("https://contoso.sharepoint.com/sites/test");
        var options = new DbContextOptionsBuilder().Options;

        // Should not throw
        extension.Validate(options);
    }

    [ConditionalFact]
    public void Validate_throws_for_empty_site_url()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("");
        var options = new DbContextOptionsBuilder().Options;

        var exception = Assert.Throws<ArgumentException>(() => extension.Validate(options));
        Assert.Contains("SharePoint site URL is required", exception.Message);
    }

    [ConditionalFact]
    public void Validate_throws_for_whitespace_site_url()
    {
        var extension = new SharePointOptionsExtension()
            .WithSiteUrl("   ");
        var options = new DbContextOptionsBuilder().Options;

        var exception = Assert.Throws<ArgumentException>(() => extension.Validate(options));
        Assert.Contains("SharePoint site URL is required", exception.Message);
    }

    [ConditionalFact]
    public void Compiled_model_is_thread_safe()
    {
        var tasks = new Task[Environment.ProcessorCount];
        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(
                () =>
                {
                    using var ctx = new EmptyContext();
                    Assert.NotNull(ctx.Model.GetRelationalDependencies());
                });
        }

        Task.WaitAll(tasks);
    }

    private class EmptyContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSharePoint("https://contoso.sharepoint.com/sites/test").UseModel(EmptyContextModel.Instance);
            }
        }
    }

    [DbContext(typeof(EmptyContext))]
    private class EmptyContextModel(bool skipDetectChanges, Guid modelId, int entityTypeCount, int typeConfigurationCount) : RuntimeModel(
        skipDetectChanges, modelId, entityTypeCount, typeConfigurationCount)
    {
        static EmptyContextModel()
        {
            var model = new EmptyContextModel(false, Guid.NewGuid(), 0, 0);
            _instance = model;
        }

        private static readonly EmptyContextModel _instance;

        public static IModel Instance
            => _instance;
    }
}
