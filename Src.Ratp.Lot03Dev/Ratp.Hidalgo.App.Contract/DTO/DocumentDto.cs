using System;
using System.Runtime.Serialization;

namespace Ratp.Hidalgo.App.Contract.DTO
{
    /// <summary>
    /// DTO Documents
    /// </summary>
    [DataContract]
    public class DocumentDto
    {
        [DataMember]
        public string NumeroAffaire { get; set; }

        [DataMember]
        public int Note { get; set; }
    }
}
