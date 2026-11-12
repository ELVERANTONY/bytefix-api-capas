#!/usr/bin/env bash
# Regenera el modelo Database First (Scaffold) desde SQL Server.
# El scaffold se genera en ByteFix.Datos/Models y luego las entidades se mueven
# a ByteFix.Entidades/Entities ajustando su namespace.
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

SA_PASSWORD="${MSSQL_SA_PASSWORD:?Define MSSQL_SA_PASSWORD en .env (copia .env.example)}"
CADENA="${BYTEFIX_CONNECTION:-Server=localhost,1433;Database=ByteFixDB;User Id=sa;Password=${SA_PASSWORD};TrustServerCertificate=True;Encrypt=False}"

dotnet ef dbcontext scaffold "$CADENA" Microsoft.EntityFrameworkCore.SqlServer \
  --project ByteFix.Datos \
  --startup-project ByteFix.Api \
  --output-dir Models \
  --context ByteFixContext \
  --no-onconfiguring \
  --force
