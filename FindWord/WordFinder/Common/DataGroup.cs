using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FindWord.Common
{
    public class DataGroup : BindableBase
    {
        public string ID { get { return "AllGroups"; } }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string SubTitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private ObservableCollection<Object> _items = new ObservableCollection<Object>();
        public ObservableCollection<Object> Items
        {
            get { return new ObservableCollection<object>(this._items.Take(50)); }
            set { _items = value; }
        }

        public IEnumerable<Object> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(6); }
        }
    }
}
