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
    
    public partial class Lieux
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lieux()
        {
            this.Ouvrages = new HashSet<Ouvrages>();
            this.Ouvrages1 = new HashSet<Ouvrages>();
            this.PlanificationGroupeOuvrage = new HashSet<PlanificationGroupeOuvrage>();
        }
    
        public int Id { get; set; }
        public int IdLigneProprietaire { get; set; }
        public int IdLigneGestionnaire { get; set; }
        public int IdLieuType { get; set; }
        public string CodePatrimonial { get; set; }
        public Nullable<bool> SurveillanceTopo { get; set; }
        public bool RecoOA11 { get; set; }
        public bool RecoAcces { get; set; }
        public string RecoAutres { get; set; }
        public int IdDescription { get; set; }
        public Nullable<bool> Actif { get; set; }
        public Nullable<double> SurfCarrelage { get; set; }
        public Nullable<int> Ordre { get; set; }
    
        public virtual Descriptions Descriptions { get; set; }
        public virtual Lignes Lignes { get; set; }
        public virtual Lignes Lignes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ouvrages> Ouvrages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ouvrages> Ouvrages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlanificationGroupeOuvrage> PlanificationGroupeOuvrage { get; set; }
        public virtual LieuxTypes LieuxTypes { get; set; }
    }
}
