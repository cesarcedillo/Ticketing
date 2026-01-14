#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
source "$ROOT/ci/common.sh"

require_cmd dotnet
require_cmd find
require_cmd sort
require_cmd tee

BACKEND_DIR="${BACKEND_DIR:-Backend}"
CONFIGURATION="${CONFIGURATION:-Release}"

# Artefactos
RESULTS_DIR="${RESULTS_DIR:-$ROOT/TestResults}"
LOGS_DIR="${LOGS_DIR:-$ROOT/ci-logs/unit}"
RUN_ID="${RUN_ID:-$(date +%Y%m%d-%H%M%S)}"

mkdir -p "$RESULTS_DIR" "$LOGS_DIR"

log "Searching for solutions under: $BACKEND_DIR"
mapfile -t SLNS < <(find "$BACKEND_DIR" -name "*.sln" -type f | sort)
[ ${#SLNS[@]} -gt 0 ] || die "No .sln files found under $BACKEND_DIR/"

log "Found ${#SLNS[@]} solution(s). Running unit tests per solution..."

failures=()

for sln in "${SLNS[@]}"; do
  if echo "$sln" | grep -qi "e2e"; then
    log "Skipping (E2E solution): $sln"
    continue
  fi

  sln_base="$(basename "$sln" .sln)"
  sln_slug="${sln_base//[^a-zA-Z0-9._-]/_}"
  run_slug="${sln_slug}.${RUN_ID}"

  # Log completo por solución
  log_file="$LOGS_DIR/${run_slug}.log"

  # Directorio de resultados por solución (evita pisar TRX)
  sol_results_dir="$RESULTS_DIR/$run_slug"
  mkdir -p "$sol_results_dir"

  log "----"
  log "Solution: $sln"
  log "Log file: $log_file"
  log "Results:  $sol_results_dir"

  start_ts="$(date +%s)"

  # En consola: solo lo mínimo. El detalle va a fichero.
  {
  echo "=== RESTORE: $sln"
  dotnet restore "$sln"

  echo "=== BUILD:   $sln"
  dotnet build "$sln" -c "$CONFIGURATION" --no-restore

  echo "=== TEST:    $sln"
  dotnet test "$sln" -c "$CONFIGURATION" --no-build \
    --results-directory "$sol_results_dir" \
    --logger "trx"
} 2>&1 | tee "$log_file" | grep -vE '(^/.*: warning (NU|CS)[0-9]{4}:|^ *[0-9]+ Warning\(s\)|^Time Elapsed|^ *Determining projects to restore|^ *All projects are up-to-date for restore\.)'

  end_ts="$(date +%s)"
  dur="$((end_ts - start_ts))"

  # Si dotnet test devuelve !=0, el script se habría detenido por set -e.
  # Para capturar fallos sin abortar todo, podríamos relajar set -e, pero mantengo set -e
  # porque es CI. Aun así, dejamos una línea resumen por cada solución que pasa.
  log "OK: $sln_base (${dur}s)"
done

log "Unit tests completed."
log "Unit logs:   $LOGS_DIR"
log "Unit TRX:    $RESULTS_DIR/<solution>.$RUN_ID/*.trx"
