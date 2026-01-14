#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
source "$ROOT/ci/common.sh"

require_cmd docker
require_cmd dotnet
require_cmd curl

COMPOSE_FILE="${COMPOSE_FILE:-docker-compose.yml}"
CONFIGURATION="${CONFIGURATION:-Release}"
RESULTS_DIR="${RESULTS_DIR:-$ROOT/TestResults}"
LOGS_DIR="${LOGS_DIR:-$ROOT/ci-logs}"

BFF_READY_URL="${BFF_READY_URL:-http://localhost:5000/healthz/ready}"
READY_TRIES="${READY_TRIES:-60}"
READY_SLEEP="${READY_SLEEP:-2}"

E2E_DEFAULT_PATH="Backend/tests/Ticketing.E2E/Ticketing.E2E.csproj"
E2E_PROJ="${E2E_PROJ:-$E2E_DEFAULT_PATH}"

mkdir -p "$RESULTS_DIR" "$LOGS_DIR"

cleanup() {
  local exit_code=$?
  log "Collecting docker compose status & logs (exit_code=$exit_code)"

  set +e
  docker compose -f "$COMPOSE_FILE" ps | tee "$LOGS_DIR/docker-compose.ps.txt"
  docker compose -f "$COMPOSE_FILE" logs --no-color > "$LOGS_DIR/docker-compose.log" 2>&1
  docker compose -f "$COMPOSE_FILE" down -v --remove-orphans >/dev/null 2>&1
  set -e

  if [ $exit_code -ne 0 ]; then
    warn "E2E failed. Logs captured in $LOGS_DIR"
  else
    log "E2E finished successfully."
  fi

  exit $exit_code
}
trap cleanup EXIT

log "Docker Compose up (build). File: $COMPOSE_FILE"
docker compose -f "$COMPOSE_FILE" version
docker compose -f "$COMPOSE_FILE" up -d --build

if ! wait_for_url "$BFF_READY_URL" "$READY_TRIES" "$READY_SLEEP"; then
  die "Service not ready in time: $BFF_READY_URL"
fi

# Resolve E2E project if not found in default location
if [ ! -f "$E2E_PROJ" ]; then
  warn "E2E project not found at: $E2E_PROJ"
  log "Searching for *E2E*.csproj under Backend..."
  found="$(find Backend -name "*E2E*.csproj" -type f | head -n 1 || true)"
  [ -n "$found" ] || die "E2E csproj not found under Backend. Set E2E_PROJ env var."
  E2E_PROJ="$found"
fi

log "Using E2E project: $E2E_PROJ"
dotnet restore "$E2E_PROJ"

log "Running E2E tests..."
dotnet test "$E2E_PROJ" -c "$CONFIGURATION" \
  --results-directory "$RESULTS_DIR" \
  --logger "trx;LogFileName=e2e-tests.trx"
