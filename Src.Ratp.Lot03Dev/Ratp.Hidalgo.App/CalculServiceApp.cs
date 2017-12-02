using Ratp.Hidalgo.App.Contract;
using System.Collections.Generic;
using Ratp.Hidalgo.App.Contract.DTO;
using Ratp.Hidalgo.Data.Contract;
using log4net;
using Ratp.Hidalgo.Data.Contract.Entities;
using Core.Common.Mapping;
using Ratp.Hidalgo.App.Mapping;
using System.Linq;
using System;
using Ratp.Hidalgo.Data.Contract.Enum;

namespace Ratp.Hidalgo.App
{
    /// <summary>
    /// Implémentation de l'interface <see cref="ICalculServiceApp"/>
    /// </summary>
    public class CalculServiceApp : ICalculServiceApp
    {
        /// <summary>
        /// Field pour stocker l'information de l'unit of work du module Hidalgo 
        /// </summary>
        private IHidalgoUnitOfWork _uow;

        /// <summary>
        /// Field pour stocker l'information de ILogger
        /// </summary>
        private readonly ILog Logger;

        public CalculServiceApp(IHidalgoUnitOfWork uow)
        {
            this._uow = uow;
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <inheritdoc />
        public IEnumerable<LigneDto> GetAllLigneByFamille()
        {
            var listeLigneMetier = this._uow.LigneRepositorie.GetAllLigneByFamille();
            IMapper<Lignes, LigneDto> mapper = new LigneToLigneDtoMapping();
            return listeLigneMetier.Select(mapper.Map).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<LieuDto> GetAllLieuxByLigneGestionnaire(int idLigneGestionnaire)
        {
            var listeLieuxMetier = this._uow.LigneRepositorie.GetAllLieuxByLigne(idLigneGestionnaire);
            IMapper<Lieux, LieuDto> mapper = new LieuToLieuDtoMapping();
            return listeLieuxMetier.Select(mapper.Map).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<NatureTravauxExternesDto> GetAllNatureTravauxExternes()
        {
            var listeNatureTve = this._uow.LigneRepositorie.GetAllNatureTravauxExternes();
            IMapper<NatureTravauxExternes, NatureTravauxExternesDto> mapper = new NatureTravauxExternesToNatureTravauxExternesMapping();
            return listeNatureTve.Select(mapper.Map).ToList();
        }

        /// <inheritdoc />
        public IEnumerable<Documents> GetAllDocumentPge(ENatureCalibrage natureCalibrage)
        {
            return this._uow.CriterePerformanceRepositorie.GetAllDocumentPge(natureCalibrage);
        }

        /// <inheritdoc />
        public void Save(ProgrammationDto newProgrammationsDto)
        {
            if (newProgrammationsDto == null)
            {
                throw new ArgumentNullException("newProgrammationsDto est null");
            }

            this.Logger.Info(string.Format("Début d'eregistrement d'une nouvelle programmation de l'année {0}", newProgrammationsDto.Anneeprogrammation));
            IMapper<ProgrammationDto, Programmations> mapper = new ProgrammationDtoToProgrammationMapping();
            this._uow.HidalgoRepository.SaveProgrammation(mapper.Map(newProgrammationsDto));
            this.Logger.Info(string.Format("Enregistrement est terminé"));
        }

        /// <inheritdoc />
        public IEnumerable<ProgrammationDocumentPgeDto> GetAllDocumentPgeByProgrammation(int idProgrammation)
        {
            this.Logger.Info(string.Format("Récupérer la liste des documents Pge calculé par programmation {0}", idProgrammation));
            IMapper<ProgrammationDocumentPGE, ProgrammationDocumentPgeDto> mapper = new PreordreFinalDtoToProgrammationDocumentPgeMapping(this._uow);
            var listePge = this._uow.HidalgoRepository.GetAllDocumentPgeByProgrammation(idProgrammation).Select(x => mapper.Map(x)).ToList();

            return listePge;
        }

        /// <inheritdoc />
        public void UpdateProgrammation(ProgrammationDto programmationDto)
        {
            if (programmationDto == null)
            {
                this.Logger.Error(string.Format("UpdateProgrammation: l'objet de la programmation est vide"));
                new ArgumentNullException("l'objet de la programmation est vide.");
            }

            this.Logger.Info(string.Format("Update de la programmation: {0}", programmationDto.Id));
            var programmationMetier = this._uow.HidalgoRepository.GetOneProgrammation(programmationDto.Id);
            var mapper = new ProgrammationDtoToProgrammationMapping();
            mapper.Map(programmationDto, programmationMetier);
            this._uow.HidalgoRepository.UpdateProgrammation(programmationMetier);
            this.Logger.Info(string.Format("Fin de l'update de la programmation: {0}", programmationDto.Id));
        }

        /// <inheritdoc />
        public void SaveProgrammationDocumentPge(IEnumerable<ProgrammationDocumentPgeDto> listProgrammationDocumentPgeDto)
        {
            if (listProgrammationDocumentPgeDto == null || listProgrammationDocumentPgeDto.Count<ProgrammationDocumentPgeDto>() == 0)
            {
                this.Logger.Error(string.Format("SaveProgrammationDocumentPge: l'objet de la programmation est vide"));
                new ArgumentNullException("l'objet de la programmation est vide.");
            }
            // TODO : RDT - recuperation de la programmation et changer Id en programmation.Id pour le logger
            //ProgrammationDto programmation = this._uow.HidalgoRepository.get;

            this.Logger.Info(string.Format("Début d'eregistrement des PGE pour la programmation", listProgrammationDocumentPgeDto.ToList<ProgrammationDocumentPgeDto>()[0].Id));

            IMapper<ProgrammationDocumentPgeDto, ProgrammationDocumentPGE> mapper = new PreordreFinalDtoToProgrammationDocumentPgeMapping(this._uow);
            foreach (ProgrammationDocumentPgeDto pge in listProgrammationDocumentPgeDto.ToList())
            {
                //pge.Programmation = programmation;

                if (pge.Id == 0)
                {
                    this.Logger.Info(string.Format("Update de la ProgrammationDocumentPge: {0}", pge.Id));
                    //var programmationDocumentPgeMetier = this._uow.HidalgoRepository.GetOneProgrammationDocumentPge(pge.Id);
                    var pgeDocumentMetier = mapper.Map(pge);
                    this._uow.HidalgoRepository.AddProgrammationDocumentPGE(pgeDocumentMetier);
                    this.Logger.Info(string.Format("Fin de l'update de la ProgrammationDocumentPge: {0}", pge.Id));
                }
                else
                {
                    this.Logger.Info(string.Format("Update de la ProgrammationDocumentPge: {0}", pge.Id));
                    var programmationDocumentPgeMetier = this._uow.HidalgoRepository.GetOneProgrammationDocumentPge(pge.Id);
                    var pgeDocumentMetier = mapper.Map(pge);
                    this._uow.HidalgoRepository.UpdateProgrammationDocumentPGE(pgeDocumentMetier);
                    this.Logger.Info(string.Format("Fin de l'update de la ProgrammationDocumentPge: {0}", pge.Id));
                }
            }
            this.Logger.Info(string.Format("Enregistrement est terminé"));

        }

        /// <inheritdoc />
        public IEnumerable<ProgrammationDto> GetAllProgrammation()
        {
            this.Logger.Info(string.Format("Récupérer la liste des programmations"));
            IMapper<Programmations, ProgrammationDto> mapper = new ProgrammationDtoToProgrammationMapping();
            var listePge = this._uow.HidalgoRepository.GetAllProgrammation().Select(x => mapper.Map(x)).ToList();

            return listePge;
        }

        /// <inheritdoc />
        public void RemoveOneProgrammation(int idProgrammation)
        {
            IEnumerable<ProgrammationDetails> listProgrammationDetails = this._uow.HidalgoRepository.GetAllProgrammationDetailsByProgrammation(idProgrammation);
            IEnumerable<ProgrammationDocumentPGE> listProgrammationDocumentPGE = this._uow.HidalgoRepository.GetAllDocumentPgeByProgrammation(idProgrammation);
            IEnumerable<ProgrammationValeurParametresDocument> listProgrammationValeurParametresDocument = this._uow.HidalgoRepository.GetAllProgrammationValeurParametresDocument(idProgrammation);
            Programmations programmation = this._uow.HidalgoRepository.GetOneProgrammation(idProgrammation);

            this.Logger.Info(string.Format("Suppression de la programmation {0}", idProgrammation));
            this._uow.HidalgoRepository.RemoveOneProgrammation(programmation, listProgrammationDetails, listProgrammationDocumentPGE, listProgrammationValeurParametresDocument);
            this.Logger.Info(string.Format("Fin de la Suppression de la programmation {0}", idProgrammation));
        }

        public ProgrammationDto GetProgrammationByNatureTypeAnnee(string natureTravaux, string typeOuvrage, int? anneeProgrammation)
        {
            ProgrammationDto programmation;
            this.Logger.Info(string.Format("Recuperation de la derniere programmation {0} {1} {2}", natureTravaux, typeOuvrage, anneeProgrammation));
            IMapper<Programmations, ProgrammationDto> mapper = new ProgrammationDtoToProgrammationMapping();
            programmation = mapper.Map(this._uow.HidalgoRepository.GetOneProgrammationByTypeNatureAnnee(typeOuvrage, natureTravaux, anneeProgrammation));
            this.Logger.Info(string.Format("Fin de la Suppression de la programmation {0}", programmation.Id));

            return programmation;
        }

        public Dictionary<ProgrammationDocumentPgeDto, IEnumerable<ValeursParametresPgeDto>> GetAllValeurParametreByPge(ProgrammationDocumentPgeDto[] listProgrammationDocumentPgeDto)
        {
            Dictionary<ProgrammationDocumentPgeDto, IEnumerable<ValeursParametresPgeDto>> dictionaire = new Dictionary<ProgrammationDocumentPgeDto, IEnumerable<ValeursParametresPgeDto>>();
            this.Logger.Info(string.Format("Récupérer la liste des Valeurs de Paramêtres des PGE"));
            foreach (ProgrammationDocumentPgeDto pge in listProgrammationDocumentPgeDto)
            {
                this.Logger.Info(string.Format("Récupérer la liste des documents Pge calculé pour la PGE {0} de la programmation {1}", pge.Id, pge.Programmation.Id));
                IMapper<ProgrammationValeurParametresDocument, ValeursParametresPgeDto> mapper = new ProgrammationParametreValeurDocumentToProgrammationParametreValeurDtoMapper();
                dictionaire.Add(pge, this._uow.HidalgoRepository.GetAllValeurParametreByPge(pge.Id).Select(x => mapper.Map(x)).ToList());
            }
            this.Logger.Info(string.Format("Fin de la Récupération de la liste des Valeurs de Paramêtres des PGE"));
            return dictionaire;
        }
    }
}
