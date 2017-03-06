using System;
using System.Linq;
using System.Reflection;

namespace Shel.Extentions
{
    public static class ShellExtentions
    {
        public static Type[] GetViewModels(this Assembly assembly) => assembly.GetTypes()
            .Where(type => !String.IsNullOrWhiteSpace(type.Namespace) &&
                           type.Namespace.EndsWith("ViewModels") &&
                           type.Name.EndsWith("ViewModel") &&
                           type.GetInterface(nameof(System.ComponentModel.INotifyPropertyChanged)) != null).ToArray();

        public static Type[] GetViews(this Assembly assembly)
            => assembly.GetTypes().Where(type => !String.IsNullOrWhiteSpace(type.Namespace) &&
                                                 type.Namespace.EndsWith("Views") &&
                                                 type.Name.EndsWith("View")).ToArray();
    }
}
