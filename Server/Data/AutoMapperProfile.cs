using AutoMapper;
using Server.Models;
using Server.Models.ProductTypes;
using Server.Models.ProductComponents;
using Shared.Models.DTOs;
using Shared.Models.DTOs.ProductTypesDTOs;
using Shared.Models.DTOs.ProductComponentsDTOs;


namespace Server.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // ProductTypes
            CreateMap<Product, ProductDTO>();
            CreateMap<Laptop, LaptopDTO>();
            CreateMap<Phone, PhoneDTO>();
            CreateMap<Headphones, HeadphonesDTO>()
                .ForMember(dest => dest.BatteryLifeInHours, opt => opt.MapFrom(src => src.BatteryLife));
            // ProductComponents
            CreateMap<Cpu, CpuDTO>();
            CreateMap<Camera, CameraDTO>();
            CreateMap<Display, DisplayDTO>();
            CreateMap<Gpu, GpuDTO>();
            CreateMap<ChargingAccessory, ChargingAccessoryDTO>();
            CreateMap<Ram, RamDTO>();
            CreateMap<Image, ImageDTO>();
        }
    }
}
