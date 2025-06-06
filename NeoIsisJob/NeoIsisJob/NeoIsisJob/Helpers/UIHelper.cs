using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace NeoIsisJob.Helpers
{
    public static class UIHelper
    {
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if (dependencyObject == null)
            {
                yield break;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);
                if (child is T childItem)
                {
                    yield return childItem;
                }

                foreach (var childOfChild in FindVisualChildren<T>(child))
                {
                    yield return childOfChild;
                }
            }
        }
    }
}