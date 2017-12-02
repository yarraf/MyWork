function rendreTodecimal(nameclasscss) {
    $(nameclasscss).keypress(function (event) {
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
           ((event.which < 48 || event.which > 57) &&
           (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') == -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });
}

function rendreTodecimalWithId(id) {
    $('#' + id).keypress(function (event) {
        var $this = $(this);
        if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
           ((event.which < 48 || event.which > 57) &&
           (event.which != 0 && event.which != 8))) {
            event.preventDefault();
        }

        var text = $(this).val();
        if ((event.which == 46) && (text.indexOf('.') == -1)) {
            setTimeout(function () {
                if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                    $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
                }
            }, 1);
        }

        if ((text.indexOf('.') != -1) &&
            (text.substring(text.indexOf('.')).length > 2) &&
            (event.which != 0 && event.which != 8) &&
            ($(this)[0].selectionStart >= text.length - 2)) {
            event.preventDefault();
        }
    });
}

function allownumericwithoutdecimal(id) {
    $('#' + id).on("keypress keyup blur", function (event) {
        $(this).val($(this).val().replace(/[^\d].+/, ""));
        if ((event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });

}

function allownumericwithdecimal(nameclasse) {
    //Eme méthode pour autoriser que le format number
    $(nameclasse).on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

function allownumericwithdecimalWithId(id) {
    //Eme méthode pour autoriser que le format number
    $('#' + id).on("keypress keyup blur", function (event) {
        //this.value = this.value.replace(/[^0-9\.]/g,'');
        $(this).val($(this).val().replace(/[^0-9]/g, ""));
        if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
            event.preventDefault();
        }
    });
}

function initialiseinputtableaureferencecriteres() {
    $('#input_b4c1').show();
    $('#input_b4c2').show();
    $('#input_b4c3').show();

    $('#input_b0c1').show();
    $('#input_b0c2').show();
    $('#input_b0c3').show();

    $('#input_b4c1').attr("disabled", "disabled");
    $('#input_b4c2').attr("disabled", "disabled");
    $('#input_b4c3').attr("disabled", "disabled");

    $('#input_b0c1').attr("disabled", "disabled");
    $('#input_b0c2').attr("disabled", "disabled");
    $('#input_b0c3').attr("disabled", "disabled");

    //$('#b1c1').hide();
    //$('#b1c2').hide();
    //$('#b1c3').hide();

    $('#input_b1c1').show();
    $('#input_b1c2').show();
    $('#input_b1c3').show();

    $('#input_b2c1').show();
    $('#input_b2c2').show();
    $('#input_b2c3').show();

    $('#input_b3c1').show();
    $('#input_b3c2').show();
    $('#input_b3c3').show();

    //pour apparaitre les valeurs Maximale en forme badge
    $('#badge_b4c1').attr("class", "badge");
    $('#badge_b4c2').attr("class", "badge");
    $('#badge_b4c3').attr("class", "badge");
}

function isverified(id) {
    //vérifier les données de la colonne C1
    // vérieir la régle b4C1 > b3C1  > b2C1 > b1C1 > b0C1
    if (id == "input_b1c1") {
        //if (parseFloat($('#input_b1c1').val()) < parseFloat($('#input_b2c1').val()) && parseFloat($('#input_b1c1').val()) > parseFloat($('#b0c1').text()))
        if (parseFloat($('#input_b1c1').val()) < parseFloat($('#old_b2c1').text()) && parseFloat($('#input_b1c1').val()) > parseFloat($('#old_b0c1').text()))
            return true;
    }

    if (id == "input_b2c1") {
        //if (parseFloat($('#input_b2c1').val()) < parseFloat($('#input_b3c1').val()) && parseFloat($('#input_b2c1').val()) > parseFloat($('#input_b1c1').val()))
        if (parseFloat($('#input_b2c1').val()) < parseFloat($('#old_b3c1').text()) && parseFloat($('#input_b2c1').val()) > parseFloat($('#old_b1c1').text()))
            return true;
    }

    if (id == "input_b3c1") {
        //if (parseFloat($('#input_b3c1').val()) < parseFloat($('#b4c1').text()) && parseFloat($('#input_b3c1').val()) > parseFloat($('#input_b2c1').val()))
        if (parseFloat($('#input_b3c1').val()) < parseFloat($('#old_b4c1').text()) && parseFloat($('#input_b3c1').val()) > parseFloat($('#old_b2c1').text()))
            return true;
    }


    //vérifier les données de la colonne C2
    // vérieir la régle b4C1 > b3C1  > b2C1 > b1C1 > b0C1
    if (id == "input_b1c2") {
        //if (parseFloat($('#input_b1c2').val()) < parseFloat($('#input_b2c2').val()) && parseFloat($('#input_b1c2').val()) > parseFloat($('#b0c2').text()))
        if (parseFloat($('#input_b1c2').val()) < parseFloat($('#old_b2c2').text()) && parseFloat($('#input_b1c2').val()) > parseFloat($('#old_b0c2').text()))
            return true;
    }

    if (id == "input_b2c2") {
        //if (parseFloat($('#input_b2c2').val()) < parseFloat($('#input_b3c2').val()) && parseFloat($('#input_b2c2').val()) > parseFloat($('#input_b1c2').val()))
        if (parseFloat($('#input_b2c2').val()) < parseFloat($('#old_b3c2').text()) && parseFloat($('#input_b2c2').val()) > parseFloat($('#old_b1c2').text()))
            return true;
    }

    if (id == "input_b3c2") {
        //if (parseFloat($('#input_b3c2').val()) < parseFloat($('#b4c2').text()) && parseFloat($('#input_b3c2').val()) > parseFloat($('#input_b2c2').val()))
        if (parseFloat($('#input_b3c2').val()) < parseFloat($('#old_b4c2').text()) && parseFloat($('#input_b3c2').val()) > parseFloat($('#old_b2c2').text()))
            return true;
    }

    //vérifier les données de la colonne C3
    // vérieir la régle b4C1 > b3C1  > b2C1 > b1C1 > b0C1
    if (id == "input_b1c3") {
        //if (parseFloat($('#input_b1c3').val()) < parseFloat($('#input_b2c3').val()) && parseFloat($('#input_b1c3').val()) > parseFloat($('#b0c3').text()))
        if (parseFloat($('#input_b1c3').val()) < parseFloat($('#old_b2c3').text()) && parseFloat($('#input_b1c3').val()) > parseFloat($('#old_b0c3').text()))
            return true;
    }

    if (id == "input_b2c3") {
        //if (parseFloat($('#input_b2c3').val()) < parseFloat($('#input_b3c3').val()) && parseFloat($('#input_b2c3').val()) > parseFloat($('#input_b1c3').val()))
        if (parseFloat($('#input_b2c3').val()) < parseFloat($('#old_b3c3').text()) && parseFloat($('#input_b2c3').val()) > parseFloat($('#old_b1c3').text()))
            return true;
    }

    if (id == "input_b3c3") {
        //if (parseFloat($('#input_b3c3').val()) < parseFloat($('#b4c3').text()) && parseFloat($('#input_b3c3').val()) > parseFloat($('#input_b2c3').val()))
        if (parseFloat($('#input_b3c3').val()) < parseFloat($('#old_b4c3').text()) && parseFloat($('#input_b3c3').val()) > parseFloat($('#old_b2c3').text()))
            return true;
    }

    return false;
}


//********** NOUVELLE PROGRAMMATION **********//
function InitialiseElements() {
    //Charger la liste des lieux par ligne
    var ligne = $('#listeofLignes').val();

    allownumericwithoutdecimal('txtAnneeSaisie');
    rendreTodecimalWithId('txtPrixUnitaireTravaux');
    rendreTodecimalWithId('txtBudgetDisponible');
    //ko
    var viewmodel = function () {
        var $this = this;
        $this.lignes = ko.observableArray();
        $this.lieux = ko.observableArray();
        $this.lieuxte = ko.observableArray();
        $this.listeRnm = ko.observableArray();
        $this.listeAnnees = ko.observableArray();
        $this.listenaturetravauxE = ko.observableArray();
        $this.listeTE = ko.observableArray();
        $this.selectedDefaultLigne = ko.observable('Choisir Ligne');
        $this.selectedDefaultLieu = ko.observable('Choisir Lieu');
        $this.selectedDefalutAnnee = ko.observable('Choisir Année');
        $this.selectedDefaultNatureTravaux = ko.observable('Nature des travaux');

        //RNM
        $this.selectedligne = ko.observable();
        $this.selectedlieu = ko.observable();
        $this.selectedAnnee = ko.observable();

        //Travaux Externes
        $this.selectedlignete = ko.observable();
        $this.selectedlieute = ko.observable();
        $this.selectedDatete = ko.observable().extend({ required: true });
        $this.selectednaturetravaurxternes = ko.observable();

        $this.anneeSaisie = ko.observable().extend({ required: true });

        //Etape 2:
        $this.selectedTypeOuvrage = ko.observable();
        $this.selectednatureTravaux = ko.observable();
        $this.listeTypeOuvrages = ko.observableArray();
        $this.listeNatureTravaux = ko.observableArray();

        $this.errMsg = ko.observable(' Données invalides, Veuillez vérifier les données de la liste des RNM ou Travaux Externes.');
        $this.infoCalcul = ko.observable('Le calcul de la nouvelle programmation est en cours...');
        $this.isloading = ko.observable(false);
        $this.csstabs = ko.observable('glyphicon glyphicon-star');
        $this.isVisible = ko.observable(false); // pour button annee saisie
        $this.npisValide = ko.observable(0);

        $this.isValideAnneeSaisie = ko.observable(false);
        $this.isOrdredAsc = ko.observable(false);
        $this.isAscLignes = ko.observable(true);
        $this.isDescLignes = ko.observable(false);
        $this.isAscLieux = ko.observable(true);
        $this.isDescLieux = ko.observable(false);
        $this.isAscAnnee = ko.observable(true);
        $this.isDescAnnee = ko.observable(false);
        $this.isAscLignesTe = ko.observable(true);
        $this.isDescLignesTe = ko.observable(false);
        $this.isAscLieuxTe = ko.observable(true);
        $this.isDescLieuxTe = ko.observable(false);
        $this.isAscNteTe = ko.observable(true);
        $this.isDescNteTe = ko.observable(false);
        $this.isAscDateTe = ko.observable(true);
        $this.isDescDateTe = ko.observable(false);
        //cette propriete est désactivé puis remplacer par la déclaration dessous suite au demande d'angel
        //$this.isValideStep = ko.computed(function () {
        //    if ($this.listeRnm().length > 0 && $this.listeTE().length > 0) {
        //        return 1;
        //    }
        //}, $this);
        $this.isValideStep = ko.observable(1);

        //Etape 3:
        $this.budgetDisponible = ko.observable();
        $this.prixUnitaireTravaux = ko.observable();
        $this.listePGE = ko.observableArray();
        $this.idProgrammation = ko.observable(0);

        //$this.listeTSPA = ko.observableArray();
        $this.NatureTravaux = ko.computed(function () {
            return $this.selectednatureTravaux() ? $this.selectednatureTravaux().Nature : 'not found';
        }, this);

        //Etape 4:
        $this.objTSPA = ko.observable(null);
        $this.listPgeToConsult = ko.observableArray();
        $this.anneeSaisie.subscribe(function (valuesaisie) {
            if (valuesaisie.length == 4) {
                $this.isVisible(true);
                $this.isValideAnneeSaisie(false);
                $('#errMsgAnneeProgrammation').hide();
            }
            else {
                $this.isVisible(false);
                $this.isValideAnneeSaisie(true);
                $('#errMsgAnneeProgrammation').show();
            }
        });

        //observable Nombre lignes RNM
        $this.nbrRnm = ko.computed(function () {
            return $this.listeRnm().length;
        }, $this);

        //observable Nombre lignes travaux externes
        $this.nbrTravauxExt = ko.computed(function () {
            return $this.listeTE().length;
        }, $this);

        var url = "/Ratp.Hidalgo.Web";
        //chargement liste lieux
        $this.selectedligne.subscribe(function (newValue) {
            if (newValue != undefined) {
                $.getJSON("/api/GetAllLieuxByLigne/" + newValue.Id, function (data) {
                    $this.lieux(data);
                }); //fin getJSON
            }
            else {
                console.log(newValue);
                $this.lieux.removeAll();
            }
        });

        $this.selectedTypeOuvrage.subscribe(function (item) {
            alert(item.Id);
        });

        //chargement liste lieu travaux externes
        $this.selectedlignete.subscribe(function (newValue) {
            if (newValue != undefined) {
                $.getJSON("/api/GetAllLieuxByLigne/" + newValue.Id, function (data) {
                    $this.lieuxte(data);
                }); //fin getJSON
            }
            else {
                $this.lieuxte.removeAll();
            }
        });

        //Get lignes
        $.getJSON("/api/GetAllLignes", function (data) {
            ko.utils.arrayForEach(data, function (item) {
                $this.lignes.push(new ligneAutomatise(item.Id, item.Name, item.Checked, item.Activee, item.Chemin));
            });

            //$this.lignes(data);
        }); //fin getJSON

        //Get travaux externes
        $.getJSON("/api/GetAllNatureTravauxExt", function (data) {
            $this.listenaturetravauxE(data);
        }); //fin

        //Get GetAllTypeOuvrages
        $.getJSON("/api/GetAllTypeOuvrages", function (data) {
            $this.listeTypeOuvrages(data);
        }); //fin getJSON

        //Get GetAllTypeOuvrages
        $.getJSON("/api/GetAllNatureTravaux", function (data) {
            $this.listeNatureTravaux(data);
        }); //fin getJSON

        //le model
        var model = {
            Idprogrammation: $this.idProgrammation,
            LignesGestionnaires: $this.lignes,
            ListeRnm: $this.listeRnm,
            ListeTravauxExt: $this.listeTE,
            AnneeProgrammation: $this.anneeSaisie,
            SelectedNatureTravaux: $this.selectednatureTravaux,
            SelectedTypeOuvrage: $this.selectedTypeOuvrage,
            PrixUnitaireTravaux: $this.prixUnitaireTravaux,
            BudgetDisponible: $this.budgetDisponible
        };

        var pgeVm = function (data, isEx) {
            id = ko.observable(data.Id),
            numeroAffaire = ko.observable(data.NumeroAffaire),
            //surface = ko.observable(data.Surface),
            //commentauire = ko.observable(data.Commentaire),
            isEx = ko.observable(isEx),
            rang = ko.observable(data.Rang),
            ligne = ko.observable(data.Ligne),
            lieu = ko.observable(data.Lieu)
        };

        //Post Enregistrer la nouvelle programmation
        $this.isValideStep2 = ko.observable(false);
        $this.saveNP = function () {
            if ($this.selectednatureTravaux() != undefined) {
                $('#loading').show();
                $('#divInfoMessage').show();
                $this.isloading(true);
                $this.infoCalcul('Le calcul de la nouvelle programmation est en cours...');
                var jsonData = ko.toJS(model);
                var xhr = $.post("/Calcul/SaveNouvelleProgrammation", jsonData)
                  .success(function (data) {
                      var idProgrammation = ko.toJS(data);
                      $this.idProgrammation(idProgrammation);
                      //console.log(idProgrammation);
                      $.getJSON("/api/GetAllDocumentPgeByProgrammation/" + idProgrammation, function (data) {
                          $this.listePGE(data);
                      });

                      $this.infoCalcul('Le calcul est terminé avec succès.');
                      $('#divInfoMessage').show();

                      //AFFICHER ETAPE 3
                      $this.isValideStep2(true);
                      $('#mytabs a[href="#contentstep3"]').tab('show');
                      $('#divValidationNp').hide();

                      $('#loading').hide();
                      $this.isloading(false);
                      $this.errMsg('');
                      $this.npisValide(0);
                  })
                .error(function () {
                    $('#loading').hide();
                    $this.isloading(false);
                    $this.infoCalcul('Une erreur est généré. Veuillez vérifier le fichier de Log.');
                    //alert('erreur de calcul');
                });
            }
            else {
                alert('Veuillez sélectionner une nature des travaux.');
            }
        };

        $this.validNewProgrammation = function () {
            //Get liste Annees
            $.getJSON("/api/GetAllAnnees/" + $this.anneeSaisie(), function (data) {
                $this.listeAnnees(data);
                console.log($("#listeofAnnee option:last").val());
                console.log($("#dateYears option[value='2020']").val());
                $('#_dateTravauxExternes').datepicker({ changeMonth: true, yearRange: $this.anneeSaisie() + ':' + $("#listeofAnnee option:last").val(), changeYear: true });

            }); //fin getJSON
        }

        //*********** INTERFACE RNM ***********//
        //save tableau RNM
        $this.saveRnm = function () {

            var ligne = $this.selectedligne();
            var lieu = $this.selectedlieu();
            var annee = $this.selectedAnnee();

            var newRnm = new Rnm(ligne, lieu, annee);

            var isExiste = ko.utils.arrayFirst($this.listeRnm(), function (item) {
                var result = item.ligne() === newRnm.ligne() && item.lieu() === newRnm.lieu() && item.annee() === newRnm.annee();
                return result;
            });

            if (!isExiste) {
                $this.listeRnm.push(newRnm);
            }
            else {
                $this.errMsg(' Ligne en double, Veuillez modifier RNM.')
                $('#divValidationNp').show();
                $this.npisValide(1);
            }
        };

        //save tableau RNM
        $this.saveTravauxExternes = function () {

            var ligne = $this.selectedlignete();
            var lieu = $this.selectedlieute();
            var nte = $this.selectednaturetravaurxternes();
            if ($("#_dateTravauxExternes").val() == undefined || $("#_dateTravauxExternes").val().length < 10) {
                $this.errMsg(' Veuillez séléctionner une date.')
                $('#divValidationNp').show();
                $this.npisValide(1);
                return;
            }
            //var date = $this.selectedDatete();
            var splitDate = $("#_dateTravauxExternes").val().split('/');
            var date = splitDate[1] + "/" + splitDate[2];

            //$('#divValidationNp').hide();
            $this.npisValide(0);
            //var datecompleted = ((new Date(date).getMonth() + 1).toString().length == 1 ? "0" + (new Date(date).getMonth() + 1) : new Date(date).getMonth() + 1) + "/" + new Date(date).getFullYear();
            var datecompleted = date;

            var newTrEx = new TravauxExt(ligne, lieu, datecompleted, nte);

            var isExiste = ko.utils.arrayFirst($this.listeTE(), function (item) {
                var result = item.ligne() === newTrEx.ligne() &&
                                item.lieu() === newTrEx.lieu() &&
                                item.NatureTravauxExt() === newTrEx.NatureTravauxExt() &&
                                item.date() === newTrEx.date();
                return result;
            });

            if (!isExiste) {
                $this.listeTE.push(newTrEx);
            }
            else {
                $this.errMsg(' Ligne en double, Veuillez modifier les données des travaux externes.')
                $('#divValidationNp').show();
                $this.npisValide(1);
            }
        };

        //supprimer ligne RNM
        $this.removeRnm = function (rnm) {
            $this.listeRnm.remove(rnm);
        };

        //supprimer ligne TE
        $this.removeTravauxExt = function (te) {
            $this.listeTE.remove(te);
        }

        //Functions
        $this.nextstep = function () {
            $('#mytabs a[href="#contentstep2"]').tab('show');
            $('#divValidationNp').hide();
            $this.errMsg('');
            $this.npisValide(0);
        };

        $this.closed = function () {
            $this.npisValide(true);
            $('#divValidationNp').hide();
        }

        /// Méthode pour obtenir l'ordre bidirectionnel par l'identifiant de la ligne
        $this.getLignesOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeRnm: $this.listeRnm()
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetLignesOrdreAsc", jsonData)
                               .success(function (data) {
                                   $this.listeRnm.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newRnm = new Rnm(item.Ligne, item.Lieu, item.Annee);
                                       $this.listeRnm.push(newRnm);
                                       $this.isOrdredAsc(true);
                                       $this.isAscLignes(false);
                                       $this.isDescLignes(true);
                                   });
                               });
            }
            else {
                //Ordre Desc
                $this.listeRnm.reverse();
                $this.isOrdredAsc(false);
                $this.isAscLignes(true);
                $this.isDescLignes(false);
            }
        }

        /// Méthode pour obtenir l'ordre bidirectionnel par l'identifiant du lieu
        $this.getLieuxOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeRnm: $this.listeRnm()
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetLieuxOrdreAsc", jsonData)
                               .success(function (data) {
                                   $this.listeRnm.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newRnm = new Rnm(item.Ligne, item.Lieu, item.Annee);
                                       $this.listeRnm.push(newRnm);

                                   });
                                   $this.isOrdredAsc(true);
                                   $this.isAscLieux(false);
                                   $this.isDescLieux(true);
                               });
            }
            else {
                //Ordre Desc
                $.post("/api/GetLieuxOrdreDesc", jsonData)
                             .success(function (data) {
                                 $this.listeRnm.removeAll();
                                 ko.utils.arrayForEach(data, function (item) {
                                     var newRnm = new Rnm(item.Ligne, item.Lieu, item.Annee);
                                     $this.listeRnm.push(newRnm);
                                 });

                                 $this.isOrdredAsc(false);
                                 $this.isAscLieux(true);
                                 $this.isDescLieux(false);
                             });
            }
        }

        /// Méthode pour obtenir l'ordre bidirectionnel par année
        $this.getAnneesOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeRnm: $this.listeRnm()
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetAnneesOrdreAsc", jsonData)
                               .success(function (data) {
                                   $this.listeRnm.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newRnm = new Rnm(item.Ligne, item.Lieu, item.Annee);
                                       $this.listeRnm.push(newRnm);
                                       $this.isOrdredAsc(true);
                                       $this.isAscAnnee(false);
                                       $this.isDescAnnee(true);
                                   });
                               });
            }
            else {
                //Ordre Desc
                $this.listeRnm.reverse();
                $this.isOrdredAsc(false);
                $this.isAscAnnee(true);
                $this.isDescAnnee(false);
            }
        }

        //Ordre Tableau Travaux Natures:
        /// Méthode pour obtenir l'ordre bidirectionnel par l'identifiant de la ligne
        $this.getLignesTeOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeTravauxExt: $this.listeTE(),
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetLignesTeOrdreAsc", jsonData)
                               .success(function (data) {
                                   $this.listeTE.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newTrEx = new TravauxExt(item.Ligne, item.Lieu, item.Date, item.NatureTravauxExt);
                                       $this.listeTE.push(newTrEx);
                                       $this.isOrdredAsc(true);
                                       $this.isAscLignesTe(false);
                                       $this.isDescLignesTe(true);
                                   });
                               });
            }
            else {
                //Ordre Desc
                $this.listeTE.reverse();
                $this.isOrdredAsc(false);
                $this.isAscLignesTe(true);
                $this.isDescLignesTe(false);
            }
        }

        /// Méthode pour obtenir l'ordre bidirectionnel par l'identifiant du lieu
        $this.getLieuxTeOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeTravauxExt: $this.listeTE(),
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetListeTravauxExterneByLieuxAsc", jsonData)
                               .success(function (data) {
                                   $this.listeTE.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newTrEx = new TravauxExt(item.Ligne, item.Lieu, item.Date, item.NatureTravauxExt);
                                       $this.listeTE.push(newTrEx);
                                       $this.isOrdredAsc(true);
                                       $this.isAscLieuxTe(false);
                                       $this.isDescLieuxTe(true);
                                   });
                               });
            }
            else {
                //Ordre Desc
                $.post("/api/GetListeTravauxExterneByLieuxDesc", jsonData)
                              .success(function (data) {
                                  $this.listeTE.removeAll();
                                  ko.utils.arrayForEach(data, function (item) {
                                      var newTrEx = new TravauxExt(item.Ligne, item.Lieu, item.Date, item.NatureTravauxExt);
                                      $this.listeTE.push(newTrEx);
                                  });
                                  $this.isOrdredAsc(false);
                                  $this.isAscLieuxTe(true);
                                  $this.isDescLieuxTe(false);
                              });
            }
        }

        /// Méthode pour obtenir l'ordre bidirectionnel par ordre des natures travaux externes
        $this.getNatureTxTeOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeTravauxExt: $this.listeTE(),
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetListeTravauxExterneByNaturesTxAsc", jsonData)
                               .success(function (data) {
                                   $this.listeTE.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newTrEx = new TravauxExt(item.Ligne, item.Lieu, item.Date, item.NatureTravauxExt);
                                       $this.listeTE.push(newTrEx);
                                       $this.isOrdredAsc(true);
                                       $this.isAscNteTe(false);
                                       $this.isDescNteTe(true);
                                   });
                               });
            }
            else {
                //Ordre Desc
                $this.listeTE.reverse();
                $this.isOrdredAsc(false);
                $this.isAscNteTe(true);
                $this.isDescNteTe(false);
            }
        }

        /// Méthode pour obtenir l'ordre bidirectionnel par ordre des dates
        $this.getDateTeOrdre = function () {

            //Préparer les données pour poster
            var model = {
                ListeTravauxExt: $this.listeTE(),
            };
            var jsonData = ko.toJS(model);

            if (!$this.isOrdredAsc()) {
                //Ordre Asc
                $.post("/api/GetListeTravauxExterneByDateAsc", jsonData)
                               .success(function (data) {
                                   $this.listeTE.removeAll();
                                   ko.utils.arrayForEach(data, function (item) {
                                       var newTrEx = new TravauxExt(item.Ligne, item.Lieu, item.Date, item.NatureTravauxExt);
                                       $this.listeTE.push(newTrEx);
                                       $this.isOrdredAsc(true);
                                       $this.isAscDateTe(false);
                                       $this.isDescDateTe(true);
                                   });
                               });
            }
            else {
                //Ordre Desc
                $this.listeTE.reverse();
                $this.isOrdredAsc(false);
                $this.isAscDateTe(true);
                $this.isDescDateTe(false);
            }
        }

        /******************************************************************************************************************************************/
        /************************************************************ ETAPE 03 ********************************************************************/
        /******************************************************************************************************************************************/
        // #region Etape3

        //Recuperation des PGE de la nouvelle planification 
        //function ChargerPGE(idProgrammation) {

        //    $this.listePGE.removeAll();
        //    //Get PGE
        //    $.getJSON("/api/GetAllDocumentPgeByProgrammation/" + idProgrammation, function (data) {
        //        $this.listePGE(data);
        //    });

        //}

        //observable Nombre de PGE
        $this.nbrPGE = ko.computed(function () {
            return $this.listePGE().length;
        }, $this);


        //Boolean de validation de l'etape 3 
        $this.validFormStep3 = ko.computed(function () {
            if ($this.listePGE().length < 0) {
                return false;
            }

            if ($this.budgetDisponible() > 999999999999999.99 || $this.prixUnitaireTravaux() > 9999999999.99 || $this.budgetDisponible() < 1 || $this.prixUnitaireTravaux() < 1) {
                return false;
            }

            return true;

        });

        //Post Enregistrer l'etape 3 (Budget Disponible et prix unitaire)
        $this.isValideStep3 = ko.observable(false);
        $this.saveEtape3 = function () {
            console.log("Enregistrement etape 3");
            if ($this.budgetDisponible() > 999999999999999.99 || $this.prixUnitaireTravaux() > 9999999999.99 || $this.budgetDisponible() < 1 || $this.prixUnitaireTravaux() < 1) {
                $('#divValidationNp').show();
                $('#divInfoMessage').hide();
                $this.errMsg('Valeur éronné : «Budget disponible» composé d’un maximum de 15 chiffres et «Prix unitaire» composé d’un maximum de 10 chiffres');
                return;
            }
            $('#divValidationNp').hide();
            $('#loading').show();
            $this.isloading(true);
            $('#divInfoMessage').show();
            $this.infoCalcul('Enregistrement de l\'etape 3 est en cours...');
            var jsonData = ko.toJS(model);
            var xhr = $.post("/Calcul/SaveNouvelleProgrammation", jsonData)
              .success(function (data) {
                  $('#mytabs a[href="#contentstep4"]').tab('show');
                  $this.infoCalcul('Enregistrement de l\'etape 3 terminé');
                  $this.isloading(false);
                  $this.isValideStep3(true);
                  InitialiseFenetreEtape4();

              })
                .error(function () {
                    $this.isValideStep3(false);
                    $('#loading').hide();
                    $this.isloading(false);
                    $this.infoCalcul('Une erreur est générée. Veuillez vérifier le fichier de Log.');
                    //alert('erreur de calcul');
                });
        };
        // #endregion
        /******************************************************************************************************************************************/
        /********************************************************** FIN ETAPE 03 ******************************************************************/
        /******************************************************************************************************************************************/



        /******************************************************************************************************************************************/
        /************************************************************ ETAPE 04 ********************************************************************/
        /******************************************************************************************************************************************/
        // #region Etape4

        //************Gestion du modal TRAVAUX SUR PLUSIEURS ANNEES************//
        //Initialisation du modal
        $('#ModalTravauxSurPlusieursAnnees').on('shown.bs.modal', function () {
            $('#anneeRestant').focus();
            console.log("modal");
            var pgeNumAffaire = $('input[name=travaux]:checked').attr("numpge");
            var pge = ko.utils.arrayFirst($this.listePGE(), function (pge) {
                return pge.NumeroAffaire == pgeNumAffaire;
            }) || 'not found';

            //Vérification des pres requis
            if (pge == 'not found') {
                $('#ModalTravauxSurPlusieursAnnees').modal('toggle');
            }
            else {
                //Recuperation des données du model selectionnée
                $this.PGESelectedTSPA = pge;
                var newModel = new modelTSPA(pge);
                $this.objTSPA(newModel);
                var listePgeTSPA = [];
                if ($this.objTSPA().IsTravauxPlusieursAnnee()) {//déjà en travaux sur plusieurs années? recuperation de toutes les années
                    for (var i = 0; i < $this.listePGE().length; i++) {
                        if ($this.listePGE()[i].NumeroAffaire == pgeNumAffaire)
                            listePgeTSPA.push($this.listePGE()[i]);
                    }
                }
                //Affiche le nombre d annee restant
                $this.anneesRestant(listePgeTSPA.length);
                $this.Tpa_BudgetAnnee.removeAll();
                for (var i = 0; i < $this.anneesRestant() ; i++) {
                    $this.Tpa_BudgetAnnee.push(new Tpa_BudgetAnnee(parseInt(parseInt($this.objTSPA().Annee()) + parseInt(i)), listePgeTSPA[i].Budget));
                }
            }
        });

        //Model Travaux sur plusieurs années
        function modelTSPA(pge) {
            var self = this;
            self.Id = ko.observable(pge.Id),
            self.Ligne = ko.observable(pge.Ligne),
            self.Lieu = ko.observable(pge.Lieu),
            self.NumeroAffaire = ko.observable(pge.NumeroAffaire),
            self.M = ko.computed(function () {
                return pge.Surface;
            }, self),
            self.Previsionnel = ko.computed(function () {
                return pge.Surface * $this.prixUnitaireTravaux();
            }, self),
            self.Consomme = ko.observable(0),
            self.PourcentageConsomme = ko.computed(function () {
                return self.Previsionnel() == 0 ? 0 : (self.Consomme() / self.Previsionnel() * 100);
            }, self),

            self.ResteConsomme = ko.observable(parseFloat((parseFloat(parseFloat(pge.Surface) * parseFloat($this.prixUnitaireTravaux()))) - parseFloat(self.Consomme()))),
            self.Commentaire = ko.observable(pge.Commentaire),
            self.Annee = ko.observable(pge.Annee);
            self.IsTravauxPlusieursAnnee = ko.observable(pge.IsTravauxPlusieursAnnee);
        }

        //Initialisation du model 
        $this.PGESelectedTSPA = ko.observable();
        $this.anneesRestant = ko.observable(0);
        $this.Tpa_BudgetAnnee = ko.observableArray();
        $this.DifferenceBudget = ko.computed(function () {

            if ($this.objTSPA() == null)
                return 0;
            var total = parseFloat(0);
            for (var i = 0; i < $this.Tpa_BudgetAnnee().length; i++) {
                if ($this.Tpa_BudgetAnnee()[i].Budget() != null) {
                    total = parseFloat(total + parseFloat($this.Tpa_BudgetAnnee()[i].Budget()));
                }
                if (parseFloat(parseFloat($this.objTSPA().ResteConsomme()) - parseFloat(total)) < 0) {
                    alert("Le budget disponible pour cette PGE a été dépassé. Veuillez changer la distribution du budget par année.");
                }
            }
            return parseFloat(parseFloat($this.objTSPA().ResteConsomme()) - parseFloat(total));
        }, this);
        $this.anneesRestant.subscribe(function (newValue) {
            if (newValue) {// Has focus
                if (newValue < 0) {
                    alert("La valeur «Nombre d’années restantes» doit être strictement supérieure à 0");
                    return
                }
                $this.Tpa_BudgetAnnee.removeAll();
                for (var i = 0; i < newValue; i++) {
                    $this.Tpa_BudgetAnnee.push(new Tpa_BudgetAnnee(parseInt(parseInt($this.objTSPA().Annee()) + parseInt(i)), -1));
                }
            }
        });

        function Tpa_BudgetAnnee(annee, budget) {
            this.AnneeBudget = ko.observable(annee);
            this.Budget = ko.observable(budget);
        }

        $this.formIsValid = ko.computed(function () {
            if ($this.anneesRestant() <= 0 || $this.DifferenceBudget() < 0)
                return false;
            for (var i = 0; i < $this.Tpa_BudgetAnnee().length ; i++) {
                if ($this.Tpa_BudgetAnnee()[i].Budget() < 0)
                    return false;
            }
            return true;
        });

        $this.CalculMontant = function (m) {
            return parseFloat(parseFloat($this.prixUnitaireTravaux()) * parseFloat(m)).toFixed(2);
        };

        $this.SaveTSPA = function () {
            // Delete les PGE deja dans la liste
            var NumAffaire = $this.PGESelectedTSPA.NumeroAffaire;
            var success = true;

            //Suppression de toutes les PGE des Année suivante avant de les recréer
            var firstPge = true;
            for (var numPGE = 0; numPGE < $this.listePGE().length; numPGE++) {
                if ($this.listePGE()[numPGE].NumeroAffaire == $this.objTSPA().NumeroAffaire())
                    if (!firstPge) {
                        $this.listePGE.remove($this.listePGE()[numPGE]);
                        numPGE--;
                    } else {
                        firstPge = false;
                    }
            }

            //Ajout de toutes les PGE redéfinis
            for (var i = 0; i < $this.Tpa_BudgetAnnee().length ; i++) {

                if (i == 0) {// on modifie seulement l'ancienne pge deja dans la base
                    $this.PGESelectedTSPA.Annee = parseInt(parseInt($this.objTSPA().Annee()) + parseInt(i));
                    $this.PGESelectedTSPA.Budget = parseFloat(parseFloat($this.Tpa_BudgetAnnee()[i].Budget()));
                    $this.PGESelectedTSPA.Commentaire = $this.objTSPA().Commentaire;
                    console.log($this.objTSPA().Commentaire());
                    $this.PGESelectedTSPA.IsTravauxPlusieursAnnee = true;
                }
                else {
                    //Copie correctement l'objet sans prendre seulement sa reference
                    var temp = $this.PGESelectedTSPA.constructor();
                    for (var attr in $this.PGESelectedTSPA) {
                        if ($this.PGESelectedTSPA.hasOwnProperty(attr))
                            temp[attr] = $this.PGESelectedTSPA[attr];
                    }
                    //Enregistre Nouvelle PGE et la renvoie
                    temp.$id = "0";
                    //Set nouvelle PGE
                    temp.Annee = parseInt(parseInt($this.objTSPA().Annee()) + parseInt(i));
                    temp.Budget = parseFloat(parseFloat($this.Tpa_BudgetAnnee()[i].Budget()));
                    temp.Commentaire = $this.objTSPA().Commentaire();
                    temp.IsTravauxPlusieursAnnee = true;
                    temp.Rang = 0;


                    //ajout dans la liste de PGE
                    for (var j = 0; j < $this.listeBudgetAnnee().length; j++) {
                        if ($this.listeBudgetAnnee()[j].Annee == temp.Annee) {
                            //On verifie que les PGE de l'années ( sur plusieurs années dans cette année ne dépasse pas le budget )
                            var somme = parseFloat(0);
                            for (var k = 0; k < $this.listeBudgetAnnee()[j].listeDesPGESurLAnnee.length ; k++) {
                                if ($this.listeBudgetAnnee()[j].listeDesPGESurLAnnee[k].IsTravauxPlusieursAnnee) {
                                    somme += parseFloat($this.listeBudgetAnnee()[j].listeDesPGESurLAnnee[k].Budget);
                                }
                            }
                            if (somme > $this.listeBudgetAnnee()[j].Budget) {
                                success = false;
                                alert("Budget insuffisant pour l'année :" + $this.listeBudgetAnnee()[j].Annee + ". Augmentez le budget annuel ou réduisez le budget de cette PGE sur cette année.")
                            } else {
                                $this.listePGE.push(temp);
                            }
                        }
                    }

                }
            }
            if (success) {
                //integration des nouvelles PGE dans la liste des PGE
                console.log($this.listePGE());
                RefreshPGE();
                $('#ModalTravauxSurPlusieursAnnees').modal('toggle');
            }
        }



        //************Gestion Consulter************//
        $this.ConsulterPGE = function () {
            $this.listPgeToConsult.removeAll();
            var pges = $('input[name=consulter]:checked').each(function () {/*.attr("numpge")*/
                var NumAffaire = $(this).attr("numpge");
                var pge = ko.utils.arrayFirst($this.listePGE(), function (pge) {
                    return pge.NumeroAffaire == NumAffaire;
                }) || 'not found';
                if (pge != 'not found') {
                    $this.listPgeToConsult.push(pge);
                }
            });

            jsonData = ko.toJS($this.listPgeToConsult());
            console.log($this.listPgeToConsult());
            console.log(jsonData);
            var xhr = $.post("/api/GetAllValeurParametres/", jsonData)
              .success(function (data) {
                  console.log(data);
                  //Envoie des Donnée //TODO : envoyer les données charger (data) dans la nouvelle fenetre 
                  window.open("Calcul/ConsulterDocumentPGE?data=" + data);
              })
                .error(function () {
                    $('#loading').hide();
                    $this.isloading(false);
                    $this.infoCalcul('Une erreur est généré. Veuillez vérifier le fichier de Log.');
                    //alert('erreur de calcul');
                });
        };


        //************Gestion Etape 4************//


        function InitialiseFenetreEtape4() {
            //Calcul du Budget
            for (var i = 0; i < $this.listePGE().length; i++) {
                if (!$this.listePGE()[i].IsTravauxPlusieursAnnee || $this.listePGE()[i].Budget == null)
                    $this.listePGE()[i].Budget = parseFloat(parseFloat($this.listePGE()[i].Surface) * parseFloat($this.prixUnitaireTravaux())).toFixed(2);;
            }
            //Verification d ex aequo et ajustement des boutons de validation
            RefreshPGE();
        }


        function RefreshPGE() {

            //Reorganise les PGE par années
            OrganisePGEParAnnee();

            //Si l enregistrement des Exaequo est terminee
            if ($this.ValidExAEQUO()) {
                OrganisationRangPGE();
            }

            //rechargement de l'encadrement
            ActualiseUpDown();
            //Recalcule du coût cumuler 
            CalculCumul();
        }


        //Gestion Budget par annee
        function BudgetSurAnnee(annee, budget, listPGE) {
            this.Annee = annee;
            this.Budget = budget;
            this.InfoAnnee = this.Annee + " - Budget disponible : " + this.Budget;
            this.listeDesPGESurLAnnee = listPGE;
            for (var i = 0; i < listPGE.length; i++) {
                if (!listPGE[i].IsTravauxPlusieursAnnee) {
                    listPGE[i].Annee = this.Annee;
                }
            }
            this.NbrPge = listPGE.length;
        }

        $this.listeBudgetAnnee = ko.observableArray();
        ///Methode permettant de découper la liste de PGE par Année de Budget. (Necessaire pour la fusion des cellules).
        //Calcul des position pour les Travaux sur plusieurs années.
        function OrganisePGEParAnnee() {
            //console.log($this.listePGE());
            $this.listeBudgetAnnee.removeAll();
            var listPgeDecoupee = [];
            var DebutAnnee = true;
            var AnneeDeDecoupage = parseInt($this.anneeSaisie());
            var somme = 0;
            for (var i = 0; i < $this.listePGE().length; i++) {
                //Au debut de chaque Année de découpage
                if (DebutAnnee) {
                    //On commence par recuperer les PGE Ajouté recemment (IsTravauxSurPlusieursAnnees) qui ne sont pas encore classé (Rang = 0) (Positionner en fin de listePGE)
                    //On les ajoute au debut de l'année
                    for (var numNewPGE = 0; numNewPGE < $this.listePGE().length; numNewPGE++) {
                        if ($this.listePGE()[numNewPGE].Rang == 0 && $this.listePGE()[numNewPGE].Annee == AnneeDeDecoupage) {
                            $this.listePGE()[numNewPGE].Rang = -1;//valeur qui sera réecrite avec la méthode OrganisationRangPGE
                            $this.listePGE().splice(i, 0, $this.listePGE()[numNewPGE]); //On deplace la ligne à la position du curseur sur l'ensemble des PGE
                            $this.listePGE().splice((numNewPGE + 1), 1); //On supprime sa correspondance en fin de tableau
                        }
                    }
                    DebutAnnee = false;
                }

                if ((((parseFloat($this.listePGE()[i].Surface) * parseFloat($this.prixUnitaireTravaux())) + somme) > (parseFloat($this.budgetDisponible()) * parseFloat(parseFloat($this.listeBudgetAnnee().length) + 1))) || ($this.listePGE()[i].Annee > AnneeDeDecoupage && $this.listePGE()[i].IsTravauxPlusieursAnnee)) {
                    $this.listeBudgetAnnee.push(new BudgetSurAnnee(AnneeDeDecoupage, $this.budgetDisponible(), listPgeDecoupee));
                    listPgeDecoupee = [];
                    DebutAnnee = true;
                    AnneeDeDecoupage++;
                }
                somme = parseFloat(somme) + (parseFloat($this.listePGE()[i].Surface) * parseFloat($this.prixUnitaireTravaux()));
                listPgeDecoupee.push($this.listePGE()[i]);
            }
            // On ajoute les PGE restantes 
            if (listPgeDecoupee.length > 0)
                $this.listeBudgetAnnee.push(new BudgetSurAnnee(AnneeDeDecoupage, $this.budgetDisponible(), listPgeDecoupee));

            //Refresh le tableau (actualise les sous attributs) : Force un refresh 
            var dataTable = $this.listePGE().slice(0);
            $this.listePGE([]);
            $this.listePGE(dataTable);

        }

        //Methode permettant d'ajuster les bouton + et - 
        function ActualiseUpDown() {
            $("#tabPGEProposee").children("tr").each(function () {
                //console.log($(this).index() + " == 0 ?   //  et " + $(this).index() + " == " + $(this).parent().children("tr").last().index() + " ?");
                if ($(this).index() == 0) {
                    $(this).find(".AvancerPGE").addClass("hideColumn");
                } else {
                    $(this).find(".AvancerPGE").removeClass("hideColumn");
                }
                if ($(this).index() == $(this).parent().children("tr").last().index()) {
                    $(this).find(".ReculerPGE").addClass("hideColumn");
                } else {
                    $(this).find(".ReculerPGE").removeClass("hideColumn");
                }
                //AfficheExAequo();
            });
        }

        //Affiche le cumule des couts des PGE. Cette valeur n'est jamais enregistré 
        function CalculCumul() {
            var cout = 0;
            //Cumul de l'étape 4
            $("#tabPGEProposee").children("tr").each(function () {
                cout = parseFloat(cout) + parseFloat($(this).find(".montant").text());
                $(this).find(".cumul").text(cout.toFixed(2));
            });
            //Cumul de l'étape 5
            $("#tabPGEProposee2").children("tr").each(function () {
                cout = parseFloat(cout) + parseFloat($(this).find(".montant").text());
                $(this).find(".cumul").text(cout.toFixed(2));
            });
        }

        //Met a jour les numeros d' ordre des PGE
        function OrganisationRangPGE() {
            var numOrdre = 1;
            for (var pge = 0 ; pge < $this.listePGE().length; pge++) {
                $this.listePGE()[pge].Rang = numOrdre;
                $this.listePGE()[pge].IsEx = false;
                $this.listePGE()[pge].IsValidEx = true;
                numOrdre++;
            }

            //Classe les PGE par Année (découpage budget)
            OrganisePGEParAnnee();
        }



        //Attributs permettant de verifier si les PGE sont ExAequo et s'ils peuvent monter et/ou descendre
        $this.CanUp = function (data) {
            //Verification post calcul des PGE (empêche le disfonctionnement lors des étapes 1 à 3)
            if ($this.listePGE().length == 0)
                return;
            var position = ko.utils.arrayIndexOf($this.listePGE(), data);

            if (typeof ($this.listePGE()[position]) == 'undefined' || typeof (($this.listePGE()[(position - 1)])) == 'undefined')
                return;

            if (position > 0) {
                //verification du precedent
                if ($this.listePGE()[position].Rang == $this.listePGE()[(position - 1)].Rang) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        };
        $this.CanDown = function (data) {
            //Verification post calcul des PGE (empêche le disfonctionnement lors des étapes 1 à 3)
            if ($this.listePGE().length == 0)
                return;
            var position = ko.utils.arrayIndexOf($this.listePGE(), data);
            if (position == -1 || $this.listePGE()[position] === 'undefined' || $this.listePGE()[(position + 1)] === 'undefined')
                return;
            if (position < ($this.listePGE().length - 1)) {
                //verification du precedent
                if ($this.listePGE()[position].Rang == $this.listePGE()[(position + 1)].Rang) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        };

        //Méthodes de déplacement des PGE
        $this.MovePGEUp = function () {
            //Recherche de la PGE

            //Recherche de la PGE Precedente

            //


            //Méthode sans la prise en compte des Travaux sur plusieurs années
            var position = ko.utils.arrayIndexOf($this.listePGE(), this);
            var tmpPrec = $this.listePGE()[(position - 1)];
            var tmpCible = $this.listePGE()[(position)];

            $this.listePGE.replace($this.listePGE()[position], tmpPrec);
            $this.listePGE.replace($this.listePGE()[(position - 1)], tmpCible);

            //Méthode pour mettre à jour les PGE
            RefreshPGE();
        };
        $this.MovePGEDown = function () {
            //Méthode sans la prise en compte des Travaux sur plusieurs années
            var position = ko.utils.arrayIndexOf($this.listePGE(), this);
            var tmpSuiv = $this.listePGE()[(position + 1)];
            var tmpCible = $this.listePGE()[(position)];

            $this.listePGE.replace($this.listePGE()[(position + 1)], tmpCible);
            $this.listePGE.replace($this.listePGE()[position], tmpSuiv);

            //Méthode pour mettre à jour les PGE
            RefreshPGE();

        };

        $this.ValidExAEQUO = ko.observable(false);
        $this.ValideEXAEQUO = function () {
            var self = this;
            $this.ValidExAEQUO(true);
            OrganisationRangPGE();
            console.log($this.listePGE());
            $.ajax({
                url: '/api/SaveDocumentPgeByListDocumentPge/',
                contentType: "application/json",
                type: "POST",
                data: JSON.stringify($this.listePGE()),
                //data: {
                //    listePGE: JSON.stringify($this.listePGE()),
                //    model: JSON.stringify(model)
                //},
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("FAIL: " + errorThrown);
                    $('#divValidationNp').show();
                    $this.errMsg('Validation deExaequo échoué.');

                },
                success: function (data, textStatus, jqXHR) {
                    console.log("SUCCES");
                }
            });

            //Recalcule du coût cumuler 
            CalculCumul();
            ActualiseUpDown();
        };

        $this.isValideStep4 = ko.observable(false);
        $this.EnregistreProgrammation = function () {

            $.ajax({
                url: '/api/SaveDocumentPgeByListDocumentPge/',
                contentType: "application/json",
                type: "POST",
                data: JSON.stringify($this.listePGE(), model),
                //data: {
                //    listePGE: JSON.stringify($this.listePGE()),
                //    model: JSON.stringify(model)
                //},
                error: function (jqXHR, textStatus, errorThrown) {
                    console.log("FAIL: " + errorThrown);
                    $('#divValidationNp').show();
                    $this.errMsg('Validation de la programmation échoué.');
                    $('#divInfoMessage').show();
                },
                success: function (data, textStatus, jqXHR) {
                    console.log("SUCCES");
                    $this.infoCalcul('Validation de la programmation terminée avec succée.');
                    $('#divInfoMessage').show();
                    $('#mytabs a[href="#contentstep5"]').tab('show');
                    $this.isValideStep4(true);

                    console.log($this.listePGE());

                }
            });


        };


        //#endregion
        /******************************************************************************************************************************************/
        /********************************************************** FIN ETAPE 04 ******************************************************************/
        /******************************************************************************************************************************************/



        /******************************************************************************************************************************************/
        /************************************************************ ETAPE 05 ********************************************************************/
        /******************************************************************************************************************************************/
        // #region Etape5


        $this.Historique = function () {
            //GRAPH POUR DEMANDE DE TRAVAUX 
        };

        $this.Categories = function () {
            window.open("Calcul/GetDocumentsPgeWithCategories?idProgrammation=" + $this.idProgrammation() + "&AnPro=" + $this.anneeSaisie() + "&Ntv=" + $this.selectednatureTravaux().Nature);
        };

        $this.Extraction = function () {
            //GESTION EXCEL
        };


        //#endregion
        /******************************************************************************************************************************************/
        /********************************************************** FIN ETAPE 05 ******************************************************************/
        /******************************************************************************************************************************************/



    };

    //Object RNM
    function Rnm(ligne, lieu, annee) {
        var $this = this;
        $this.ligne = ko.observable(ligne);
        $this.lieu = ko.observable(lieu);
        $this.annee = ko.observable(annee);
    }

    //objet travaux Externe
    function TravauxExt(ligne, lieu, date, natutetravauxext) {
        var $this = this;
        $this.ligne = ko.observable(ligne);
        $this.lieu = ko.observable(lieu);
        $this.date = ko.observable(date);
        $this.NatureTravauxExt = ko.observable(natutetravauxext);
    }

    //custome Binding pour datepicker
    ko.bindingHandlers.datepicker = {
        init: function (element, valueAccessor, allBindingsAccessor) {
            //initialize datepicker with some optional options
            var options = allBindingsAccessor().datepickerOptions || {};
            $(element).datepicker({
                changeMonth: true,
                changeYear: true,
                //onClose: function (dateText, inst) {
                //    $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
                //}
            });

            $(element).datepicker(options);

            //handle the field changing
            ko.utils.registerEventHandler(element, "change", function () {
                var observable = valueAccessor();
                var date = $(element).datepicker("getDate");
                if (date != null) {
                    //var datecomplet = date.getMonth() + "/" + date.getFullYear();
                    var datecomplet = date.getMonth() + "/" + date.getFullYear();
                }

                observable(date);
            });

            //handle disposal (if KO removes by the template binding)
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).datepicker("destroy");
            });

        },
        //update the control when the view model changes
        update: function (element, valueAccessor) {
            var value = ko.utils.unwrapObservable(valueAccessor()),
                current = $(element).datepicker("getDate");

            if (value - current !== 0) {
                $(element).datepicker("setDate", value);
            }
        }
    };

    ko.applyBindings(new viewmodel());
}




