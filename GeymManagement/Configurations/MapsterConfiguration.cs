using GymManagement.Domain.DTOs.Members.Requests;
using GymManagement.Domain.DTOs.Members.Responses;
using GymManagement.Domain.DTOs.Trainers.Requests;
using GymManagement.Domain.DTOs.Trainers.Responses;
using GymManagement.Domain.DTOs.Plans.Requests;
using GymManagement.Domain.DTOs.Plans.Responses;
using GymManagement.Presentation.ViewModels.Member;
using GymManagement.Presentation.ViewModels.HealthRecord;
using GymManagement.Presentation.ViewModels.Trainer;
using GymManagement.Presentation.ViewModels.Plan;
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
    }
}
