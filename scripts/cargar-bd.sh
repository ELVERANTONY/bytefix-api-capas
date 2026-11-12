#!/usr/bin/env bash
# Carga el script SQL en el contenedor de SQL Server.
set -euo pipefail

RAIZ="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$RAIZ"

# Lee la clave desde .env si existe (archivo local, no versionado).
if [[ -f .env ]]; then
  set -a
  # shellcheck disable=SC1091
  . ./.env
  set +a
fi

CONTENEDOR="${CONTENEDOR:-bytefix-sql}"
SA_PASSWORD="${MSSQL_SA_PASSWORD:?Define MSSQL_SA_PASSWORD en .env (copia .env.example)}"

docker cp db/ByteFixDB.sql "$CONTENEDOR:/tmp/ByteFixDB.sql"
docker exec "$CONTENEDOR" /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "$SA_PASSWORD" -C -i /tmp/ByteFixDB.sql
