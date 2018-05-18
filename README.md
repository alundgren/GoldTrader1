# GoldTrader1
Simple proof of concept gold trading application

##Server

- The server runs an orderbook in memory
- Order are received using a web api on /api/orderbook/add
- Market state and trades are reported to clients using signalr

##Client

- The client is a simple vue app that sends orders to the server and receives updates using signalr
- Different users are simulated by adding a userid parameter (http://localhost:58265/?userId=1, http://localhost:58265/?userId=2)
- The client tracks anything added to the market by any user since they connected and any trades where they were involved but has no memory