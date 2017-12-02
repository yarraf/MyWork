using System;
using System.Collections.Generic;
using Ratp.Hidalgo.Data.Contract.Entities;
using Ratp.Hidalgo.Data.Contract.Repositories;
using Ratp.Hidalgo.Model.Properties;
using System.Linq;
using log4net;

namespace Ratp.Hidalgo.Model.Repositories
{
    /// <summary>
    /// Implémentation de la repositorie de calibrage
    /// </summary>
    public class CalibrageRepositorie : ICalibrageRepositorie
    {
        /// <summary>
        /// Field pour stocker l'information Logger
        /// </summary>
        private readonly ILog Logger;

        /// <summary>
        /// Initialise des données
        /// </summary>
        public CalibrageRepositorie()
        {
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <inheritdoc />
        public IEnumerable<CoefficientPonderationParametresCriteresCalibrage> GetAllCoefficientPonderation()
        {
            IEnumerable<CoefficientPonderationParametresCriteresCalibrage> listeCoefficientPonderation = null;
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    listeCoefficientPonderation = context.CoefficientPonderationParametresCriteresCalibrage.ToList();
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            }

            return listeCoefficientPonderation;
        }

        /// <inheritdoc />
        public CoefficientPonderationParametresCriteresCalibrage GetOneCoefficientPonderationById(int id)
        {
            CoefficientPonderationParametresCriteresCalibrage result = null;

            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    result = context.CoefficientPonderationParametresCriteresCalibrage.Single(x => x.Id == id);
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public void UpdateCoefficientPonderationParametreMaconnerie(CoefficientPonderationParametresCriteresCalibrage coefficientPonderationParametre)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    var coefficientPonderationToUpdate = context.CoefficientPonderationParametresCriteresCalibrage.Find(coefficientPonderationParametre.Id);
                    if (coefficientPonderationToUpdate != null)
                    {
                        coefficientPonderationToUpdate.ValeurMaconnerie = coefficientPonderationParametre.ValeurMaconnerie;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            }
        }

        /// <inheritdoc />
        public void UpdateCoefficientPonderationParametreEnduit(CoefficientPonderationParametresCriteresCalibrage coefficientPonderationParametre)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    var coefficientPonderationToUpdate = context.CoefficientPonderationParametresCriteresCalibrage.Find(coefficientPonderationParametre.Id);
                    if (coefficientPonderationToUpdate != null)
                    {
                        coefficientPonderationToUpdate.ValeurEnduit = coefficientPonderationParametre.ValeurEnduit;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }
            }
        }

        /// <inheritdoc />
        public void UpdateCoefficientPonderationParametresMaçonnerie(CoefficientPonderationParametresCriteresCalibrage coefficientPonderation)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    var coefficientPonderationToUpdate = context.CoefficientPonderationParametresCriteresCalibrage.Find(coefficientPonderation.Id);
                    if (coefficientPonderationToUpdate.ValeurMaconnerie != coefficientPonderation.ValeurMaconnerie)
                    {
                        coefficientPonderationToUpdate.ValeurMaconnerie = coefficientPonderation.ValeurMaconnerie;
                        //context.Entry(coefficientPonderationToUpdate).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }

            }
        }
        public void UpdateCoefficientPonderationParametresEnduit(CoefficientPonderationParametresCriteresCalibrage coefficientPonderation)
        {
            using (RatpHidalgoEntities context = new RatpHidalgoEntities(Resource1.RatpConnectionString))
            {
                try
                {
                    var coefficientPonderationToUpdate = context.CoefficientPonderationParametresCriteresCalibrage.Find(coefficientPonderation.Id);
                    if (coefficientPonderationToUpdate.ValeurEnduit != coefficientPonderation.ValeurEnduit)
                    {
                        coefficientPonderationToUpdate.ValeurEnduit = coefficientPonderation.ValeurEnduit;
                        //context.Entry(coefficientPonderationToUpdate).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    this.Logger.Error(ex.Message);
                }

            }
        }
    }
}
