using Ratp.Hidalgo.Data.Contract.Repositories;

namespace Ratp.Hidalgo.Data.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHidalgoUnitOfWork
    {
        /// <summary>
        /// 
        /// </summary>
        IHidalgoRepository HidalgoRepository { get; set; }

        /// <summary>
        /// Obtient le repositorie du Critere Performance
        /// </summary>
        ICriterePerformanceRepositorie CriterePerformanceRepositorie { get; set; }

        /// <summary>
        /// Obtient le repositorie du module calibrage
        /// </summary>
        ICalibrageRepositorie CalibrageRepositorie { get; set; }

        /// <summary>
        /// Obtient le repositorie des lignes
        /// </summary>
        ILigneRepositorie LigneRepositorie { get; set; }
    }
}
