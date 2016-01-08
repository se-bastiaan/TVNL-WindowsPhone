using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WindowsPhone81TVNL.Helpers
{
    public static class SettingsHelper
    {
        /// <summary> 
        /// Function to read a setting value
        /// </summary> 
        public static T ReadSettingsValue<T>(string key)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Debug.WriteLine("null returned");
                return default(T);
            }
            else
            {
                T value = (T)ApplicationData.Current.LocalSettings.Values[key];
                Debug.WriteLine("value found " + value.ToString());
                return value;
            }
        }

        /// <summary> 
        /// Function to read a setting value with default return
        /// </summary> 
        public static T ReadSettingsValue<T>(string key, T defaultReturn)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Debug.WriteLine("null returned");
                return defaultReturn;
            }
            else
            {
                T value = (T)ApplicationData.Current.LocalSettings.Values[key];
                Debug.WriteLine("value found " + value.ToString());
                return value;
            }
        }

        /// <summary> 
        /// Function to read a setting value and clear it after reading it 
        /// </summary> 
        public static T ReadResetSettingsValue<T>(string key)
        {
            Debug.WriteLine(key);
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Debug.WriteLine("null returned");
                return default(T);
            }
            else
            {
                T value = (T)ApplicationData.Current.LocalSettings.Values[key];
                ApplicationData.Current.LocalSettings.Values.Remove(key);
                Debug.WriteLine("value found " + value.ToString());
                return value;
            }
        }

        /// <summary> 
        /// Save a key value pair in settings. Create if it doesn't exist 
        /// </summary> 
        public static void SaveSettingsValue(string key, object value)
        {
            Debug.WriteLine(key + ":" + value.ToString());
            if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                ApplicationData.Current.LocalSettings.Values.Add(key, value);
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values[key] = value;
            }
        }
    }
}
