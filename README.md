# Web application communication interface with Thermal Printers and Scanner using Signal-R

Objective:
The proposed asset demonstrates the Angular Web application communication with Hand-held scanner and Thermal printers which are connected to user machine through Signal-R communication.

![image](https://github.com/chouguler/POC-Web-ThermalPrinting-SignalR/assets/45493809/67ac0223-caf4-4119-b170-fd8c16b8aca2)

1.	The Angular Web application communicate with “Self-Hosted Signal-R Hub” developed in Microsoft ASP.NET hosted on user workstation locally in the form of Windows Service. Users install this windows service using the setup file (Setup.msi) on their local machine. We refer/call it as Thin client.

2.	Thin Client implements Signal-R hub server which provides the dynamic web socket methods to receive and send data to and from between front end web application and Window Service in real-time. 

3.	Thin Client receives the label data (EPL commands) from front end web application and push it to thermal printer to print label data using USB cable & Serial Cable. In this case, the front-end web application invokes the hub methods to send the print data to hub. Hub methods internally uses Microsoft .NET library (System.IO namespace) to connect the Serial/USB ports of thermal printer.
4.	Also, Thin Client communicates with hand-held Serial scanner to receive scanned data from Serial scanner connected through Serial cable and it broadcast the scanned data to front end web application. In this case, the hub invokes the client-side methods to broadcast the scanned data to web application.
