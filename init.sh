#!/bin/bash
set -e

# Verificar se as variáveis obrigatórias estão definidas
: "${USER_BRIS:?Variable USER_BRIS is required}"
: "${PASSWORD_BRIS:?Variable PASSWORD_BRIS is required}"
: "${USER_IMPERIUM:?Variable USER_IMPERIUM is required}"
: "${PASSWORD_IMPERIUM:?Variable PASSWORD_IMPERIUM is required}"
echo "Variáveis de ambiente:"
echo "USER_BRIS: $USER_BRIS"
echo "PASSWORD_BRIS: $PASSWORD_BRIS"
echo "USER_IMPERIUM: $USER_IMPERIUM"
echo "PASSWORD_IMPERIUM: $PASSWORD_IMPERIUM"

echo "Criando usuários e bancos de dados:"

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    CREATE USER ${USER_BRIS} WITH PASSWORD '${PASSWORD_BRIS}';
    CREATE USER ${USER_IMPERIUM} WITH PASSWORD '${PASSWORD_IMPERIUM}';

    CREATE DATABASE bris_database;
    CREATE DATABASE imperium_database;

    GRANT ALL PRIVILEGES ON DATABASE bris_database TO ${POSTGRES_USER};
    GRANT CONNECT ON DATABASE bris_database TO ${USER_BRIS};

    GRANT ALL PRIVILEGES ON DATABASE imperium_database TO ${POSTGRES_USER};
    GRANT CONNECT ON DATABASE imperium_database TO ${USER_IMPERIUM};
EOSQL