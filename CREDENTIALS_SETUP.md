# Setting Up Credentials for Test Execution

This guide explains how to securely store test credentials so they're never visible in source control.

## Option 1: User Secrets (Recommended for Local Development)

User secrets are stored locally on your machine outside the project directory. They're perfect for development.

### Setup Steps:

1. **Open PowerShell** in your project directory

2. **Set your credentials:**
   ```powershell
   dotnet user-secrets set "Credentials:Username" "milos.jancic83@gmail.com"
   dotnet user-secrets set "Credentials:Password" "mikitest83"
   ```

3. **Verify they're set:**
   ```powershell
   dotnet user-secrets list
   ```

That's it! Your tests will now automatically load these credentials.

### Location of User Secrets:
- **Windows:** `C:\Users\[YourUsername]\AppData\Roaming\Microsoft\UserSecrets\PlaywrightTests-SecretId\secrets.json`
- **macOS:** `~/.microsoft/usersecrets/PlaywrightTests-SecretId/secrets.json`
- **Linux:** `~/.microsoft/usersecrets/PlaywrightTests-SecretId/secrets.json`

---

## Option 2: Environment Variables (For CI/CD Pipelines)

Use environment variables when running tests in GitHub Actions, Azure DevOps, or other CI/CD systems.

### Setup:

```powershell
# Set environment variables (in your CI/CD pipeline)
$env:TEST_USERNAME = "milos.jancic83@gmail.com"
$env:TEST_PASSWORD = "mikitest83"

# Or in bash/GitHub Actions
export TEST_USERNAME="milos.jancic83@gmail.com"
export TEST_PASSWORD="mikitest83"
```

### In GitHub Actions Example:
```yaml
env:
  TEST_USERNAME: ${{ secrets.TEST_USERNAME }}
  TEST_PASSWORD: ${{ secrets.TEST_PASSWORD }}
```

---

## How It Works

The `CredentialsHelper` class tries to load credentials in this order:
1. User secrets (if running locally)
2. Environment variables (if set)
3. Throws an error if neither is found

### Using in Your Tests:
```csharp
using PlaywrightTests.Utilities;

// In your test method:
string username = CredentialsHelper.GetUsername();
string password = CredentialsHelper.GetPassword();
```

---

## Important Notes

✅ **DO:**
- Store credentials in user secrets or environment variables
- Use `CredentialsHelper` to retrieve credentials in tests
- Update `.gitignore` to prevent accidental commits

❌ **DON'T:**
- Hardcode credentials in source files
- Store credentials in JSON files in the repository
- Commit `.env` files or secrets configuration

---

## Troubleshooting

### "Username not found" error
**Solution:** Run the setup commands from Option 1 above.

### Clearing secrets
```powershell
dotnet user-secrets clear
```

### Removing a specific secret
```powershell
dotnet user-secrets remove "Credentials:Username"
```

### Tests still not finding credentials
1. Ensure you're running from the project directory containing `PlaywrightTests.csproj`
2. Rebuild the project: `dotnet build`
3. Verify secrets are set: `dotnet user-secrets list`
