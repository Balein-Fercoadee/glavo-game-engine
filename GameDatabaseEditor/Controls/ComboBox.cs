using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseEditor.Controls
{
    public class ComboBox : Picker
    {
        private string? _selectedValueProperty;

        public string? SelectedValueProperty
        {
            get
            {
                return _selectedValueProperty;
            }
            set
            {
                _selectedValueProperty = value;
            }
        }

        [Bindable(true)]
        public object SelectedValue
        {
            get
            {
                if (_selectedValueProperty == null)
                {
                    return this.SelectedItem;
                }
                else
                {

                }
            }
            set
            {

            }
        }

    }
}
