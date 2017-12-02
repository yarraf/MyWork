using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.Const
{
    public class ValidationEnduit : ValidationAttribute

    {
        public ValidationEnduit()
        {

        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString().Contains(".") && !string.IsNullOrEmpty(value.ToString()))
                {
                    var index = value.ToString().IndexOf('.');
                    if (value.ToString().Substring(0, index).Length > 5)
                    {
                        return false;
                    }
                }
                else if (!string.IsNullOrEmpty(value.ToString()))
                {
                    if (value.ToString().Length > 5)
                    {
                        return false;
                    }
                }
            }


            return true;
        }
    }
}