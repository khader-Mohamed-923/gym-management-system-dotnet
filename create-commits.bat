@echo off
echo Resetting current staging area...
git reset

echo Committing Admin Dashboard Services...
git add GeymInfrastructure/Services/Admin/AdminDashboardService.cs
git add GeymManagement/Views/Admin/Dashboard.cshtml
git commit -m "refactor(admin): update admin dashboard service and view"

echo Committing Booking and Sessions Views...
git add GeymManagement/Controllers/BookingController.cs
git add GeymManagement/Views/Booking/MyBookings.cshtml
git add GeymManagement/Views/Booking/Schedule.cshtml
git add GeymManagement/Views/Booking/SessionMembers.cshtml
git add GeymManagement/Views/Sessions/Index.cshtml
git add GeymManagement/Views/Sessions/Create.cshtml
git add GeymManagement/Views/Sessions/Delete.cshtml
git add GeymManagement/Views/Sessions/Details.cshtml
git add GeymManagement/Views/Sessions/Edit.cshtml
git commit -m "refactor(booking): update booking controller and related session views"

echo Committing Member Views...
git add GeymManagement/Views/Member/Bookings.cshtml
git add GeymManagement/Views/Member/CompleteProfile.cshtml
git add GeymManagement/Views/Member/Dashboard.cshtml
git add GeymManagement/Views/Member/Memberships.cshtml
git add GeymManagement/Views/Member/Profile.cshtml
git add GeymManagement/Views/Members/Create.cshtml
git add GeymManagement/Views/Members/Delete.cshtml
git add GeymManagement/Views/Members/Details.cshtml
git add GeymManagement/Views/Members/Edit.cshtml
git add GeymManagement/Views/Members/HealthRecord.cshtml
git add GeymManagement/Views/Members/Index.cshtml
git commit -m "refactor(members): restructure member views and profiles"

echo Committing Memberships and Plans Views...
git add GeymManagement/Views/Memberships/Create.cshtml
git add GeymManagement/Views/Memberships/Index.cshtml
git add GeymManagement/Views/Plans/Details.cshtml
git add GeymManagement/Views/Plans/Edit.cshtml
git add GeymManagement/Views/Plans/Index.cshtml
git commit -m "refactor(plans): update membership and plan views"

echo Committing Trainer Views...
git add GeymManagement/Views/Trainers/Create.cshtml
git add GeymManagement/Views/Trainers/Dashboard.cshtml
git add GeymManagement/Views/Trainers/Delete.cshtml
git add GeymManagement/Views/Trainers/Details.cshtml
git add GeymManagement/Views/Trainers/Edit.cshtml
git add GeymManagement/Views/Trainers/Index.cshtml
git commit -m "refactor(trainers): update trainer dashboard and management views"

echo Committing Authentication Views...
git add GeymManagement/Views/Auth/Login.cshtml
git add GeymManagement/Views/Auth/Register.cshtml
git commit -m "refactor(auth): update authentication views"

echo Committing Shared Layouts and Home Views...
git add GeymManagement/Views/Home/Index.cshtml
git add GeymManagement/Views/Home/Privacy.cshtml
git add GeymManagement/Views/Shared/Error.cshtml
git add GeymManagement/Views/Shared/_AdminLayout.cshtml
git add GeymManagement/Views/Shared/_Layout.cshtml
git add GeymManagement/Views/Shared/_LogoutPartial.cshtml
git add GeymManagement/Views/Shared/_MemberLayout.cshtml
git add GeymManagement/Views/Shared/_TrainerLayout.cshtml
git commit -m "refactor(ui): update shared layouts and home views"

echo Committing Styling Adjustments...
git rm GeymManagement/wwwroot/css/Style.css
git rm GeymManagement/wwwroot/css/iron-ui.css
git add GeymManagement/wwwroot/css/site.css
git commit -m "style: remove obsolete css files and update site.css"

echo Committing Documentation and Assets...
git add README.md
git add gym-kinetic-design-skill.md
git add photos/
git commit -m "docs: add kinetic design skill, update README and photos"

echo Committing chore updates...
git add create-commits.bat
git commit -m "chore: update create-commits script"

echo Finished atomic commits!
git status
