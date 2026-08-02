#!/usr/bin/env python3
"""
Fetch a Sentry issue (by short ID, e.g. "MED-123") plus its latest event's
stack trace, and print a single structured JSON blob to stdout.

Auth and org come from environment variables so no secret ever passes
through chat or gets written to disk:
    SENTRY_AUTH_TOKEN   (required)  user auth token, scopes: org:read, project:read, event:read
    SENTRY_ORG          (optional)  org slug; auto-detected if you belong to exactly one org
    SENTRY_BASE_URL     (optional)  defaults to https://sentry.io ; set for self-hosted Sentry

Usage:
    python fetch_sentry_issue.py MED-123
"""
import json
import os
import sys
import urllib.error
import urllib.request


def api_get(base_url: str, token: str, path: str):
    url = f"{base_url}/api/0/{path}"
    req = urllib.request.Request(url, headers={"Authorization": f"Bearer {token}"})
    try:
        with urllib.request.urlopen(req, timeout=30) as resp:
            return json.loads(resp.read().decode("utf-8"))
    except urllib.error.HTTPError as e:
        body = e.read().decode("utf-8", errors="replace")
        raise SystemExit(
            f"Sentry API error {e.code} for {url}\n{body}\n\n"
            "If this is 401/403, your SENTRY_AUTH_TOKEN is missing/expired or lacks "
            "org:read/project:read/event:read scopes. See references/setup.md."
        )
    except urllib.error.URLError as e:
        raise SystemExit(f"Could not reach Sentry at {url}: {e.reason}")


def resolve_org(base_url: str, token: str) -> str:
    org = os.environ.get("SENTRY_ORG")
    if org:
        return org
    orgs = api_get(base_url, token, "organizations/")
    if len(orgs) == 1:
        return orgs[0]["slug"]
    if len(orgs) == 0:
        raise SystemExit("This token has no organizations. Check the token's scopes.")
    slugs = ", ".join(o["slug"] for o in orgs)
    raise SystemExit(
        f"Belongs to multiple orgs ({slugs}) - set SENTRY_ORG to pick one."
    )


def extract_frames(event: dict):
    """Return exception type/value plus stack frames, crash frame first."""
    for entry in event.get("entries", []):
        if entry.get("type") != "exception":
            continue
        values = entry.get("data", {}).get("values", [])
        if not values:
            continue
        exc = values[0]
        frames = exc.get("stacktrace", {}).get("frames", []) or []
        # Sentry lists frames oldest-call-first; the crash site is last.
        # Reverse so the crash frame is first - that's usually what matters most.
        frames = list(reversed(frames))
        return {
            "exception_type": exc.get("type"),
            "exception_value": exc.get("value"),
            "frames": [
                {
                    "filename": f.get("filename"),
                    "module": f.get("module"),
                    "function": f.get("function"),
                    "lineNo": f.get("lineNo"),
                    "inApp": f.get("inApp"),
                    "context": f.get("context"),
                }
                for f in frames
            ],
        }
    return {"exception_type": None, "exception_value": None, "frames": []}


def main():
    if len(sys.argv) != 2:
        raise SystemExit("Usage: fetch_sentry_issue.py <SHORT-ID e.g. MED-123>")
    short_id = sys.argv[1]

    token = os.environ.get("SENTRY_AUTH_TOKEN")
    if not token:
        raise SystemExit(
            "SENTRY_AUTH_TOKEN is not set. See references/setup.md for how to create "
            "and set it - it must be set in your own environment, never pasted into chat."
        )
    base_url = os.environ.get("SENTRY_BASE_URL", "https://sentry.io").rstrip("/")

    org = resolve_org(base_url, token)

    resolved = api_get(base_url, token, f"organizations/{org}/shortids/{short_id}/")
    group_id = resolved["group"]["id"]
    project_slug = resolved["group"]["project"]["slug"]

    issue = api_get(base_url, token, f"organizations/{org}/issues/{group_id}/")
    event = api_get(
        base_url, token, f"organizations/{org}/issues/{group_id}/events/latest/"
    )

    result = {
        "shortId": issue.get("shortId"),
        "title": issue.get("title"),
        "culprit": issue.get("culprit"),
        "level": issue.get("level"),
        "status": issue.get("status"),
        "count": issue.get("count"),
        "userCount": issue.get("userCount"),
        "firstSeen": issue.get("firstSeen"),
        "lastSeen": issue.get("lastSeen"),
        "firstRelease": (issue.get("firstRelease") or {}).get("version"),
        "lastRelease": (issue.get("lastRelease") or {}).get("version"),
        "platform": issue.get("platform"),
        "permalink": issue.get("permalink"),
        "project": project_slug,
        "tags": [
            {"key": t.get("key"), "name": t.get("name")} for t in issue.get("tags", [])
        ],
        **extract_frames(event),
    }
    print(json.dumps(result, indent=2))


if __name__ == "__main__":
    main()
