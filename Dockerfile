ARG PWD_DIR=/usr/src/app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS backend-build
ARG PWD_DIR
WORKDIR ${PWD_DIR}

COPY ./API/*.csproj ./
RUN dotnet restore
COPY ./API/* ./
COPY . .
RUN rm -Rf ${PWD_DIR}/raghavendra-app
RUN dotnet publish SriRaghavendraApp.sln -c Release -o out

FROM node:16 AS ui-build
ARG PWD_DIR
ENV PATH ${PWD_DIR}/node_modules/.bin:$PATH
WORKDIR ${PWD_DIR}
COPY ./raghavendra-app/ ./angularapp/
RUN cd angularapp/ && npm config set legacy-peer-deps true && npm install @angular/cli@11 && npm install --save --legacy-peer-deps && npm run build:prod

FROM mcr.microsoft.com/dotnet/aspnet:5.0 
ARG PWD_DIR
WORKDIR /final-code
COPY --from=backend-build ${PWD_DIR}/out/* ./
COPY --from=ui-build ${PWD_DIR}/angularapp/dist/raghavendra-app ./wwwroot/
EXPOSE 80
ENTRYPOINT ["dotnet","API.dll"]

# FROM ubuntu
# WORKDIR /finaldirectory
# COPY --from=final-build ./final-code/* .
# ENTRYPOINT ["dotnet","API.dll"]
