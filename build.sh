#!/usr/bin/env bash

docker-compose -f docker-compose.yml -f docker-compose.override.yml up
dotnet test ./Jet.Service.Manifest.UnitTest.Client/Jet.Service.Manifest.UnitTest.Client.csproj