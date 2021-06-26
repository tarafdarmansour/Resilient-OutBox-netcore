FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME


COPY AccountingService/AccountingService.csproj $APP_HOME/AccountingService/
COPY CustomerService/CustomerService.csproj $APP_HOME/CustomerService/
RUN  cd $APP_HOME/AccountingService && dotnet restore

COPY AccountingService/. $APP_HOME/AccountingService/
COPY CustomerService/. $APP_HOME/CustomerService/
RUN cd $APP_HOME/AccountingService && dotnet build


FROM build AS publish
WORKDIR $APP_HOME/AccountingService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /opt/app/AccountingService/out ./
ENTRYPOINT ["dotnet", "AccountingService.dll"]