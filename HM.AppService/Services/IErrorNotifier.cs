﻿namespace HM.AppService.Services;

public interface IErrorNotifier
{
    void NotifyError(String message);
}
