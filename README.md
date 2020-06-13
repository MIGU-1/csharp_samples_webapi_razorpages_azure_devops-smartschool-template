# Tutorial 'SmartSchool (Azure)'

# Lehrziele

* ASP.NET Core Razor Pages
* ASP.NET Core WebApi
* Entity Framework Core
* Unit of Work / Repositories
* Validierung
* Azure Grundlagen
* Azure Web App
* Azure DevOps
* Azure Build Pipelines
* Azure Release Pipelines

# Allgemeines

Es ist eine einfache Anwendung zur Verwaltung von Sensoren un deren Messwerte gegegeben. Diese lokal lauffährige Anwednung (Web API und Web App) soll nun per Continuous Integration und Continuous Deliviery gebaut und ausgeliefert werden. Somit können kurze Release Zyklen eingehalten werden was die Kennzahl `time to market` für neue Features drastisch reduziert.

Die Grundfunktionalität ist bereits implementiert und befindet sich im Ordner `source/`

# Überblick


## Core

Die Entitätsklassen sind bereits angelegt. Auch die Annotationen zur Definition (inkl. der Migrationen) der Datenbank sind bereits implementiert.

![class-diagram](images/00_classdiagram.png)

## Import / Persistence

Die einzelnen Zeilen der `csv`-Datei werden verarbeitet sodass am Schluss eine Collection von `Measurements` mit ihren `Sensoren` zurückgegeben wird.

![import-console](images/01_import_console.png)

Die Migration wurde bereits angelegt und muss nur verwendet werden, wenn an den Entitätsklassen Änderungen vorgenommen werden:

* Persistence als StartupProject
* `Add-Migration InitialMigration`
* `UpdateDatabase` mit Kontrolle, ob DB angelegt wurde
* Die Daten über die ImportConsole importieren.

Die `ConnectionStrings` wurden in den relevanten Projekten schon in der `appSettings.json` festgelegt.

## ASP.NET Core Razor Pages (SmartSchool.Web)

Die Web App besteht aus folgenden Seiten.

### Page "Index"

![Overview](images/02_overview.png)

**Anforderungen:**

* Anzeige der Sensoren
    * Sortierung nach Name und Location
* Dritte Spalte enthält die Anzahl der Messungen für den Sensor
    * Nicht alle Messwerte aus der Datenbank laden!
* Der Name führt als Link zu einer Übersicht der Messwerte für den Sensor
* Der Link "Edit Sensor" verweist auf eine Bearbeitungsseite für den Sensor


### Page "Members/Edit"

![Edit](images/03_edit.png)

**Anforderungen:**

* Funktionalität
   * Name, Unit und Location können geändert werden
* Validierungen
   * Siehe nächste Überschrift
   * Es sind entsprechende Fehlermeldungen beim fehlerhaften Feld auszugeben
* "Save" führt zur Überblicksseite des Sensors mit den letzten 20 Messungen
* "Back to List" führt wieder auf die Hauptseite

Nach dem Anlegen kontrollieren Sie das Ergebnis über die Hauptseite.

#### Validierung

1. Name (Pflichtfeld)
1. Location (Pflichtfeld)

![Validierung1](images/04_validierung_1.png)


#### Zusätzliche Validierung (Spezialistenaufgabe)

1. Es dürfen keine zwei Sensoren mit dem gleichen Namen und der gleichen Location existieren!

![Validierung2](images/05_validierung_2.png)

### Page "Sensors/List?id=7"

Diese Seite erreicht man über den Namenslink (1. Spalte) der Hauptseite.

Neben den Stammdaten des Sensors sind die letzten 20 Messwerte inklusive Zeit, Wert und Einheit auszugeben.

![Details](images/06_details.png)

## ASP.NET WebApi (SmartSchool.Web)

Es sind bereits folgende API-Controller implementiert.

### Route `/api/measurements/id`

* Die Struktur der Daten entnehmen Sie dem Musterausdruck:

   ![api-measurements](images/07_api-measurements-id.png)

* Sollte die id nicht existieren, ist NotFound zurückzugeben

   ![api-measurements](images/08_NotFound.png)


### Route `/api/measurements/measurementat`

* Liefert für den Sensor den zu der Zeit gültigen Wert
* SensorId 5: `20.10.2018 13:00`

    ![MeasurementsAt](images/09_api_measurements-at.png)

