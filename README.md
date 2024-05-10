# SERVICE-ORIENTED ARCHITECTURE(SOA) SIMULATOR 

This application allows the client to register a new user into the 
service’s authentication server and then use the credentials of the user to log-in, access and 
then interact with the available services, this application also allows the service providers to 
publish or unpublish services. This application is divided into 5 key parts, the first one is the 
Authenticator Project, the second part is the Registry Project, the third part is the Service 
Provider Project , the fourth part is the Service Publishing Console Application, and the last 
part is the Client GUI Application project. 

For this application to work properly the authenticator project must be up and running first, 
and then the registry project and the service provider project must be up and running, only 
then can you run either the client GUI application or the service publishing application. If the 
GUI application or the service publishing application are run while the other projects are 
closed the application will not work properly.