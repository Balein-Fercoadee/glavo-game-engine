using GameEngine.GameData;
using GameDatabaseEditor.Utilities;
using System.Globalization;


namespace GameDatabaseEditor
{
    public class IdToObjectConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter">This should be the collection of objects to do an ID search.</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            Object gameObject = null;
            int objectId = (int)value;
            if (objectId > -1)
            {
                if(parameter is Binding binding)
                {
                    parameter = BindingUtilities.GetPropertyValue(binding.Source, binding.Path);
                }

                IEnumerable<IIdentifiable> identifiables = (IEnumerable<IIdentifiable>)parameter;
                var identifiable = identifiables.Where(i => i.Id == objectId).FirstOrDefault();
                gameObject = identifiable;
            }

            return gameObject;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int objectId = ((IIdentifiable)value).Id;
            return objectId;
        }
    }
}
