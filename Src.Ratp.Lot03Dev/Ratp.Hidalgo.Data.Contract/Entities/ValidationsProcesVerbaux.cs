//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ratp.Hidalgo.Data.Contract.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ValidationsProcesVerbaux
    {
        public int Id { get; set; }
        public int IdValidation { get; set; }
        public int IdProcesVerbal { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int IdUtilisateur { get; set; }
    
        public virtual ProcesVerbaux ProcesVerbaux { get; set; }
        public virtual Validations Validations { get; set; }
    }
}
