// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Microsoft;

namespace GuidGen;

/// <summary>
/// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
/// </summary>
public abstract class BindableBase : INotifyPropertyChanged
{
    /// <summary>
    /// Links between properties that are related.
    /// </summary>
    private Dictionary<string, HashSet<string>>? dependentPropertiesMap;

    /// <summary>
    /// Multicast event for property change notifications.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets a value indicating whether this instance is being disposed.
    /// </summary>
    /// <remarks>
    /// Unless overridden by a derived class, this always returns <c>false</c>.
    /// </remarks>
    protected virtual bool IsDisposing
    {
        get { return false; }
    }

    /// <summary>
    /// Checks if a property already matches a desired value.  Sets the property and
    /// notifies listeners only when necessary.
    /// </summary>
    /// <typeparam name="T">Type of the property.</typeparam>
    /// <param name="storage">Reference to a property with both getter and setter.</param>
    /// <param name="value">Desired value for the property.</param>
    /// <param name="propertyName">Name of the property used to notify listeners.  This
    /// value is optional and can be provided automatically when invoked from compilers that
    /// support CallerMemberName.</param>
    /// <returns>True if the value was changed, false if the existing value matched the
    /// desired value.</returns>
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (typeof(T).IsClass)
        {
            if (object.ReferenceEquals(storage, value))
            {
                return false;
            }
        }
        else
        { // struct
            if (EqualityComparer<T>.Default.Equals(storage, value))
            {
                return false;
            }
        }

        storage = value;
        this.OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Notifies listeners that a property value has changed.
    /// </summary>
    /// <param name="propertyName">Name of the property used to notify listeners.  This
    /// value is optional and can be provided automatically when invoked from compilers
    /// that support <see cref="CallerMemberNameAttribute"/>.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        Requires.NotNull(propertyName!, nameof(propertyName));
        if (this.IsDisposing)
        {
            return;
        }

        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // The field could actually be null because the DataContractSerializer
        // skips the constructor when deserializing.
        if (this.dependentPropertiesMap != null && this.dependentPropertiesMap.TryGetValue(propertyName, out HashSet<string> dependentProperties))
        {
            foreach (var dependentProperty in dependentProperties)
            {
                this.OnPropertyChanged(dependentProperty);
            }
        }
    }

    /// <summary>
    /// Registers one property as a dependent property such that changed events for one
    /// causes the changed events of the other.
    /// </summary>
    /// <param name="baseProperty">The property with backing field that may change..</param>
    /// <param name="dependentProperty">The property that that derives its value from <paramref name="baseProperty"/>.</param>
    protected void RegisterDependentProperty(string baseProperty, string dependentProperty)
    {
        Requires.NotNullOrEmpty(baseProperty, "baseProperty");
        Requires.NotNullOrEmpty(dependentProperty, "dependentProperty");

        if (this.dependentPropertiesMap == null)
        {
            this.dependentPropertiesMap = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
        }

        if (!this.dependentPropertiesMap.TryGetValue(baseProperty, out HashSet<string> dependentProperties))
        {
            this.dependentPropertiesMap[baseProperty] = dependentProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        dependentProperties.Add(dependentProperty);
    }
}
