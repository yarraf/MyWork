
//********** MODIFICATION PROGRAMMATION **********//
function InitialiseElementsMP() {
    //ko
    var viewmodelModification = function () {
        var $this = this;

        $this.isloading = ko.observable(false);
        $this.infoCalcul = ko.observable();
        $this.anneeProgrammation = ko.observable();
        $this.listeTypeOuvrages = ko.observableArray();
        $this.listeNatureTravaux = ko.observableArray();
        $this.selectedTypeOuvrage = ko.observable();
        $this.selectednatureTravaux = ko.observable();

        var modelModification = {
            Idprogrammation: null,
            LignesGestionnaires: null,
            ListeRnm: null,
            ListeTravauxExt: null,
            AnneeProgrammation: $this.anneeProgrammation,
            SelectedNatureTravaux: $this.selectednatureTravaux,
            SelectedTypeOuvrage: $this.selectedTypeOuvrage,
            PrixUnitaireTravaux: null,
            BudgetDisponible: null
        };

        //Get GetAllTypeOuvrages
        $.getJSON("/api/GetAllTypeOuvrages", function (data) {
            $this.listeTypeOuvrages(data);
        }); //fin getJSON

        //Get GetAllTypeOuvrages
        $.getJSON("/api/GetAllNatureTravaux", function (data) {
            $this.listeNatureTravaux(data);
        }); //fin getJSON

        $this.chargeProgrammation = function () {
            $this.isloading(true);
            console.log(modelModification)
            var jsonData = ko.toJS(modelModification);
            var xhr = $.post("/Calcul/AfficherProgrammation", jsonData)
                .success(function (data) {

                })
                .error(function () {
                    $('#divInfoMessage').show();
                    $this.isloading(false);
                    $this.infoCalcul('Une erreur est généré. Veuillez vérifier le fichier de Log.');
                });
        };


    };
    ko.applyBindings(new viewmodelModification());
}

//********** Consultation Suivi PROGRAMMATION **********//
function InitialiseElementsCSP() {
    //ko
    var viewmodelSuivi = function () {
        var $this = this;

        $this.isloading = ko.observable(false);
        $this.infoCalcul = ko.observable();
        $this.listeTypeOuvrages = ko.observableArray();
        $this.listeNatureTravaux = ko.observableArray();
        $this.selectedTypeOuvrage = ko.observable();
        $this.selectednatureTravaux = ko.observable();


        var modelSuivi = {
            SelectedNatureTravaux: $this.selectednatureTravaux,
            SelectedTypeOuvrage: $this.selectedTypeOuvrage,

        };

        //Get GetAllTypeOuvrages
        $.getJSON("/api/GetAllTypeOuvrages", function (data) {
            $this.listeTypeOuvrages(data);
        }); //fin getJSON

        //Get GetAllTypeOuvrages
        $.getJSON("/api/GetAllNatureTravaux", function (data) {
            $this.listeNatureTravaux(data);
        }); //fin getJSON

        $this.chargeProgrammationConsultation = function () {

            $this.isloading(true);
            console.log(modelSuivi)
            var jsonData = ko.toJS(modelSuivi);
            var xhr = $.post("/Calcul/AfficherProgrammation", jsonData)
                .success(function (data) {
                    $('#divInfoMessage').show();
                    $this.isloading(false);
                    $this.infoCalcul('Chargement de la programmation terminée.');
                })
                .error(function () {
                    $('#divInfoMessage').show();
                    $this.isloading(false);
                    $this.infoCalcul('Une erreur est généré. Veuillez vérifier le fichier de Log.');
                });
        };


    };
    ko.applyBindings(new viewmodelSuivi());
}


//********************** Archivage *********************//
function InitialiseElementsArchivage() {
    //ko
    var viewmodelArchivage = function () {
        var $this = this;

        $this.isloading = ko.observable(false);
        $this.infoCalcul = ko.observable();
        $this.listProgramme = ko.observableArray();

        $.getJSON("/api/GetAllProgrammation", function (data) {
            $this.listProgramme(data);
        }); //fin getJSON

        $this.OuvrirExcel = function () {
            console.log(this);
            var idProgrammation = this.Id;
            $.ajax({
                url: '/api/ExecuteInstanceExcel/' + idProgrammation,
                contentType: "application/json",
                type: "POST"
            });

        }

        $this.confirmeToDelete = function () {
            console.log(this);
            $this.programationToDelete = this;
        }

        $this.programationToDelete = null;

        $this.suppressionProgrammation = function () {
            $('#loading').show();
            $this.isloading(true);
            var idProgrammation = $this.programationToDelete.Id;
            var programmation = $this.programationToDelete;
            console.log(idProgrammation);
            console.log($this.programationToDelete);
            $.ajax({
                url: '/api/RemoveProgrammation/' + idProgrammation,
                contentType: "application/json",
                type: "POST",
                error: function (jqXHR, textStatus, errorThrown) {
                    $('#divInfoMessage').show();
                    $this.infoCalcul('Une erreur est généré. Veuillez vérifier le fichier de Log.');
                    $('#loading').hide();
                    $this.isloading(false);
                },
                success: function (data, textStatus, jqXHR) {
                    $('#divInfoMessage').show();
                    $this.listProgramme.remove(programmation);
                    $this.isloading(false);
                    $this.infoCalcul('Suppression terminée avec succés');
                    $('#loading').hide();
                    $this.isloading(false);
                }
            });
        
        };

    };
    ko.applyBindings(new viewmodelArchivage());
}