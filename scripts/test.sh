#!/bin/bash
cd Tests
dotnet clean
dotnet build
# dotnet test
dotnet test --logger "console;verbosity=detailed"