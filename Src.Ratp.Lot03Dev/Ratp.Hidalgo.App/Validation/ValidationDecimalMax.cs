using Ratp.Hidalgo.App.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ratp.Hidalgo.App.Validation
{
    /// <summary>
    /// Classe de validations
    /// </summary>
    public static class ValidationDecimalMax //<T> where T: class
    {
        //public T Poco { get; set; }
        //public  ValidationDecimalMax(T poco)
        //{
        //    this.Poco = poco;
        //}

        /// <summary>
        /// Méthode permettant de valider la taille spécifie pour les paramètres de pondération
        /// </summary>
        /// <param name="valeur">la valeur decimal du paramètre</param>
        /// <param name="msgError">erreur généré</param>
        /// <param name="code">Code du paramètre de pondération</param>
        /// <returns>valeur de type boolean</returns>
        public static bool ValidateDecimal(decimal? valeur, string code, out string msgError)
        {
            int smalldecimal = 5;
            int longueDecimal = 10;
            msgError = string.Empty;

            if (valeur == null)
            {
                msgError = Resource1.MsgErrValidationDecimal;
                return false;
            }

            //Les valeurs négatives ne sont pas acceptées à partir de 0.
            //if (valeur.GetValueOrDefault() < 1)
            //{
            //    msgError = string.Format(Resource1.MsgErrValidationDecimal, code);
            //    return false;
            //}

            int index = 0;

            if (valeur.ToString().Contains(","))
            {
                 index = valeur.ToString().IndexOf(',');
            }
            else
            {
                index = valeur.ToString().Length;
            }
            
            var part1 = valeur.ToString().Substring(0, index);

            switch (code)
            {
                case "Pt":
                    //champ numérique de 5 chiffres
                    if (part1.Length > longueDecimal)
                    {
                        msgError = string.Format(Resource1.MsgErrValidationDecimal, code);
                        return false;
                    }
                    break;
                default:

                    //champ numérique de 5 chiffres
                    if (part1.Length > smalldecimal)

                    {
                        msgError = string.Format(Resource1.MsgErrValidationDecimal, code);
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
