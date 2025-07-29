# GitHub Actions Workflows

This directory contains GitHub Actions workflows for CI/CD automation.

## Available Workflows

### 1. `simple-ci.yml` - Basic CI
**Triggers:** Push to `main`, `master`, `develop`, `Testing` branches and PRs to `main`/`master`

**What it does:**
- ✅ Checks out code
- ✅ Sets up .NET 8.0
- ✅ Restores dependencies
- ✅ Builds the solution
- ✅ Runs all tests

**Use this for:** Quick feedback on code changes

### 2. `dotnet.yml` - Full CI/CD Pipeline
**Triggers:** Push to `main`, `master`, `develop` branches and PRs to `main`/`master`

**What it does:**
- ✅ All basic CI steps
- ✅ Code coverage analysis (on main/master pushes)
- ✅ Builds and publishes API artifacts
- ✅ Builds and publishes Web App artifacts
- ✅ Uploads test results and coverage reports

**Use this for:** Production deployments and comprehensive testing

## How to Use

1. **Push your code** to any of the configured branches
2. **Check the Actions tab** in your GitHub repository
3. **Monitor the workflow** as it runs
4. **Review results** and artifacts

## Branch Strategy

- **`Testing`** - Your current branch for development and testing
- **`develop`** - Integration branch for features
- **`main`/`master`** - Production-ready code