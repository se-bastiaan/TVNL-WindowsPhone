using System;
using System.Collections.Generic;
using System.Globalization;

namespace WindowsPhone81TVNL.Helpers
{
    public class TimeKeyGroup<T>
    {
        /// <summary>
        /// The delegate that is used to get the key information.
        /// </summary>
        /// <param name="item">An object of type T</param>
        /// <returns>The key value to use for this object</returns>
        public delegate DateTime GetKeyDelegate(T item);

        /// <summary>
        /// The Key of this group.
        /// </summary>
        public string Key { get; set; }

        public List<T> Items
        {
            get;
            set;
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="key">The key for this group.</param>
        public TimeKeyGroup(string key)
        {
            Key = key;
            Items = new List<T>();
        }

        public override string ToString()
        {
            return Key;
        }

        public static List<TimeKeyGroup<T>> CreateGroups(List<T> items, GetKeyDelegate getKey, bool sort)
        {
            return CreateGroups(items, null, getKey, sort);
        }

        /// <summary>
        /// Create a list of AlphaGroup<T> with keys set by a SortedLocaleGrouping.
        /// </summary>
        /// <param name="items">The items to place in the groups.</param>
        /// <param name="ci">The CultureInfo to group and sort by.</param>
        /// <param name="getKey">A delegate to get the key from an item.</param>
        /// <param name="sort">Will sort the data if true.</param>
        /// <returns>An items source for a LongListSelector</returns>
        public static List<TimeKeyGroup<T>> CreateGroups(List<T> items, CultureInfo ci, GetKeyDelegate getKey, bool sort)
        {
            items.Sort((t1, t2) => getKey(t1).CompareTo(getKey(t2)));

            DateTime currentTime = DateTime.Today;
            List<TimeKeyGroup<T>> list = new List<TimeKeyGroup<T>>();

            TimeKeyGroup<T> group = null;

            for (int i = 0; i < items.Count; i++)
            {
                if (group == null) group = new TimeKeyGroup<T>(currentTime.ToString("dddd"));

                T item = items[i];
                DateTime itemTime = getKey(item);
                DateTime nextTime = currentTime.AddDays(1);

                if (itemTime > currentTime && itemTime < nextTime)
                {
                    group.Items.Add(item);
                }
                else if (itemTime > nextTime)
                {
                    list.Add(group);
                    group = null;
                    currentTime = nextTime;
                    nextTime = currentTime.AddDays(1);
                    i--;
                }
            }

            return list;
        }
    }
}
