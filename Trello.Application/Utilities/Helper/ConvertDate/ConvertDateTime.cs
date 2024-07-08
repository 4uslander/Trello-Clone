using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello.Application.Utilities.Helper.ConvertDate
{
    public static class ConvertDateTime
    {
        private static readonly TimeZoneInfo TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        public static DateTime? ConvertToSEA(DateTime? dateTime)
        {
            if (!dateTime.HasValue) return null;

            if (dateTime.Value.Kind == DateTimeKind.Utc) return dateTime;

            DateTime dateTimeWithKind = DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(dateTimeWithKind, TimeZoneInfo);
        }
        public static string ConvertDateToString(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd MMM yyyy") : "";
        }

        public static string ConvertDateToStringNumber(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd-MM-yyyy") : "";
        }

        public static string ConvertDateToStringNumberThreeline(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("dd/MM/yyyy") : "";
        }

        public static string ConvertDateToStringForMeeting(DateTime? date)
        {
            return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "";
        }

        public static DateTime ConvertStringToDateTimeExcel(string dateString)
        {
            DateTime dateTime;

            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else
            {
                throw new ArgumentException("Invalid date string");
            }
        }

        public static DateTime ConvertTimeToSEA(DateTime date)
        {
            var seaDateTime = TimeZoneInfo.ConvertTime(date,
                              TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
            return seaDateTime;
        }

        public static List<string> GetDateRange(string startDate, string endDate)
        {
            List<string> dateList = new List<string>();

            DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                dateList.Add(date.ToString("dd/MM/yyyy"));
            }

            return dateList;
        }

        public static List<DateTime> ConvertStringListToDateList(List<string> dateStrings)
        {
            List<DateTime> dateList = new List<DateTime>();

            foreach (string dateString in dateStrings)
            {
                DateTime date;
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    dateList.Add(date);
                }
                else
                {
                    throw new Exception();
                }
            }

            return dateList;
        }
    }
}
