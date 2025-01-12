# Weather Thingy

## Overview

This repositroy includes modules for the Weather Thingy LoRa Weather App Project.
It's purpouse is to create a windows application that reads weather sensor data from The Things Network and displays it in a pleasant and user friendly UI.

### Features

* Windows app user interface with real time updates and scalable graphs
* Ready to host webpage of Weather Thingy App
* SQL server API to pull sensor data from a database
* MQTT parser to pull data from MQTT and display it in console
* Arduino code for the user to connect their own sensors to The Things Network

## Installation

### General Prerequisites

* Visual Studio, min. 2022 with .NET Multi-Platform App UI

### MQTT_PARSER

#### Process

1. Clone the repositroy
2. Open the mqtt_parser/mqtt_parser.sln solution in Visual Studio
3. Build the program in *debug mode* for *Windows*

* Keep in mind that the application is already running 24/7 on a *raspberry pi 4* device, running a second version will not affect the database but will show SQL erros in the log files for inserting duplicated PKs.

#### Usage

* The Application does not require any input from the user.
* The Application will open a console page with minimal messages, confirming connection or alerting for diconnection.
* a daily Log file will be generated under the directory *mqtt_parser\bin\Debug\net8.0*, with file name *log(YYYY-MM-DD).txt*

### Windows APP

#### Process

At the time that the project is still supported the setup for the windows app is very simple.

1. Clone the repositroy
2. Open the App/WeatherThingy/WeatherThingy.sln solution in Visual Studio
3. Build the program in *debug mode* for *Windows*

#### Usage

* Home page of the app displays data received most recently from sensors in a given city
* Navigate between pages using buttons at the top of the app window
* Detail page shows scalable graphs with user selected data
* Choose what data to show using the buttons and data selectors above the chart space

## Authors

    Dung Phan                      546821
    Karolina Gogolin               543772
    Mikołaj Materka                548471
    Peter Joseph Mikhail Samaan    545727

## Tech stack

### Languages

* C#
* C++
* HTML5
* TypeScript
* CSS

### Libraries and frameworks

* LiveCharts
* CommunityToolkit.Mvvm
* ASP.NET
* Swagger
* Newsoft.Json
* MQTTnet
* Microsoft.Data.SqlClient
* Microsoft MAUI
* ChartJS
* TomTom Map API
* Arduino_MKRENV
* MKRWAN
