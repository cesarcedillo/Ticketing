#!/usr/bin/env bash
set -euo pipefail
ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
source "$ROOT/ci/common.sh"

RUN_BUILD="${RUN_BUILD:-true}"
RUN_UNITS="${RUN_UNITS:-true}"
RUN_E2E="${RUN_E2E:-true}"
CONFIGURATION="${CONFIGURATION:-Release}"

log "CI Orchestrator"
log "RUN_BUILD=$RUN_BUILD | RUN_UNITS=$RUN_UNITS | RUN_E2E=$RUN_E2E | CONFIGURATION=$CONFIGURATION"

if [ "$RUN_BUILD" = "true" ]; then
  bash "$ROOT/ci/build-per-solution.sh"
else
  warn "Build skipped."
fi

if [ "$RUN_UNITS" = "true" ]; then
  bash "$ROOT/ci/unit-per-solution.sh"
else
  warn "Unit tests skipped."
fi

if [ "$RUN_E2E" = "true" ]; then
  bash "$ROOT/ci/e2e.sh"
else
  warn "E2E skipped."
fi

log "CI completed."