* SensorId 5: `20.10.2018 12:59:16`

    ![MeasurementsAt](images/10_api_measurements-at2.png)

* SensorId 5: `20.10.2018 12:59:17`

    ![MeasurementsAt](images/11_api_measurements-at3.png)

* SensorId 5: `20.10.2017 13:00`

    ![MeasurementsAt](images/12_api_measurements-at4.png)


### Post-Route `/api/measurements`

* Erzeugt für den Sensor einen neuen Messwert

    ![Post-1](images/13_post_1.png)

    ![Post-2](images/14_post_2.png)

* Zeit darf nicht in der Zukunft liegen

    ![Post-3](images/15_post_3.png)

* Ungültige SensorId liefert `NotFound`


### Route `/api/sensors`

* Liefert die Sensornamen alphabetisch sortiert

    ![api/sensors](images/16_sensors_1.png)

### Route `/api/sensors/id`

* Liefert die Sensordaten (entsprechend Muster)

    ![api/sensors](images/17_sensors_2.png)

* Gibt es die id nicht, wird `NotFound` geliefert

    ![api/sensors](images/18_sensors_3.png)

### Route `/api/sensors/countofmeasurements`

* Liefert die Sensordaten (entsprechend Muster) und die Anzahl der Messungen je Sensor
* Sortiert nach Sensorname und Location
* Es sind möglichst wenige Daten von der Datenbank zu laden
* Die Dauer der Abfrage ist zu optimieren

    ![api/sensors](images/19_countofmeasurements_1.png)

### Route `/api/sensors/averagevalues`

* Liefert die Sensordaten (entsprechend Muster) und den Durchschnitt der Messwerte je Sensor
* Sortiert nach Sensorname und Location
* Es sind möglichst wenige Daten von der Datenbank zu laden
* Die Dauer der Abfrage ist zu optimieren

    ![api/sensors](images/20_avgofmeasurements_1.png)


# Vorbereitung

