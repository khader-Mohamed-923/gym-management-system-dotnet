using GeymManagement.DbContexts;
using GymManagement.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.Infrastructure.Repositories;

public class MemberRepository(GymDbContext dbContext) : Repository<Member>(dbContext),IMemberRepository
{
}
 