using Ratp.Hidalgo.Data.Contract;
using Ratp.Hidalgo.Data.Contract.Repositories;
using System;

namespace Ratp.Hidalgo.Model
{
    /// <summary>
    /// UnitOfWork pour le module Hidalgo
    /// </summary>
    public class HidalgoUnitOfWork : IHidalgoUnitOfWork, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hidalgoRepo"></param>
        public HidalgoUnitOfWork(IHidalgoRepository hidalgoRepo, ICriterePerformanceRepositorie criterePerformanceRepositorie, ICalibrageRepositorie calibrageRepositorie, ILigneRepositorie ligneRepositorie)
        {
            this.HidalgoRepository = hidalgoRepo;
            this.CriterePerformanceRepositorie = criterePerformanceRepositorie;
            this.CalibrageRepositorie = calibrageRepositorie;
            this.LigneRepositorie = ligneRepositorie;
        }

        /// <inheritdoc />
        public IHidalgoRepository HidalgoRepository { get; set; }

        /// <summary>
        /// Méthode déspose permettant de détuit les objets inutile, et en suite liberer la mémoire.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ICriterePerformanceRepositorie CriterePerformanceRepositorie { get; set; }

        /// <inheritdoc />
        public ICalibrageRepositorie CalibrageRepositorie { get; set; }

        /// <inheritdoc />
        public ILigneRepositorie LigneRepositorie { get; set; }
    }
}
