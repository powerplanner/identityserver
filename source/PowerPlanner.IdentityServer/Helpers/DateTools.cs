using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PowerPlanner.IdentityServer.Helpers
{
    public static class DateTools
    {
        public static DateTime Last(DayOfWeek day)
        {
            return Last(day, DateTime.Today);
        }
        /// <summary>
        /// Returns the date that the last specified "day" occurred on, relative to the "from" time. If "day" is Monday and "from" has a DayOfWeek of "Monday", the "from" date will be returned, meaning that it is inclusive of today.
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime Last(DayOfWeek day, DateTime from)
        {
            int subtract;

            int currDay;
            if (from.DayOfWeek >= day)
                currDay = (int)from.DayOfWeek;
            else
                currDay = (int)from.DayOfWeek + 7;

            /// Day = Sunday = 0
            /// From = Monday = 1
            /// Subtract = 0 - 1 = -1
            /// 
            /// Day = Wednesday = 3
            /// From = Saturday = 6
            /// Subtract = 3 - 6 = -3

            subtract = (int)day - currDay;


            /// Day = Saturday = 6
            /// From = Sunday = 0
            /// Subtract = 6 - (0 + 7) = -1
            /// 
            /// Day = Wednesday = 3
            /// From = Monday = 1
            /// Subtract = 3 - (1 + 7) = 5
            /// 

            return from.AddDays(subtract).Date;
        }
    }
}
