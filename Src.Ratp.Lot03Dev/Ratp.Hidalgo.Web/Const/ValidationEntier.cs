using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.Const
{
    public class ValidationEntier : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        private int Taille;

        public ValidationEntier(int taille)
        {
            this.Taille = taille;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString().Contains("."))
                {
                    return false;
                }
            }

            return true;
        }
    }
    public class RangeValidation : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        private int TailleMax;

        private int TailleMin;

        public RangeValidation(int min, int max)
        {
            this.TailleMax = max;
            this.TailleMin = min;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                if (value.ToString().Contains("."))
                {
                    var index = value.ToString().IndexOf('.');
                    if (value.ToString().Substring(0, index).Length > TailleMax || value.ToString().Substring(0, index).Length < TailleMin)
                    {
                        return false;
                    }
                }
                else
                {
                    if (value.ToString().Length > TailleMax || value.ToString().Length < TailleMin)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public class ValidationZero : System.ComponentModel.DataAnnotations.ValidationAttribute
    {
        private bool isValidInferieurZero;
        private bool isValidInferieurEgaleZero;

        public ValidationZero(bool validInferieurZero, bool validInferieurEgaleZero)
        {
            isValidInferieurZero = validInferieurZero;
            isValidInferieurEgaleZero = validInferieurEgaleZero;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                //if (value.ToString().Length == 1)
                //{
                int outparam;
                if (int.TryParse(value.ToString(), out outparam))
                {
                    if (outparam <= 0 && isValidInferieurEgaleZero)
                    {
                        return false;
                    }

                    if (outparam < 0 && isValidInferieurZero)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

                //}
            }

            return true;
        }
    }

}