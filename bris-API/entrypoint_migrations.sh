#!/bin/sh
set -e

# Verifica se o comando foi passado
if [ -n "$EF_COMMAND" ]; then
    echo "Executando: dotnet ef database $EF_COMMAND"
    dotnet ef database $EF_COMMAND
else
    echo "Nenhum comando especificado. Container será encerrado."
fi