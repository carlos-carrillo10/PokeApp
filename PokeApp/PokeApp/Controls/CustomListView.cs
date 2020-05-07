using PokeApp.Models.Pokedex;
using PokeApp.Models.PokemonInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PokeApp.Controls
{
    public class CustomListView : ListView
    {
        public CustomListView()
        {
            this.ItemTapped += CustomListView_ItemTapped;
        }

        #region Events

        private void CustomListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            //don't do anything if we just de-selected the row.
            if (e.Item == null) return;
            Task.Delay(500);
            // Deselect the item.
            if (sender is ListView lv) lv.SelectedItem = null;

        }

        #endregion
    }
}
