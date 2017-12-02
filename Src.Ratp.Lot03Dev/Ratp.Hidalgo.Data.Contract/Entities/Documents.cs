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
    
    public partial class Documents
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Documents()
        {
            this.Documents1 = new HashSet<Documents>();
            this.ProgrammationDocumentPGE = new HashSet<ProgrammationDocumentPGE>();
            this.DesordresDocuments = new HashSet<DesordresDocuments>();
            this.ValidationsDocuments = new HashSet<ValidationsDocuments>();
        }
    
        public int Id { get; set; }
        public int IdDocumentType { get; set; }
        public Nullable<int> IdDocumentParent { get; set; }
        public Nullable<int> IdProcesVerbal { get; set; }
        public Nullable<int> IdChef { get; set; }
        public Nullable<int> IdDemandeur { get; set; }
        public string NumeroAffaire { get; set; }
        public string Descriptions { get; set; }
        public string DescPiecesJointes { get; set; }
        public string RemarquesGenerales { get; set; }
        public Nullable<System.DateTime> DateCreation { get; set; }
        public Nullable<System.DateTime> AnneeProgrammation { get; set; }
        public string ObservationsPrestation { get; set; }
        public string CommentairePrestation { get; set; }
        public Nullable<int> IdProcesVerbalAncien { get; set; }
        public string TypeCondition { get; set; }
        public string DestinataireDE { get; set; }
        public Nullable<int> IdDesitinataireDE { get; set; }
        public Nullable<System.DateTime> DateConsultationDE { get; set; }
        public Nullable<int> ValidationRE { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Documents> Documents1 { get; set; }
        public virtual Documents Documents2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProgrammationDocumentPGE> ProgrammationDocumentPGE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DesordresDocuments> DesordresDocuments { get; set; }
        public virtual Utilisateurs Utilisateurs { get; set; }
        public virtual Utilisateurs Utilisateurs1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ValidationsDocuments> ValidationsDocuments { get; set; }
    }
}
