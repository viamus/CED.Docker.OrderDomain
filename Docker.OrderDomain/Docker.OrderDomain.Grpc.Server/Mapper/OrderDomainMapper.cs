using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.OrderDomain.Grpc.Mapper
{
    public class OrderDomainMapper
    {
        private static readonly OrderDomainMapper _Instance = new OrderDomainMapper();

        public MapperConfiguration Mapper;

        public static OrderDomainMapper Instance
        {
            get
            {
                return _Instance;
            }
        }

        public void LoadMapperConfig()
        {
            Mapper = new MapperConfiguration(mapper =>
            {
                LoadOrderConfigurationMapper(mapper);
                LoadOrderProductConfigurationMapper(mapper);
            });
        }

        private void LoadOrderConfigurationMapper(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<SendOrderRequest, Context.Order>()
                .ForMember(target => target.Ref, m => m.MapFrom(source => source.Ref));
        }

        private void LoadOrderProductConfigurationMapper(IMapperConfigurationExpression mapper)
        {
            mapper.CreateMap<OrderProduct, Context.OrderProduct>()
                .ForMember(target => target.OrderRef, m => m.MapFrom(source => source.Ref))
                .ForMember(target => target.ProductName, m => m.MapFrom(source => source.ProductName))
                .ForMember(target => target.Value, m => m.MapFrom(source => Decimal.Parse(source.Value)));
        }

    }
}
