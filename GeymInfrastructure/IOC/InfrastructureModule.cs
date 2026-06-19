using Autofac;
using GeymInfrastructure.Repositories;
using GymManagement.Domain.Repositories;
using GymManagement.Domain.Services.Members;
using GymManagement.Infrastructure.BackgroundJobs;
using GymManagement.Infrastructure.Services.Media;
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

        builder.RegisterType<SessionRepository>()
               .As<ISessionRepository>()
               .InstancePerLifetimeScope();

        builder.RegisterType<BookingRepository>()
               .As<IBookingRepository>()
               .InstancePerLifetimeScope();

        builder.RegisterType<DataCleanupJob>()
       .As<IHostedService>()
       .InstancePerDependency();

        builder.RegisterType<CloudinaryService>()
               .As<IImageService>()
               .InstancePerLifetimeScope();

    }
}
