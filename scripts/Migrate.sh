#!/bin/bash

name=${1:-new_migration}

cd FIN
dotnet ef migrations add $name