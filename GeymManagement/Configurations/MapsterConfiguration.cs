using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Domain.DTOs.Members.Responses;
using GymManagement.Domain.DTOs.Trainers.Requests;
using GymManagement.Domain.DTOs.Trainers.Responses;
using GymManagement.Domain.DTOs.Plans.Requests;
using GymManagement.Domain.DTOs.Plans.Responses;
using GymManagement.Domain.DTOs.Sessions.Requests;
using GymManagement.Domain.DTOs.Sessions.Responses;
using GymManagement.Presentation.ViewModels.Member;
using GymManagement.Presentation.ViewModels.HealthRecord;
using GymManagement.Presentation.ViewModels.Trainer;
using GymManagement.Presentation.ViewModels.Plan;
using GymManagement.Presentation.ViewModels.Session;
using GymManagement.Presentation.ViewModels.Membership;
using GymManagement.Domain.DTOs.Memberships.Requests;
using GymManagement.Domain.DTOs.Memberships.Responses;
using Mapster;

namespace GymManagement.Presentation.Configurations;

public static class MapsterConfiguration
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<MemberCreateViewModel, CreateMemberRequest>.NewConfig()
            .Map(dest => dest.HealthRecord, src => new CreateHealthRecordDto
            {
                Height = src.HealthRecord.Height ?? 0,
                Weight = src.HealthRecord.Weight ?? 0,
                BloodType = src.HealthRecord.BloodType,
                Note = src.HealthRecord.Note
            });

        TypeAdapterConfig<MemberResponse, MemberIndexViewModel>.NewConfig();

        TypeAdapterConfig<MemberDetailsResponse, MemberDetailsViewModel>.NewConfig();

        TypeAdapterConfig<HealthRecordResponse, HealthRecordDetailsViewModel>.NewConfig();

        TypeAdapterConfig<MemberEditResponse, MemberEditViewModel>.NewConfig();

        TypeAdapterConfig<MemberEditViewModel, UpdateMemberRequest>.NewConfig();

        TypeAdapterConfig<TrainerCreateViewModel, CreateTrainerRequest>.NewConfig();

        TypeAdapterConfig<TrainerResponse, TrainerIndexViewModel>.NewConfig();

        TypeAdapterConfig<TrainerDetailsResponse, TrainerDetailsViewModel>.NewConfig();

        TypeAdapterConfig<TrainerEditResponse, TrainerEditViewModel>.NewConfig();

        TypeAdapterConfig<TrainerEditViewModel, UpdateTrainerRequest>.NewConfig();

        TypeAdapterConfig<PlanResponse, PlanIndexViewModel>.NewConfig();

        TypeAdapterConfig<PlanResponse, EditPlanViewModel>.NewConfig();

        TypeAdapterConfig<EditPlanViewModel, UpdatePlanRequest>.NewConfig();

        TypeAdapterConfig<SessionResponse, SessionIndexViewModel>.NewConfig()
            .Map(dest => dest.Status, src => ComputeStatus(src.StartDate, src.EndDate));

        TypeAdapterConfig<SessionCreateViewModel, CreateSessionRequest>.NewConfig();

        TypeAdapterConfig<SessionEditResponse, SessionEditViewModel>.NewConfig();

        TypeAdapterConfig<SessionEditViewModel, UpdateSessionRequest>.NewConfig();

        TypeAdapterConfig<SessionDetailsResponse, SessionDetailsViewModel>.NewConfig()
            .Map(dest => dest.Status, src => ComputeStatus(src.StartDate, src.EndDate));

        TypeAdapterConfig<MembershipResponse, MembershipIndexViewModel>.NewConfig()
            .Map(dest => dest.Status, src => ComputeMembershipStatus(src.EndDate));

        TypeAdapterConfig<MembershipCreateViewModel, CreateMembershipRequest>.NewConfig();
    }

    private static string ComputeMembershipStatus(DateTime endDate)
    {
        return DateTime.Now > endDate ? "Expired" : "Active";
    }

    private static string ComputeStatus(DateTime startDate, DateTime endDate)
    {
        var now = DateTime.Now;
        if (now < startDate)
            return "Upcoming";
        if (now >= startDate && now <= endDate)
            return "Ongoing";
        return "Completed";
    }
}
