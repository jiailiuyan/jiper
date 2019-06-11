using System;
using System.Collections.Generic;
using System.Linq;

namespace Ji.CommonHelper.Helpers
{
    /// <summary>
    /// Class PowerHelper.
    /// </summary>
    public class PowerHelper
    {
        #region Public 方法

        /// <summary>
        /// Adds the power.
        /// </summary>
        /// <param name="powers">The powers.</param>
        /// <param name="power">The power.</param>
        public static void AddPower(ulong[] powers, int power)
        {
            powers[power / 63] |= (ulong)Math.Pow(2, power % 63);
        }

        public static void AddPowers(ulong[] powers, IEnumerable<int> power)
        {
            foreach (var item in power)
            {
                AddPower(powers, item);
            }
        }

        public static bool ContainsAny(ulong[] powers, IEnumerable<int> longs)
        {
            foreach (var item in longs)
            {
                if (IsExitPower(powers, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is exit power] [the specified powers].
        /// </summary>
        /// <param name="powers">The powers.</param>
        /// <param name="power">The power.</param>
        /// <returns><c>true</c> if [is exit power] [the specified powers]; otherwise, <c>false</c>.</returns>
        public static bool IsExitPower(ulong[] powers, int power)
        {
            if (powers != null && powers.Length > power / 63)
            {
                return (powers[power / 63] & (ulong)Math.Pow(2, power % 63)) == (ulong)Math.Pow(2, power % 63);
            }
            return false;
        }

        /// <summary>
        /// Removes the power.
        /// </summary>
        /// <param name="powers">The powers.</param>
        /// <param name="power">The power.</param>
        public static void RemovePower(ref ulong[] powers, ulong power)
        {
            powers[power / 63] &= ~((ulong)Math.Pow(2, power % 63));
        }

        public static string PowersToString(ulong[] powers)
        {
            try
            {
                string res = string.Empty;
                for (int i = 0; i < powers.Length - 1; i++)
                {
                    if (powers[i] != 0)
                    {
                        res += string.Format("{0};{1},", i, powers[i]);
                    }
                }
                if (powers[powers.Length - 1] != 0)
                {
                    res += string.Format("{0};{1}", powers.Length - 1, powers[powers.Length - 1]);
                }
                return res;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static ulong[] StringToPowers(string powersString)
        {
            try
            {
                if (powersString != null)
                {
                    var values = powersString.Split(',');
                    Dictionary<int, ulong> dic = new Dictionary<int, ulong>();
                    foreach (var item in values)
                    {
                        if (!string.IsNullOrEmpty(item) && item != "null")
                        {
                            var values1 = item.Split(';');
                            dic.Add(int.Parse(values1[0]), ulong.Parse(values1[1]));
                        }
                    }
                    if (dic.Count > 0)
                    {
                        ulong[] res = new ulong[dic.Max(i => i.Key) + 1];
                        foreach (var item in dic)
                        {
                            res[item.Key] = item.Value;
                        }
                        return res;
                    }
                }
            }
            catch { }
            return null;
        }

        #endregion Public 方法
    }
}