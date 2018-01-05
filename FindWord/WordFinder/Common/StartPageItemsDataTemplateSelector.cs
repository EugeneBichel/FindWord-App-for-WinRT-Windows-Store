using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FindWord.ViewModels;

namespace FindWord.Common
{
    public class StartPageItemsDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate WordTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is StartPageViewModel)
                return WordTemplate;

            return base.SelectTemplateCore(item, container);
        }
    }
}