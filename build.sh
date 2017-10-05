#!/usr/bin/env bash

sudo docker-compose -f docker-compose.yml -f docker-compose.override.yml up
sudo dotnet test ./Jet.Service.Manifest.UnitTest.Client/Jet.Service.Manifest.UnitTest.Client.csproj
