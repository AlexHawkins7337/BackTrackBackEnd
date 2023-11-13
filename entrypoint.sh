#!/bin/bash
set -e
run_cmd="dotnet BackTrackBackEnd.dll"

>&2 echo "Wait until confimation that DB is up"

cd /src/BackTrackBackEnd

dotnet-ef database update

cd /app/publish

>&2 echo "DB is up - executing command"
exec $run_cmd
