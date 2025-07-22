using AutoMapper;

namespace BLLAccountingDemo.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<DTO.Kid, EFAccounting.Entities.Kid>();
            CreateMap<EFAccounting.Entities.Kid, DTO.Kid>();

            CreateMap<DTO.Price, EFAccounting.Entities.Price>();
            CreateMap<EFAccounting.Entities.Price,  DTO.Price>();

            CreateMap<DTO.WDay, EFAccounting.Entities.WDay>();
            CreateMap<EFAccounting.Entities.WDay, DTO.WDay>();

            CreateMap<DTO.SiblingRelationship, EFAccounting.Entities.SiblingRelationship>();
            CreateMap<EFAccounting.Entities.SiblingRelationship, DTO.SiblingRelationship>();
        }
    }
}
