# Credential setup and security

## Resolution policy

Credentials are resolved only when authentication calls `CredentialsHelper`.
`GetUsername()` and `GetPassword()` share one thread-safe, lazily validated pair.

1. If either `TEST_USERNAME` or `TEST_PASSWORD` is defined, both must be nonblank.
   This environment pair wins over User Secrets. Invalid pairs fail without fallback.
2. If neither is defined, CI fails closed before reading User Secrets.
3. Outside CI, both `Credentials:Username` and `Credentials:Password` must be
   nonblank in this project's .NET User Secrets store.

Values are never mixed across sources or trimmed. No other environment credential
names are supported. Whitespace-only values are invalid. Both getters validate the
entire pair; unauthenticated tests that do not call them need no credentials.
The pair, or its resolution failure, is cached for the process. Restart the test
process after changing credentials or CI markers.

CI detection recognizes `GITHUB_ACTIONS=true` (case-insensitive), nonblank
`JENKINS_URL` or `JENKINS_HOME`, or a `BUILD_TAG` starting with `jenkins-`
(case-insensitive). Preserve these markers when launching tests through wrappers
or containers. A local shell retaining these markers also disables local fallback.

## Local development setup

.NET User Secrets are local development storage outside the repository, not an
encrypted secret vault. Protect the OS account and filesystem permissions.
The project already declares its `UserSecretsId`; do not create a repository copy
of the secrets file. Logical keys use placeholder values only in this example:

```json
{
  "Credentials:Username": "<TEST_ACCOUNT_USERNAME>",
  "Credentials:Password": "<TEST_ACCOUNT_PASSWORD>"
}
```

For actual setup, use PowerShell 7 from the repository directory in a trusted
terminal without transcription, debug tracing, or screen recording. This script
prompts without echoing either value and pipes JSON to the CLI through standard
input. Actual values do not enter shell history or command-line arguments. The
CLI and script still hold plaintext in memory temporarily; cleanup cannot promise
memory erasure. Do not print variables, configuration, or stored secrets.

ASCII-only JSON transport avoids sending non-ASCII text across the native CLI
boundary. JSON Unicode escape sequences preserve the original Unicode credential
values when the CLI parses the payload.

```powershell
$setupUsername = Read-Host 'Test account username' -AsSecureString
$setupPassword = Read-Host 'Test account password' -AsSecureString
$setupPair = @{}
try {
    $setupPair['Credentials:Username'] = [System.Net.NetworkCredential]::new('', $setupUsername).Password
    $setupPair['Credentials:Password'] = [System.Net.NetworkCredential]::new('', $setupPassword).Password
    if ([string]::IsNullOrWhiteSpace($setupPair['Credentials:Username']) -or
        [string]::IsNullOrWhiteSpace($setupPair['Credentials:Password'])) {
        throw 'Both credential values must be nonblank.'
    }
    $setupPair | ConvertTo-Json -Compress -EscapeHandling EscapeNonAscii | dotnet user-secrets set --project PlaywrightTests.csproj
    if ($LASTEXITCODE -ne 0) { throw 'User Secrets setup failed.' }
}
finally {
    $setupPair.Clear()
    $setupUsername.Dispose()
    $setupPassword.Dispose()
    Remove-Variable setupPair, setupUsername, setupPassword
}
```

This updates both credential keys without requiring a readout of stored values.
Remove stale `TEST_USERNAME` and `TEST_PASSWORD` definitions before using local
fallback. If using environment credentials instead, populate both through an
approved nonprinting secret injection mechanism; never type real passwords into
shell assignments or CLI arguments. Validate resolution with dummy values first.

## GitHub Actions and Jenkins

Create GitHub Actions repository secrets named `TEST_USERNAME` and `TEST_PASSWORD`
using GitHub's secret-entry interface. Both workflows bind them only to test
execution steps. Missing or blank CI credentials fail authentication explicitly;
User Secrets cannot rescue a misconfigured CI run. Fork PRs normally cannot access
repository secrets; never expose secrets to untrusted PR code to bypass this.

For Jenkins, store a Username with password credential with a placeholder ID such
as `<JENKINS_TEST_CREDENTIAL_ID>`. Bind it using Credentials Binding only around
test execution, with username variable `TEST_USERNAME` and password variable
`TEST_PASSWORD`. Preserve the Jenkins CI markers described above. Do not use
Groovy string interpolation, echo, environment dumps, or command arguments to pass
credential values. Keep build/install/report steps outside the binding scope.
Jenkins configuration is managed separately and is not included in this repository.

## Traces, migration, and historical exposure

Raw tracing is disabled by default. Only `PLAYWRIGHT_TRACE_ENABLED=true`
(case-insensitive, without padding) opts in. Both GitHub workflows explicitly set
it to `'false'` and do not upload raw traces. Jenkins should also explicitly set
it to false. Existing trace naming and local opt-in remain available.

Raw traces may contain credentials, fill parameters, request bodies, authentication
tokens, Authorization headers, cookies, screenshots, and authenticated data. They
are not sanitized or safe to share. Opt in only for an explicitly approved
investigation with restricted storage and appropriate account precautions.

Replacing current credential values does not remove Git history, old clones,
previous logs, or diagnostic artifacts. The old account remains compromised;
rotate its password and revoke sessions where supported, or contact the service
operator. A new account does not secure the old one. Historical cleanup and
artifact remediation require a separate approved operation. Do not use the new
account until the containment changes have been reviewed and adopted.
