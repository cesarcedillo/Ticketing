#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
source "$ROOT/ci/common.sh"

require_cmd dotnet
require_cmd find
require_cmd sort
require_cmd tee
require_cmd grep
require_cmd date

BACKEND_DIR="${BACKEND_DIR:-Backend}"
CONFIGURATION="${CONFIGURATION:-Release}"

# Artefactos
LOGS_DIR="${LOGS_DIR:-$ROOT/ci-logs/build}"
RUN_ID="${RUN_ID:-$(date +%Y%m%d-%H%M%S)}"

# Opcional: filtrar warnings del output en consola, manteniendo el log completo.
# true  => consola más limpia (pero el fichero .log sigue teniendo TODO)
# false => consola muestra todo
FILTER_CONSOLE_WARNINGS="${FILTER_CONSOLE_WARNINGS:-true}"

mkdir -p "$LOGS_DIR"

log "Build-per-solution"
log "RUN_ID=$RUN_ID | CONFIGURATION=$CONFIGURATION"
log "Searching for solutions under: $BACKEND_DIR"

mapfile -t SLNS < <(find "$BACKEND_DIR" -name "*.sln" -type f | sort)
[ ${#SLNS[@]} -gt 0 ] || die "No .sln files found under $BACKEND_DIR/"

log "Found ${#SLNS[@]} solution(s). Building per solution..."

for sln in "${SLNS[@]}"; do
  if echo "$sln" | grep -qi "e2e"; then
    log "Skipping (E2E solution): $sln"
    continue
  fi

  sln_base="$(basename "$sln" .sln)"
  sln_slug="${sln_base//[^a-zA-Z0-9._-]/_}"
  run_slug="${sln_slug}.${RUN_ID}"
  log_file="$LOGS_DIR/${run_slug}.log"

  log "----"
  log "Solution: $sln"
  log "Log file: $log_file"

  start_ts="$(date +%s)"

  if [ "$FILTER_CONSOLE_WARNINGS" = "true" ]; then
    # Log completo al fichero; consola “limpia” (sin spam de warnings).
    {
      echo "=== RESTORE: $sln"
      dotnet restore "$sln"

      echo "=== BUILD:   $sln"
      dotnet build "$sln" -c "$CONFIGURATION" --no-restore
    } 2>&1 | tee "$log_file" | grep -vE '(^/.*: warning (NU|CS)[0-9]{4}:|^ *[0-9]+ Warning\(s\)|^Time Elapsed|^ *Determining projects to restore|^ *All projects are up-to-date for restore\.)' || true
  else
    # Consola muestra todo (igual que el fichero).
    {
      echo "=== RESTORE: $sln"
      dotnet restore "$sln"

      echo "=== BUILD:   $sln"
      dotnet build "$sln" -c "$CONFIGURATION" --no-restore
    } 2>&1 | tee "$log_file"
  fi

  end_ts="$(date +%s)"
  dur="$((end_ts - start_ts))"

  log "OK: $sln_base (${dur}s)"
done

log "Build completed."
log "Build logs: $LOGS_DIR"
