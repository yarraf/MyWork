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
    
    public partial class Secteurs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Secteurs()
        {
            this.GroupesOuvrages = new HashSet<GroupesOuvrages>();
            this.PlanificationGroupeOuvrage = new HashSet<PlanificationGroupeOuvrage>();
        }
    
        public int Id { get; set; }
        public string Libelle { get; set; }
        public int IdVisiteType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupesOuvrages> GroupesOuvrages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanificationGroupeOuvrage> PlanificationGroupeOuvrage { get; set; }
    }
}
