using AutoMapper;

namespace BookApi.Application.Common.Mappings
{
    public interface IMapFrom<T>
    {
        virtual void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType());
    }
}