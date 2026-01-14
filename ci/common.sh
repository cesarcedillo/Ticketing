#!/usr/bin/env bash
set -euo pipefail

log()  { printf "\n[%s] %s\n" "$(date +'%H:%M:%S')" "$*"; }
warn() { printf "\n[%s] WARNING: %s\n" "$(date +'%H:%M:%S')" "$*" >&2; }
die()  { printf "\n[%s] ERROR: %s\n" "$(date +'%H:%M:%S')" "$*" >&2; exit 1; }

require_cmd() {
  command -v "$1" >/dev/null 2>&1 || die "Command not found: $1"
}

# wait_for_url <url> <tries> <sleep_seconds>
wait_for_url() {
  local url="$1"
  local tries="${2:-60}"
  local sleep_s="${3:-2}"

  log "Waiting for: $url (tries=$tries, sleep=${sleep_s}s)"
  for ((i=1; i<=tries; i++)); do
    if curl -fsS "$url" >/dev/null 2>&1; then
      log "Ready: $url"
      return 0
    fi
    sleep "$sleep_s"
  done

  return 1
}

# Resolve repo root even if called from another folder
repo_root() {
  git rev-parse --show-toplevel 2>/dev/null || pwd
}