Microsoft bietet neben [GitHub](https://github.com), welches hauptsächlich zum Verwalten von Open Source Projekten verwendet wird auch [Azure DevOps](https://dev.azure.com) an.

## Aufgaben

1. Melden Sie sich bei [Azure DevOps](https://dev.azure.com/) mit ihrem \<IhrName\>@htblaleonding.onmicrosoft.com Benutzer an.
2. Erstellen Sie ein neues Projekt:

   ![Neues Projekt](images/devops/00_create-project.png)

   * Name: `SmartSchool-Azure`
   * Visiblity: `Public`

   ![Neues Projekt - Details](images/devops/01_create-project.png)

3. Klonen Sie das (leere) Azure DevOps Projekt in Visual Studio

   ![Projekt klonen](images/devops/02_clone-code.png)

   * Wählen Sie in Visual Studio ihren \<IhrName\>@htblaleonding.onmicrosoft.com Benutzer aus.
   * Wählen Sie den lokalen Pfad aus, wohin das Projekt geklont werden soll (z.B.: `c:\HTL\SmartSchool-Azure`).

4. Übernehmen Sie die bereits fertige Implementierung `source/` in das geklonte lokale Projektverzeichnis (z.B.: `c:\HTL\SmartSchool-Azure`).
5. Öffnen Sie die Solution `SmartSchool.sln` in Visual Studio.
6. In Visual Studio sollten Sie nun folgende Darstellung haben:

   ![Solution Explorer](images/preparation/00_solution-explorer.png)

7. Vorbereitung der lokalen Datenbank
   1. Definieren Sie das Projekt `SmartSchool.ImportConsole` als `Startup Project`
   2. Starten Sie das Projekt `SmartSchool.ImportConsole` per F5
   3. Folgende Ausgabe sollte ersichtlich sein:

      ![Import Console](images/preparation/01_import-console.png)


8. Starten der Web App bzw. der Web Api
   1. Definieren Sie das Projekt `SmartSchool.Web` als `Startup Project`
   2. Starten Sie das Projekt `SmartSchool.Web` per F5
   3. Folgende Ausgabe sollte ersichtlich sein:

      ![Web App](images/preparation/02_webapp_overview.png)

   4. Beim Klick auf `API` in der Navigationsleiste oben solle folgende Ausgabe ersichtlich sein:

      ![Web Api](images/preparation/03_webapp_swagger.png)

9. Commiten und pushen sie die Änderungen in das Azure DevOps Projekt

# Continuous Integration

**Die  Web App bzw. die Web Api (Swagger) sollten wie in den Screenshots dargestellt aufgerufen werden. Erst danach können die folgende Punkte des Tutorials durchgeführt werden!**

Unter [Continuous Integration](https://en.wikipedia.org/wiki/Continuous_integration) versteht man die fortlaufende und permanente Zusammenführung aller Komponenten zur kompletten Anwendung.

Dh. dass bei jeglicher Code-Änderung (Push in das Git-Repository) ein Compile- und Testzyklus durchlaufen wird und am Ende als Resultat die ausführbare Anwendung als Artefakt hinterlegt wird. Auf diesem hinterlegten Artefakt setzt dann im Anschluss [Continuous Deliviery](https://en.wikipedia.org/wiki/Continuous_delivery) auf - siehe dazu dann das nächste Kapitel.

## Aufgaben

1. Melden Sie sich bei [Azure DevOps](https://dev.azure.com/) mit ihrem \<IhrName\>@htblaleonding.onmicrosoft.com Benutzer an.
2. Navigieren Sie im Projekt `SmartSchool-Azure` zum Abschnitt Pipelines:

   ![Pipelines](images/ci/00_pipelines.png)

3. Erstellen Sie eine neue Pipeline:

   ![Neue Pipeline](images/ci/01_create-pipeline.png)

4. Wählen Sie die Quelle für ihren Source Code aus - in diesem Fall `Azure Repos Git`:

   ![Pipelines - Source Provider](images/ci/02_pipline-git.png)

5. Wählen Sie das Repository aus - in diesem Fall `SmartSchool-Azure`:

   ![Pipelines - Repo](images/ci/03_pipline-git-repo.png)

6. Wählen Sie die Vorlage für ihre Pipeline aus - in diesem Fall `Starter pipeline`:

   ![Pipelines - Repo](images/ci/04_pipline-template.png)

7. Bennen Sie die Pipeline 'build-all.yml' und ersetzen Sie die Vorlage mit folgendem Code (Source zum Kopieren finden Sie unten):

   ![Pipelines - Repo](images/ci/05_pipeline-overview.png)

   1. Der `Trigger` legt fest wann die Pipeline ausgeführt werden soll [Details](https://docs.microsoft.com/en-us/azure/devops/pipelines/repos/azure-repos-git?view=azure-devops&tabs=yaml#ci-triggers). In diesem Fall wird die Pipeline bei jedem Push auf den `master` Branch ausgeführt.

   2. Der `Pool` legt die Umgebung fest in welcher die Pipeline ausgeführt werden soll [Details](https://docs.microsoft.com/en-us/azure/devops/pipelines/agents/hosted?view=azure-devops&tabs=yaml). In diesem Fall ist dies ein Windows Server 2019 (`windows-latest`).

   3. Im Abschnitt `Variables` können Variablen definiert werden, welche im Weiteren dann verwendet werden können.

   4. Dieser Step stellt sicher, dass das Werkzeug `NuGet` zur Verfügung steht.

   5. In diesem Schritt werden die `NuGet`-Pakete, welche in den einzelnen Projekten definiert sind geladen.

   6. Nun wird die gesamte Solution kompiliert.

   7. In diesem Schritt werden die Unit Tests durchgeführt.

   8. Hier wird die Datei `.runsettings` in das `Staging`-Verezichnis kopiert. Dorthin werden alle Dateien kopiert, welche im letzten Schritt (9) dann als `Build Artefakt` veröffentlicht werden sollen.

   9. Abschließend werden alle Dateien aus dem `Staging`-Verzeichnis als `Build Artefakt` veröffentlicht. Diese Artefakte dienen als Basis für die Auslieferung in der `Release Pipeline`.

   **Source:**

   ```yml
   trigger:
    - master

    pool:
      vmImage: 'windows-latest'

    variables:
      solution: '**/*.sln'
      buildPlatform: 'Any CPU'
      buildConfiguration: 'Release'

    steps:
    - task: NuGetToolInstaller@1
      displayName: NuGet (Installation)

    - task: NuGetCommand@2
      displayName: NuGet (Restore)
      inputs:
        restoreSolution: '$(solution)'

    - task: VSBuild@1
      displayName: Build Solution
      inputs:
        solution: '$(solution)'
        msbuildArgs: '/p:DeployOnBuild=true /p:WebPublishMethod=Package /p:PackageAsSingleFile=true /p:SkipInvalidConfigurations=true /p:DesktopBuildPackageLocation="$(build.artifactStagingDirectory)\WebApp\WebApp.zip" /p:DeployIisAppPath="Default Web Site" /p:GenerateDocumentation=true'
        platform: '$(buildPlatform)'
        configuration: '$(buildConfiguration)'

    - task: DotNetCoreCLI@2
      displayName: 'Run Unit Tests'
      inputs:
        command: 'test'
        arguments: '--no-build --configuration $(buildConfiguration)'
        publishTestResults: true
        projects: '**/SmartSchool.Test.csproj'

    - task: CopyFiles@2
      displayName: Copy Test Configuration
      inputs:
        SourceFolder: '$(Build.SourcesDirectory)'
        Contents: '**/*.runsettings'
        TargetFolder: '$(build.artifactStagingDirectory)'

    - task: PublishBuildArtifacts@1
      displayName: Publish SmartSchool.Web as Build Artefact
      inputs:
        artifactName: drop
   ```

7. Führen Sie die Pipline rechts oben per `Run` aus.

8. Während der Ausführung der Pipline können Sie sich die Logs der einzelnen Scrhitte anzeigen lassen:

   ![Pipelines - Run](images/ci/06_pipeline-run.png)

9. Nachdem die Pipeline ausgeführt wurde sehen Sie folgendes Ergebnis:

   ![Pipelines - Run](images/ci/07_pipeline-result.png)

   Sie sehen einerseits, dass alle Schritte der Build Pipline erfolgreich durchgeführt wurden, sowei der kompilierte Code aus dem `master`-Branch stammt, genau ein `Build Artefakt` erzeugt/veröffentlicht wurde und dass alle Tests (100%) erfolgreich durchgeführt wurden.


# Continuous Delivery

**Die Build-Pipeline muss erfolgreich durchlaufen werden. Erst danach können die folgende Punkte des Tutorials durchgeführt werden!**

Auf Basis der kompilierten Artefakte (Web App mit Razor Pages und die Web Api) kann nun eine Auslieferung erfolgen. Sollte bei jedem `Build` eine Auslieferung erfolgen so spricht man von [Continuous Deliviery](https://en.wikipedia.org/wiki/Continuous_delivery).

## Aufgaben

1. Melden Sie sich im [Azure Portal](https://portal.azure.com) mit ihrem \<IhrName\>@htblaleonding.onmicrosoft.com Benutzer an.

2. Dort sollten Sie eine  `Resource Group` mit ihrem Namen finden, in welcher Sie nun die in `Azure` gehostete Web-Site konfigurieren werden.

     *Hinweis:* Eine `Resource Group` stellt einen Behälter dar, in welchem verschiedenste Azure Services zusammengefasst werden können.

     ![Resource Group - Overview](images/cd/00_resource-group-overview.png)

3. Wählen Sie ihre `Resource Group` aus und klicken Sie auf `Add`:

     ![Add Resource](images/cd/01_add-resource.png)

4. Suchen Sie nach dem Service `Web App`

5. Legen Sie folgende Konfiguration für ihre neue `Web App` fest:

     ![Add Resource](images/cd/02_add-webapp-prod.png)

     **Bei Punkt (1) ist wichtig, dass Sie anstatt `smartschool-jf` hinten ihr Kürzel anstatt `-jf` verwenden!**

5. Analog zur `Web App` für die Produktivumgebung legen Sie auch eine `Web App` für die Staging Umgebung an:

     ![Add Resource](images/cd/03_add-webapp-staging.png)

     **Bei Punkt (1) ist wichtig, dass Sie anstatt `smartschool-jf-staging` ihr Kürzel anstatt `-jf-` verwenden!**

6. Konfiguration der erstellten `Web App` Services (`smartschool-jf-staging` und `smartschool-jf`)

   1. `Connection String` für die Verbindung zur Datenbank in beiden `Web App` Services konfigurieren:

      ![Add ConnectionString](images/cd/04_add-connection-string.png)

        | Bezeichnung | Wert                                                                                                                                                                                                                                                      |
        |-------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
        | Name        | `DefaultConnection`                                                                                                                                                                                                                                       |
        | Value       | `Server=tcp:htl-samples.database.windows.net,1433;Initial Catalog=SmartSchoolAzure;Persist Security Info=False;User ID=appuser;Password=htlleonding2020#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;` |
        | Type        | `SQLAzure`                                                                                                                                                                                                                                                |


        ![Set ConnectionString](images/cd/05_set-connection-string.png)

   2. `ASPNETCORE_ENVIRONMENT` Application Setting in der **Staging** `Web App` konfigurieren - somit werden detailierte Fehlermeldungen ausgegeben:

        ![Set ASPNETCORE_ENVIRONMENT](images/cd/06_set-aspnetcore_environment.png)


        | Bezeichnung | Wert                                                                                                                                                                                                                                                      |
        |-------------|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
        | Name        | `ASPNETCORE_ENVIRONMENT`                                                                                                                                                                                                                                       |
        | Value       | `Development` |


7. Melden Sie sich bei [Azure DevOps](https://dev.azure.com/) mit ihrem \<IhrName\>@htblaleonding.onmicrosoft.com Benutzer an.
8. Navigieren Sie im Projekt `SmartSchool-Azure` zum Abschnitt Pipelines:

     ![Pipelines](images/cd/07_pipelines.png)

9. Erstellen Sie eine neue Pipeline:

     ![Neue Pipeline](images/cd/08_create-release-pipeline.png)

10. Wählen Sie als Vorlage `Empty job` aus:

     ![Template wählen](images/cd/09_choose-template.png)

11. Benennen Sie ihre `Release Pipeline` auf `Deployment` um:

     ![Pipeline umbenennen](images/cd/10_rename-pipeline.png)

12. Konfigurieren Sie das `Build Artefact` welches als Basis für ihr Deployment dienen soll:

     ![Artefakt wählen](images/cd/11_configure-build-artefact.png)

13. `Trigger` für das Deployment konfigurieren:

     ![Trigger konfigurieren](images/cd/12_configure-trigger.png)

     Mit diesem `Trigger` wird bei jedem erfolgreichem Build die `Release Pipeline` für die Auslieferung angestoßen.

14. Konfigurieren Sie die Auslieferung in die `Staging` Umgebung:

     ![Trigger konfigurieren](images/cd/12_configure-trigger.png)

15. Legen Sie fest in welche `Web App` die gebauten Artefakte ausgeliefert werden sollen:

     ![Auslieferung konfigurieren](images/cd/gifs/00_add-deployment-task.gif)

16. Dupliziern Sie die `Staging` Stage und nennen Sie sie `Production`:

     ![Stage klonen](images/cd/gifs/01_clone-stage.gif)

      Mit dieser Stage wird nach dem Deployment in die `Staging` Umgebung auch ein Deployment in die Produktivumgebung durchgeführt.

16. Festlegen eines `Pre deployment approvals` für die `Production` Stage:

     ![Pre Deployment Approval](images/cd/gifs/02_production-approval.gif)

      Dies hat den Grund, dass eine Persion das Deployment auf die Produktivumgebung freigeben muss.

16. Manuelles Deployment durchführen:

     ![Create Release](images/cd/gifs/03_create-release.gif)

     Die `Release Pipeline` ist liefert standardmäßig nun jedes mal in die `Staging`- bzw. `Production`-Umgebung aus, sobald ein neuer Build vorhanden ist.
     Zum Testen kann auch manuell ein `Release` erstellt werden. Das Erstellen eines `Releases` ist gleichbedeutend mit dem Anfordern einer Auslieferung.


16. `Staging` Umgebung auf korrekte Funktionalität prüfen:

     ![Check Staging](images/cd/gifs/04_check-staging.gif)


17. Nach dem `Staging`-Deployment wird nun in die `Production`-Umgebung ausliefern:

     ![Production Deployment](images/cd/gifs/05_deploy-to-production.gif)

18. `Staging` Umgebung auf korrekte Funktionalität prüfen:

     ![Check Production](images/cd/gifs/06_check-production.gif)

19. Ansicht des `Release` nach dem Deployment:

     ![Release](images/cd/13_final-result.png)

     Somit ist das `Release-1` in beiden Umgebungen (`Staging` und `Production`) sauber ausgeliefert.



