using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ratp.Hidalgo.Web.MarionV3ServiceReference;

namespace Ratp.Hidalgo.Web.Infra
{
    public class MarionProxy : System.ServiceModel.ClientBase<MarionV3ServiceReference.MarionServicesSoap>, MarionV3ServiceReference.MarionServicesSoap
    {
        public MarionProxy(string nameEndPoint) : base(nameEndPoint)
        {
        }

        public long AddAnnotation(long pvId, long objetCalqueId)
        {
            throw new NotImplementedException();
        }

        public long AddCalque(DescriptionCalque calque)
        {
            throw new NotImplementedException();
        }

        public long AddCampagne(Campagne campagne)
        {
            throw new NotImplementedException();
        }

        public bool AddCouleurPersonalisees(CouleurPerso color)
        {
            throw new NotImplementedException();
        }

        public long AddDessin(Dessin dessin)
        {
            throw new NotImplementedException();
        }

        public int AddDeveloppe(Developpe dev, long objetCalqueId)
        {
            throw new NotImplementedException();
        }

        public long AddJalon(Jalon jalon, long campagneId)
        {
            throw new NotImplementedException();
        }

        public bool AddLienAnnotation(long objetCalqueId, long annotationId, long pvId)
        {
            throw new NotImplementedException();
        }

        public bool AddLienCampagnePV(long campagneId, long pvId)
        {
            throw new NotImplementedException();
        }

        public int AddObjetCalque(DescriptionObjetCalque objetCalque, long calqueId)
        {
            throw new NotImplementedException();
        }

        public int AddPK(PointKilometrique pk, long objetCalqueId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteCalque(long calqueId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDessin(long dessinId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteJalon(long jalonId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLienAnnotation(long objetCalqueId, long pvId, long annotationId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLienCampagnePV(long campagneId, long pvId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteObjetCalque(long objetCalqueId)
        {
            throw new NotImplementedException();
        }

        public bool DeletePositionsFenetres(long userId)
        {
            throw new NotImplementedException();
        }

        public Calque[] GetCalqueByCalqueIds(int[] calquesId)
        {
            throw new NotImplementedException();
        }

        public Calque[] GetCalqueByCampagneID(long campagneId)
        {
            throw new NotImplementedException();
        }

        public Campagne[] GetCampagneById(int campagneId)
        {
            throw new NotImplementedException();
        }

        public DescriptionContour GetContourObjetCalque(long objetCalqueId, long userId)
        {
            throw new NotImplementedException();
        }

        public CouleurPerso[] GetCouleurPersonalisees(long userId)
        {
            throw new NotImplementedException();
        }

        public long GetFonctionIDFromCode(int codeFonction)
        {
            throw new NotImplementedException();
        }

        public EnTeteAnnotation[] GetListeAnnotFromCampagne(long campagneId)
        {
            throw new NotImplementedException();
        }

        public Annotation[] GetListeAnnotFromPV(long pvId)
        {
            throw new NotImplementedException();
        }

        public DescriptionCalque[] GetListeCalquesImportable(long campagneId, long[] calquesExclus)
        {
            throw new NotImplementedException();
        }

        public EnTeteCampagne[] GetListeCampagnes()
        {
            throw new NotImplementedException();
        }

        public DescriptionCarte[] GetListeCartes()
        {
            throw new NotImplementedException();
        }

        public Dessin[] GetListeDessins()
        {
            throw new NotImplementedException();
        }

        public Dessin[] GetListeDessinsV2(string nomVue)
        {
            throw new NotImplementedException();
        }

        public Groupe[] GetListeGroupes(long lieuId)
        {
            throw new NotImplementedException();
        }

        public Jalon[] GetListeJalons(long campagneId)
        {
            throw new NotImplementedException();
        }

        public LabelCarte[] GetListeLabels()
        {
            throw new NotImplementedException();
        }

        public long[] GetListeLiensAnnotations(long objetCalqueId)
        {
            throw new NotImplementedException();
        }

        public Lieu[] GetListeLieux(long ligneGestionnaireId, int[] typesId)
        {
            throw new NotImplementedException();
        }

        public Ligne[] GetListeLignes()
        {
            throw new NotImplementedException();
        }

        public PosDlg[] GetListePositionsFenetres(int userId)
        {
            throw new NotImplementedException();
        }

        public PV[] GetListePVsFromCampagne(long campagneId)
        {
            throw new NotImplementedException();
        }

        public PV[] GetListePVsFromLieu(long lieuId)
        {
            throw new NotImplementedException();
        }

        public TypeLieu[] GetListeTypeLieu()
        {
            throw new NotImplementedException();
        }

        public Carte GetNomVue(int carteId)
        {
            throw new NotImplementedException();
        }

        public TypeDesordre[] GetTypesDesordres()
        {
            throw new NotImplementedException();
        }

        public Utilisateur GetUtilisateur(Utilisateur utilisateur)
        {
            throw new NotImplementedException();
        }

        public long InsertCampagneHistorique(long avancementCampagneId, DateTime histoCampagneDate, long ligneId, long lieuId, DateTime campagneDateDebut)
        {
            throw new NotImplementedException();
        }

        public int[] IsListeObjetsCalquesInFiltrage(int[] calque, int[] famille, int[] type, int[] notenum, int noteCar, bool noteCarNone)
        {
            throw new NotImplementedException();
        }

        public bool IsObjetCalqueInFiltrage(long objetCalqueId, int[] famille, int[] type, int[] notenum, int noteCar, bool noteCarNone)
        {
            throw new NotImplementedException();
        }

        public MarionV3ServiceReference.Utilisateur IsUserAuthorized(int userId)
        {
          return  this.Channel.IsUserAuthorized(userId);
        }

        public bool ResetCouleurPersonalisees(long userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateAnnotation(long objetCalqueId, long pvId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCalque(DescriptionCalque calque)
        {
            throw new NotImplementedException();
        }

        public bool UpdateCouleurPersonalisees(CouleurPerso color)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDessin(Dessin dessin)
        {
            throw new NotImplementedException();
        }

        public bool UpdateDeveloppe(Developpe dev)
        {
            throw new NotImplementedException();
        }

        public bool UpdateJalon(Jalon jalon, long campagneId)
        {
            throw new NotImplementedException();
        }

        public int UpdateLibelleDate()
        {
            throw new NotImplementedException();
        }

        public bool UpdateObjetCalque(DescriptionObjetCalque objetCalque, long calqueId)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePK(PointKilometrique pk)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePositionsFenetres(PosDlg[] dialogPositionList, long userId)
        {
            throw new NotImplementedException();
        }
    }
}