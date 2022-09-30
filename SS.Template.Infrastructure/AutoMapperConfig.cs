using System;
using AutoMapper;
using SS.Template.Domain.Model;

namespace SS.Template
{
    public static class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression cfg)
        {
            if (cfg is null)
            {
                throw new ArgumentNullException(nameof(cfg));
            }

            cfg.ForAllMaps(AutoIgnorePropertiesInternal);
        }

        private static void AutoIgnorePropertiesInternal(TypeMap map, IMappingExpression expression)
        {
            if (typeof(IHaveDateCreated).IsAssignableFrom(map.DestinationType))
            {
                expression.ForMember(nameof(IHaveDateCreated.DateCreated), e => e.Ignore());
            }

            if (typeof(IHaveDateUpdated).IsAssignableFrom(map.DestinationType))
            {
                expression.ForMember(nameof(IHaveDateUpdated.DateUpdated), e => e.Ignore());
            }
        }
    }
}
