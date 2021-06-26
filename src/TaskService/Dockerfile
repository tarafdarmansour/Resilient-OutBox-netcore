FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

ENV APP_HOME /opt/app
RUN mkdir $APP_HOME
WORKDIR $APP_HOME


COPY TaskService/TaskService.csproj $APP_HOME/TaskService/
COPY CustomerService/CustomerService.csproj $APP_HOME/CustomerService/
RUN  cd $APP_HOME/TaskService && dotnet restore

COPY TaskService/. $APP_HOME/TaskService/
COPY CustomerService/. $APP_HOME/CustomerService/
RUN cd $APP_HOME/TaskService && dotnet build


FROM build AS publish
WORKDIR $APP_HOME/TaskService
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /opt/app/TaskService/out ./
ENTRYPOINT ["dotnet", "TaskService.dll"]