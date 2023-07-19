# WholesalerProvisioningSystem
The Wholesaler Provisioning System is designed to facilitate the provisioning of fiber lines associated with households by external operators. The system allows for reservation, activation/deactivation, and termination of fiber lines based on defined modalities.
The telecommunications operators, in order to expand their fixed network coverage, establish agreements with other operators to share information about households where they have coverage and can provide services. The system created in the previous practical work is extended to enable external operators to access and perform the provisioning actions.

The reservation process is synchronous, where the server responds to a customer's request for reservation with information on whether the reservation was successful or not, along with a unique reservation number associated with the household and modality. The activation, deactivation, and termination processes are asynchronous. When an external operator wishes to activate, deactivate, or terminate a fiber line, they send a request to the server, which is executed asynchronously. The client receives notifications through a message subscription system, to which they need to subscribe beforehand.

The server-side procedures  be implemented as remote procedures in a client/server pattern. The following remote procedures need to be implemented with their specifications, along with handling of asynchronous messages:

    RESERVATION:
        The client sends the modality and the household for which they want to make the reservation.
        The server responds indicating whether the reservation was successful or not, providing a unique administrative reservation number for further actions.

    ACTIVATION:
        The client sends the administrative reservation number for which they want to activate the service.
        The server verifies the client and checks if the service can be activated (must be in a RESERVED or DEACTIVATED state).
        The server responds synchronously, indicating if activation is possible and providing an estimated time for completion.
        The client subscribes to the "EVENTS" topic created by the server to receive activation notifications.
        The server publishes the corresponding activation message to the "EVENTS" topic. Sleep can be used to simulate the process.

    DEACTIVATION:
        The client sends the administrative reservation number for which they want to deactivate the service.
        The server verifies the client and checks if the service can be deactivated (must be in an ACTIVE state).
        The server responds synchronously, indicating if deactivation is possible and providing an estimated time for completion.
        The client subscribes to the "EVENTS" topic created by the server to receive deactivation notifications.
        The server publishes the corresponding deactivation message to the "EVENTS" topic. Sleep can be used to simulate the process.

    TERMINATION:
        Termination allows the resources associated with a service to be released. To terminate a service, it must have been previously deactivated.
        The client sends the administrative reservation number for which they want to terminate the service.
        The server verifies the client and checks if the service can be terminated (must be in a DEACTIVATED state). It then releases the resources.
        The server responds synchronously, indicating if termination is possible and providing an estimated time for completion.
        The client subscribes to the "EVENTS" topic created by the server to receive termination notifications.
        The server publishes the corresponding termination message to the "EVENTS" topic. Sleep can be used to simulate the process.

The server must:

    Identify and authenticate each operator performing the requests.
    Allow the administrator to manage the available coverage for each operator.
    Enable the administrator to list all RESERVED, ACTIVE, and TERMINATED services.
    Persistently store information in a database (e.g., MS SQL Express or MySQL).
    Restrict the use of a household by multiple operators. Each reservation corresponds to a single operator.

The server is programmed using .NET Core 3, gRPC, and RabbitMQ for asynchronous message handling.

The Client for the "Administrator/Operator" User:

    The client receive or request from the administrator/operator:
        An identification name for the administrator.
        A password for authentication.
    Connect to the server and list the available coverage.
    Connect to the server and list the data related to ongoing processes (reservation, activation, deactivation, termination).
    The client can be programmed using .NET Core 3 with a simple text-based interface (command-line application) or as a Windows Forms application with a graphical interface.

The Client for the "External Operator" User:

    The client receive or request from the operator:
        An identification name for the operator.
        A password for authentication.
    Connect to the server and perform the defined actions (reservation, activation, deactivation, termination of resources).
    Connect to the server and subscribe to the defined topic for the identified actions (reservation, activation, deactivation, termination of resources).
    The client can be programmed for platforms other than .NET Core, such as Android, PHP, or Python.

