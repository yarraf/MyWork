using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.Const
{
    public static class MyExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetValueConverted(this Decimal? str)
        {
            return str.ToString().Replace(',', '.');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetValueConvertedWitoutdecimal(this Decimal? str)
        {
            if (str.ToString().Contains(","))
            {
                var index = str.ToString().IndexOf(',');
                return str.ToString().Substring(0, index);
            }

            return str.ToString().Replace(',', '.');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetValueConvertedToDecimal(this string str)
        {
            return str.Replace('.', ',');
        }
    }
}