using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 77061;
            const string apiKey = "a698b5a1-a882-4014-a0a7-5a325480776f";
            const string apiSecret = "12d49412-cff2-4297-95ac-20ef759dffb4";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("Parcial Arq Sofware", "Softwware Architecture");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem monitoringSystem = model.AddSoftwareSystem("UBER", "mantener un servicio optimizado que haga coincidir las necesidades de losclientes con los proveedores de servicio de transporte");
            SoftwareSystem riders = model.AddSoftwareSystem("Riders", "Servicio de gestión de demanda de pasajeros (Riders)");
            SoftwareSystem cabs = model.AddSoftwareSystem("Cabs", "Servicio de gestión de oferta de conductores");
            SoftwareSystem disco = model.AddSoftwareSystem("Disco", "Servicio de reduccion de consumo");
            SoftwareSystem s3 = model.AddSoftwareSystem("S3", "Servicio de calculo de distancia");
            SoftwareSystem almacenamiento = model.AddSoftwareSystem("Almacenamiento", "Almacenamiento de informacion");
            SoftwareSystem externo = model.AddSoftwareSystem("Servicios Externos", "Servicio de pagos ");
            



            Person cliente = model.AddPerson("Cliente", "Cliente peruano.");
            Person conductor = model.AddPerson("Conductor", "Conductor peruano.");

            
            cliente.Uses(monitoringSystem, "Realizan pedidos de taxis");
            conductor.Uses(monitoringSystem, "Realizan viajes para clientes");

            monitoringSystem.Uses(cabs, "Este servicio tiene varias instancias de un microservicio que registra la geolocalización de los conductores cada4 segundos para hacer coincidir la ubicación del pasajero con los conductores más cercanos que tengan alcanceal lugar de destino. La información se deja en un broker Apache Kafka que va registrando la geolocalización delconductor en tiempo real. ");
            monitoringSystem.Uses(riders, "Este servicio está compuesto por varias instancias de un microservicio que deja la información en un brokeApache Kafka que registra la geolocalización de un pasajero que solicita un servicio Uber (lugar de recojo) y laeolocalización del lugar de destino. ");
            monitoringSystem.Uses(disco, "Este microservicio se encarga de minimizar tiempo de espera, reducir la conducción extra y reducir consumopara el conductor. La función principal es identificar al cliente con la lista de conductores más cercanos y hacerla oferta de servicios para que sea tomada por el conductor. ");
            monitoringSystem.Uses(s3, "calcula ladistancia del pasajero a los conductores más cercanos");
            monitoringSystem.Uses(almacenamiento, "bases de datos deben tener capacidad para escalar horizontalmente, de esta manera Uber puede agregarmás servidores.");
            monitoringSystem.Uses(externo, "Se utiliza servicios de pago con Paypal, Mastercard, Visa y los servicios de Google Maps para haceractualizaciones a la red S3 de Uber");

            
            SystemContextView contextView = viewSet.CreateSystemContextView(monitoringSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            cliente.AddTags("Cliente");
            conductor.AddTags("Conductor");

            monitoringSystem.AddTags("SistemaMonitoreo");
            riders.AddTags("Riders");
            cabs.AddTags("Cabs");
            disco.AddTags("Disco");
            almacenamiento.AddTags("Almacenamiento");
            s3.AddTags("S3");
            externo.AddTags("ServiciosExternos");



            

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Cliente") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Conductor") { Background = "#B22222", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaMonitoreo") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Riders") { Background = "#CD5C5C", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Cabs") { Background = "#4682B4", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Disco") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("S3") { Background = "#FFA07A", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("Almacenamiento") { Background = "#32CD32", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("ServiciosExternos") { Background = "#FFB6C1", Color = "#ffffff", Shape = Shape.RoundedBox });




            // 2. Diagrama de Contenedores
            Container mobileApplication = monitoringSystem.AddContainer("APP Pasajeros", "app para los pasajeros", "Java y Scala");
            Container webApplication = monitoringSystem.AddContainer("App Conductores", "app para los conductores.", "Java y Scala");
            Container landingPage = monitoringSystem.AddContainer("Centro de activación presencial", "", "Python");
            Container apiRest = monitoringSystem.AddContainer("API Rest", "API Rest", "NodeJS (NestJS) port 8080");
            Container flightPlanningContext = monitoringSystem.AddContainer("Bussines Context", "Bounded Context del Microservicio de Planificación de viajes", "NodeJS (NestJS)");
          //  Container airportContext = monitoringSystem.AddContainer("Airport Context", "Bounded Context del Microservicio de información de Aeropuertos", "NodeJS (NestJS)");
           // Container aircraftInventoryContext = monitoringSystem.AddContainer("Aircraft Inventory Context", "Bounded Context del Microservicio de Inventario de Aviones", "NodeJS (NestJS)");
            Container vaccinesInventoryContext = monitoringSystem.AddContainer("Security Context", "Bounded Context del Microservicio de Seguridad de datos", "NodeJS (NestJS)");
            Container monitoringContext = monitoringSystem.AddContainer("Payments Context", "Bounded Context del Microservicio de Pagos y tranferencias ", "NodeJS (NestJS)");
            Container database = monitoringSystem.AddContainer("Database Payment ", "", "Oracle");
            Container databasePayment = monitoringSystem.AddContainer("Database Context", "", "Oracle");
            Container databaseSecurity = monitoringSystem.AddContainer("Database Security", "", "Oracle");

            
            cliente.Uses(mobileApplication, "Consulta");
            cliente.Uses(webApplication, "Consulta");
            cliente.Uses(landingPage, "Consulta");
            
            conductor.Uses(mobileApplication, "Consulta");
            conductor.Uses(webApplication, "Consulta");
            conductor.Uses(landingPage, "Consulta");
            

            mobileApplication.Uses(apiRest, "API Request", "JSON/HTTPS");
            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            apiRest.Uses(flightPlanningContext, "", "");
   
            apiRest.Uses(vaccinesInventoryContext, "", "");
            apiRest.Uses(monitoringContext, "", "");
            
            flightPlanningContext.Uses(databasePayment, "", "JDBC");
    
            vaccinesInventoryContext.Uses(databaseSecurity, "", "JDBC");
            monitoringContext.Uses(database, "", "JDBC");
            
            monitoringContext.Uses(riders, "API Request", "JSON/HTTPS");
            monitoringContext.Uses(cabs, "API Request", "JSON/HTTPS");
            monitoringContext.Uses(disco, "API Request", "JSON/HTTPS");
            monitoringContext.Uses(almacenamiento, "API Request", "JSON/HTTPS");
            monitoringContext.Uses(s3, "API Request", "JSON/HTTPS");
            monitoringContext.Uses(externo, "API Request", "JSON/HTTPS");


            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");
            databaseSecurity.AddTags("Database");
            databasePayment.AddTags("Database");


            flightPlanningContext.AddTags("FlightPlanningContext");
      
            vaccinesInventoryContext.AddTags("VaccinesInventoryContext");
            monitoringContext.AddTags("MonitoringContext");

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#00FFFF", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("FlightPlanningContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AirportContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AircraftInventoryContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VaccinesInventoryContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });
            ContainerView containerView = viewSet.CreateContainerView(monitoringSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3. Diagrama de Componentes
            Component domainLayer = monitoringContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component monitoringController = monitoringContext.AddComponent("Riders Controller", "REST API endpoints de monitoreo.", "NodeJS (NestJS) REST Controller");
            Component monitoringApplicationService = monitoringContext.AddComponent("Riders Service", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "NestJS Component");
            Component flightRepository = monitoringContext.AddComponent("Transaction Repository", "Información del viaje", "NestJS Component");
            Component vaccineLoteRepository = monitoringContext.AddComponent("Procces Repository", "Información del estado del viaje", "NestJS Component");
            Component locationRepository = monitoringContext.AddComponent("Location Repository", "Ubicación del viaje", "NestJS Component");
            Component aircraftSystemFacade = monitoringContext.AddComponent("Broker Repository", "", "NestJS Component");

            apiRest.Uses(monitoringController, "", "JSON/HTTPS");
            monitoringController.Uses(monitoringApplicationService, "Invoca métodos de monitoreo");
            monitoringController.Uses(aircraftSystemFacade, "Usa");
            monitoringApplicationService.Uses(domainLayer, "Usa", "");
            monitoringApplicationService.Uses(flightRepository, "", "JDBC");
            monitoringApplicationService.Uses(vaccineLoteRepository, "", "JDBC");
            monitoringApplicationService.Uses(locationRepository, "", "JDBC");
            flightRepository.Uses(databasePayment, "", "JDBC");
            vaccineLoteRepository.Uses(databaseSecurity, "", "JDBC");
            locationRepository.Uses(database, "", "JDBC");
            locationRepository.Uses(riders, "", "JSON/HTTPS");
            aircraftSystemFacade.Uses(cabs, "JSON/HTTPS");
            aircraftSystemFacade.Uses(disco, "JSON/HTTPS");
            aircraftSystemFacade.Uses(almacenamiento, "JSON/HTTPS");
            aircraftSystemFacade.Uses(s3, "JSON/HTTPS");
            aircraftSystemFacade.Uses(externo, "JSON/HTTPS");

            // Tags
            domainLayer.AddTags("DomainLayer");
            monitoringController.AddTags("MonitoringController");
            monitoringApplicationService.AddTags("MonitoringApplicationService");
            flightRepository.AddTags("FlightRepository");
            vaccineLoteRepository.AddTags("VaccineLoteRepository");
            locationRepository.AddTags("LocationRepository");
            aircraftSystemFacade.AddTags("AircraftSystemFacade");
            
            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringApplicationService") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("VaccineLoteRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("LocationRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("AircraftSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(monitoringContext, "Components", "Component Diagram");
            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(mobileApplication);
            componentView.Add(webApplication);
            componentView.Add(apiRest);
            componentView.Add(database);
            componentView.Add(databaseSecurity);
            componentView.Add(databasePayment);
            componentView.Add(cabs);
            componentView.Add(disco);
            componentView.Add(almacenamiento);
            componentView.Add(s3);
            componentView.Add(externo);
            componentView.Add(riders);
            componentView.AddAllComponents();
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}