@echo off
echo ===================================================
echo Creating GitHub Atomic Commits...
echo ===================================================

echo.
echo [1/4] Committing Trainer Feature Implementation...
git add GeymDomain/Services/Trainers/ITrainerService.cs
git add GeymDomain/Services/Trainers/TrainerService.cs
git add GeymDomain/ViewModels/Trainer/
git add GeymManagement/Controllers/TrainersController.cs
git add GeymManagement/Views/Trainers/
git commit -m "feat(trainers): implement comprehensive trainer management module" -m "Adds Create, Read, Update, and Delete capabilities for the Trainer domain including respective views, viewmodels, and service logic."

echo.
echo [2/4] Committing Delete Messaging Updates...
git add GeymManagement/Views/Members/Delete.cshtml
git commit -m "refactor(ui): clarify member delete confirmation messaging" -m "Replaces technical hard-delete terminology with user-friendly, business-focused messaging."

echo.
echo [3/4] Committing Codebase Comment Cleanup (Migrations)...
git add GeymInfrastructure/Migrations/
git commit -m "refactor(data): remove auto-generated comments from EF Core migrations"

echo.
echo [4/4] Committing Codebase Comment Cleanup (Views)...
git add GeymManagement/Views/Home/
git add GeymManagement/Views/Members/
git add GeymManagement/Views/Plans/
git add GeymManagement/Views/Shared/
git commit -m "refactor(ui): remove HTML and Razor comments across all presentation views"

echo.
echo ===================================================
echo Done! Run 'git status' to verify working directory.
echo ===================================================
