using System;
using UnityEngine;

namespace Common.Types
{
    public record Date
    {
        public int Year { get; init; }
        public int Day { get; init; }

        public Date() : this(1, 1) { }

        public Date(int day, int year)
        {
            day = Math.Max(1, day);
            var overflowingDaysAsYear = Mathf.FloorToInt((float)day / (DateModel.LastDayOfYear + 1));
            var restDays = Math.Max(1, day % (DateModel.LastDayOfYear + 1));
            Year = overflowingDaysAsYear + year;
            Day = restDays;
        }

        public int AsDays()
        {
            return Day + DateModel.LastDayOfYear * (Year - 1);
        }

        public static bool operator >(Date left, Date right)
        {
            if (left.Year > right.Year) return true;
            if (left.Year < right.Year) return false;
            return left.Day > right.Day;
        }

        public static bool operator <(Date left, Date right)
        {
            if (left.Year < right.Year) return true;
            if (left.Year > right.Year) return false;
            return left.Day < right.Day;
        }

        public static bool operator >=(Date left, Date right)
        {
            return !(left < right);
        }

        public static bool operator <=(Date left, Date right)
        {
            return !(left > right);
        }

        public static Date operator +(Date left, Date right)
        {
            return new Date(left.Day + right.Day, left.Year + right.Year);
        }

        public static Date operator ++(Date date)
        {
            return date + 1;
        }

        public static Date operator -(Date left, Date right)
        {
            return new Date(left.Day - right.Day, left.Year - right.Year);
        }

        public static Date operator +(Date date, int days)
        {
            return new Date(date.Day + days, date.Year);
        }

        public static Date operator -(Date date, int days)
        {
            return date + -days;
        }

        public override string ToString()
        {
            return $"Year: {Year}, Day: {Day}";
        }

        public string ToDisplayString()
        {
            return $"Day {Day} of Year {Year}";
        }
    }
}