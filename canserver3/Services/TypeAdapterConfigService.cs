using Avalonia.Collections;
using canserver3.Models;
using canserver3.ViewModels;
using Mapster;
using System;

namespace canserver3.Services
{
    public static class TypeAdapterConfigService
    {
        public static void Configure()
        {
            TypeAdapterConfig<AbstractFunctionNodeViewModel, AbstractFunctionNodeModel>.NewConfig();

            TypeAdapterConfig<AbstractFunctionNodeModel, AbstractFunctionNodeViewModel>.NewConfig();


        }
        public static void ConfigureEnums()
        {
            
        }
        public static void ConfigureBasicTypes()
        {
            // 1. 整数类型
            TypeAdapterConfig<byte, byte>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<sbyte, sbyte>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<short, short>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<ushort, ushort>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<int, int>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<uint, uint>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<long, long>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<ulong, ulong>.NewConfig().MapWith(src => src);

            // 2. 浮点类型
            TypeAdapterConfig<float, float>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<double, double>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<decimal, decimal>.NewConfig().MapWith(src => src);

            // 3. 其他值类型
            TypeAdapterConfig<bool, bool>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<char, char>.NewConfig().MapWith(src => src);

            // 4. 系统类型
            TypeAdapterConfig<IntPtr, IntPtr>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<UIntPtr, UIntPtr>.NewConfig().MapWith(src => src);
            // 可空整数类型
            TypeAdapterConfig<byte?, byte?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<sbyte?, sbyte?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<short?, short?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<ushort?, ushort?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<int?, int?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<uint?, uint?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<long?, long?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<ulong?, ulong?>.NewConfig().MapWith(src => src);

            // 可空浮点类型
            TypeAdapterConfig<float?, float?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<double?, double?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<decimal?, decimal?>.NewConfig().MapWith(src => src);

            // 其他可空值类型
            TypeAdapterConfig<bool?, bool?>.NewConfig().MapWith(src => src);
            TypeAdapterConfig<char?, char?>.NewConfig().MapWith(src => src);
        }
    }
}
