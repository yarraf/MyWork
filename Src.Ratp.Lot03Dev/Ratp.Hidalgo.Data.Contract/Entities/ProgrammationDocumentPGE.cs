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
    
    public partial class ProgrammationDocumentPGE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProgrammationDocumentPGE()
        {
            this.DocumentPgeTravauxAnnees = new HashSet<DocumentPgeTravauxAnnees>();
            this.ProgrammationValeurParametresDocument = new HashSet<ProgrammationValeurParametresDocument>();
        }
    
        public int Id { get; set; }
        public int IdProgrammation { get; set; }
        public string NumeroAffaire { get; set; }
        public int NumRang { get; set; }
        public int NumMedian { get; set; }
        public Nullable<bool> IsExAequo { get; set; }
        public Nullable<bool> IsValideEx { get; set; }
        public Nullable<double> Surface { get; set; }
        public string Commentaire { get; set; }
        public Nullable<int> Annee { get; set; }
        public Nullable<decimal> Budget { get; set; }
        public Nullable<bool> IsTravauxPlusieursAnnee { get; set; }
        public Nullable<int> IdDocument { get; set; }
        public Nullable<Ratp.Hidalgo.Data.Contract.Enum.ECategoriesPge> Categorie { get; set; }
    
        public virtual Programmations Programmations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentPgeTravauxAnnees> DocumentPgeTravauxAnnees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgrammationValeurParametresDocument> ProgrammationValeurParametresDocument { get; set; }
        public virtual Documents Documents { get; set; }
    }
}