/******************************************************************************************************************************************/
/************************************************************ ETAPE Consulter PGE *********************************************************/
/******************************************************************************************************************************************/





/******************************************************************************************************************************************/
/********************************************************** FIN ETAPE Consulter PGE *******************************************************/
/******************************************************************************************************************************************/




/******************************************************************************************************************************************/
/************************************************************ ETAPE Categorie PGE *********************************************************/
/******************************************************************************************************************************************/
function initCategorie() {

    var ViewModelCategorie = function () {
        $this = this;

        var model = {
            ListePGE: $this.listePGE
        };

        $this.listePGE = ko.observableArray();
        console.log($this.listePGE());
        ////le model
        //var modelPGE = {
        //    Id: $this.Id,
        //    Programmation:$this.Programmation,
        //    NumeroAffaire:$this.NumeroAffaire,
        //    Lieu:$this.Lieu,
        //    Ligne:$this.Ligne,
        //    Median:$this.Median,
        //    Rang:$this.Rang,
        //    Surface:$this.Surface,
        //    Annee: $this.Annee,
        //    Budget: $this.Budget,
        //    Commentaire:$this.Commentaire,
        //    IsEx:$this.IsEx,
        //    IsValidEx:$this.IsValidEx,
        //    IsTravauxPlusieursAnnee: $this.IsTravauxPlusieursAnnee

        //};

        $this.CategoriesPGE = ko.observableArray([
                   new Categorie("CAT 4", $this.listePGE),
                   new Categorie("CAT 3", $this.listePGE),
                   new Categorie("CAT 2", $this.listePGE),
                   new Categorie("CAT 1", $this.listePGE)
        ])
        function Categorie(cat, listPGE, cout) {
            this.Categorie = cat;
            this.PGE = listPGE;
            this.Cout = function () {
                var total = parseFloat(0);
                $.each(listPGE, function (i) {
                    total += parseFloat(subjects[i].Rang);
                });
                return parseFloat(total).toFixed(2);
            };
            this.rowSpan = listPGE.length;
        }

    }

    ko.applyBindings(new ViewModelCategorie());

}



/******************************************************************************************************************************************/
/********************************************************** FIN ETAPE Categorie PGE *******************************************************/
/******************************************************************************************************************************************/

var ligneAutomatise = function (id, name, checked, activee, chemin) {
    this.Id = id;
    this.Name = name;
    this.Checked = ko.observable(checked);
    //this.Checked.subscribe(function (value) {

    //    alert(viewmodel.lignes().length);
    //});

    this.Activee = activee;
    this.Chemin = chemin;

}