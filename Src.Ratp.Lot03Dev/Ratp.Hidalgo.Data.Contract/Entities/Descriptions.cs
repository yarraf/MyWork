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
    
    public partial class Descriptions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Descriptions()
        {
            this.Lieux = new HashSet<Lieux>();
            this.Ouvrages = new HashSet<Ouvrages>();
        }
    
        public int Id { get; set; }
        public int IdVisiteType { get; set; }
        public string Libelle { get; set; }
        public string PkDebut { get; set; }
        public string PkFin { get; set; }
        public string DescriptionComp { get; set; }
        public string CoordonneesEnv { get; set; }
        public string CoordonneesConc { get; set; }
        public string ConsignesSecurite { get; set; }
        public Nullable<bool> Convention { get; set; }
        public string DateConstruction { get; set; }
        public string DescModifications { get; set; }
        public Nullable<bool> VisiblePavois { get; set; }
        public string DateModification { get; set; }
        public string NiveauNappe { get; set; }
        public string Commentaire { get; set; }
        public string DonneesComplementaires { get; set; }
        public string Position { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lieux> Lieux { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ouvrages> Ouvrages { get; set; }
    }
}
