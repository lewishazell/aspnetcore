// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.AspNetCore.Components;

/// <summary>
/// Builder used to configure a <see cref="RazorComponentApplication"/> instance.
/// </summary>
public class ComponentApplicationBuilder
{
    private readonly HashSet<string> _assemblies = new();
    private readonly PageCollectionBuilder _pageCollectionBuilder = new();
    private readonly ComponentCollectionBuilder _componentCollectionBuilder = new();

    /// <summary>
    /// Adds a given assembly and associated pages and components to the build.
    /// </summary>
    /// <param name="libraryBuilder">The assembly with the pages and components.</param>
    /// <exception cref="InvalidOperationException">When the assembly has already been added
    /// to this component application builder.
    /// </exception>
    public void AddLibrary(ComponentLibraryBuilder libraryBuilder)
    {
        if (_assemblies.Contains(libraryBuilder.Name))
        {
            throw new InvalidOperationException("Assembly already defined.");
        }
        _assemblies.Add(libraryBuilder.Name);
        _pageCollectionBuilder.AddFromLibraryInfo(libraryBuilder.Pages);
        _componentCollectionBuilder.AddFromLibraryInfo(libraryBuilder.Components);
    }

    /// <summary>
    /// Builds the component application definition.
    /// </summary>
    /// <returns>The <see cref="RazorComponentApplication"/>.</returns>
    public RazorComponentApplication Build()
    {
        return new RazorComponentApplication(
            _pageCollectionBuilder.ToPageCollection(),
            _componentCollectionBuilder.ToComponentCollection());
    }

    /// <summary>
    /// Indicates whether the current <see cref="ComponentApplicationBuilder"/> instance
    /// has the given <paramref name="assemblyName"/>.
    /// </summary>
    /// <param name="assemblyName">The name of the assembly to check.</param>
    /// <returns><c>true</c> when present; <c>false</c> otherwise.</returns>
    public bool HasAssembly(string assemblyName)
    {
        return _assemblies.Contains(assemblyName);
    }

    /// <summary>
    /// Combines the two <see cref="ComponentApplicationBuilder"/> instances.
    /// </summary>
    /// <param name="other">The <see cref="ComponentApplicationBuilder"/> to merge.</param>
    public void Combine(ComponentApplicationBuilder other)
    {
        _assemblies.UnionWith(other._assemblies);
        Pages.Combine(other.Pages);
        Components.Combine(other.Components);
    }

    /// <summary>
    /// Excludes the assemblies and other definitions in <paramref name="builder"/> from the
    /// current <see cref="ComponentApplicationBuilder"/>.
    /// </summary>
    /// <param name="builder"></param>
    public void Exclude(ComponentApplicationBuilder builder)
    {
        _assemblies.ExceptWith(builder._assemblies);
        Pages.Exclude(builder.Pages);
        Components.Exclude(builder.Components);
    }

    /// <summary>
    /// Removes the given <paramref name="assembly"/> and the associated definitions from
    /// the current <see cref="ComponentApplicationBuilder"/>.
    /// </summary>
    /// <param name="assembly">The assembly name.</param>
    public void Remove(string assembly)
    {
        _assemblies.Remove(assembly);
        Pages.RemoveFromAssembly(assembly);
        Components.Remove(assembly);
    }

    /// <summary>
    /// Gets the page component collection available in this builder instance.
    /// </summary>
    public PageCollectionBuilder Pages { get; } = new PageCollectionBuilder();

    /// <summary>
    /// Gets the component collection available in this builder instance.
    /// </summary>
    public ComponentCollectionBuilder Components { get; } = new ComponentCollectionBuilder();
}