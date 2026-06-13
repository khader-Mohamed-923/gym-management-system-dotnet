using Autofac;
using GeymInfrastructure.Repositories;
using GymManagement.Domain.Repositories;
using GymManagement.Infrastructure.BackgroundJobs;
using Microsoft.Extensions.Hosting;

namespace GymManagement.Infrastructure.IoC;

public class InfrastructureModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {

        builder.RegisterType<MemberRepository>()
               .As<IMemberRepository>()
               .InstancePerLifetimeScope();

        builder.RegisterType<TrainerRepository>()
               .As<ITrainerRepository>()
               .InstancePerLifetimeScope();

        builder.RegisterType<DataCleanupJob>()
       .As<IHostedService>()
       .InstancePerDependency();

    }
}
