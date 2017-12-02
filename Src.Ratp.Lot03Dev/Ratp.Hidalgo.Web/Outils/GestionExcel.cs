using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ratp.Hidalgo.App.Contract.DTO;
using System.Configuration;
using System.IO;
using log4net;

using Microsoft.Office.Interop.Excel;


namespace Ratp.Hidalgo.Web.Outils
{
    public class GestionExcel
    {
        /// <summary>
        /// Field pour stocker l'information de ILogger
        /// </summary>
        private readonly ILog Logger;

        private int idProgrammation;
        private IEnumerable<ProgrammationDocumentPgeDto> listePGE;
        private string CheminExcel = ConfigurationManager.AppSettings["CheminDossierExcel"];

        Application _Application;
        Workbooks _LesWorkBooks;
        _Workbook _MonClasseur;
        _Worksheet _MaFeuille;

        public GestionExcel(int idProgrammation, IEnumerable<ProgrammationDocumentPgeDto> listePGE)
        {
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            this.idProgrammation = idProgrammation;
            this.listePGE = listePGE;
        }

        public GestionExcel(int idProgrammation)
        {
            this.Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            this.idProgrammation = idProgrammation;
        }

        public bool OpenExcel()
        {
            try
            {
                //Démarre Excel et récupère l'application 
                _Application = new Application();
                _Application.Visible = false;
                return true;
            }
            catch (Exception e)
            {
                this.Logger.Error("Impossible de créer une instance d'Excel.");
                return false;
            }
        }


        /// <summary> 
        /// Teste si le fichier existe, si oui on l'ouvre sinon on le crée et l'enregistre 
        /// </summary> 
        /// <returns></returns> 
        public bool CreerOuOuvrirFichier()
        {
            try { 
                if (File.Exists(this.CheminExcel + "/Programmation_" + this.idProgrammation))
                {
                    _LesWorkBooks = _Application.Workbooks;
                    _MonClasseur = _LesWorkBooks.Open(this.CheminExcel + "/Programmation_" + this.idProgrammation);
                    _MaFeuille = (_Worksheet)_Application.ActiveSheet;

                    return true;
                }
                else
                {
                    //Récupère le WorkBook 
                    _MonClasseur = _Application.Workbooks.Add();
                    //Récupère la feuille Active 
                    _MaFeuille = (_Worksheet)_MonClasseur.ActiveSheet;
                    this.InscritEntete();
                    _MonClasseur.SaveAs(this.CheminExcel + "/Programmation_" + this.idProgrammation);
                    return true;
                }
            }
            catch (Exception e)
            {
                this.Logger.Error("Impossible d'ouvrir ou de créer le fichier Excel.");
                FermerExcel();
                return false;
            }
        }


        public void AfficherExcel()
        {
            if (this.OpenExcel()) { 
                this.CreerOuOuvrirFichier();
                _Application.Visible = true;
            }
        }

