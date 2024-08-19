namespace HM.AppComponents.AppService.Services.Generic;

public interface IConfigValueProvider<TValue, TToken>
{
    TValue? GetValue(TToken token);
}