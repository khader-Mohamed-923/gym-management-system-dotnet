using GymManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace GymManagement.Presentation.Controllers;

[Authorize(Roles = nameof(Role.Admin))]
public abstract class BaseAdminController : BaseController
{
}