        public bool FermerExcel()
        {
            try
            {
                _MonClasseur.Save();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(_MaFeuille);
                _MaFeuille = null;
                if (_MonClasseur != null)
                {
                    _MonClasseur.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(_MonClasseur);
                    _MonClasseur = null;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool InscritEntete()
        {
            //fusion des champs
            FusionCellule("A1","A6");
            _MaFeuille.Cells[1, "A"] = "ID";
            FusionCellule("B1","B6");
            _MaFeuille.Cells[1, "B"] = "Ligne";
            FusionCellule("C1","C6");
            _MaFeuille.Cells[1, "C"] = "Lieu";
            FusionCellule("D1","D6");
            _MaFeuille.Cells[1, "D"] = "Num. d'affaire";

            FusionCellule("E1","F2");
            _MaFeuille.Cells[1, "E"] = "Localisation";
            FusionCellule("E3","E6");
            _MaFeuille.Cells[3, "E"] = "du PK";
            FusionCellule("F3","F6");
            _MaFeuille.Cells[3, "F"] = "au PK";

            FusionCellule("G1","G6");
            _MaFeuille.Cells[1, "G"] = "Moyen Envis";
            FusionCellule("H1","H6");
            _MaFeuille.Cells[1, "H"] = "m²";

            FusionCellule("I1","M3");
            _MaFeuille.Cells[1, "I"] = "CONTRAINTES";
            FusionCellule("I4","I6");
            _MaFeuille.Cells[4, "I"] = "E010 A Faire/ A Prolonger";
            FusionCellule("J4","J6");
            _MaFeuille.Cells[4, "J"] = "Etudes";
            FusionCellule("K4","K6");
            _MaFeuille.Cells[4, "K"] = "Preco";
            FusionCellule("L4","M4");
            _MaFeuille.Cells[4, "L"] = "Dates de réalisation";
            FusionCellule("L5","L6");
            _MaFeuille.Cells[5, "L"] = "Début au plus tôt";
            FusionCellule("M5","M6");
            _MaFeuille.Cells[5, "M"] = "Fin au plus tard";

            FusionCellule("N1","O3");
            _MaFeuille.Cells[1, "N"] = "Dates de Validité des Emprises sur Voirie";
            FusionCellule("N4","N6");
            _MaFeuille.Cells[4, "N"] = "Début au plus tôt";
            FusionCellule("O4","O6");
            _MaFeuille.Cells[4, "O"] = "Fin au plus tard";

            FusionCellule("P1","P3");
            _MaFeuille.Cells[1, "P"] = "Numéro d'Enregistrement CTV";
            FusionCellule("P4","P6");
            FusionCellule("Q1","Q3");
            _MaFeuille.Cells[1, "Q"] = "Montant Prévisionnel (€)";
            FusionCellule("Q4","Q6");
            FusionCellule("R1","R3");
            _MaFeuille.Cells[1, "R"] = "Montant Travaux Complémentaires";
            FusionCellule("R4","R6");
            FusionCellule("S1","S3");
            _MaFeuille.Cells[1, "S"] = "Coût cumulés (€)";
            FusionCellule("S4","S6");
            _MaFeuille.Cells[4, "S"] = "Coût total";
            FusionCellule("T1","T6");
            _MaFeuille.Cells[1, "T"] = "N° XAGA";
            FusionCellule("U1","U6");
            _MaFeuille.Cells[1, "U"] = "Chargé d'affaire";
            FusionCellule("V1","V6");
            _MaFeuille.Cells[1, "V"] = "INFORMATIONS RELATIVES AUX CHANTIERS";

            //Encadrement et centrage 
            Range range = _MaFeuille.get_Range("A1", "V6");
            range.EntireColumn.AutoFit();
           // range.HorizontalAlignment = ;
           // range.VerticalAlignment = ;
           // var x = range.VerticalAlignment;
            range.EntireRow.Font.Bold = true;
            Borders border = range.Borders;
            border.LineStyle = XlLineStyle.xlContinuous; 
            border.Weight = 2d;


            return true;
        }

        public void FusionCellule(string cell1, string cell2)
        {
            _MaFeuille.get_Range(cell1, cell2).Merge();
        }

        internal void EditerDonneePge()
        {
            //Lit toutes les PGE de listePGE et met à jour si nécessaire ou insère en cas de nouvelle ligne
            //Les Pge sont affiché à partir de la ligne 7
            //A : id ; B Ligne; C:Lieu; D: Num. d'affaitre; H: m²; Q: Montant previsionnel; S: cout cumulés
            int row = 7;
            decimal? coutCumule = 0;
            foreach(var pge in this.listePGE)
            {
                //Recherche la PGE si existe copie toutes les données inserte et supprime l'ancienne et réecrit 
                //Recherche 

                //Deplace et supprime la ligne vide

                //Insert les nouvelles valeurs editable
               try
                {
                    _MaFeuille.Cells[row, "A"] = pge.Id;
                    _MaFeuille.Cells[row, "D"] = pge.NumeroAffaire;
                    _MaFeuille.Cells[row, "H"] = pge.Surface;
                    _MaFeuille.Cells[row, "Q"] = (decimal)pge.Surface*(pge.Programmation.PrixUnitaire>0? pge.Programmation.PrixUnitaire:0);
                    coutCumule = coutCumule + pge.Budget;
                    _MaFeuille.Cells[row, "S"] = coutCumule;

                    _MaFeuille.Cells[row, "B"] = pge.Ligne.Name;
                    _MaFeuille.Cells[row, "C"] = pge.Lieu.Name;
                }
                catch (Exception e) {
                    //logger
                    
                }
                row++;
            }
        }

    }
}