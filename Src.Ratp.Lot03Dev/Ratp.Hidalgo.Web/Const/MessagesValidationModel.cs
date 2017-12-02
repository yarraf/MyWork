using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratp.Hidalgo.Web.Const
{
    /// <summary>
    /// Les messages de validation
    /// </summary>
    public static class MessagesValidationModel
    {
        public const string MsgErrValidationRequired = " Veuillez renseigner ce champ.";
        public const string MsgErrValidationRange = " Veuillez renseigner une valeur de 5 chiffre.";
        public const string MsgErrValidationType = " Veuillez renseigner une valeur correct";
        public const string MsgErrValidationEnduit = " Veuillez renseigner une valeur supérieure ou égale 0";
        public const string MsgErrValidationMaçonnerie = " Veuillez renseigner une valeur supérieure ou égale 1";
        public const string MsgErrValidationRangeEntier = " Veuillez renseigner une valeur max de 10 chiffre.";
        public const string MsgErrValidationStandard = " Valeur saisie est invalide.";
        public const string MsgErrValidationV = "La valeur du seuil de veto doit être strictement supérieure à 0";
        public const string MsgErrValidationRange2 = " Veuillez renseigner une valeur de 10 chiffre maximum.";
        public const string MsgErrValidationRange3 = " Veuillez renseigner une valeur de 15 chiffre maximum.";
    }
}