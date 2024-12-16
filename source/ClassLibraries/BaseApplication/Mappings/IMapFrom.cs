using AutoMapper;

namespace BaseApplication.Mappings;

public interface IMapFrom<T>
{
    void Mapping(Profile profile) => profile.CreateMap(typeof(T), GetType()).IgnoreAllPropertiesWithAnInaccessibleSetter();
}
