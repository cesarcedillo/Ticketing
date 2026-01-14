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
BUILD_LOGS_DIR="${BUILD_LOGS_DIR:-$ROOT/ci-logs/build}"
RUN_ID="${RUN_ID:-$(date +%Y%m%d-%H%M%S)}"

mkdir -p "$RESULTS_DIR" "$LOGS_DIR" "$BUILD_LOGS_DIR"

log "Unit-per-solution"
log "RUN_ID=$RUN_ID | CONFIGURATION=$CONFIGURATION"

# 1) Reutiliza build-per-solution.sh (restore + build por solución)
log "Running build-per-solution first..."
chmod +x "$ROOT/ci/build-per-solution.sh"
BACKEND_DIR="$BACKEND_DIR" CONFIGURATION="$CONFIGURATION" RUN_ID="$RUN_ID" LOGS_DIR="$BUILD_LOGS_DIR" \
  bash "$ROOT/ci/build-per-solution.sh"

# 2) Descubre soluciones (podrías también refactorizar discovery a una lib común si quieres)
log "Searching for solutions under: $BACKEND_DIR"
mapfile -t SLNS < <(find "$BACKEND_DIR" -name "*.sln" -type f | sort)
[ ${#SLNS[@]} -gt 0 ] || die "No .sln files found under $BACKEND_DIR/"

log "Found ${#SLNS[@]} solution(s). Running unit tests per solution..."

for sln in "${SLNS[@]}"; do
  if echo "$sln" | grep -qi "e2e"; then
    log "Skipping (E2E solution): $sln"
    continue
  fi

  sln_base="$(basename "$sln" .sln)"
  sln_slug="${sln_base//[^a-zA-Z0-9._-]/_}"
  run_slug="${sln_slug}.${RUN_ID}"

  log_file="$LOGS_DIR/${run_slug}.log"
  sol_results_dir="$RESULTS_DIR/$run_slug"
  mkdir -p "$sol_results_dir"

  log "----"
  log "Solution: $sln"
  log "Log file: $log_file"
  log "Results:  $sol_results_dir"

  start_ts="$(date +%s)"

  # Nota: con pipefail, OJO a grep:
  # - grep puede devolver 1 si no hay coincidencias
  # - eso haría fallar el script
  # Por eso añadimos "|| true" al grep.
  {
    echo "=== TEST: $sln"
    dotnet test "$sln" -c "$CONFIGURATION" --no-build --no-restore \
      --results-directory "$sol_results_dir" \
      --logger "trx"
  } 2>&1 | tee "$log_file" | grep -vE '(^/.*: warning (NU|CS)[0-9]{4}:|^ *[0-9]+ Warning\(s\)|^Time Elapsed|^ *Determining projects to restore|^ *All projects are up-to-date for restore\.)' || true

  end_ts="$(date +%s)"
  dur="$((end_ts - start_ts))"

  log "OK: $sln_base (${dur}s)"
done

log "Unit tests completed."
log "Build logs:  $BUILD_LOGS_DIR"
log "Unit logs:   $LOGS_DIR"
log "Unit TRX:    $RESULTS_DIR/<solution>.$RUN_ID/*.trx"
