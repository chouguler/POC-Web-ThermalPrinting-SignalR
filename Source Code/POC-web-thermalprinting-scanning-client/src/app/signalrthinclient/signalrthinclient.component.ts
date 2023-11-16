import { Component } from '@angular/core';
declare var $: any;

@Component({
  selector: 'app-signalrthinclient',
  templateUrl: './signalrthinclient.component.html',
  styleUrls: ['./signalrthinclient.component.css']
})
export class SignalrthinclientComponent {

  public trackingNumber: string = "";
  public labelData: string = "";
  public hubBaseUrl: string = 'http://localhost:6789/signalr';

  constructor() { }

  ngOnInit() {
    this.startHubConnection();
  }

  startHubConnection() {

    $.connection.hub.url = this.hubBaseUrl;
    var waspHubProxy = $.connection.WASPThinClientHub;

    //Server calls this client function - subscribe before start
    waspHubProxy.client.getScannerDataBySerial = function (message: string) {
      console.log("Received: " + message);
      $('#trackingNumber').val(message);
    };

    //after connection starts
    $.connection.hub.start()
      .done(function () {
        console.log('Now connected, connection ID=' + $.connection.hub.id);

        //$(function CallMe() { alert('hi')});

        //Call server function - initializeSerialScanner()       
        waspHubProxy.server.initializeSerialScanner("0001", "009600", "8", "1", "N");

        //Call server function - initializeSerialPrinter()        
        waspHubProxy.server.initializeSerialPrinter("0002", "009600", "8", "1", "N", "ZPL", "-50");

        //Call server function - initializeUSBPrinter()
        waspHubProxy.server.initializeUSBPrinter("Zebra", "ZPL", "-50");

        $('#printViaSerial').click(function () {
          //Call server function - printLabelDataBySerial()
          waspHubProxy.server.printLabelDataBySerial($('#labelData').val()).done(function (response: string) {
            $('#response').html(response);
          });
          $('#labelData').val('').focus();
        });

        $('#printViaUSB').click(function () {
          //Call server function - printLabelDataByUSB()
          waspHubProxy.server.printLabelDataByUSB($('#labelData').val()).done(function (response: string) {
            $('#response').html(response);
          });
          $('#labelData').val('').focus();
        });

      }).fail(function () { console.log('Could not Connect!'); });

    //if connection disconnected
    $.connection.hub.disconnected(function () {
      setTimeout(function () {
        $.connection.hub.start();
        console.log('Now re-connected, connection ID=' + $.connection.hub.id);
      }, 2000); // Restart connection after 2 seconds.
    });
  }
}
