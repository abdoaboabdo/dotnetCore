using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Vega.Controllers.Resources;
using Vega.Models;

namespace vega.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Make, MakeResource>();
            CreateMap<Model, ModelResource>();
            CreateMap<Feature, FeatureResource>();
            CreateMap<Vehicle, VehicleResource>()
                .ForMember(vr=>vr.Contact,opt=>opt.MapFrom(v=>new ContactResource{Name =v.ContactName,Email=v.ContactEmail,Phone=v.ContactPhone}))
                .ForMember(vr=>vr.Features,opt=>opt.MapFrom(v=>v.VehicleFeatures.Select(vf=>vf.FeatureId)));


            CreateMap<VehicleResource, Vehicle>()
                .ForMember(v=>v.Id,opt=> opt.Ignore())
                .ForMember(v=>v.ContactName,opt => opt.MapFrom(vr => vr.Contact.Name))
                .ForMember(v=>v.ContactEmail,opt => opt.MapFrom(vr => vr.Contact.Email))
                .ForMember(v=>v.ContactPhone,opt => opt.MapFrom(vr => vr.Contact.Phone))
                .ForMember(v=>v.VehicleFeatures,opt => opt.Ignore())
                .AfterMap((vr,v) => {
                    // var removeFeatures=new List<VehicleFeature>();
                    // foreach (var f in v.VehicleFeatures)
                    // {
                    //     if(!vr.Features.Contains(f.FeatureId))
                    //         removeFeatures.Add(f);
                    // }
                    var removeFeatures = v.VehicleFeatures.Where(f => !vr.Features.Contains(f.FeatureId));
                    foreach (var f in removeFeatures)
                        v.VehicleFeatures.Remove(f);

                    // foreach (var id in vr.Features)
                    // {
                    //     if(!v.VehicleFeatures.Any(f=>f.FeatureId == id))
                    //         v.VehicleFeatures.Add(new VehicleFeature{FeatureId=id});
                    // }
                    var AddedFeatures = vr.Features.Where(id => !v.VehicleFeatures.Any(f=>f.FeatureId == id)).Select(id=>new VehicleFeature{FeatureId=id});
                    foreach (var f in AddedFeatures)
                        v.VehicleFeatures.Add(f);
                });
        }
    }
}